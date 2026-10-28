using ConectaVita.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConectaVita.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _api;

    public HomeController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Token") == null)
            return RedirectToAction("Login", "Account");

        var pacientes = await _api.GetAsync<List<object>>("api/pacientes") ?? new();
        var fisios = await _api.GetAsync<List<object>>("api/fisioterapeutas") ?? new();
        var agendamentos = await _api.GetAsync<List<object>>("api/agendamentos") ?? new();

        ViewBag.TotalPacientes = pacientes.Count;
        ViewBag.TotalFisios = fisios.Count;
        ViewBag.TotalAgendamentos = agendamentos.Count;

        return View();
    }
}