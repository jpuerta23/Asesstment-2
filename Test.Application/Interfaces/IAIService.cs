using Test.Domain.Entities;

namespace Test.Application.Interfaces;

public interface IAIService
{
    Task<string> AnswerQuestionAsync(string question, IEnumerable<Empleado> contextData);
}
