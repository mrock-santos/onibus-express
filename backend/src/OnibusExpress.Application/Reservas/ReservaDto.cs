namespace OnibusExpress.Application.Reservas;

public record ReservaDto(
    Guid Id,
    string CodigoReserva,
    Guid ViagemId,
    string NomePassageiro,
    int NumeroAssento,
    string Status,
    DateTime CriadaEm);