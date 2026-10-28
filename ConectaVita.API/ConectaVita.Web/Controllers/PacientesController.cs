using ConectaVita.Web.Models;
using ConectaVita.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConectaVita.Web.Controllers;

public class PacientesController : Controller
{
    private readonly ApiService _api;

    public PacientesController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var pacientes = await _api.GetAsync<List<PacienteViewModel>>("api/pacientes") ?? new();
        return View(pacientes);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(PacienteViewModel model)
    {
        await _api.PostAsync("api/pacientes", model);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var paciente = await _api.GetAsync<PacienteViewModel>($"api/pacientes/{id}");
        if (paciente == null) return NotFound();
        return View(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PacienteViewModel model)
    {
        await _api.PutAsync($"api/pacientes/{model.Id}", model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync($"api/pacientes/{id}");
        return RedirectToAction("Index");
    }
}