using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>Estado de cuenta del huésped y check-out con descuento opcional.</summary>
/// <remarks>RF-21, RF-22, RN-09, RN-11; CU-08</remarks>
public partial class FormCheckOut : FormularioBase
{
    private sealed record FilaLinea(
        [property: DisplayName("Concepto")] string Concepto,
        [property: DisplayName("Cant.")] int Cantidad,
        [property: DisplayName("Precio unitario")] string Precio,
        [property: DisplayName("Subtotal")] string Subtotal);

    private readonly ServiciosHotel? _s;
    private readonly Reserva? _reserva;

    /// <summary>Factura generada tras el check-out (null si se canceló).</summary>
    public Factura? FacturaGenerada { get; private set; }

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormCheckOut()
    {
        InitializeComponent();
        Estilizador.AplicarTabla(tablaLineas);
    }

    /// <summary>Crea el diálogo para una reserva En curso.</summary>
    public FormCheckOut(ServiciosHotel servicios, Reserva reserva) : this()
    {
        _s = servicios;
        _reserva = reserva;
        Text = $"Check-out — {reserva.Codigo}";
        TextoGuardar = "Realizar check-out";
        numDescuento.Maximum = servicios.Configuracion.DescuentoMaximo;
        var huesped = servicios.Huespedes.ObtenerPorId(reserva.HuespedId);
        var hab = servicios.Habitaciones.ObtenerPorId(reserva.HabitacionId);
        lblInfo.Text = $"Huésped: {huesped?.NombreCompleto}   ·   NIT: {huesped?.Nit}\n" +
                       $"Habitación {hab?.Numero} ({hab?.Tipo})   ·   Check-in: {reserva.FechaCheckIn:dd/MM/yyyy HH:mm}   ·   Salida: {servicios.Reloj.Ahora:dd/MM/yyyy HH:mm}";
        lblDescuento.Text = $"Descuento (0 – {servicios.Configuracion.DescuentoMaximo:0}%)";
        Recalcular();
    }

    private void Recalcular()
    {
        if (_s is null || _reserva is null) return;
        var r = _s.Estadias.PrevisualizarFactura(_reserva.Id, numDescuento.Value);
        if (!r.Exito || r.Valor is null)
        {
            lblTotales.Text = r.Mensaje;
            return;
        }
        var f = r.Valor;
        tablaLineas.DataSource = f.Lineas.Select(l => new FilaLinea(
            l.Concepto, l.Cantidad, Constantes.FormatearMoneda(l.PrecioUnitario), Constantes.FormatearMoneda(l.Subtotal))).ToList();
        lblTotales.Text =
            $"Subtotal: {Constantes.FormatearMoneda(f.Subtotal)}\n" +
            $"Descuento ({f.PorcentajeDescuento:0.##}%): − {Constantes.FormatearMoneda(f.Descuento)}\n" +
            $"IVA ({_s.Configuracion.PorcentajeIva:0}%): {Constantes.FormatearMoneda(f.Iva)}\n" +
            $"Impuesto de turismo ({_s.Configuracion.PorcentajeTurismo:0}%): {Constantes.FormatearMoneda(f.ImpuestoTurismo)}\n" +
            $"TOTAL: {Constantes.FormatearMoneda(f.Total)}";
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null || _reserva is null) return false;
        if (!Mensajes.Confirmar(this, "¿Confirmar el check-out? Se generará la factura y la habitación pasará a limpieza.",
                "Confirmar check-out", textoAceptar: "Check-out")) return false;
        var r = _s.Estadias.CheckOut(_reserva.Id, numDescuento.Value);
        if (!Procesar(r)) return false;
        FacturaGenerada = r.Valor;
        return true;
    }

    private void NumDescuento_ValueChanged(object? sender, EventArgs e) => Recalcular();
}
