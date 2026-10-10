using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Formularios;

namespace Hotel.UI.Vistas;

/// <summary>Listado de habitaciones con filtros y acciones de estado.</summary>
/// <remarks>RF-03..RF-07; CU-02. Registrar, editar y eliminar son del Administrador.</remarks>
public class VistaHabitaciones : VistaListadoBase
{
    private const string Todos = "Todos";

    private sealed record Fila(
        int Id,
        [property: DisplayName("Número")] string Numero,
        [property: DisplayName("Piso")] int Piso,
        [property: DisplayName("Tipo")] string Tipo,
        [property: DisplayName("Capacidad")] int Capacidad,
        [property: DisplayName("Tarifa por noche")] string Tarifa,
        [property: DisplayName("Estado")] string Estado,
        [property: DisplayName("Descripción")] string Descripcion) : IFilaConId;

    private readonly ServiciosHotel? _s;
    private readonly ComboBox _cmbTipo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly ComboBox _cmbEstado = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
    private readonly ComboBox _cmbPiso = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 80 };
    private readonly CheckBox _chkInactivas = new() { Text = "Incluir inactivas", AutoSize = true, Margin = new Padding(14, 7, 0, 0) };
    private readonly BotonModerno _btnMantenimiento = new() { Text = "Mantenimiento", Icono = "", Variante = VarianteBoton.Secundario, Width = 170, Height = 40 };
    private readonly BotonModerno _btnLiberar = new() { Text = "Liberar", Icono = "", Variante = VarianteBoton.Secundario, Width = 120, Height = 40 };

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaHabitaciones() { }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaHabitaciones(ServiciosHotel servicios)
    {
        _s = servicios;
        AyudaBusqueda = "Buscar por número o descripción...";
        var admin = servicios.Autenticacion.EsAdministrador;
        PermiteNuevo = PermiteEditar = PermiteEliminar = admin;
        PermiteDetalle = false;
        TextoEliminar = "Eliminar";

        _cmbTipo.Items.Add(Todos);
        _cmbTipo.Items.AddRange(Enum.GetNames<TipoHabitacion>());
        _cmbEstado.Items.Add(Todos);
        _cmbEstado.Items.AddRange(Enum.GetNames<EstadoHabitacion>());
        _cmbEstado.Items.Add(ReporteServicio.ReservadaHoy);
        _cmbPiso.Items.Add(Todos);
        for (var piso = 1; piso <= 4; piso++) _cmbPiso.Items.Add(piso.ToString());
        foreach (var combo in new[] { _cmbTipo, _cmbEstado, _cmbPiso }) combo.SelectedIndex = 0;

        AgregarFiltro("Tipo", _cmbTipo);
        AgregarFiltro("Estado", _cmbEstado);
        AgregarFiltro("Piso", _cmbPiso);
        PanelFiltros.Controls.Add(_chkInactivas);

        _btnMantenimiento.Click += (_, _) => AlternarMantenimiento();
        _btnLiberar.Click += (_, _) => Liberar();
        _btnMantenimiento.Visible = admin;
        AgregarAccion(_btnMantenimiento);
        AgregarAccion(_btnLiberar);

        void Recargar(object? s, EventArgs e) => CargarDatos();
        _cmbTipo.SelectedIndexChanged += Recargar;
        _cmbEstado.SelectedIndexChanged += Recargar;
        _cmbPiso.SelectedIndexChanged += Recargar;
        _chkInactivas.CheckedChanged += Recargar;
    }

    /// <inheritdoc />
    public override string TituloVista => "Habitaciones";

    /// <inheritdoc />
    protected override void CargarDatos()
    {
        if (_s is null) return;
        var reservadasHoy = _s.Habitaciones.ReservadasHoy();
        var estadoTexto = _cmbEstado.SelectedItem as string ?? Todos;
        var soloReservadas = estadoTexto == ReporteServicio.ReservadaHoy;

        var filtro = new FiltroHabitacion(
            Tipo: _cmbTipo.SelectedIndex > 0 ? Enum.Parse<TipoHabitacion>((string)_cmbTipo.SelectedItem!) : null,
            Estado: soloReservadas ? EstadoHabitacion.Disponible
                : _cmbEstado.SelectedIndex > 0 ? Enum.Parse<EstadoHabitacion>(estadoTexto) : null,
            Piso: _cmbPiso.SelectedIndex > 0 ? int.Parse((string)_cmbPiso.SelectedItem!) : null,
            Texto: TextoBusqueda,
            IncluirInactivas: _chkInactivas.Checked);

        var filas = _s.Habitaciones.Buscar(filtro)
            .Where(h => !soloReservadas || reservadasHoy.Contains(h.Id))
            .Select(h => new Fila(h.Id, h.Numero, h.Piso, h.Tipo.ToString(), h.Capacidad,
                Constantes.FormatearMoneda(h.TarifaNoche), EstadoVisible(h, reservadasHoy), h.Descripcion))
            .ToList();
        MostrarFilas(filas);
    }

    private static string EstadoVisible(Habitacion h, ISet<int> reservadasHoy) =>
        !h.Activo ? "Inactiva"
        : h.Estado == EstadoHabitacion.Disponible && reservadasHoy.Contains(h.Id) ? ReporteServicio.ReservadaHoy
        : h.Estado.ToString();

    /// <inheritdoc />
    protected override void AlCambiarSeleccion()
    {
        var estado = FilaSeleccionada<Fila>()?.Estado;
        var enMantenimiento = estado == nameof(EstadoHabitacion.Mantenimiento);
        var disponible = estado is nameof(EstadoHabitacion.Disponible) or "Reservada hoy";
        _btnMantenimiento.Text = enMantenimiento ? "Quitar mantenim." : "Mantenimiento";
        _btnMantenimiento.Enabled = enMantenimiento || disponible;
        _btnLiberar.Enabled = estado == nameof(EstadoHabitacion.Limpieza);
    }

    /// <inheritdoc />
    protected override void Nuevo()
    {
        if (_s is null) return;
        using var form = new FormHabitacion(_s);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Editar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var habitacion = _s.Habitaciones.ObtenerPorId(fila.Id);
        if (habitacion is null) return;
        using var form = new FormHabitacion(_s, habitacion);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Eliminar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var texto = $"¿Eliminar la habitación {fila.Numero}?\nSi tiene reservas registradas se desactivará en lugar de eliminarse.";
        if (!Mensajes.Confirmar(FindForm(), texto, "Eliminar habitación", peligro: true, textoAceptar: "Eliminar")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Habitaciones.Eliminar(fila.Id));
        CargarDatos();
    }

    private void AlternarMantenimiento()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var destino = fila.Estado == nameof(EstadoHabitacion.Mantenimiento)
            ? EstadoHabitacion.Disponible : EstadoHabitacion.Mantenimiento;
        var accion = destino == EstadoHabitacion.Mantenimiento ? "poner en mantenimiento" : "volver a poner disponible";
        if (!Mensajes.Confirmar(FindForm(), $"¿Desea {accion} la habitación {fila.Numero}?", "Cambiar estado")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Habitaciones.CambiarEstado(fila.Id, destino), avisarExito: false);
        CargarDatos();
    }

    private void Liberar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        Mensajes.MostrarResultado(FindForm(), _s.Habitaciones.Liberar(fila.Id), avisarExito: false);
        CargarDatos();
    }
}
