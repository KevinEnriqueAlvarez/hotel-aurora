using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Servicios;
using Hotel.UI.Formularios;

namespace Hotel.UI.Vistas;

/// <summary>Listado de huéspedes con búsqueda por nombre o documento.</summary>
/// <remarks>RF-08..RF-12; CU-03</remarks>
public class VistaHuespedes : VistaListadoBase
{
    private sealed record Fila(
        int Id,
        [property: DisplayName("Nombre")] string Nombre,
        [property: DisplayName("Documento")] string Documento,
        [property: DisplayName("Teléfono")] string Telefono,
        [property: DisplayName("Correo")] string Correo,
        [property: DisplayName("NIT")] string Nit,
        [property: DisplayName("Nacionalidad")] string Nacionalidad,
        [property: DisplayName("Estado")] string Estado) : IFilaConId;

    private readonly ServiciosHotel? _s;
    private readonly CheckBox _chkInactivos = new() { Text = "Incluir inactivos", AutoSize = true, Margin = new Padding(14, 7, 0, 0) };

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaHuespedes() { }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaHuespedes(ServiciosHotel servicios)
    {
        _s = servicios;
        AyudaBusqueda = "Buscar por nombre o documento...";
        PanelFiltros.Controls.Add(_chkInactivos);
        _chkInactivos.CheckedChanged += (_, _) => CargarDatos();
    }

    /// <inheritdoc />
    public override string TituloVista => "Huéspedes";

    /// <inheritdoc />
    protected override void CargarDatos()
    {
        if (_s is null) return;
        MostrarFilas(_s.Huespedes.Buscar(TextoBusqueda, _chkInactivos.Checked)
            .Select(h => new Fila(h.Id, h.NombreCompleto, $"{h.TipoDocumento}: {h.NumeroDocumento}", h.Telefono,
                h.Correo, h.Nit, h.Nacionalidad, h.Activo ? "Activo" : "Inactivo"))
            .ToList());
    }

    /// <inheritdoc />
    protected override void Nuevo()
    {
        if (_s is null) return;
        using var form = new FormHuesped(_s);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Editar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var huesped = _s.Huespedes.ObtenerPorId(fila.Id);
        if (huesped is null) return;
        using var form = new FormHuesped(_s, huesped);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Eliminar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var texto = $"¿Eliminar a {fila.Nombre}?\nSi tiene reservas registradas se desactivará en lugar de eliminarse.";
        if (!Mensajes.Confirmar(FindForm(), texto, "Eliminar huésped", peligro: true, textoAceptar: "Eliminar")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Huespedes.Eliminar(fila.Id));
        CargarDatos();
    }

    /// <inheritdoc />
    protected override void VerDetalle()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var huesped = _s.Huespedes.ObtenerPorId(fila.Id);
        if (huesped is null) return;
        using var form = new FormHuespedDetalle(_s, huesped);
        form.ShowDialog(FindForm());
    }
}
