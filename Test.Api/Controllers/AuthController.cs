using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test.Api.Dtos;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Infrastructure.Data;

namespace Test.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthController(AppDbContext context, IConfiguration configuration, IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if user already exists
        if (_context.Users.Any(u => u.Username == dto.Email))
            return BadRequest("User already exists");

        // Create Empleado
        var empleado = new Empleado
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            Salario = dto.Salario,
            FechaIngreso = dto.FechaIngreso.ToUniversalTime(), // Ensure UTC
            FechaNacimiento = DateTime.UtcNow, // Default or add to DTO
            CargoId = dto.CargoId,
            DepartamentoId = dto.DepartamentoId,
            NivelEducativoId = dto.NivelEducativoId,
            EstadoEmpleadoId = 1, // Default to Activo
            PerfilProfesional = dto.PerfilProfesional
        };

        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();

        // Create User
        var user = new User
        {
            Username = dto.Email,
            Password = dto.Password, // In production, hash this!
            Role = Role.Customer
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Send Welcome Email
        try
        {
            await _emailService.SendWelcomeEmailAsync(dto.Email, $"{dto.Nombres} {dto.Apellidos}");
        }
        catch (Exception ex)
        {
            // Log error but don't fail registration
            Console.WriteLine($"Error sending email: {ex.Message}");
        }

        return Ok(new { Message = "Registration successful" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}
