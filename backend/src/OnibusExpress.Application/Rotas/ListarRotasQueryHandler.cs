using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Rotas;

public class ListarRotasQueryHandler : IRequestHandler<ListarRotasQuery, List<RotaDto>>
{
    private readonly AppDbContext _context;

    public ListarRotasQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RotaDto>> Handle(ListarRotasQuery request, CancellationToken cancellationToken)
    {
        return await _context.Rotas
            .AsNoTracking()
            .Select(r => new RotaDto(r.Id, r.Origem, r.Destino, r.DuracaoEstimada))
            .ToListAsync(cancellationToken);
    }
}