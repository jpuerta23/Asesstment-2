using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Test.Web.ViewModels;

public class EmpleadoEditViewModel
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
    [Required(ErrorMessage = "El Cargo es requerido")]
    public int CargoId { get; set; }

    [Display(Name = "Departamento")]
    [Required(ErrorMessage = "El Departamento es requerido")]
    public int DepartamentoId { get; set; }

    [Display(Name = "Nivel Educativo")]
    [Required(ErrorMessage = "El Nivel Educativo es requerido")]
    public int NivelEducativoId { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El Estado es requerido")]
    public int EstadoEmpleadoId { get; set; }

    // Dropdowns
    public IEnumerable<SelectListItem>? Cargos { get; set; }
    public IEnumerable<SelectListItem>? Departamentos { get; set; }
    public IEnumerable<SelectListItem>? NivelesEducativos { get; set; }
    public IEnumerable<SelectListItem>? EstadosEmpleado { get; set; }
}
