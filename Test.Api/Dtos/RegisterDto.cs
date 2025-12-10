using System.ComponentModel.DataAnnotations;

namespace Test.Api.Dtos;

public class RegisterDto
{
    [Required]
    public string Nombres { get; set; }

    [Required]
    public string Apellidos { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Telefono { get; set; }

    public string Direccion { get; set; }
    
    // Additional fields for Empleado
    public decimal Salario { get; set; }
    public DateTime FechaIngreso { get; set; }
    public int CargoId { get; set; }
    public int DepartamentoId { get; set; }
    public int NivelEducativoId { get; set; }
    public string PerfilProfesional { get; set; }
}
