using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Factura de una estadía con sus pagos; nunca se elimina, solo se anula.</summary>
/// <remarks>RF-22, RF-23, RF-24, RN-11, RN-12, RN-13</remarks>
public class Factura : EntidadBase
{
    private List<LineaFactura> _lineas = [];
    private List<Pago> _pagos = [];

    /// <summary>Número correlativo <c>FAC-000001</c> (RN-15).</summary>
    public string Numero { get; set; } = "";
    /// <summary>Reserva facturada.</summary>
    [JsonInclude] public int ReservaId { get; private set; }
    /// <summary>Huésped facturado.</summary>
    [JsonInclude] public int HuespedId { get; private set; }
    /// <summary>NIT del cliente.</summary>
    [JsonInclude] public string NitCliente { get; private set; } = Constantes.NitConsumidorFinal;
    /// <summary>Nombre del cliente.</summary>
    [JsonInclude] public string NombreCliente { get; private set; } = "";
    /// <summary>Fecha de emisión.</summary>
    [JsonInclude] public DateTime FechaEmision { get; private set; }
    /// <summary>Porcentaje de descuento aplicado.</summary>
    [JsonInclude] public decimal PorcentajeDescuento { get; private set; }
    /// <summary>Alojamiento + cargos.</summary>
    [JsonInclude] public decimal Subtotal { get; private set; }
    /// <summary>Monto del descuento.</summary>
    [JsonInclude] public decimal Descuento { get; private set; }
    /// <summary>IVA sobre la base.</summary>
    [JsonInclude] public decimal Iva { get; private set; }
    /// <summary>Impuesto de turismo sobre el alojamiento.</summary>
    [JsonInclude] public decimal ImpuestoTurismo { get; private set; }
    /// <summary>Total a pagar.</summary>
    [JsonInclude] public decimal Total { get; private set; }
    /// <summary>Estado actual.</summary>
    [JsonInclude] public EstadoFactura Estado { get; private set; } = EstadoFactura.Emitida;
    /// <summary>Motivo de la anulación.</summary>
    [JsonInclude] public string? MotivoAnulacion { get; private set; }

    /// <summary>Líneas de detalle.</summary>
    [JsonInclude]
    public IReadOnlyList<LineaFactura> Lineas
    {
        get => _lineas;
        private set => _lineas = value?.ToList() ?? [];
    }

    /// <summary>Pagos registrados; se agregan con <see cref="RegistrarPago"/>.</summary>
    [JsonInclude]
    public IReadOnlyList<Pago> Pagos
    {
        get => _pagos;
        private set => _pagos = value?.ToList() ?? [];
    }

    /// <summary>Suma de los pagos.</summary>
    [JsonIgnore] public decimal TotalPagado => _pagos.Sum(p => p.Monto);
    /// <summary>Total − pagado (cero si está anulada).</summary>
    [JsonIgnore] public decimal Saldo => Estado == EstadoFactura.Anulada ? 0 : Total - TotalPagado;

    /// <summary>Genera la factura de una estadía aplicando RN-11.</summary>
    /// <remarks>RF-22, RN-09, RN-11</remarks>
    /// <exception cref="ReglaNegocioException">Descuento fuera de rango.</exception>
    public static Factura Generar(Reserva reserva, Habitacion habitacion, Huesped huesped,
        ConfiguracionHotel configuracion, decimal porcentajeDescuento, DateTime fecha)
    {
        if (porcentajeDescuento < 0 || porcentajeDescuento > configuracion.DescuentoMaximo)
            throw new ReglaNegocioException($"El descuento debe estar entre 0 % y {configuracion.DescuentoMaximo} %.");

        var noches = reserva.NochesCobradas(fecha);
        var lineas = new List<LineaFactura>
        {
            new() { Concepto = $"Alojamiento hab. {habitacion.Numero} ({habitacion.Tipo})", Cantidad = noches, PrecioUnitario = reserva.TarifaPactada }
        };
        lineas.AddRange(reserva.Cargos.Select(c => new LineaFactura
            { Concepto = c.Descripcion, Cantidad = c.Cantidad, PrecioUnitario = c.PrecioUnitario }));

        var alojamiento = lineas[0].Subtotal;
        var factor = porcentajeDescuento / 100m;
        var subtotal = lineas.Sum(l => l.Subtotal);
        var descuento = Constantes.Redondear(subtotal * factor);
        var baseImponible = subtotal - descuento;
        var iva = Constantes.Redondear(baseImponible * configuracion.PorcentajeIva / 100m);
        var turismo = Constantes.Redondear(alojamiento * (1 - factor) * configuracion.PorcentajeTurismo / 100m);

        return new Factura
        {
            ReservaId = reserva.Id,
            HuespedId = huesped.Id,
            NitCliente = Validador.Normalizar(huesped.Nit),
            NombreCliente = huesped.NombreCompleto,
            FechaEmision = fecha,
            FechaCreacion = fecha,
            _lineas = lineas,
            PorcentajeDescuento = porcentajeDescuento,
            Subtotal = subtotal,
            Descuento = descuento,
            Iva = iva,
            ImpuestoTurismo = turismo,
            Total = baseImponible + iva + turismo
        };
    }

    /// <summary>Registra un pago parcial o total; con saldo 0 la factura pasa a Pagada.</summary>
    /// <remarks>RF-23, RN-12, RN-13</remarks>
    /// <exception cref="ReglaNegocioException">Factura anulada o pagada, pago inválido o monto mayor al saldo.</exception>
    public void RegistrarPago(Pago pago)
    {
        if (Estado == EstadoFactura.Anulada) throw new ReglaNegocioException("Una factura anulada no admite pagos.");
        if (Estado == EstadoFactura.Pagada) throw new ReglaNegocioException("La factura ya está pagada.");
        var errores = pago.Validar().ToList();
        if (errores.Count > 0) throw new ReglaNegocioException(errores[0]);
        if (pago.Monto > Saldo)
            throw new ReglaNegocioException(
                $"El monto ({Constantes.FormatearMoneda(pago.Monto)}) excede el saldo ({Constantes.FormatearMoneda(Saldo)}).");
        _pagos.Add(pago);
        if (Saldo == 0) Estado = EstadoFactura.Pagada;
    }

    /// <summary>Anula la factura con motivo obligatorio; la factura se conserva.</summary>
    /// <remarks>RF-24, RN-13</remarks>
    public void Anular(string motivo)
    {
        if (Estado == EstadoFactura.Anulada) throw new ReglaNegocioException("La factura ya está anulada.");
        if (string.IsNullOrWhiteSpace(motivo)) throw new ReglaNegocioException("El motivo de anulación es obligatorio.");
        Estado = EstadoFactura.Anulada;
        MotivoAnulacion = motivo.Trim();
    }

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        if (ReservaId <= 0) yield return "La factura debe referirse a una reserva.";
        if (_lineas.Count == 0) yield return "La factura debe tener al menos una línea.";
    }
}
