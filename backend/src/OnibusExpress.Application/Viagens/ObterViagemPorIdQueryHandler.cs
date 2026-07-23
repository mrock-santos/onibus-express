using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Viagens;

public class ObterViagemPorIdQueryHandler : IRequestHandler<ObterViagemPorIdQuery, ViagemDetalhesDto?>
{
    private readonly AppDbContext _context;

    public ObterViagemPorIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ViagemDetalhesDto?> Handle(ObterViagemPorIdQuery request, CancellationToken cancellationToken)
    {
        var viagem = await _context.Viagens
            .Include(v => v.Rota)
            .Include(v => v.Reservas)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (viagem is null)
            return null;

        var assentosOcupados = viagem.Reservas
            .Where(r => r.Status != StatusReserva.Cancelada)
            .Select(r => r.NumeroAssento)
            .OrderBy(n => n)
            .ToList();

        return new ViagemDetalhesDto(
            viagem.Id,
            viagem.Rota.Origem,
            viagem.Rota.Destino,
            viagem.DataHoraPartida,
            viagem.PrecoBase,
            viagem.TotalAssentos,
            viagem.AssentosDisponiveis(),
            assentosOcupados);
    }
}