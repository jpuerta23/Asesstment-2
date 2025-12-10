using ClosedXML.Excel;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Domain.Interfaces;

namespace Test.Infrastructure.Services;

public class ExcelService : IExcelService
{
    private readonly IGenericRepository<Empleado> _empleadoRepository;
    private readonly IGenericRepository<Cargo> _cargoRepository;
    private readonly IGenericRepository<Departamento> _departamentoRepository;
    private readonly IGenericRepository<NivelEducativo> _nivelEducativoRepository;
    private readonly IGenericRepository<EstadoEmpleado> _estadoEmpleadoRepository;

    public ExcelService(
        IGenericRepository<Empleado> empleadoRepository,
        IGenericRepository<Cargo> cargoRepository,
        IGenericRepository<Departamento> departamentoRepository,
        IGenericRepository<NivelEducativo> nivelEducativoRepository,
        IGenericRepository<EstadoEmpleado> estadoEmpleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
        _cargoRepository = cargoRepository;
        _departamentoRepository = departamentoRepository;
        _nivelEducativoRepository = nivelEducativoRepository;
        _estadoEmpleadoRepository = estadoEmpleadoRepository;
    }

    public async Task ImportEmployeesAsync(Stream fileStream)
    {
        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

        foreach (var row in rows)
        {
            // Extract values
            var nombres = row.Cell(2).GetValue<string>();
            var apellidos = row.Cell(3).GetValue<string>();
            
            DateTime fechaNacimiento;
            if (!row.Cell(4).TryGetValue(out fechaNacimiento))
            {
                // Fallback: try parsing string
                var dateStr = row.Cell(4).GetValue<string>();
                if (!DateTime.TryParse(dateStr, out fechaNacimiento))
                {
                    fechaNacimiento = DateTime.MinValue; // Or handle error
                }
            }
            fechaNacimiento = DateTime.SpecifyKind(fechaNacimiento, DateTimeKind.Utc);

            var direccion = row.Cell(5).GetValue<string>();
            var telefono = row.Cell(6).GetValue<string>();
            var email = row.Cell(7).GetValue<string>();
            var cargoNombre = row.Cell(8).GetValue<string>();
            
            decimal salario;
            if (!row.Cell(9).TryGetValue(out salario))
            {
                var salarioStr = row.Cell(9).GetValue<string>();
                if (!string.IsNullOrWhiteSpace(salarioStr))
                {
                    // Remove currency symbols and whitespace, keep digits, dots, commas, minus
                    salarioStr = System.Text.RegularExpressions.Regex.Replace(salarioStr, @"[^\d.,-]", "");

                    // Try parsing with InvariantCulture (dot as decimal separator)
                    if (!decimal.TryParse(salarioStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out salario))
                    {
                        // If failed, try with es-ES culture (comma as decimal separator)
                        if (!decimal.TryParse(salarioStr, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("es-ES"), out salario))
                        {
                            salario = 0;
                        }
                    }
                }
            }
            
            DateTime fechaIngreso;
            if (!row.Cell(10).TryGetValue(out fechaIngreso))
            {
                var dateStr = row.Cell(10).GetValue<string>();
                 if (!DateTime.TryParse(dateStr, out fechaIngreso))
                {
                    fechaIngreso = DateTime.UtcNow; // Or handle error
                }
            }
            fechaIngreso = DateTime.SpecifyKind(fechaIngreso, DateTimeKind.Utc);

            var estadoNombre = row.Cell(11).GetValue<string>();
            var nivelEducativoNombre = row.Cell(12).GetValue<string>();
            var perfilProfesional = row.Cell(13).GetValue<string>();
            var departamentoNombre = row.Cell(14).GetValue<string>();

            // Handle Related Entities
            var cargo = await _cargoRepository.FindAsync(c => c.Nombre == cargoNombre);
            if (cargo == null)
            {
                cargo = new Cargo { Nombre = cargoNombre };
                await _cargoRepository.AddAsync(cargo);
                // Re-fetch to get ID? Or AddAsync should set ID if EF Core tracks it. 
                // Repository AddAsync calls SaveChanges, so ID should be set.
            }

            var departamento = await _departamentoRepository.FindAsync(d => d.Nombre == departamentoNombre);
            if (departamento == null)
            {
                departamento = new Departamento { Nombre = departamentoNombre };
                await _departamentoRepository.AddAsync(departamento);
            }

            var nivelEducativo = await _nivelEducativoRepository.FindAsync(n => n.Nombre == nivelEducativoNombre);
            if (nivelEducativo == null)
            {
                nivelEducativo = new NivelEducativo { Nombre = nivelEducativoNombre };
                await _nivelEducativoRepository.AddAsync(nivelEducativo);
            }

            var estadoEmpleado = await _estadoEmpleadoRepository.FindAsync(e => e.Nombre == estadoNombre);
            if (estadoEmpleado == null)
            {
                estadoEmpleado = new EstadoEmpleado { Nombre = estadoNombre };
                await _estadoEmpleadoRepository.AddAsync(estadoEmpleado);
            }

            // Handle Empleado
            var empleado = await _empleadoRepository.FindAsync(e => e.Email == email);
            if (empleado == null)
            {
                empleado = new Empleado
                {
                    Nombres = nombres,
                    Apellidos = apellidos,
                    FechaNacimiento = fechaNacimiento,
                    Direccion = direccion,
                    Telefono = telefono,
                    Email = email,
                    CargoId = cargo.Id,
                    Salario = salario,
                    FechaIngreso = fechaIngreso,
                    EstadoEmpleadoId = estadoEmpleado.Id,
                    NivelEducativoId = nivelEducativo.Id,
                    PerfilProfesional = perfilProfesional,
                    DepartamentoId = departamento.Id
                };
                await _empleadoRepository.AddAsync(empleado);
            }
            else
            {
                // Update existing
                empleado.Nombres = nombres;
                empleado.Apellidos = apellidos;
                empleado.FechaNacimiento = fechaNacimiento;
                empleado.Direccion = direccion;
                empleado.Telefono = telefono;
                empleado.CargoId = cargo.Id;
                empleado.Salario = salario;
                empleado.FechaIngreso = fechaIngreso;
                empleado.EstadoEmpleadoId = estadoEmpleado.Id;
                empleado.NivelEducativoId = nivelEducativo.Id;
                empleado.PerfilProfesional = perfilProfesional;
                empleado.DepartamentoId = departamento.Id;
                
                await _empleadoRepository.UpdateAsync(empleado);
            }
        }
    }
}
