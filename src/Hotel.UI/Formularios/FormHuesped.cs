using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Registro y edición de un huésped con validación en tiempo real.</summary>
/// <remarks>RF-08, RF-09, RN-01; CU-03</remarks>
public partial class FormHuesped : FormularioBase
{
    private readonly ServiciosHotel? _s;
    private readonly Huesped? _original;

    /// <summary>Huésped registrado o editado (disponible tras cerrar con OK).</summary>
    public Huesped? Resultado { get; private set; }

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormHuesped()
    {
        InitializeComponent();
        cmbTipoDocumento.DataSource = Enum.GetValues<TipoDocumento>();
        dtpNacimiento.MaxDate = DateTime.Today;
        dtpNacimiento.Value = DateTime.Today.AddYears(-30);
    }

    /// <summary>Crea el formulario para registrar (<paramref name="existente"/> nulo) o editar.</summary>
    public FormHuesped(ServiciosHotel servicios, Huesped? existente = null) : this()
    {
        _s = servicios;
        _original = existente;
        Text = existente is null ? "Nuevo huésped" : $"Editar huésped — {existente.NombreCompleto}";
        if (existente is not null) Cargar(existente);
        ConfigurarValidacion();
    }

    private void Cargar(Huesped h)
    {
        txtNombres.Text = h.Nombres;
        txtApellidos.Text = h.Apellidos;
        cmbTipoDocumento.SelectedItem = h.TipoDocumento;
        txtDocumento.Text = h.NumeroDocumento;
        txtTelefono.Text = h.Telefono;
        txtCorreo.Text = h.Correo;
        txtNacionalidad.Text = h.Nacionalidad;
        if (h.FechaNacimiento != default) dtpNacimiento.Value = h.FechaNacimiento;
        txtNit.Text = h.Nit;
        txtNotas.Text = h.Notas;
    }

    private void ConfigurarValidacion()
    {
        Validar(txtNombres, () => Validador.EsNombreValido(txtNombres.Text.Trim()) ? null : "Entre 2 y 60 letras.");
        Validar(txtApellidos, () => Validador.EsNombreValido(txtApellidos.Text.Trim()) ? null : "Entre 2 y 60 letras.");
        Validar(txtDocumento, () =>
            Validador.EsDocumentoValido(TipoSeleccionado, txtDocumento.Text.Trim())
                ? null
                : TipoSeleccionado == TipoDocumento.Dpi ? "El DPI debe tener 13 dígitos." : "El pasaporte debe tener 6 a 20 caracteres alfanuméricos.");
        Validar(txtTelefono, () => Validador.EsTelefonoValido(txtTelefono.Text.Trim()) ? null : "8 a 15 dígitos (+ inicial opcional).");
        Validar(txtCorreo, () => Validador.EsCorreoValido(txtCorreo.Text) ? null : "Correo no válido.");
        Validar(txtNit, () => Validador.EsNitValido(txtNit.Text) ? null : "Use CF o un NIT con verificador correcto.");
        Validar(dtpNacimiento, () =>
            dtpNacimiento.Value.Date <= DateTime.Today.AddYears(-Constantes.EdadMinima) ? null : $"Debe ser mayor de {Constantes.EdadMinima} años.");
        cmbTipoDocumento.SelectedIndexChanged += (_, _) => Errores.SetError(txtDocumento, "");
    }

    private TipoDocumento TipoSeleccionado => (TipoDocumento)(cmbTipoDocumento.SelectedItem ?? TipoDocumento.Dpi);

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null) return false;
        var huesped = _original ?? new Huesped();
        huesped.Nombres = txtNombres.Text.Trim();
        huesped.Apellidos = txtApellidos.Text.Trim();
        huesped.TipoDocumento = TipoSeleccionado;
        huesped.NumeroDocumento = txtDocumento.Text.Trim();
        huesped.Telefono = txtTelefono.Text.Trim();
        huesped.Correo = txtCorreo.Text.Trim();
        huesped.Nacionalidad = txtNacionalidad.Text.Trim();
        huesped.FechaNacimiento = dtpNacimiento.Value.Date;
        huesped.Nit = txtNit.Text.Trim();
        huesped.Notas = txtNotas.Text.Trim();

        if (_original is null)
        {
            var r = _s.Huespedes.Registrar(huesped);
            if (!Procesar(r)) return false;
            Resultado = r.Valor;
            return true;
        }
        if (!Procesar(_s.Huespedes.Editar(huesped))) return false;
        Resultado = huesped;
        return true;
    }
}
