using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence.Configurations;

public class PassageiroConfiguration : IEntityTypeConfiguration<Passageiro>
{
    public void Configure(EntityTypeBuilder<Passageiro> builder)
    {
        builder.ToTable("passageiros");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Cpf).IsRequired().HasMaxLength(11);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(200);
        
        builder.Property(p => p.DataNascimento)
                .IsRequired()
                .HasColumnType("date");

        // Um passageiro com o mesmo CPF não deveria ser duplicado no banco
        builder.HasIndex(p => p.Cpf).IsUnique();
    }
}