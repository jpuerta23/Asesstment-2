namespace Test.Application.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string name);
}
