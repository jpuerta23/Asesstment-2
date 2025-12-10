using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Application.Interfaces;
using Test.Domain.Interfaces;
using Test.Web.ViewModels;

[Authorize]
public class DashboardController : Controller
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IAIService _aiService;

    public DashboardController(IEmpleadoRepository empleadoRepository, IAIService aiService)
    {
        _empleadoRepository = empleadoRepository;
        _aiService = aiService;
    }

    public async Task<IActionResult> Index()
    {
        var empleados = await _empleadoRepository.GetAllWithDetailsAsync();

        var viewModel = new DashboardViewModel
        {
            TotalEmpleados = empleados.Count(),
            EmpleadosEnVacaciones = empleados.Count(e => e.EstadoEmpleado?.Nombre == "Vacaciones"),
            EmpleadosActivos = empleados.Count(e => e.EstadoEmpleado?.Nombre == "Activo")
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AskAI(string question)
    {
        var empleados = await _empleadoRepository.GetAllWithDetailsAsync();
        var answer = await _aiService.AnswerQuestionAsync(question, empleados);
        return Json(new { answer });
    }
}
