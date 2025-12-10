namespace Test.Domain.Entities;

using System.Collections.Generic;

public class Cargo
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public ICollection<Empleado> Empleados { get; set; }
}