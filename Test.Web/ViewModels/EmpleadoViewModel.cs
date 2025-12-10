using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Test.Web.ViewModels;

public class EmpleadoViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nombres { get; set; }

    [Required, MaxLength(150)]
    public string Apellidos { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    [MaxLength(250)]
    public string Direccion { get; set; }

    [Required]
    public string Telefono { get; set; }

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; }

    [Required]
    public decimal Salario { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; }

    public string PerfilProfesional { get; set; }

    // Foreign Keys
    [Display(Name = "Cargo")]
    public int CargoId { get; set; }

    [Display(Name = "Departamento")]
    public int DepartamentoId { get; set; }

    [Display(Name = "Nivel Educativo")]
    public int NivelEducativoId { get; set; }

    [Display(Name = "Estado")]
    public int EstadoEmpleadoId { get; set; }

    // Display Names for Index/Details
    public string? CargoNombre { get; set; }
    public string? DepartamentoNombre { get; set; }
    public string? NivelEducativoNombre { get; set; }
    public string? EstadoEmpleadoNombre { get; set; }

    // Dropdowns
    public IEnumerable<SelectListItem>? Cargos { get; set; }
    public IEnumerable<SelectListItem>? Departamentos { get; set; }
    public IEnumerable<SelectListItem>? NivelesEducativos { get; set; }
    public IEnumerable<SelectListItem>? EstadosEmpleado { get; set; }
}
