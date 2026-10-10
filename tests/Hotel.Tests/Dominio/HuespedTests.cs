using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Validación de <see cref="Huesped"/> (RF-08): documento, mayoría de edad y NIT.</summary>
public class HuespedTests
{
    private static readonly DateTime Hoy = new(2026, 3, 10);

    private static Huesped Valido() => new()
    {
        Nombres = "María José", Apellidos = "Pérez Núñez", TipoDocumento = TipoDocumento.Dpi,
        NumeroDocumento = "2456789012345", Telefono = "+50255512345", Correo = "maria@correo.com",
        FechaNacimiento = new DateTime(1990, 5, 20), Nit = "CF"
    };

    [Fact]
    public void Validar_HuespedCorrecto_SinErrores() => Assert.Empty(Valido().Validar(Hoy));

    [Fact]
    public void Validar_Menor18_DevuelveError()
    {
        var h = Valido();
        h.FechaNacimiento = Hoy.AddYears(-18).AddDays(1);
        Assert.Single(h.Validar(Hoy));
    }

    [Fact]
    public void Validar_Exactamente18_EsValido()
    {
        var h = Valido();
        h.FechaNacimiento = Hoy.AddYears(-18);
        Assert.Empty(h.Validar(Hoy));
    }

    [Fact]
    public void Validar_NitIncorrecto_DevuelveError()
    {
        var h = Valido();
        h.Nit = "10000004";
        Assert.Single(h.Validar(Hoy));
    }

    [Fact]
    public void Validar_DpiCorto_DevuelveError()
    {
        var h = Valido();
        h.NumeroDocumento = "123";
        Assert.Single(h.Validar(Hoy));
    }

    [Fact]
    public void Validar_PasaporteExtranjero_EsValido()
    {
        var h = Valido();
        h.TipoDocumento = TipoDocumento.Pasaporte;
        h.NumeroDocumento = "X1234567";
        Assert.Empty(h.Validar(Hoy));
    }

    [Fact]
    public void NombreCompleto_UneNombresYApellidos() =>
        Assert.Equal("María José Pérez Núñez", Valido().NombreCompleto);
}
