using System.ComponentModel;
using Hotel.UI.Controles;
using Hotel.UI.Tema;

namespace Hotel.UI.Vistas;

/// <summary>
/// Base de las vistas de listado: buscador que filtra al escribir, zona de filtros,
/// botones de acción, tabla estilizada, contador y doble clic para editar.
/// </summary>
/// <remarks>Sección 10.5 «VistaListadoBase»; herencia visual</remarks>
public partial class VistaListadoBase : VistaBase
{
    /// <summary>Crea el listado con el tema aplicado.</summary>
    public VistaListadoBase()
    {
        InitializeComponent();
        Estilizador.AplicarTabla(tabla);
        Estilizador.AplicarCampos(tarjetaFiltros);
        tarjetaFiltros.ColorTarjeta = Paleta.Tarjeta;
        lblContador.ForeColor = Paleta.TextoSecundario;
    }

    /// <summary>Muestra el botón Nuevo.</summary>
    [Category("Behavior"), DefaultValue(true)]
    public bool PermiteNuevo { get => btnNuevo.Visible; set => btnNuevo.Visible = value; }

    /// <summary>Muestra el botón Editar (y habilita doble clic = editar).</summary>
    [Category("Behavior"), DefaultValue(true)]
    public bool PermiteEditar { get => btnEditar.Visible; set => btnEditar.Visible = value; }

    /// <summary>Muestra el botón Eliminar/Desactivar.</summary>
    [Category("Behavior"), DefaultValue(true)]
    public bool PermiteEliminar { get => btnEliminar.Visible; set => btnEliminar.Visible = value; }

    /// <summary>Muestra el botón Ver detalle.</summary>
    [Category("Behavior"), DefaultValue(true)]
    public bool PermiteDetalle { get => btnDetalle.Visible; set => btnDetalle.Visible = value; }

    /// <summary>Texto del botón de baja («Eliminar» o «Desactivar»).</summary>
    [Category("Appearance"), DefaultValue("Eliminar")]
    public string TextoEliminar { get => btnEliminar.Text; set => btnEliminar.Text = value; }

    /// <summary>Tabla del listado.</summary>
    protected DataGridView Tabla => tabla;

    /// <summary>Zona donde las vistas hijas agregan sus filtros.</summary>
    protected FlowLayoutPanel PanelFiltros => flpFiltros;

    /// <summary>Texto escrito en el buscador.</summary>
    protected string TextoBusqueda => txtBuscar.Text.Trim();

    /// <summary>Texto de ayuda del buscador.</summary>
    protected string AyudaBusqueda { get => txtBuscar.PlaceholderText; set => txtBuscar.PlaceholderText = value; }

    /// <summary>Agrega un botón de acción adicional junto a los estándar.</summary>
    protected void AgregarAccion(BotonModerno boton)
    {
        boton.Margin = new Padding(0, 0, 8, 0);
        flpAcciones.Controls.Add(boton);
    }

    /// <summary>Agrega un filtro con su etiqueta a la zona de filtros.</summary>
    protected void AgregarFiltro(string etiqueta, Control control)
    {
        var rotulo = new Label
        {
            Text = etiqueta, AutoSize = true, ForeColor = Paleta.TextoSecundario, Font = Tipografia.EtiquetaPequena,
            Margin = new Padding(12, 9, 4, 0), Tag = "personalizada"
        };
        control.Margin = new Padding(0, 4, 0, 0);
        flpFiltros.Controls.Add(rotulo);
        flpFiltros.Controls.Add(control);
        Estilizador.AplicarCampos(flpFiltros);
    }

    /// <summary>Carga los datos; las vistas hijas lo sobrescriben.</summary>
    protected virtual void CargarDatos() { }

    /// <summary>Acción del botón Nuevo.</summary>
    protected virtual void Nuevo() { }

    /// <summary>Acción del botón Editar y del doble clic.</summary>
    protected virtual void Editar() { }

    /// <summary>Acción del botón Eliminar/Desactivar.</summary>
    protected virtual void Eliminar() { }

    /// <summary>Acción del botón Ver detalle.</summary>
    protected virtual void VerDetalle() { }

    /// <summary>Se invoca al cambiar la fila seleccionada para habilitar acciones extra.</summary>
    protected virtual void AlCambiarSeleccion() { }

    /// <summary>Fila seleccionada, o null.</summary>
    protected T? FilaSeleccionada<T>() where T : class => tabla.CurrentRow?.DataBoundItem as T;

    /// <summary>Muestra las filas, oculta la columna Id, conserva la selección y actualiza el contador.</summary>
    protected void MostrarFilas<T>(IReadOnlyList<T> filas) where T : class, IFilaConId
    {
        var idSeleccionado = FilaSeleccionada<T>()?.Id;
        tabla.DataSource = filas.ToList();
        if (tabla.Columns["Id"] is { } id) id.Visible = false;

        if (idSeleccionado is not null)
            foreach (DataGridViewRow fila in tabla.Rows)
                if (fila.DataBoundItem is T t && t.Id == idSeleccionado)
                {
                    tabla.CurrentCell = fila.Cells.Cast<DataGridViewCell>().First(c => c.Visible);
                    break;
                }
        lblContador.Text = filas.Count == 1 ? "1 registro" : $"{filas.Count} registros";
        ActualizarBotones();
    }

    /// <inheritdoc />
    public override void Refrescar() => CargarDatos();

    /// <summary>Carga inicial de datos.</summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!DesignMode) CargarDatos();
    }

    private void ActualizarBotones()
    {
        var hay = tabla.CurrentRow is not null;
        btnEditar.Enabled = hay;
        btnEliminar.Enabled = hay;
        btnDetalle.Enabled = hay;
        AlCambiarSeleccion();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e) => CargarDatos();
    private void BtnNuevo_Click(object? sender, EventArgs e) => Nuevo();
    private void BtnEditar_Click(object? sender, EventArgs e) => Editar();
    private void BtnEliminar_Click(object? sender, EventArgs e) => Eliminar();
    private void BtnDetalle_Click(object? sender, EventArgs e) => VerDetalle();
    private void Tabla_SelectionChanged(object? sender, EventArgs e) => ActualizarBotones();

    private void Tabla_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (btnEditar.Visible) Editar();
        else if (btnDetalle.Visible) VerDetalle();
    }
}
