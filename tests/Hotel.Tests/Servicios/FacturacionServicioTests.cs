using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>Pagos, anulación y resumen de <see cref="FacturacionServicio"/> (RN-12, RN-13).</summary>
public sealed class FacturacionServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    // 2 noches × 500 = 1000; IVA 120; turismo 100; total 1220
    private Factura NuevaFactura()
    {
        var reserva = _e.EstadiaEnCurso(noches: 2, tarifa: 500m);
        _e.Reloj.Ahora = _e.Reloj.Ahora.Date.AddDays(2).AddHours(10);
        return _e.S.Estadias.CheckOut(reserva.Id).Valor!;
    }

    [Fact]
    public void RegistrarPago_Parcial_DejaSaldo()
    {
        var f = NuevaFactura();
        var r = _e.S.Facturacion.RegistrarPago(f.Id, new PagoEfectivo { Monto = 500m, MontoRecibido = 500m });

        Assert.True(r.Exito, r.Mensaje);
        var guardada = _e.S.Facturacion.ObtenerPorId(f.Id)!;
        Assert.Equal(720m, guardada.Saldo);
        Assert.Equal(EstadoFactura.Emitida, guardada.Estado);
        Assert.Equal(_e.Reloj.Ahora, guardada.Pagos[0].Fecha);
    }

    [Fact]
    public void RegistrarPago_PagosQueCompletan_PasaAPagada()
    {
        var f = NuevaFactura();
        _e.S.Facturacion.RegistrarPago(f.Id, new PagoEfectivo { Monto = 500m, MontoRecibido = 500m });
        _e.S.Facturacion.RegistrarPago(f.Id,
            new PagoTransferencia { Monto = 720m, Banco = "BAC", NumeroReferencia = "R-1" });
        Assert.Equal(EstadoFactura.Pagada, _e.S.Facturacion.ObtenerPorId(f.Id)!.Estado);
    }

    [Fact]
    public void RegistrarPago_MayorAlSaldo_RechazaYNoPersiste()
    {
        var f = NuevaFactura();
        var r = _e.S.Facturacion.RegistrarPago(f.Id, new PagoEfectivo { Monto = 1220.01m, MontoRecibido = 2000m });

        Assert.False(r.Exito);
        Assert.Contains("excede el saldo", r.Mensaje);
        Assert.Empty(_e.S.Facturacion.ObtenerPorId(f.Id)!.Pagos);
    }

    [Fact]
    public void RegistrarPago_TarjetaSinAutorizacion_Rechaza()
    {
        var f = NuevaFactura();
        var r = _e.S.Facturacion.RegistrarPago(f.Id,
            new PagoTarjeta { Monto = 100m, UltimosCuatroDigitos = "1234", NumeroAutorizacion = "" });
        Assert.False(r.Exito);
    }

    [Fact]
    public void RegistrarPago_FacturaInexistente_Rechaza() =>
        Assert.False(_e.S.Facturacion.RegistrarPago(99, new PagoEfectivo { Monto = 1m, MontoRecibido = 1m }).Exito);

    [Fact]
    public void Anular_Administrador_MarcaAnuladaYBloqueaPagos()
    {
        var f = NuevaFactura();
        var r = _e.S.Facturacion.Anular(f.Id, "Datos incorrectos");

        Assert.True(r.Exito, r.Mensaje);
        Assert.Equal(EstadoFactura.Anulada, _e.S.Facturacion.ObtenerPorId(f.Id)!.Estado);
        Assert.False(_e.S.Facturacion.RegistrarPago(f.Id, new PagoEfectivo { Monto = 10m, MontoRecibido = 10m }).Exito);
    }

    [Fact]
    public void Anular_Recepcionista_RechazaPorPermiso()
    {
        var f = NuevaFactura();
        _e.IniciarComoRecepcionista();

        var r = _e.S.Facturacion.Anular(f.Id, "Intento");

        Assert.False(r.Exito);
        Assert.Contains("Administrador", r.Mensaje);
        Assert.Equal(EstadoFactura.Emitida, _e.S.Facturacion.ObtenerPorId(f.Id)!.Estado);
    }

    [Fact]
    public void Anular_SinMotivo_Rechaza()
    {
        var f = NuevaFactura();
        Assert.False(_e.S.Facturacion.Anular(f.Id, " ").Exito);
    }

    [Fact]
    public void Anular_FacturaNoSeElimina_SigueEnElHistorial()
    {
        var f = NuevaFactura();
        _e.S.Facturacion.Anular(f.Id, "Error");
        Assert.Single(_e.S.Facturacion.Buscar(new FiltroFactura(Estado: EstadoFactura.Anulada)));
    }

    [Fact]
    public void ResumenPeriodo_AnuladasNoSuman()
    {
        var a = NuevaFactura();
        var b = NuevaFactura();
        _e.S.Facturacion.RegistrarPago(a.Id, new PagoEfectivo { Monto = 200m, MontoRecibido = 200m });
        _e.S.Facturacion.Anular(b.Id, "Error");

        var resumen = _e.S.Facturacion.ResumenPeriodo(null, null);

        Assert.Equal(1, resumen.Cantidad);
        Assert.Equal(1, resumen.CantidadAnuladas);
        Assert.Equal(1220m, resumen.TotalFacturado);
        Assert.Equal(200m, resumen.TotalCobrado);
        Assert.Equal(1020m, resumen.SaldoPendiente);
    }

    [Fact]
    public void Buscar_PorFechasTextoYEstado_Filtra()
    {
        var a = NuevaFactura();                       // emitida el 12/03
        _e.Reloj.Ahora = _e.Reloj.Ahora.AddDays(10);
        var b = NuevaFactura();                       // emitida el 24/03
        _e.S.Facturacion.RegistrarPago(b.Id, new PagoEfectivo { Monto = 1220m, MontoRecibido = 1220m });

        Assert.Equal(2, _e.S.Facturacion.Buscar().Count);
        Assert.Single(_e.S.Facturacion.Buscar(new FiltroFactura(Desde: a.FechaEmision.Date.AddDays(5))));
        Assert.Single(_e.S.Facturacion.Buscar(new FiltroFactura(Estado: EstadoFactura.Pagada)));
        Assert.Equal(2, _e.S.Facturacion.Buscar(new FiltroFactura(Texto: "FAC-")).Count);
        Assert.Empty(_e.S.Facturacion.Buscar(new FiltroFactura(Texto: "inexistente")));
    }
}
