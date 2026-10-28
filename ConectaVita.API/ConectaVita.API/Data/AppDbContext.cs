using ConectaVita.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ConectaVita.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Fisioterapeuta> Fisioterapeutas { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
}