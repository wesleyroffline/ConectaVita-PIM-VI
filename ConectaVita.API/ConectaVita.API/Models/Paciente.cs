namespace ConectaVita.API.Models;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CPF { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public DateTime? DataNascimento { get; set; }
}