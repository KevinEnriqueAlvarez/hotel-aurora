using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Valida DPI, pasaporte, NIT, correo, teléfono, nombres y contraseña (sección 8).</summary>
public class ValidadorTests
{
    [Theory]
    [InlineData("1234567890123", true)]
    [InlineData("123456789012", false)]
    [InlineData("12345678901234", false)]
    [InlineData("12345678901A3", false)]
    [InlineData("", false)]
    public void EsDpiValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsDpiValido(valor));

    [Theory]
    [InlineData("AB123456", true)]
    [InlineData("A1234", false)]
    [InlineData("AB-123456", false)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTU", false)]
    public void EsPasaporteValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsPasaporteValido(valor));

    [Theory]
    [InlineData("CF", true)]
    [InlineData("cf", true)]
    [InlineData("10000003", true)]      // verificador numérico
    [InlineData("1000000-3", true)]     // con guion
    [InlineData("6K", true)]            // verificador K
    [InlineData("10000004", false)]
    [InlineData("6", false)]
    [InlineData("ABC", false)]
    [InlineData("", false)]
    public void EsNitValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsNitValido(valor));

    [Fact]
    public void CalcularVerificadorNit_SumaMultiploDe11MenosUno_DevuelveK() =>
        Assert.Equal('K', Validador.CalcularVerificadorNit("6"));

    [Fact]
    public void CalcularVerificadorNit_SumaMultiploDe11_DevuelveCero() =>
        // "14": 1*3 + 4*2 = 11 -> resto 0 -> r = (11 - 0) % 11 = 0
        Assert.Equal('0', Validador.CalcularVerificadorNit("14"));

    [Theory]
    [InlineData("juan@correo.com", true)]
    [InlineData("juan.perez@hotel.gt", true)]
    [InlineData("juan@", false)]
    [InlineData("sin-arroba", false)]
    [InlineData("", false)]
    public void EsCorreoValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsCorreoValido(valor));

    [Theory]
    [InlineData("55512345", true)]
    [InlineData("+50255512345", true)]
    [InlineData("1234567", false)]
    [InlineData("1234567890123456", false)]
    [InlineData("5551-2345", false)]
    public void EsTelefonoValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsTelefonoValido(valor));

    [Theory]
    [InlineData("María José", true)]
    [InlineData("Nuñez", true)]
    [InlineData("A", false)]
    [InlineData("Juan2", false)]
    [InlineData("  Juan", false)]
    public void EsNombreValido_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsNombreValido(valor));

    [Theory]
    [InlineData("Admin123*", true)]
    [InlineData("admin123*", false)]
    [InlineData("ADMINISTRADOR", false)]
    [InlineData("Ab1", false)]
    public void EsContrasenaValida_Casos_DevuelveEsperado(string valor, bool esperado) =>
        Assert.Equal(esperado, Validador.EsContrasenaValida(valor));

    [Fact]
    public void EsDocumentoValido_SegunTipo_UsaLaReglaCorrecta()
    {
        Assert.True(Validador.EsDocumentoValido(TipoDocumento.Dpi, "1234567890123"));
        Assert.False(Validador.EsDocumentoValido(TipoDocumento.Dpi, "AB123456"));
        Assert.True(Validador.EsDocumentoValido(TipoDocumento.Pasaporte, "AB123456"));
    }
}
