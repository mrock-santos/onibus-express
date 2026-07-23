namespace OnibusExpress.Domain.Entities;

public class Passageiro
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascimento { get; private set; }

    private Passageiro() { }

    public Passageiro(string nome, string cpf, string email, DateTime dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        if (!CpfValidator.EhValido(cpf))
            throw new ArgumentException("CPF inválido.", nameof(cpf));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("E-mail inválido.", nameof(email));

        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = CpfValidator.RemoverFormatacao(cpf);
        Email = email;
        DataNascimento = dataNascimento;
    }
}