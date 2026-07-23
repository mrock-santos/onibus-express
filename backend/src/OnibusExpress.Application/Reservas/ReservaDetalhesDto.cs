namespace OnibusExpress.Application.Reservas;

public record ReservaDetalhesDto(
    Guid Id,
    string CodigoReserva,
    string Status,
    int NumeroAssento,
    DateTime CriadaEm,
    string NomePassageiro,
    string EmailPassageiro,
    Guid ViagemId,
    string Origem,
    string Destino,
    DateTime DataHoraPartida,
    decimal PrecoBase);