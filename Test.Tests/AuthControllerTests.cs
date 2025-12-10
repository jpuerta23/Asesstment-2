using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Test.Api.Controllers;
using Test.Api.Dtos;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Infrastructure.Data;
using Xunit;

namespace Test.Tests;

public class AuthControllerTests
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AppDbContext _context;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockEmailService = new Mock<IEmailService>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup InMemory Database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;
        _context = new AppDbContext(options);

        // Setup Configuration Mock
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("ThisIsASecureKeyForJwtAuthentication123!");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("Test.Api");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("Test.Web");

        _controller = new AuthController(_context, _mockConfiguration.Object, _mockEmailService.Object);
    }

    [Fact]
    public async Task Register_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Nombres = "John",
            Apellidos = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Telefono = "1234567890",
            Direccion = "123 Main St",
            Salario = 50000,
            FechaIngreso = DateTime.UtcNow,
            CargoId = 1,
            DepartamentoId = 1,
            NivelEducativoId = 1,
            PerfilProfesional = "Developer"
        };

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        // Verify Empleado created
        var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == dto.Email);
        Assert.NotNull(empleado);
        Assert.Equal(dto.Nombres, empleado.Nombres);

        // Verify User created
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Email);
        Assert.NotNull(user);
        Assert.Equal(Role.Customer, user.Role);

        // Verify Email sent
        _mockEmailService.Verify(x => x.SendWelcomeEmailAsync(dto.Email, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Login_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User
        {
            Username = "test@example.com",
            Password = "password123",
            Role = Role.Customer
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        var dto = new LoginDto
        {
            Username = "test@example.com",
            Password = "password123"
        };

        // Act
        var result = _controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        // Check if token property exists in response
        var val = okResult.Value;
        var tokenProperty = val.GetType().GetProperty("Token");
        Assert.NotNull(tokenProperty);
        var tokenValue = tokenProperty.GetValue(val) as string;
        Assert.False(string.IsNullOrEmpty(tokenValue));
    }
}
