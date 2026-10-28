using ConectaVita.API.Data;
using ConectaVita.API.DTOs;
using ConectaVita.API.Models;
using ConectaVita.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectaVita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new { message = "E-mail já cadastrado." });

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Perfil = request.Perfil
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var token = _tokenService.GerarToken(usuario);

        return Ok(new AuthResponse
        {
            Token = token,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null)
            return Unauthorized(new { message = "E-mail ou senha inválidos." });

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            return Unauthorized(new { message = "E-mail ou senha inválidos." });

        var token = _tokenService.GerarToken(usuario);

        return Ok(new AuthResponse
        {
            Token = token,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil
        });
    }
}