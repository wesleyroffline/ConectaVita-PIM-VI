using ConectaVita.API.Data;
using ConectaVita.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectaVita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FisioterapeutasController : ControllerBase
{
    private readonly AppDbContext _context;

    public FisioterapeutasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Fisioterapeutas.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fisio = await _context.Fisioterapeutas.FindAsync(id);
        if (fisio == null) return NotFound();
        return Ok(fisio);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Fisioterapeuta fisio)
    {
        _context.Fisioterapeutas.Add(fisio);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = fisio.Id }, fisio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Fisioterapeuta fisio)
    {
        if (id != fisio.Id) return BadRequest();
        _context.Entry(fisio).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fisio = await _context.Fisioterapeutas.FindAsync(id);
        if (fisio == null) return NotFound();
        _context.Fisioterapeutas.Remove(fisio);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}