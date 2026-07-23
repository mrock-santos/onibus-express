using MediatR;

namespace OnibusExpress.Application.Rotas;

// Uma Query é só uma classe simples que representa "eu quero esses dados".
// IRequest<T> vem do MediatR: T é o tipo que a resposta vai ter.
public record ListarRotasQuery : IRequest<List<RotaDto>>;