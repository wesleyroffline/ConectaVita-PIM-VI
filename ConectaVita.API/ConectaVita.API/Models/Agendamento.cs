namespace ConectaVita.API.Models;

public class Agendamento
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
    public int FisioterapeutaId { get; set; }
    public Fisioterapeuta? Fisioterapeuta { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Agendado";
    public string? Observacoes { get; set; }
}