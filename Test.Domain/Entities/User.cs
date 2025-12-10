namespace Test.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; } // In a real app, this should be hashed

    public Role Role { get; set; }
}

public enum Role
{
    Admin,
    Customer
}
