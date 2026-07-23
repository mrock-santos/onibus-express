namespace OnibusExpress.Application.Reservas;

public record CriarReservaDto(
    Guid ViagemId,
    string NomePassageiro,
    string Cpf,
    string Email,
    DateTime DataNascimento,
    int NumeroAssento);