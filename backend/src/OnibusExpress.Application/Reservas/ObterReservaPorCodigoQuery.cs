using MediatR;

namespace OnibusExpress.Application.Reservas;

public record ObterReservaPorCodigoQuery(string Codigo) : IRequest<ReservaDetalhesDto?>;