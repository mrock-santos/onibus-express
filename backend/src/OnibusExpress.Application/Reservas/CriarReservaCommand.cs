using MediatR;

namespace OnibusExpress.Application.Reservas;

public record CriarReservaCommand(CriarReservaDto Dados) : IRequest<ReservaDto>;