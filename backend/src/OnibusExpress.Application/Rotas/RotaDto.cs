namespace OnibusExpress.Application.Rotas;

public record RotaDto(
    Guid Id,
    string Origem,
    string Destino,
    TimeSpan DuracaoEstimada);