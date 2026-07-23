using MediatR;

namespace OnibusExpress.Application.Reservas;

public record CancelarReservaCommand(string Codigo) : IRequest;