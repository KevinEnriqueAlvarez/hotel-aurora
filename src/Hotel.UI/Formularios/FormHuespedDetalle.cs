using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>Detalle de un huésped con la pestaña «Historial de estadías».</summary>
/// <remarks>RF-12; CU-11</remarks>
public partial class FormHuespedDetalle : FormularioBase
{
    private sealed record FilaEstadia(
        [property: DisplayName("Reserva")] string Reserva,
        [property: DisplayName("Entrada")] string Entrada,
        [property: DisplayName("Salida")] string Salida,
        [property: DisplayName("Habitación")] string Habitacion,
        [property: DisplayName("Estado")] string Estado,
        [property: DisplayName("Total facturado")] string Total);

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormHuespedDetalle()
    {
        InitializeComponent();
        TextoGuardar = "Cerrar";
        MostrarCancelar = false;
        selector.Opciones = ["Datos personales", "Historial de estadías"];
        Estilizador.AplicarTabla(tablaHistorial);
    }

    /// <summary>Muestra al huésped con su historial.</summary>
    public FormHuespedDetalle(ServiciosHotel servicios, Huesped huesped) : this()
    {
        Text = $"Huésped — {huesped.NombreCompleto}";
        lblNombre.Text = huesped.NombreCompleto;
        insignia.Estado = huesped.Activo ? "Activo" : "Inactivo";
        CargarDatos(huesped);

        var historial = servicios.Huespedes.HistorialEstadias(huesped.Id);
        tablaHistorial.DataSource = historial.Select(h => new FilaEstadia(
            h.CodigoReserva, h.Entrada.ToString(Constantes.FormatoFecha), h.Salida.ToString(Constantes.FormatoFecha),
            h.Habitacion, h.Estado.ToString(), Constantes.FormatearMoneda(h.TotalFacturado))).ToList();
        lblResumen.Text = historial.Count == 0
            ? "Sin estadías registradas."
            : $"{historial.Count} estadía(s) · Total facturado {Constantes.FormatearMoneda(historial.Sum(h => h.TotalFacturado))}";
    }

    private void CargarDatos(Huesped h)
    {
        (string, string)[] filas =
        [
            ("Documento", $"{h.TipoDocumento}: {h.NumeroDocumento}"),
            ("NIT", h.Nit),
            ("Teléfono", h.Telefono),
            ("Correo", h.Correo),
            ("Nacionalidad", h.Nacionalidad),
            ("Nacimiento", h.FechaNacimiento.ToString(Constantes.FormatoFecha)),
            ("Registrado", h.FechaCreacion.ToString(Constantes.FormatoFecha)),
            ("Notas", string.IsNullOrWhiteSpace(h.Notas) ? "—" : h.Notas)
        ];
        tblDatos.RowCount = filas.Length;
        tblDatos.RowStyles.Clear();
        foreach (var (etiqueta, valor) in filas)
        {
            tblDatos.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tblDatos.Controls.Add(new Label
            {
                Text = etiqueta, ForeColor = Paleta.TextoSecundario, Font = Tipografia.EtiquetaPequena,
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Tag = "personalizada"
            });
            tblDatos.Controls.Add(new Label
            {
                Text = valor, ForeColor = Paleta.TextoPrincipal, Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft, Tag = "personalizada"
            });
        }
    }

    private void Selector_IndiceCambiado(object? sender, EventArgs e)
    {
        var datos = selector.IndiceSeleccionado == 0;
        tblDatos.Visible = datos;
        tablaHistorial.Visible = !datos;
        lblResumen.Visible = !datos;
    }
}
