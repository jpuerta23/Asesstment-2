using Microsoft.EntityFrameworkCore;
using Test.Domain.Interfaces;
using Test.Domain.Entities;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositories;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Empleado>> GetAllWithDetailsAsync()
    {
        return await _context.Empleados
            .Include(e => e.Cargo)
            .Include(e => e.Departamento)
            .Include(e => e.NivelEducativo)
            .Include(e => e.EstadoEmpleado)
            .ToListAsync();
    }

    public IQueryable<Empleado> GetAllWithDetailsQueryable()
    {
        return _context.Empleados
            .Include(e => e.Cargo)
            .Include(e => e.Departamento)
            .Include(e => e.NivelEducativo)
            .Include(e => e.EstadoEmpleado);
    }

    public async Task<Empleado> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Empleados
            .Include(e => e.Cargo)
            .Include(e => e.Departamento)
            .Include(e => e.NivelEducativo)
            .Include(e => e.EstadoEmpleado)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
