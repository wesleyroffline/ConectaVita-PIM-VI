using ConectaVita.Web.Models;
using ConectaVita.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConectaVita.Web.Controllers;

public class FisioterapeutasController : Controller
{
    private readonly ApiService _api;

    public FisioterapeutasController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _api.GetAsync<List<FisioterapeutaViewModel>>("api/fisioterapeutas");
        if (lista == null) lista = new List<FisioterapeutaViewModel>();
        return View(lista);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(FisioterapeutaViewModel model)
    {
        await _api.PostAsync("api/fisioterapeutas", model);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _api.GetAsync<FisioterapeutaViewModel>($"api/fisioterapeutas/{id}");
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FisioterapeutaViewModel model)
    {
        await _api.PutAsync($"api/fisioterapeutas/{model.Id}", model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync($"api/fisioterapeutas/{id}");
        return RedirectToAction("Index");
    }
}