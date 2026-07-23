using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence.Configurations;

public class RotaConfiguration : IEntityTypeConfiguration<Rota>
{
    public void Configure(EntityTypeBuilder<Rota> builder)
    {
        builder.ToTable("rotas");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Origem)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Destino)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.DuracaoEstimada)
            .IsRequired();
    }
}