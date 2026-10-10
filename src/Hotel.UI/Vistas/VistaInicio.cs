using Hotel.Core.Comun;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Tema;

namespace Hotel.UI.Vistas;

/// <summary>Tablero de inicio: KPI, ingresos de 12 meses, estado de habitaciones y cobros.</summary>
/// <remarks>E1; sección 11</remarks>
public partial class VistaInicio : VistaBase
{
    private readonly ServiciosHotel? _s;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaInicio()
    {
        InitializeComponent();
        foreach (var titulo in new[] { lblIngresosTitulo, lblHabitacionesTitulo, lblCobrosTitulo })
        {
            titulo.Font = Tipografia.TituloTarjeta;
            titulo.ForeColor = Paleta.Marino;
        }
        lblIngresosTotal.Font = new Font(Tipografia.Valor.FontFamily, 17f, FontStyle.Bold);
        lblIngresosTotal.ForeColor = Paleta.Marino;
        lblSaldoValor.Font = Tipografia.TituloTarjeta;
        lblSaldoValor.ForeColor = Paleta.Marino;
        lblSaldoTitulo.ForeColor = Paleta.TextoSecundario;
        selector.Opciones = ["Ingresos", "Reservas", "Ocupación"];
        kpiOcupacion.Icono = "";
        kpiIngresos.Icono = "";
        kpiReservas.Icono = "";
        kpiLlegadas.Icono = "";
        kpiIngresos.Acento = Paleta.Azul;
        kpiReservas.Acento = Paleta.Pizarra;
        kpiLlegadas.Acento = Paleta.Cian;
    }

    /// <summary>Crea el tablero con los servicios de la aplicación.</summary>
    public VistaInicio(ServiciosHotel servicios) : this()
    {
        _s = servicios;
        Refrescar();
    }

    /// <inheritdoc />
    public override string TituloVista => "Inicio";

    /// <inheritdoc />
    public override void Refrescar()
    {
        if (_s is null) return;
        CargarIndicadores();
        CargarSerie();
        CargarHabitaciones();
    }

    private void CargarIndicadores()
    {
        var k = _s!.Reportes.Indicadores();
        var mes = Constantes.Cultura.DateTimeFormat.GetMonthName(_s.Reloj.Ahora.Month);
        kpiOcupacion.Titulo = "Ocupación de hoy";
        kpiOcupacion.Valor = $"{k.OcupacionHoy:0.#}%";
        kpiOcupacion.Subtitulo = $"{k.HabitacionesOcupadas} de {k.HabitacionesTotal} habitaciones ocupadas";
        kpiIngresos.Titulo = "Ingresos del mes";
        kpiIngresos.Valor = Constantes.FormatearMoneda(k.IngresosMes);
        kpiIngresos.Subtitulo = $"Facturado en {mes}";
        kpiReservas.Titulo = "Reservas activas";
        kpiReservas.Valor = k.ReservasActivas.ToString();
        kpiReservas.Subtitulo = "Pendientes, confirmadas y en curso";
        kpiLlegadas.Titulo = "Llegadas de hoy";
        kpiLlegadas.Valor = k.LlegadasHoy.ToString();
        kpiLlegadas.Subtitulo = "Reservas con entrada hoy";

        var total = k.Cobrado + k.SaldoPorCobrar;
        var pctCobrado = total == 0 ? 0 : k.Cobrado / total * 100m;
        barCobrado.Valor = pctCobrado;
        barCobrado.TextoValor = $"{Constantes.FormatearMoneda(k.Cobrado)}  ({pctCobrado:0}%)";
        barPendiente.Valor = total == 0 ? 0 : 100m - pctCobrado;
        barPendiente.TextoValor = $"{Constantes.FormatearMoneda(k.SaldoPorCobrar)}  ({100m - pctCobrado:0}%)";
        lblSaldoValor.Text = Constantes.FormatearMoneda(k.SaldoPorCobrar);
    }

    private void CargarSerie()
    {
        var serie = (SerieReporte)Math.Max(0, selector.IndiceSeleccionado);
        var puntos = _s!.Reportes.SerieMensual(serie);
        grafico.FormatoValor = serie switch
        {
            SerieReporte.Ingresos => v => "Q" + GraficoBarras.FormatoCorto(v),
            SerieReporte.Ocupacion => v => $"{v:0.#}%",
            _ => v => $"{v:0}"
        };
        grafico.Datos = puntos.Select((p, i) => new PuntoBarra(p.Etiqueta, p.Valor, i >= puntos.Count - 3)).ToList();
        lblIngresosTotal.Text = serie switch
        {
            SerieReporte.Ingresos => Constantes.FormatearMoneda(puntos.Sum(p => p.Valor)),
            SerieReporte.Reservas => $"{puntos.Sum(p => p.Valor):0} reservas",
            _ => $"{(puntos.Count == 0 ? 0 : puntos.Average(p => p.Valor)):0.#}% prom."
        };
        lblIngresosTitulo.Text = serie switch
        {
            SerieReporte.Ingresos => "Ingresos",
            SerieReporte.Reservas => "Reservas",
            _ => "Ocupación"
        };
    }

    private void CargarHabitaciones()
    {
        var conteo = _s!.Reportes.HabitacionesPorEstado();
        Color ColorDe(string nombre) => nombre switch
        {
            "Ocupada" => Paleta.Marino,
            "Limpieza" => Paleta.Pizarra,
            ReporteServicio.ReservadaHoy => Paleta.Azul,
            "Disponible" => Paleta.Cian,
            _ => Paleta.Advertencia
        };
        dona.Segmentos = conteo.Select(c => new SegmentoDona(c.Nombre, c.Cantidad, ColorDe(c.Nombre))).ToList();
        dona.TextoCentral = conteo.Sum(c => c.Cantidad).ToString();
        dona.SubtituloCentral = "habitaciones";
    }

    private void Selector_IndiceCambiado(object? sender, EventArgs e)
    {
        if (_s is not null) CargarSerie();
    }
}
