using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence.Configurations;

public class ViagemConfiguration : IEntityTypeConfiguration<Viagem>
{
    public void Configure(EntityTypeBuilder<Viagem> builder)
    {
        builder.ToTable("viagens");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.DataHoraPartida).IsRequired();
        builder.Property(v => v.PrecoBase).HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(v => v.TotalAssentos).IsRequired();

        builder.HasOne(v => v.Rota)
            .WithMany()
            .HasForeignKey(v => v.RotaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Mapeia o campo privado _reservas como a coleção de navegação
        builder.HasMany(v => v.Reservas)
            .WithOne(r => r.Viagem)
            .HasForeignKey(r => r.ViagemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata.FindNavigation(nameof(Viagem.Reservas))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}