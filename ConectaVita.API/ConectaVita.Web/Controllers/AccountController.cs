using ConectaVita.Web.Models;
using ConectaVita.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConectaVita.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _api;

    public AccountController(ApiService api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var response = await _api.PostLoginAsync("api/auth/login", new { model.Email, model.Senha });
        if (response == null)
        {
            ViewBag.Erro = "E-mail ou senha inválidos.";
            return View(model);
        }

        var auth = JsonSerializer.Deserialize<AuthResponseViewModel>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        HttpContext.Session.SetString("Token", auth!.Token);
        HttpContext.Session.SetString("Nome", auth.Nome);
        HttpContext.Session.SetString("Perfil", auth.Perfil);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}