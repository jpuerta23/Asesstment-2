namespace Test.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;

public class Empleado
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nombres { get; set; }

    [Required, MaxLength(150)]
    public string Apellidos { get; set; }

    public DateTime FechaNacimiento { get; set; }

    [MaxLength(250)]
    public string Direccion { get; set; }

    public string Telefono { get; set; }

    [MaxLength(200)]
    public string Email { get; set; }

    public decimal Salario { get; set; }
    public DateTime FechaIngreso { get; set; }

    public string PerfilProfesional { get; set; }

    // Relaciones
    public int CargoId { get; set; }
    public Cargo Cargo { get; set; }

    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; }

    public int NivelEducativoId { get; set; }
    public NivelEducativo NivelEducativo { get; set; }

    public int EstadoEmpleadoId { get; set; }
    public EstadoEmpleado EstadoEmpleado { get; set; }
}