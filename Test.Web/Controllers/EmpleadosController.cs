using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Test.Domain.Interfaces;
using Test.Web.ViewModels;

namespace Test.Web.Controllers;

public class EmpleadosController : Controller
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IGenericRepository<Cargo> _cargoRepository;
    private readonly IGenericRepository<Departamento> _departamentoRepository;
    private readonly IGenericRepository<NivelEducativo> _nivelEducativoRepository;
    private readonly IGenericRepository<EstadoEmpleado> _estadoEmpleadoRepository;
    private readonly IExcelService _excelService;
    private readonly IPdfService _pdfService;

    public EmpleadosController(
        IEmpleadoRepository empleadoRepository,
        IGenericRepository<Cargo> cargoRepository,
        IGenericRepository<Departamento> departamentoRepository,
        IGenericRepository<NivelEducativo> nivelEducativoRepository,
        IGenericRepository<EstadoEmpleado> estadoEmpleadoRepository,
        IExcelService excelService,
        IPdfService pdfService)
    {
        _empleadoRepository = empleadoRepository;
        _cargoRepository = cargoRepository;
        _departamentoRepository = departamentoRepository;
        _nivelEducativoRepository = nivelEducativoRepository;
        _estadoEmpleadoRepository = estadoEmpleadoRepository;
        _excelService = excelService;
        _pdfService = pdfService;
    }


    public async Task<IActionResult> DownloadPdf(int id)
    {
        var empleado = await _empleadoRepository.GetByIdWithDetailsAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }

        var pdfBytes = _pdfService.GenerateResume(empleado);
        return File(pdfBytes, "application/pdf", $"HojaDeVida_{empleado.Nombres}_{empleado.Apellidos}.pdf");
    }

    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            await _excelService.ImportEmployeesAsync(stream);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Empleados
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
    {
        var empleadosQuery = _empleadoRepository.GetAllWithDetailsQueryable();
        
        var viewModelsQuery = empleadosQuery.Select(e => new EmpleadoViewModel
        {
            Id = e.Id,
            Nombres = e.Nombres,
            Apellidos = e.Apellidos,
            FechaNacimiento = e.FechaNacimiento,
            Direccion = e.Direccion,
            Telefono = e.Telefono,
            Email = e.Email,
            Salario = e.Salario,
            FechaIngreso = e.FechaIngreso,
            PerfilProfesional = e.PerfilProfesional,
            CargoNombre = e.Cargo != null ? e.Cargo.Nombre : null,
            DepartamentoNombre = e.Departamento != null ? e.Departamento.Nombre : null,
            NivelEducativoNombre = e.NivelEducativo != null ? e.NivelEducativo.Nombre : null,
            EstadoEmpleadoNombre = e.EstadoEmpleado != null ? e.EstadoEmpleado.Nombre : null
        });

        var paginatedList = await Test.Web.Helpers.PaginatedList<EmpleadoViewModel>.CreateAsync(
            viewModelsQuery, pageNumber, pageSize);

        return View(paginatedList);
    }

    // GET: Empleados/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var empleado = await _empleadoRepository.GetByIdWithDetailsAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }

        var viewModel = new EmpleadoViewModel
        {
            Id = empleado.Id,
            Nombres = empleado.Nombres,
            Apellidos = empleado.Apellidos,
            FechaNacimiento = empleado.FechaNacimiento,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            Salario = empleado.Salario,
            FechaIngreso = empleado.FechaIngreso,
            PerfilProfesional = empleado.PerfilProfesional,
            CargoNombre = empleado.Cargo?.Nombre,
            DepartamentoNombre = empleado.Departamento?.Nombre,
            NivelEducativoNombre = empleado.NivelEducativo?.Nombre,
            EstadoEmpleadoNombre = empleado.EstadoEmpleado?.Nombre
        };

        return View(viewModel);
    }

    // GET: Empleados/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new EmpleadoCreateViewModel();
        await PopulateDropdowns(viewModel);
        return View(viewModel);
    }

    // POST: Empleados/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmpleadoCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var empleado = new Empleado
            {
                Nombres = viewModel.Nombres,
                Apellidos = viewModel.Apellidos,
                FechaNacimiento = DateTime.SpecifyKind(viewModel.FechaNacimiento, DateTimeKind.Utc),
                Direccion = viewModel.Direccion,
                Telefono = viewModel.Telefono,
                Email = viewModel.Email,
                Salario = viewModel.Salario,
                FechaIngreso = DateTime.SpecifyKind(viewModel.FechaIngreso, DateTimeKind.Utc),
                PerfilProfesional = viewModel.PerfilProfesional,
                CargoId = viewModel.CargoId,
                DepartamentoId = viewModel.DepartamentoId,
                NivelEducativoId = viewModel.NivelEducativoId,
                EstadoEmpleadoId = viewModel.EstadoEmpleadoId
            };

            await _empleadoRepository.AddAsync(empleado);
            return RedirectToAction(nameof(Index));
        }
        await PopulateDropdowns(viewModel);
        return View(viewModel);
    }

    // GET: Empleados/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var empleado = await _empleadoRepository.GetByIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }

        var viewModel = new EmpleadoEditViewModel
        {
            Id = empleado.Id,
            Nombres = empleado.Nombres,
            Apellidos = empleado.Apellidos,
            FechaNacimiento = empleado.FechaNacimiento,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            Salario = empleado.Salario,
            FechaIngreso = empleado.FechaIngreso,
            PerfilProfesional = empleado.PerfilProfesional,
            CargoId = empleado.CargoId,
            DepartamentoId = empleado.DepartamentoId,
            NivelEducativoId = empleado.NivelEducativoId,
            EstadoEmpleadoId = empleado.EstadoEmpleadoId
        };

        await PopulateDropdowns(viewModel);
        return View(viewModel);
    }

    // POST: Empleados/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmpleadoEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var empleado = await _empleadoRepository.GetByIdAsync(id);
                if (empleado == null) return NotFound();

                empleado.Nombres = viewModel.Nombres;
                empleado.Apellidos = viewModel.Apellidos;
                empleado.FechaNacimiento = DateTime.SpecifyKind(viewModel.FechaNacimiento, DateTimeKind.Utc);
                empleado.Direccion = viewModel.Direccion;
                empleado.Telefono = viewModel.Telefono;
                empleado.Email = viewModel.Email;
                empleado.Salario = viewModel.Salario;
                empleado.FechaIngreso = DateTime.SpecifyKind(viewModel.FechaIngreso, DateTimeKind.Utc);
                empleado.PerfilProfesional = viewModel.PerfilProfesional;
                empleado.CargoId = viewModel.CargoId;
                empleado.DepartamentoId = viewModel.DepartamentoId;
                empleado.NivelEducativoId = viewModel.NivelEducativoId;
                empleado.EstadoEmpleadoId = viewModel.EstadoEmpleadoId;

                await _empleadoRepository.UpdateAsync(empleado);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _empleadoRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        await PopulateDropdowns(viewModel);
        return View(viewModel);
    }

    // GET: Empleados/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var empleado = await _empleadoRepository.GetByIdWithDetailsAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }

        var viewModel = new EmpleadoViewModel
        {
            Id = empleado.Id,
            Nombres = empleado.Nombres,
            Apellidos = empleado.Apellidos,
            FechaNacimiento = empleado.FechaNacimiento,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            Salario = empleado.Salario,
            FechaIngreso = empleado.FechaIngreso,
            PerfilProfesional = empleado.PerfilProfesional,
            CargoNombre = empleado.Cargo?.Nombre,
            DepartamentoNombre = empleado.Departamento?.Nombre,
            NivelEducativoNombre = empleado.NivelEducativo?.Nombre,
            EstadoEmpleadoNombre = empleado.EstadoEmpleado?.Nombre
        };

        return View(viewModel);
    }

    // POST: Empleados/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _empleadoRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns(dynamic viewModel)
    {
        var cargos = await _cargoRepository.GetAllAsync();
        var departamentos = await _departamentoRepository.GetAllAsync();
        var niveles = await _nivelEducativoRepository.GetAllAsync();
        var estados = await _estadoEmpleadoRepository.GetAllAsync();

        viewModel.Cargos = cargos.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre });
        viewModel.Departamentos = departamentos.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nombre });
        viewModel.NivelesEducativos = niveles.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Nombre });
        viewModel.EstadosEmpleado = estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre });
    }
}
