using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Viagens;

public class BuscarViagensQueryHandler : IRequestHandler<BuscarViagensQuery, List<ViagemDto>>
{
    private readonly AppDbContext _context;

    public BuscarViagensQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ViagemDto>> Handle(BuscarViagensQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Viagens
            .Include(v => v.Rota)
            .Include(v => v.Reservas)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Origem))
            query = query.Where(v => v.Rota.Origem.ToLower() == request.Origem.ToLower());

        if (!string.IsNullOrWhiteSpace(request.Destino))
            query = query.Where(v => v.Rota.Destino.ToLower() == request.Destino.ToLower());

        if (request.Data.HasValue)
        {
            var inicioDoDia = request.Data.Value.ToDateTime(TimeOnly.MinValue);
            var fimDoDia = request.Data.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(v => v.DataHoraPartida >= inicioDoDia && v.DataHoraPartida <= fimDoDia);
        }

        var viagens = await query.ToListAsync(cancellationToken);

        return viagens
            .Select(v => new ViagemDto(
                v.Id,
                v.Rota.Origem,
                v.Rota.Destino,
                v.DataHoraPartida,
                v.PrecoBase,
                v.TotalAssentos,
                v.AssentosDisponiveis()))
            .ToList();
    }
}