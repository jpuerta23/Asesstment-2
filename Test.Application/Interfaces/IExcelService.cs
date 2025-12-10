namespace Test.Application.Interfaces;

public interface IExcelService
{
    Task ImportEmployeesAsync(Stream fileStream);
}
