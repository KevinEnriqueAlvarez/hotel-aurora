using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Tema;
using Hotel.UI.Vistas;

namespace Hotel.UI.Formularios;

/// <summary>Ventana principal: barra lateral, encabezado y área donde se cargan las vistas.</summary>
/// <remarks>RF-01 (menú según rol); sección 10.4</remarks>
public partial class FormPrincipal : Form
{
    private readonly ServiciosHotel? _s;
    private readonly Dictionary<BotonNavegacion, Func<VistaBase>> _vistas = [];
    private BotonNavegacion? _activo;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormPrincipal()
    {
        InitializeComponent();
        BackColor = Paleta.Fondo;
        Font = Tipografia.Texto;
        pnlLateral.BackColor = Paleta.Marino;
        pnlEncabezado.BackColor = Paleta.Fondo;
        lblTitulo.Font = Tipografia.TituloVista;
        lblTitulo.ForeColor = Paleta.Marino;
        lblFecha.ForeColor = Paleta.TextoSecundario;
        lblHotel.Font = Tipografia.TituloTarjeta;
        lblHotel.ForeColor = Color.White;
        lblUsuario.ForeColor = Color.White;
        lblUsuario.Font = Tipografia.Etiqueta;
        lblRol.ForeColor = Paleta.AzulPestana;
        lblRol.Font = Tipografia.Pequena;
    }

    /// <summary>Crea la ventana con los servicios y la sesión iniciada.</summary>
    public FormPrincipal(ServiciosHotel servicios) : this()
    {
        _s = servicios;
        var usuario = servicios.Autenticacion.UsuarioActual;
        lblHotel.Text = servicios.Configuracion.NombreHotel;
        lblUsuario.Text = usuario?.NombreCompleto ?? "";
        lblRol.Text = usuario?.Rol.ToString() ?? "";
        btnUsuarios.Visible = servicios.Autenticacion.EsAdministrador;
        Text = $"{servicios.Configuracion.NombreHotel} — Sistema de reservas";

        _vistas[btnInicio] = () => new VistaInicio(servicios);
        _vistas[btnHabitaciones] = () => new VistaHabitaciones(servicios);
        _vistas[btnHuespedes] = () => new VistaHuespedes(servicios);
        _vistas[btnReservas] = () => new VistaReservas(servicios);
        _vistas[btnRecepcion] = () => new VistaRecepcion(servicios);
        _vistas[btnFacturacion] = () => new VistaFacturacion(servicios);
        _vistas[btnUsuarios] = () => new VistaUsuarios(servicios);
    }

    /// <summary>Muestra la vista de inicio al abrir.</summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (_s is not null) Navegar(btnInicio);
    }

    private void Navegar(BotonNavegacion destino)
    {
        if (!_vistas.TryGetValue(destino, out var crear)) return;
        if (_activo is not null) _activo.Activo = false;
        _activo = destino;
        destino.Activo = true;

        SuspendLayout();
        foreach (Control anterior in pnlContenido.Controls.Cast<Control>().ToList())
        {
            pnlContenido.Controls.Remove(anterior);
            anterior.Dispose();
        }
        var vista = crear();
        vista.Dock = DockStyle.Fill;
        pnlContenido.Controls.Add(vista);
        lblTitulo.Text = vista.TituloVista;
        lblFecha.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy", Core.Comun.Constantes.Cultura);
        ResumeLayout();
    }

    private void BotonNavegacion_Click(object? sender, EventArgs e)
    {
        if (sender is BotonNavegacion boton) Navegar(boton);
    }

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        if (!Mensajes.Confirmar(this, "¿Desea cerrar la sesión?", "Cerrar sesión", textoAceptar: "Cerrar sesión")) return;
        _s?.Autenticacion.CerrarSesion();
        DialogResult = DialogResult.Retry;
    }
}
