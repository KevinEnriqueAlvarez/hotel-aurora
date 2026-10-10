using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Formularios;
using Hotel.UI.Tema;

namespace Hotel.UI.Vistas;

/// <summary>Recepción: llegadas de hoy (check-in), huéspedes en casa (cargos, check-out) y habitaciones por limpiar.</summary>
/// <remarks>RF-19, RF-20, RF-21, RF-07; CU-06, CU-07, CU-08</remarks>
public partial class VistaRecepcion : VistaBase
{
    private sealed record FilaLlegada(
        int Id,
        [property: DisplayName("Código")] string Codigo,
        [property: DisplayName("Huésped")] string Huesped,
        [property: DisplayName("Habitación")] string Habitacion,
        [property: DisplayName("Salida")] string Salida);

    private sealed record FilaEnCasa(
        int Id,
        [property: DisplayName("Código")] string Codigo,
        [property: DisplayName("Huésped")] string Huesped,
        [property: DisplayName("Habitación")] string Habitacion,
        [property: DisplayName("Salida")] string Salida,
        [property: DisplayName("Cargos")] string Cargos);

    private sealed record FilaLimpieza(
        int Id,
        [property: DisplayName("Habitación")] string Numero,
        [property: DisplayName("Piso")] int Piso,
        [property: DisplayName("Tipo")] string Tipo);

    private readonly ServiciosHotel? _s;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaRecepcion()
    {
        InitializeComponent();
        Estilizador.AplicarTabla(tablaLlegadas);
        Estilizador.AplicarTabla(tablaEnCasa);
        Estilizador.AplicarTabla(tablaLimpieza);
        foreach (var titulo in new[] { lblLlegadas, lblEnCasa, lblLimpieza })
        {
            titulo.Font = Tipografia.TituloTarjeta;
            titulo.ForeColor = Paleta.Marino;
        }
    }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaRecepcion(ServiciosHotel servicios) : this()
    {
        _s = servicios;
        Refrescar();
    }

    /// <inheritdoc />
    public override string TituloVista => "Recepción";

    /// <inheritdoc />
    public override void Refrescar()
    {
        if (_s is null) return;
        var huespedes = _s.Huespedes.ObtenerTodos().ToDictionary(h => h.Id);
        var habitaciones = _s.Habitaciones.Buscar(new FiltroHabitacion(IncluirInactivas: true)).ToDictionary(h => h.Id);
        string Nombre(int id) => huespedes.TryGetValue(id, out var h) ? h.NombreCompleto : "—";
        string Numero(int id) => habitaciones.TryGetValue(id, out var h) ? h.Numero : "—";

        var llegadas = _s.Reservas.LlegadasDelDia();
        Mostrar(tablaLlegadas, llegadas.Select(r => new FilaLlegada(r.Id, r.Codigo, Nombre(r.HuespedId),
            Numero(r.HabitacionId), r.FechaSalida.ToString(Constantes.FormatoFecha))).ToList());
        lblLlegadas.Text = $"Llegadas de hoy ({llegadas.Count})";

        var enCasa = _s.Reservas.HuespedesEnCasa();
        Mostrar(tablaEnCasa, enCasa.Select(r => new FilaEnCasa(r.Id, r.Codigo, Nombre(r.HuespedId), Numero(r.HabitacionId),
            r.FechaSalida.ToString(Constantes.FormatoFecha), Constantes.FormatearMoneda(r.TotalCargos))).ToList());
        lblEnCasa.Text = $"Huéspedes en casa ({enCasa.Count})";

        var limpieza = _s.Habitaciones.Buscar(new FiltroHabitacion(Estado: EstadoHabitacion.Limpieza));
        Mostrar(tablaLimpieza, limpieza.Select(h => new FilaLimpieza(h.Id, h.Numero, h.Piso, h.Tipo.ToString())).ToList());
        lblLimpieza.Text = $"Por limpiar ({limpieza.Count})";
        ActualizarBotones();
    }

    private static void Mostrar<T>(DataGridView tabla, IReadOnlyList<T> filas)
    {
        var anterior = tabla.CurrentRow?.Index ?? 0;
        tabla.DataSource = filas.ToList();
        if (tabla.Columns["Id"] is { } id) id.Visible = false;
        foreach (DataGridViewColumn columna in tabla.Columns)
        {
            columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            columna.MinimumWidth = 40;
            columna.FillWeight = columna.Name switch
            {
                "Codigo" => 27, "Huesped" => 38, "Habitacion" or "Numero" => 13, "Salida" => 22,
                "Cargos" => 22, "Piso" => 10, _ => 20
            };
            if (columna.Name is "Numero" or "Habitacion") columna.HeaderText = "Hab.";
        }
        tabla.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        tabla.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        if (filas.Count > 0) tabla.CurrentCell = tabla.Rows[Math.Min(anterior, filas.Count - 1)].Cells
            .Cast<DataGridViewCell>().First(c => c.Visible);
    }

    private void ActualizarBotones()
    {
        btnCheckIn.Enabled = tablaLlegadas.CurrentRow is not null;
        btnCargo.Enabled = btnCheckOut.Enabled = tablaEnCasa.CurrentRow is not null;
        btnLiberar.Enabled = tablaLimpieza.CurrentRow is not null;
    }

    private void Tabla_SelectionChanged(object? sender, EventArgs e) => ActualizarBotones();

    private Reserva? ReservaDe(DataGridView tabla) => tabla.CurrentRow?.DataBoundItem switch
    {
        FilaLlegada f => _s?.Reservas.ObtenerPorId(f.Id),
        FilaEnCasa f => _s?.Reservas.ObtenerPorId(f.Id),
        _ => null
    };

    private void BtnCheckIn_Click(object? sender, EventArgs e)
    {
        if (_s is null || ReservaDe(tablaLlegadas) is not { } reserva) return;
        using var form = new FormCheckIn(_s, reserva);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) Refrescar();
    }

    private void BtnCargo_Click(object? sender, EventArgs e)
    {
        if (_s is null || ReservaDe(tablaEnCasa) is not { } reserva) return;
        using var form = new FormCargo(_s, reserva);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) Refrescar();
    }

    private void BtnCheckOut_Click(object? sender, EventArgs e)
    {
        if (_s is null || ReservaDe(tablaEnCasa) is not { } reserva) return;
        using var form = new FormCheckOut(_s, reserva);
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.FacturaGenerada is null) return;
        Refrescar();
        AbrirFactura(form.FacturaGenerada);
    }

    private void BtnLiberar_Click(object? sender, EventArgs e)
    {
        if (_s is null || tablaLimpieza.CurrentRow?.DataBoundItem is not FilaLimpieza fila) return;
        if (Mensajes.MostrarResultado(FindForm(), _s.Habitaciones.Liberar(fila.Id), avisarExito: false)) Refrescar();
    }

    private void AbrirFactura(Factura factura)
    {
        Mensajes.Exito(FindForm(), $"Factura {factura.Numero} generada por {Constantes.FormatearMoneda(factura.Total)}.");
    }
}
