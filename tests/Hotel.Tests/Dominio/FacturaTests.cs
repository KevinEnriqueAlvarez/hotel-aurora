using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Cálculos de RN-11, pagos (RN-12) y anulación (RN-13) de <see cref="Factura"/>.</summary>
public class FacturaTests
{
    private static readonly DateTime Entrada = new(2026, 3, 10);
    private static readonly ConfiguracionHotel Config = new();

    private static Factura Generar(decimal descuento = 0m, int noches = 2, decimal cargoPrecio = 100m)
    {
        var hab = new Habitacion { Id = 1, Numero = "101", Tipo = TipoHabitacion.Doble, Capacidad = 2, TarifaNoche = 500m };
        var hue = new Huesped { Id = 1, Nombres = "Ana", Apellidos = "López", Nit = "CF" };
        var reserva = Reserva.Crear(1, 1, Entrada, Entrada.AddDays(noches), 2, 500m);
        reserva.Id = 1;
        reserva.Confirmar();
        reserva.RegistrarCheckIn(Entrada);
        reserva.AgregarCargo(new CargoAdicional { Descripcion = "Spa", Cantidad = 1, PrecioUnitario = cargoPrecio });
        return Factura.Generar(reserva, hab, hue, Config, descuento, Entrada.AddDays(noches));
    }

    [Fact]
    public void Generar_SinDescuento_CalculaSegunRN11()
    {
        // alojamiento 1000 + cargo 100 = 1100; IVA 12% = 132; turismo 10% de 1000 = 100
        var f = Generar();
        Assert.Equal(1100m, f.Subtotal);
        Assert.Equal(0m, f.Descuento);
        Assert.Equal(132m, f.Iva);
        Assert.Equal(100m, f.ImpuestoTurismo);
        Assert.Equal(1332m, f.Total);
        Assert.Equal(2, f.Lineas.Count);
    }

    [Fact]
    public void Generar_ConDescuento10_CalculaSegunRN11()
    {
        // descuento 110; base 990; IVA 118.80; turismo = 1000 * 0.9 * 10% = 90; total 1198.80
        var f = Generar(descuento: 10m);
        Assert.Equal(110m, f.Descuento);
        Assert.Equal(118.80m, f.Iva);
        Assert.Equal(90m, f.ImpuestoTurismo);
        Assert.Equal(1198.80m, f.Total);
    }

    [Fact]
    public void Generar_DescuentoSobreElMaximo_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Generar(descuento: 31m));

    [Fact]
    public void Generar_DescuentoNegativo_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Generar(descuento: -1m));

    [Fact]
    public void Generar_RedondeaAwayFromZero()
    {
        // 1 cargo de 0.05 -> subtotal 1000.05; IVA = 120.006 -> 120.01
        var f = Generar(cargoPrecio: 0.05m);
        Assert.Equal(120.01m, f.Iva);
    }

    [Fact]
    public void RegistrarPago_Parcial_DejaSaldoYEstadoEmitida()
    {
        var f = Generar();
        f.RegistrarPago(new PagoTransferencia { Monto = 332m, Banco = "BAC", NumeroReferencia = "1" });
        Assert.Equal(1000m, f.Saldo);
        Assert.Equal(EstadoFactura.Emitida, f.Estado);
    }

    [Fact]
    public void RegistrarPago_SaldoCompleto_PasaAPagada()
    {
        var f = Generar();
        f.RegistrarPago(new PagoEfectivo { Monto = 1332m, MontoRecibido = 1400m });
        Assert.Equal(0m, f.Saldo);
        Assert.Equal(EstadoFactura.Pagada, f.Estado);
    }

    [Fact]
    public void RegistrarPago_DosPagosQueCompletan_PasaAPagada()
    {
        var f = Generar();
        f.RegistrarPago(new PagoEfectivo { Monto = 1000m, MontoRecibido = 1000m });
        f.RegistrarPago(new PagoTarjeta { Monto = 332m, UltimosCuatroDigitos = "4242", NumeroAutorizacion = "A1" });
        Assert.Equal(EstadoFactura.Pagada, f.Estado);
        Assert.Equal(2, f.Pagos.Count);
    }

    [Fact]
    public void RegistrarPago_MayorAlSaldo_LanzaReglaNegocio()
    {
        var f = Generar();
        Assert.Throws<ReglaNegocioException>(() =>
            f.RegistrarPago(new PagoEfectivo { Monto = 1332.01m, MontoRecibido = 2000m }));
        Assert.Empty(f.Pagos);
    }

    [Fact]
    public void RegistrarPago_MontoCero_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() =>
            Generar().RegistrarPago(new PagoEfectivo { Monto = 0m, MontoRecibido = 0m }));

    [Fact]
    public void RegistrarPago_FacturaAnulada_LanzaReglaNegocio()
    {
        var f = Generar();
        f.Anular("Error de captura");
        Assert.Throws<ReglaNegocioException>(() =>
            f.RegistrarPago(new PagoEfectivo { Monto = 10m, MontoRecibido = 10m }));
    }

    [Fact]
    public void RegistrarPago_FacturaPagada_LanzaReglaNegocio()
    {
        var f = Generar();
        f.RegistrarPago(new PagoEfectivo { Monto = 1332m, MontoRecibido = 1332m });
        Assert.Throws<ReglaNegocioException>(() =>
            f.RegistrarPago(new PagoEfectivo { Monto = 1m, MontoRecibido = 1m }));
    }

    [Fact]
    public void Anular_SinMotivo_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Generar().Anular(""));

    [Fact]
    public void Anular_ConMotivo_MarcaAnuladaYSaldoCero()
    {
        var f = Generar();
        f.Anular("Datos incorrectos");
        Assert.Equal(EstadoFactura.Anulada, f.Estado);
        Assert.Equal("Datos incorrectos", f.MotivoAnulacion);
        Assert.Equal(0m, f.Saldo);
    }

    [Fact]
    public void Anular_DosVeces_LanzaReglaNegocio()
    {
        var f = Generar();
        f.Anular("x");
        Assert.Throws<ReglaNegocioException>(() => f.Anular("y"));
    }
}
