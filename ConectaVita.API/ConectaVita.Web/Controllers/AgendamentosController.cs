using ConectaVita.Web.Models;
using ConectaVita.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConectaVita.Web.Controllers;

public class AgendamentosController : Controller
{
    private readonly ApiService _api;

    public AgendamentosController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _api.GetAsync<List<AgendamentoViewModel>>("api/agendamentos") ?? new();
        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AgendamentoViewModel model)
    {
        await _api.PostAsync("api/agendamentos", new
        {
            model.PacienteId,
            model.FisioterapeutaId,
            model.DataHora,
            model.Observacoes
        });
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _api.GetAsync<AgendamentoViewModel>($"api/agendamentos/{id}");
        if (item == null) return NotFound();
        await CarregarListas();
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AgendamentoViewModel model)
    {
        await _api.PutAsync($"api/agendamentos/{model.Id}", new
        {
            model.PacienteId,
            model.FisioterapeutaId,
            model.DataHora,
            model.Observacoes
        });
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync($"api/agendamentos/{id}");
        return RedirectToAction("Index");
    }

    private async Task CarregarListas()
    {
        var pacientes = await _api.GetAsync<List<PacienteViewModel>>("api/pacientes") ?? new();
        var fisios = await _api.GetAsync<List<FisioterapeutaViewModel>>("api/fisioterapeutas") ?? new();

        ViewBag.Pacientes = new SelectList(pacientes, "Id", "Nome");
        ViewBag.Fisioterapeutas = new SelectList(fisios, "Id", "Nome");
    }
}