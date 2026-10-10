using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Registra un cargo adicional del catálogo en una estadía en curso.</summary>
/// <remarks>RF-20, RN-08; CU-07</remarks>
public partial class FormCargo : FormularioBase
{
    private readonly ServiciosHotel? _s;
    private readonly Reserva? _reserva;
    private IReadOnlyList<ServicioAdicional> _servicios = [];

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormCargo() => InitializeComponent();

    /// <summary>Crea el diálogo para la reserva indicada.</summary>
    public FormCargo(ServiciosHotel servicios, Reserva reserva) : this()
    {
        _s = servicios;
        _reserva = reserva;
        Text = $"Agregar cargo — {reserva.Codigo}";
        TextoGuardar = "Agregar cargo";
        _servicios = servicios.Estadias.ObtenerServicios();
        cmbServicio.DataSource = _servicios
            .Select(s => $"{s.Nombre} ({s.Categoria}) — {Constantes.FormatearMoneda(s.Precio)}").ToList();
        Validar(numCantidad, () => numCantidad.Value < 1 ? "La cantidad debe ser mayor que cero." : null);
        ActualizarTotal();
    }

    private void ActualizarTotal()
    {
        var servicio = cmbServicio.SelectedIndex >= 0 && cmbServicio.SelectedIndex < _servicios.Count
            ? _servicios[cmbServicio.SelectedIndex] : null;
        lblTotal.Text = servicio is null ? "" : $"Total del cargo: {Constantes.FormatearMoneda(servicio.Precio * numCantidad.Value)}";
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null || _reserva is null || cmbServicio.SelectedIndex < 0) return false;
        return Procesar(_s.Estadias.AgregarCargo(_reserva.Id, _servicios[cmbServicio.SelectedIndex].Id, (int)numCantidad.Value));
    }

    private void Campo_Cambiado(object? sender, EventArgs e) => ActualizarTotal();
}
