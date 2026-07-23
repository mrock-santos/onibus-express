namespace OnibusExpress.Domain.Entities;

public class Viagem
{
    public Guid Id { get; private set; }
    public Guid RotaId { get; private set; }
    public Rota Rota { get; private set; } = null!;
    public DateTime DataHoraPartida { get; private set; }
    public decimal PrecoBase { get; private set; }
    public int TotalAssentos { get; private set; }

    private readonly List<Reserva> _reservas = new();
    public IReadOnlyCollection<Reserva> Reservas => _reservas.AsReadOnly();

    private Viagem() { }

    public Viagem(Guid rotaId, DateTime dataHoraPartida, decimal precoBase, int totalAssentos)
    {
        if (dataHoraPartida <= DateTime.UtcNow)
            throw new ArgumentException("Data/hora de partida deve ser no futuro.");

        if (precoBase <= 0)
            throw new ArgumentException("Preço base deve ser maior que zero.");

        if (totalAssentos <= 0)
            throw new ArgumentException("Total de assentos deve ser maior que zero.");

        Id = Guid.NewGuid();
        RotaId = rotaId;
        DataHoraPartida = dataHoraPartida;
        PrecoBase = precoBase;
        TotalAssentos = totalAssentos;
    }

    // Método de domínio: calcula assentos disponíveis com base nas reservas ativas
    public int AssentosDisponiveis()
    {
        var ocupados = _reservas.Count(r => r.Status != StatusReserva.Cancelada);
        return TotalAssentos - ocupados;
    }

    public bool AssentoEstaOcupado(int numeroAssento)
    {
        return _reservas.Any(r =>
            r.NumeroAssento == numeroAssento &&
            r.Status != StatusReserva.Cancelada);
    }

    public bool JaFoiRealizada()
    {
        return DataHoraPartida <= DateTime.UtcNow;
    }
}