using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Entities;
using Test.Infrastructure.Data;

namespace Test.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartamentosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Departamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetDepartamentos()
    {
        return await _context.Departamentos
            .Select(d => new { d.Id, d.Nombre })
            .ToListAsync();
    }
}
