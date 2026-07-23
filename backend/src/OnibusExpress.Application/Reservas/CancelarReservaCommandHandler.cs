using MediatR;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Application.Reservas;

public class CancelarReservaCommandHandler : IRequestHandler<CancelarReservaCommand>
{
    private readonly AppDbContext _context;

    public CancelarReservaCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CancelarReservaCommand request, CancellationToken cancellationToken)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Viagem)
            .FirstOrDefaultAsync(r => r.CodigoReserva == request.Codigo, cancellationToken);

        if (reserva is null)
            throw new KeyNotFoundException("Reserva não encontrada.");

        // A validação da regra das 2h está encapsulada no próprio método
        // Cancelar() da entidade Reserva (Domain) — reaproveitamos a regra
        // de negócio já definida lá, em vez de duplicá-la aqui.
        reserva.Cancelar(reserva.Viagem.DataHoraPartida);

        await _context.SaveChangesAsync(cancellationToken);
    }
}