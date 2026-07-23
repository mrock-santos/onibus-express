using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Rota> Rotas => Set<Rota>();
    public DbSet<Viagem> Viagens => Set<Viagem>();
    public DbSet<Passageiro> Passageiros => Set<Passageiro>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}