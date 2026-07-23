using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Reservas;

public class ObterReservaPorCodigoQueryHandler : IRequestHandler<ObterReservaPorCodigoQuery, ReservaDetalhesDto?>
{
    private readonly AppDbContext _context;

    public ObterReservaPorCodigoQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReservaDetalhesDto?> Handle(ObterReservaPorCodigoQuery request, CancellationToken cancellationToken)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Passageiro)
            .Include(r => r.Viagem)
                .ThenInclude(v => v.Rota)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.CodigoReserva == request.Codigo, cancellationToken);

        if (reserva is null)
            return null;

        return new ReservaDetalhesDto(
            reserva.Id,
            reserva.CodigoReserva,
            reserva.Status.ToString(),
            reserva.NumeroAssento,
            reserva.CriadaEm,
            reserva.Passageiro.Nome,
            reserva.Passageiro.Email,
            reserva.Viagem.Id,
            reserva.Viagem.Rota.Origem,
            reserva.Viagem.Rota.Destino,
            reserva.Viagem.DataHoraPartida,
            reserva.Viagem.PrecoBase);
    }
}