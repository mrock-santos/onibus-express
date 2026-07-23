using OnibusExpress.Domain.Entities;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class CpfValidatorTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void EhValido_DeveRetornarTrue_ParaCpfValido(string cpf)
    {
        var resultado = CpfValidator.EhValido(cpf);
        Assert.True(resultado);
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")]
    [InlineData("123")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc.def.ghi-jk")]
    public void EhValido_DeveRetornarFalse_ParaCpfInvalido(string? cpf)
    {
        var resultado = CpfValidator.EhValido(cpf!);
        Assert.False(resultado);
    }

    [Fact]
    public void RemoverFormatacao_DeveRemoverPontosETracos()
    {
        var resultado = CpfValidator.RemoverFormatacao("529.982.247-25");
        Assert.Equal("52998224725", resultado);
    }
}