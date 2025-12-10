using Microsoft.EntityFrameworkCore;
using Test.Domain.Entities;

namespace Test.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<NivelEducativo> NivelesEducativos { get; set; }
    public DbSet<EstadoEmpleado> EstadosEmpleado { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Empleado>()
            .HasOne(e => e.Cargo)
            .WithMany(c => c.Empleados)
            .HasForeignKey(e => e.CargoId);

        modelBuilder.Entity<Empleado>()
            .HasOne(e => e.Departamento)
            .WithMany(d => d.Empleados)
            .HasForeignKey(e => e.DepartamentoId);

        modelBuilder.Entity<Empleado>()
            .HasOne(e => e.NivelEducativo)
            .WithMany(ne => ne.Empleados)
            .HasForeignKey(e => e.NivelEducativoId);

        modelBuilder.Entity<Empleado>()
            .HasOne(e => e.EstadoEmpleado)
            .WithMany(ee => ee.Empleados)
            .HasForeignKey(e => e.EstadoEmpleadoId);


        Seed.SeedData(modelBuilder);
    }
}