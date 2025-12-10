using Test.Domain.Entities;

namespace Test.Application.Interfaces;

public interface IPdfService
{
    byte[] GenerateResume(Empleado empleado);
}
