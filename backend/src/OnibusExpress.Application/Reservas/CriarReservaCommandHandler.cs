using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.Services;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Reservas;

public class CriarReservaCommandHandler : IRequestHandler<CriarReservaCommand, ReservaDto>
{
    private readonly AppDbContext _context;

    public CriarReservaCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReservaDto> Handle(CriarReservaCommand request, CancellationToken cancellationToken)
    {
        var dados = request.Dados;

        // 1. Buscar a viagem, com suas reservas já carregadas
        var viagem = await _context.Viagens
            .Include(v => v.Reservas)
            .FirstOrDefaultAsync(v => v.Id == dados.ViagemId, cancellationToken);

        if (viagem is null)
            throw new InvalidOperationException("Viagem não encontrada.");

        // 2. Regra: não pode reservar viagem já realizada
        if (viagem.JaFoiRealizada())
            throw new InvalidOperationException("Não é possível reservar uma viagem que já foi realizada.");

        // 3. Regra: assento válido dentro do total, e não pode estar ocupado
        if (dados.NumeroAssento < 1 || dados.NumeroAssento > viagem.TotalAssentos)
            throw new InvalidOperationException($"Número de assento inválido. Escolha entre 1 e {viagem.TotalAssentos}.");

        if (viagem.AssentoEstaOcupado(dados.NumeroAssento))
            throw new InvalidOperationException("Este assento já está ocupado.");

        // 4. Passageiro: reutiliza se o CPF já existir, senão cria um novo
        var cpfLimpo = CpfValidator.RemoverFormatacao(dados.Cpf);
        var passageiro = await _context.Passageiros
            .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo, cancellationToken);

        if (passageiro is null)
        {
            passageiro = new Passageiro(dados.NomePassageiro, dados.Cpf, dados.Email, dados.DataNascimento);
            _context.Passageiros.Add(passageiro);
        }

        // 5. Gerar código de reserva único, com retry em caso de colisão (raríssimo)
        string codigoReserva;
        var tentativas = 0;
        do
        {
            codigoReserva = GeradorCodigoReserva.Gerar();
            tentativas++;

            if (tentativas > 10)
                throw new InvalidOperationException("Não foi possível gerar um código de reserva único. Tente novamente.");
        }
        while (await _context.Reservas.AnyAsync(r => r.CodigoReserva == codigoReserva, cancellationToken));

        // 6. Criar a reserva (entidade rica, já valida no construtor)
        var reserva = new Reserva(viagem.Id, passageiro.Id, dados.NumeroAssento, codigoReserva);
        _context.Reservas.Add(reserva);

        await _context.SaveChangesAsync(cancellationToken);

        return new ReservaDto(
            reserva.Id,
            reserva.CodigoReserva,
            reserva.ViagemId,
            passageiro.Nome,
            reserva.NumeroAssento,
            reserva.Status.ToString(),
            reserva.CriadaEm);
    }
}