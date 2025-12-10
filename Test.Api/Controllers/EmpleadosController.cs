using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Infrastructure.Data;

namespace Test.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmpleadosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPdfService _pdfService;

    public EmpleadosController(AppDbContext context, IPdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<object>> GetMe()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var empleado = await _context.Empleados
            .Include(e => e.Cargo)
            .Include(e => e.Departamento)
            .Include(e => e.NivelEducativo)
            .Include(e => e.EstadoEmpleado)
            .FirstOrDefaultAsync(e => e.Email == username);

        if (empleado == null)
            return NotFound("Employee profile not found for this user.");

        return new
        {
            empleado.Id,
            empleado.Nombres,
            empleado.Apellidos,
            empleado.Email,
            empleado.Telefono,
            empleado.Direccion,
            empleado.Salario,
            empleado.FechaIngreso,
            empleado.FechaNacimiento,
            Cargo = empleado.Cargo?.Nombre,
            Departamento = empleado.Departamento?.Nombre,
            NivelEducativo = empleado.NivelEducativo?.Nombre,
            Estado = empleado.EstadoEmpleado?.Nombre,
            empleado.PerfilProfesional
        };
    }

    [HttpGet("me/pdf")]
    public async Task<IActionResult> DownloadPdf()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var empleado = await _context.Empleados
            .Include(e => e.Cargo)
            .Include(e => e.Departamento)
            .Include(e => e.NivelEducativo)
            .Include(e => e.EstadoEmpleado)
            .FirstOrDefaultAsync(e => e.Email == username);

        if (empleado == null)
            return NotFound("Employee profile not found for this user.");

        var pdfBytes = _pdfService.GenerateResume(empleado);

        return File(pdfBytes, "application/pdf", $"HojaDeVida_{empleado.Nombres}_{empleado.Apellidos}.pdf");
    }
}
