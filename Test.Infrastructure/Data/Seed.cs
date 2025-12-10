using Microsoft.EntityFrameworkCore;
using Test.Domain.Entities;

namespace Test.Infrastructure.Data;

public static class Seed
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Cargos
        modelBuilder.Entity<Cargo>().HasData(
            new Cargo { Id = 1, Nombre = "Gerente" },
            new Cargo { Id = 2, Nombre = "Supervisor" },
            new Cargo { Id = 3, Nombre = "Analista" },
            new Cargo { Id = 4, Nombre = "Asistente" },
            new Cargo { Id = 5, Nombre = "Técnico" }
        );

        // Seed Departamentos
        modelBuilder.Entity<Departamento>().HasData(
            new Departamento { Id = 1, Nombre = "Recursos Humanos" },
            new Departamento { Id = 2, Nombre = "Tecnología" },
            new Departamento { Id = 3, Nombre = "Ventas" },
            new Departamento { Id = 4, Nombre = "Finanzas" },
            new Departamento { Id = 5, Nombre = "Operaciones" }
        );

        // Seed NivelesEducativos
        modelBuilder.Entity<NivelEducativo>().HasData(
            new NivelEducativo { Id = 1, Nombre = "Bachillerato" },
            new NivelEducativo { Id = 2, Nombre = "Técnico" },
            new NivelEducativo { Id = 3, Nombre = "Tecnólogo" },
            new NivelEducativo { Id = 4, Nombre = "Profesional" },
            new NivelEducativo { Id = 5, Nombre = "Especialización" },
            new NivelEducativo { Id = 6, Nombre = "Maestría" },
            new NivelEducativo { Id = 7, Nombre = "Doctorado" }
        );

        // Seed EstadosEmpleado
        modelBuilder.Entity<EstadoEmpleado>().HasData(
            new EstadoEmpleado { Id = 1, Nombre = "Activo" },
            new EstadoEmpleado { Id = 2, Nombre = "Inactivo" },
            new EstadoEmpleado { Id = 3, Nombre = "Vacaciones" }
        );

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            Password = "admin", // In a real app, hash this!
            Role = Role.Admin
        });
    }
}
