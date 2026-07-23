using MediatR;

namespace OnibusExpress.Application.Viagens;

// Query com parâmetros: origem, destino e data são opcionais,
// permitindo buscas mais amplas se o usuário não preencher tudo.
public record BuscarViagensQuery(
    string? Origem,
    string? Destino,
    DateOnly? Data) : IRequest<List<ViagemDto>>;