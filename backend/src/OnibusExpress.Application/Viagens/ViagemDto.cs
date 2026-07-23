namespace OnibusExpress.Application.Viagens;

public record ViagemDto(
    Guid Id,
    string Origem,
    string Destino,
    DateTime DataHoraPartida,
    decimal PrecoBase,
    int TotalAssentos,
    int AssentosDisponiveis);