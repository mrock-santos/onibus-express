namespace OnibusExpress.Domain.Entities;

public class Rota
{
    public Guid Id { get; private set; }
    public string Origem { get; private set; }
    public string Destino { get; private set; }
    public TimeSpan DuracaoEstimada { get; private set; }

    // Construtor privado sem parâmetros — exigência do Entity Framework Core
    // para conseguir "materializar" (reconstruir) o objeto vindo do banco.
    private Rota()
    {
        Origem = null!;
        Destino = null!;
    }

    public Rota(string origem, string destino, TimeSpan duracaoEstimada)
    {
        if (string.IsNullOrWhiteSpace(origem))
            throw new ArgumentException("Origem é obrigatória.", nameof(origem));

        if (string.IsNullOrWhiteSpace(destino))
            throw new ArgumentException("Destino é obrigatório.", nameof(destino));

        if (origem.Equals(destino, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Origem e destino não podem ser iguais.");

        if (duracaoEstimada <= TimeSpan.Zero)
            throw new ArgumentException("Duração estimada deve ser maior que zero.");

        Id = Guid.NewGuid();
        Origem = origem;
        Destino = destino;
        DuracaoEstimada = duracaoEstimada;
    }
}