using MediatR;

namespace OnibusExpress.Application.Viagens;

public record ObterViagemPorIdQuery(Guid Id) : IRequest<ViagemDetalhesDto?>;