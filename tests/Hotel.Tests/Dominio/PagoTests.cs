using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Polimorfismo de <see cref="Pago"/>: validación y detalle por método (RN-12).</summary>
public class PagoTests
{
    [Fact]
    public void PagoEfectivo_RecibidoMenorAlMonto_NoEsValido() =>
        Assert.NotEmpty(new PagoEfectivo { Monto = 100m, MontoRecibido = 50m }.Validar());

    [Fact]
    public void PagoEfectivo_Recibido150Monto100_CalculaCambio50()
    {
        var p = new PagoEfectivo { Monto = 100m, MontoRecibido = 150m };
        Assert.Empty(p.Validar());
        Assert.Equal(50m, p.Cambio);
        Assert.Equal("Efectivo", p.Metodo);
    }

    [Theory]
    [InlineData("123", "A1")]
    [InlineData("12345", "A1")]
    [InlineData("12a4", "A1")]
    [InlineData("1234", "")]
    public void PagoTarjeta_DatosInvalidos_NoEsValido(string digitos, string autorizacion) =>
        Assert.NotEmpty(new PagoTarjeta { Monto = 10m, UltimosCuatroDigitos = digitos, NumeroAutorizacion = autorizacion }.Validar());

    [Fact]
    public void PagoTarjeta_DatosCorrectos_EsValidoYOcultaElNumero()
    {
        var p = new PagoTarjeta { Monto = 10m, UltimosCuatroDigitos = "4242", NumeroAutorizacion = "AUT-9" };
        Assert.Empty(p.Validar());
        Assert.Contains("**** 4242", p.Detalle());
    }

    [Fact]
    public void PagoTransferencia_SinBancoNiReferencia_NoEsValido() =>
        Assert.Equal(2, new PagoTransferencia { Monto = 10m }.Validar().Count());

    [Fact]
    public void Validar_MontoCero_NoEsValido() =>
        Assert.NotEmpty(new PagoTransferencia { Monto = 0m, Banco = "BAC", NumeroReferencia = "1" }.Validar());
}
