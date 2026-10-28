using ConectaVita.API.Data;
using ConectaVita.API.DTOs;
using ConectaVita.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectaVita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgendamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgendamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var agendamentos = await _context.Agendamentos
            .Include(a => a.Paciente)
            .Include(a => a.Fisioterapeuta)
            .Select(a => new AgendamentoDTO
            {
                Id = a.Id,
                PacienteId = a.PacienteId,
                PacienteNome = a.Paciente!.Nome,
                FisioterapeutaId = a.FisioterapeutaId,
                FisioterapeutaNome = a.Fisioterapeuta!.Nome,
                DataHora = a.DataHora,
                Status = a.Status,
                Observacoes = a.Observacoes
            })
            .ToListAsync();

        return Ok(agendamentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var a = await _context.Agendamentos
            .Include(x => x.Paciente)
            .Include(x => x.Fisioterapeuta)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (a == null) return NotFound();

        return Ok(new AgendamentoDTO
        {
            Id = a.Id,
            PacienteId = a.PacienteId,
            PacienteNome = a.Paciente!.Nome,
            FisioterapeutaId = a.FisioterapeutaId,
            FisioterapeutaNome = a.Fisioterapeuta!.Nome,
            DataHora = a.DataHora,
            Status = a.Status,
            Observacoes = a.Observacoes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AgendamentoCreateDTO dto)
    {
        var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == dto.PacienteId);
        if (!pacienteExiste) return BadRequest(new { message = "Paciente não encontrado." });

        var fisioExiste = await _context.Fisioterapeutas.AnyAsync(f => f.Id == dto.FisioterapeutaId);
        if (!fisioExiste) return BadRequest(new { message = "Fisioterapeuta não encontrado." });

        var agendamento = new Agendamento
        {
            PacienteId = dto.PacienteId,
            FisioterapeutaId = dto.FisioterapeutaId,
            DataHora = dto.DataHora,
            Observacoes = dto.Observacoes,
            Status = "Agendado"
        };

        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = agendamento.Id }, agendamento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AgendamentoCreateDTO dto)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento == null) return NotFound();

        agendamento.PacienteId = dto.PacienteId;
        agendamento.FisioterapeutaId = dto.FisioterapeutaId;
        agendamento.DataHora = dto.DataHora;
        agendamento.Observacoes = dto.Observacoes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento == null) return NotFound();

        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}