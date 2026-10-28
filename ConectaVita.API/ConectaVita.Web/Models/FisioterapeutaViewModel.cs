namespace ConectaVita.Web.Models;

public class FisioterapeutaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Especialidade { get; set; }
    public string? CREFITO { get; set; }
}