using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Pagos, anulación, historial y resumen de facturación.</summary>
/// <remarks>RF-23, RF-24, RF-25, RN-12, RN-13</remarks>
public class FacturacionServicio : ServicioBase
{
    private readonly IRepositorio<Factura> _facturas;
    private readonly AutenticacionServicio _auth;
    private readonly IReloj _reloj;

    /// <summary>Crea el servicio.</summary>
    public FacturacionServicio(IRepositorio<Factura> facturas, AutenticacionServicio auth, IReloj reloj)
    {
        _facturas = facturas;
        _auth = auth;
        _reloj = reloj;
    }

    /// <summary>Busca una factura por Id.</summary>
    public Factura? ObtenerPorId(int id) => _facturas.ObtenerPorId(id);

    /// <summary>Registra un pago (parcial o total) en la factura.</summary>
    /// <remarks>RF-23, RN-12, RN-13</remarks>
    public Resultado RegistrarPago(int facturaId, Pago pago) => Ejecutar(() =>
    {
        var factura = _facturas.ObtenerPorId(facturaId) ?? throw new ReglaNegocioException("La factura no existe.");
        pago.Fecha = _reloj.Ahora;
        factura.RegistrarPago(pago);
        _facturas.Actualizar(factura);
    }, "Pago registrado.");

    /// <summary>Anula la factura con motivo; solo Administrador.</summary>
    /// <remarks>RF-24, RN-13</remarks>
    public Resultado Anular(int facturaId, string motivo) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("anular facturas");
        var factura = _facturas.ObtenerPorId(facturaId) ?? throw new ReglaNegocioException("La factura no existe.");
        factura.Anular(motivo);
        _facturas.Actualizar(factura);
    }, "Factura anulada.");

    /// <summary>Historial filtrado por fecha de emisión, estado y texto (número, cliente o NIT), más reciente primero.</summary>
    /// <remarks>RF-25</remarks>
    public IReadOnlyList<Factura> Buscar(FiltroFactura? filtro = null)
    {
        filtro ??= new FiltroFactura();
        var texto = filtro.Texto?.Trim();
        return _facturas.Buscar(f =>
                (filtro.Desde is null || f.FechaEmision.Date >= filtro.Desde.Value.Date)
                && (filtro.Hasta is null || f.FechaEmision.Date <= filtro.Hasta.Value.Date)
                && (filtro.Estado is null || f.Estado == filtro.Estado)
                && (string.IsNullOrEmpty(texto)
                    || f.Numero.Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || f.NombreCliente.Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || f.NitCliente.Contains(texto, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(f => f.FechaEmision).ThenByDescending(f => f.Id)
            .ToList();
    }

    /// <summary>Totales del periodo; las facturas anuladas no suman.</summary>
    /// <remarks>RF-25</remarks>
    public ResumenFacturacion ResumenPeriodo(DateTime? desde, DateTime? hasta)
    {
        var facturas = Buscar(new FiltroFactura(desde, hasta));
        var vigentes = facturas.Where(f => f.Estado != EstadoFactura.Anulada).ToList();
        return new ResumenFacturacion(
            Cantidad: vigentes.Count,
            CantidadAnuladas: facturas.Count - vigentes.Count,
            TotalFacturado: vigentes.Sum(f => f.Total),
            TotalCobrado: vigentes.Sum(f => f.TotalPagado),
            SaldoPendiente: vigentes.Sum(f => f.Saldo));
    }
}
