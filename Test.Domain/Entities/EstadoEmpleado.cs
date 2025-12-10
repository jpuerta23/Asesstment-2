namespace Test.Domain.Entities;

using System.Collections.Generic;

public class EstadoEmpleado
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public ICollection<Empleado> Empleados { get; set; }
}