using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Registro y edición de un usuario del sistema (solo Administrador).</summary>
/// <remarks>RF-02, RN-01, RNF-06; CU-12</remarks>
public partial class FormUsuario : FormularioBase
{
    private readonly ServiciosHotel? _s;
    private readonly Usuario? _original;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormUsuario()
    {
        InitializeComponent();
        cmbTipoDocumento.DataSource = Enum.GetValues<TipoDocumento>();
        cmbRol.DataSource = Enum.GetValues<RolUsuario>();
    }

    /// <summary>Crea el formulario para registrar (<paramref name="existente"/> nulo) o editar.</summary>
    public FormUsuario(ServiciosHotel servicios, Usuario? existente = null) : this()
    {
        _s = servicios;
        _original = existente;
        Text = existente is null ? "Nuevo usuario" : $"Editar usuario — {existente.NombreUsuario}";

        var esNuevo = existente is null;
        lblClave.Visible = txtClave.Visible = esNuevo;
        chkActivo.Visible = !esNuevo;
        if (existente is not null) Cargar(existente);

        Validar(txtNombres, () => Validador.EsNombreValido(txtNombres.Text.Trim()) ? null : "Entre 2 y 60 letras.");
        Validar(txtApellidos, () => Validador.EsNombreValido(txtApellidos.Text.Trim()) ? null : "Entre 2 y 60 letras.");
        Validar(txtDocumento, () =>
            Validador.EsDocumentoValido(Tipo, txtDocumento.Text.Trim()) ? null : "Documento no válido (DPI: 13 dígitos).");
        Validar(txtTelefono, () => Validador.EsTelefonoValido(txtTelefono.Text.Trim()) ? null : "8 a 15 dígitos.");
        Validar(txtCorreo, () => Validador.EsCorreoValido(txtCorreo.Text) ? null : "Correo no válido.");
        Validar(txtUsuario, () =>
            txtUsuario.Text.Trim().Length is >= 3 and <= 30 && txtUsuario.Text.Trim().All(c => char.IsLetterOrDigit(c) || c is '.' or '_')
                ? null
                : "3 a 30 letras, números, punto o guion bajo.");
        if (esNuevo)
            Validar(txtClave, () => Validador.EsContrasenaValida(txtClave.Text) ? null : Validador.MensajeContrasena);
    }

    private TipoDocumento Tipo => (TipoDocumento)(cmbTipoDocumento.SelectedItem ?? TipoDocumento.Dpi);

    private void Cargar(Usuario u)
    {
        txtNombres.Text = u.Nombres;
        txtApellidos.Text = u.Apellidos;
        cmbTipoDocumento.SelectedItem = u.TipoDocumento;
        txtDocumento.Text = u.NumeroDocumento;
        txtTelefono.Text = u.Telefono;
        txtCorreo.Text = u.Correo;
        txtUsuario.Text = u.NombreUsuario;
        cmbRol.SelectedItem = u.Rol;
        chkActivo.Checked = u.Activo;
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null) return false;
        var usuario = _original ?? new Usuario();
        usuario.Nombres = txtNombres.Text.Trim();
        usuario.Apellidos = txtApellidos.Text.Trim();
        usuario.TipoDocumento = Tipo;
        usuario.NumeroDocumento = txtDocumento.Text.Trim();
        usuario.Telefono = txtTelefono.Text.Trim();
        usuario.Correo = txtCorreo.Text.Trim();
        usuario.NombreUsuario = txtUsuario.Text.Trim();
        usuario.Rol = (RolUsuario)(cmbRol.SelectedItem ?? RolUsuario.Recepcionista);

        if (_original is null) return Procesar(_s.Usuarios.Registrar(usuario, txtClave.Text));
        usuario.Activo = chkActivo.Checked;
        return Procesar(_s.Usuarios.Editar(usuario));
    }
}
