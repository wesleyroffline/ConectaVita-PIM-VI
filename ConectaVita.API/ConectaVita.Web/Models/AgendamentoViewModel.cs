namespace ConectaVita.Web.Models;

public class AgendamentoViewModel
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