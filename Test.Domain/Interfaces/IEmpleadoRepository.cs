using Test.Domain.Entities;

namespace Test.Domain.Interfaces;

public interface IEmpleadoRepository : IGenericRepository<Empleado>
{
    Task<IEnumerable<Empleado>> GetAllWithDetailsAsync();
    IQueryable<Empleado> GetAllWithDetailsQueryable();
    Task<Empleado> GetByIdWithDetailsAsync(int id);
}
