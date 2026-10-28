namespace ConectaVita.API.DTOs;

public class AgendamentoDTO
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string? PacienteNome { get; set; }
    public int FisioterapeutaId { get; set; }
    public string? FisioterapeutaNome { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Agendado";
    public string? Observacoes { get; set; }
}

public class AgendamentoCreateDTO
{
    public int PacienteId { get; set; }
    public int FisioterapeutaId { get; set; }
    public DateTime DataHora { get; set; }
    public string? Observacoes { get; set; }
}