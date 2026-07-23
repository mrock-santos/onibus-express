namespace OnibusExpress.Domain.Entities;

public static class CpfValidator
{
    public static string RemoverFormatacao(string cpf)
    {
        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    public static bool EhValido(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = RemoverFormatacao(cpf);

        if (cpf.Length != 11)
            return false;

        // Rejeita sequências repetidas (ex: 111.111.111-11), que passam
        // matematicamente no cálculo do dígito verificador mas são inválidas
        if (cpf.Distinct().Count() == 1)
            return false;

        var numeros = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        var primeiroDigito = CalcularDigitoVerificador(numeros, 9);
        if (primeiroDigito != numeros[9])
            return false;

        var segundoDigito = CalcularDigitoVerificador(numeros, 10);
        if (segundoDigito != numeros[10])
            return false;

        return true;
    }

    private static int CalcularDigitoVerificador(int[] numeros, int quantidadeDigitos)
    {
        var multiplicador = quantidadeDigitos + 1;
        var soma = 0;

        for (var i = 0; i < quantidadeDigitos; i++)
        {
            soma += numeros[i] * multiplicador;
            multiplicador--;
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}