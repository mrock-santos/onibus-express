namespace OnibusExpress.Domain.Services;

public static class GeradorCodigoReserva
{
    private const string Letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly Random Random = new();

    // Gera um código no formato ABC-12345 (3 letras, hífen, 5 números)
    public static string Gerar()
    {
        var letras = new char[3];
        for (var i = 0; i < 3; i++)
            letras[i] = Letras[Random.Next(Letras.Length)];

        var numero = Random.Next(0, 100000).ToString("D5");

        return $"{new string(letras)}-{numero}";
    }
}