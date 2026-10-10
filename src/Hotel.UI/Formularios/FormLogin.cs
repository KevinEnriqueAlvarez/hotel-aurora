using Hotel.Core.Servicios;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>Ventana de inicio de sesión (420×520, centrada).</summary>
/// <remarks>RF-01, CU-01; sección 10.4</remarks>
public partial class FormLogin : Form
{
    private readonly ServiciosHotel? _servicios;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormLogin()
    {
        InitializeComponent();
        BackColor = Paleta.Fondo;
        Font = Tipografia.Texto;
        lblHotel.Font = Tipografia.TituloVista;
        lblHotel.ForeColor = Paleta.Marino;
        lblSubtitulo.ForeColor = Paleta.TextoSecundario;
        lblError.ForeColor = Paleta.Error;
        lblLogo.Font = Tipografia.Icono(30f);
        lblLogo.ForeColor = Paleta.Teal;
        Estilizador.AplicarCampos(tarjeta);
    }

    /// <summary>Crea el formulario con los servicios de la aplicación.</summary>
    public FormLogin(ServiciosHotel servicios) : this()
    {
        _servicios = servicios;
        lblHotel.Text = servicios.Configuracion.NombreHotel;
    }

    private void BtnIngresar_Click(object? sender, EventArgs e)
    {
        lblError.Text = "";
        if (_servicios is null) return;
        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrEmpty(txtClave.Text))
        {
            lblError.Text = "Escriba su usuario y contraseña.";
            return;
        }

        Cursor = Cursors.WaitCursor;
        var resultado = _servicios.Autenticacion.IniciarSesion(txtUsuario.Text, txtClave.Text);
        Cursor = Cursors.Default;

        if (resultado.Exito) { DialogResult = DialogResult.OK; return; }
        lblError.Text = resultado.Mensaje;
        txtClave.SelectAll();
        txtClave.Focus();
    }

    private void BtnMostrar_Click(object? sender, EventArgs e)
    {
        txtClave.UseSystemPasswordChar = !txtClave.UseSystemPasswordChar;
        btnMostrar.Icono = txtClave.UseSystemPasswordChar ? "" : "";
    }

    private void Campo_TextChanged(object? sender, EventArgs e) => lblError.Text = "";
}
