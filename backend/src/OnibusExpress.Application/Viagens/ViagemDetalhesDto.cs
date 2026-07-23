namespace OnibusExpress.Application.Viagens;

public record ViagemDetalhesDto(
    Guid Id,
    string Origem,
    string Destino,
    DateTime DataHoraPartida,
    decimal PrecoBase,
    int TotalAssentos,
    int AssentosDisponiveis,
    List<int> AssentosOcupados);