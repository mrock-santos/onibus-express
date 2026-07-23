using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence.Configurations;

public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("reservas");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.NumeroAssento).IsRequired();
        builder.Property(r => r.Status).IsRequired().HasConversion<string>();
        builder.Property(r => r.CodigoReserva).IsRequired().HasMaxLength(20);
        builder.Property(r => r.CriadaEm).IsRequired();

        builder.HasOne(r => r.Passageiro)
            .WithMany()
            .HasForeignKey(r => r.PassageiroId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regra de negócio no nível do banco: não pode existir 2 reservas
        // confirmadas para o mesmo assento na mesma viagem
        builder.HasIndex(r => new { r.ViagemId, r.NumeroAssento })
            .IsUnique()
            .HasFilter("\"Status\" = 'Confirmada'");

        builder.HasIndex(r => r.CodigoReserva).IsUnique();
    }
}