namespace OnibusExpress.Domain.Entities;

public enum StatusReserva
{
    Confirmada,
    Cancelada
}

public class Reserva
{
    public Guid Id { get; private set; }
    public Guid ViagemId { get; private set; }
    public Viagem Viagem { get; private set; } = null!;
    public Guid PassageiroId { get; private set; }
    public Passageiro Passageiro { get; private set; } = null!;
    public int NumeroAssento { get; private set; }
    public StatusReserva Status { get; private set; }
    public string CodigoReserva { get; private set; }
    public DateTime CriadaEm { get; private set; }

    private Reserva() { }

    public Reserva(Guid viagemId, Guid passageiroId, int numeroAssento, string codigoReserva)
    {
        if (numeroAssento <= 0)
            throw new ArgumentException("Número do assento deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(codigoReserva))
            throw new ArgumentException("Código da reserva é obrigatório.");

        Id = Guid.NewGuid();
        ViagemId = viagemId;
        PassageiroId = passageiroId;
        NumeroAssento = numeroAssento;
        CodigoReserva = codigoReserva;
        Status = StatusReserva.Confirmada;
        CriadaEm = DateTime.UtcNow;
    }

    // Regra de negócio: cancelamento só permitido até 2h antes da partida
    public void Cancelar(DateTime dataHoraPartidaViagem)
    {
        if (Status == StatusReserva.Cancelada)
            throw new InvalidOperationException("Reserva já está cancelada.");

        var limiteParaCancelamento = dataHoraPartidaViagem.AddHours(-2);
        if (DateTime.UtcNow > limiteParaCancelamento)
            throw new InvalidOperationException(
                "Cancelamento não permitido a menos de 2 horas da partida.");

        Status = StatusReserva.Cancelada;
    }
}