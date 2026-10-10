using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.UI.Controles;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>
/// Base de los diálogos: estilo común, botones Guardar/Cancelar, <see cref="ErrorProvider"/>
/// y validación en tiempo real. Los formularios hijos agregan sus campos a <see cref="Contenido"/>.
/// </summary>
/// <remarks>Sección 10.5 «FormularioBase»; sección 10.6; herencia de formularios</remarks>
public partial class FormularioBase : Form
{
    private readonly List<(Control Campo, Func<string?> Regla)> _reglas = [];

    /// <summary>Crea la base con el tema aplicado.</summary>
    public FormularioBase()
    {
        InitializeComponent();
        BackColor = Paleta.Fondo;
        Font = Tipografia.Texto;
        lblTitulo.Font = Tipografia.TituloTarjeta;
        lblTitulo.ForeColor = Paleta.Marino;
        pnlPie.BackColor = Paleta.Tarjeta;
    }

    /// <summary>Panel donde los formularios hijos colocan sus campos.</summary>
    protected Panel Contenido => pnlContenido;

    /// <summary>Indicador de errores de validación por campo.</summary>
    protected ErrorProvider Errores => errorProvider;

    /// <summary>Texto del botón principal (por defecto «Guardar»).</summary>
    [Category("Appearance"), DefaultValue("Guardar")]
    public string TextoGuardar
    {
        get => btnGuardar.Text;
        set => btnGuardar.Text = value;
    }

    /// <summary>Muestra u oculta el botón Cancelar (diálogos solo informativos).</summary>
    [Category("Appearance"), DefaultValue(true)]
    public bool MostrarCancelar
    {
        get => btnCancelar.Visible;
        set => btnCancelar.Visible = value;
    }

    /// <summary>Persiste los datos; devuelve true para cerrar el diálogo con éxito.</summary>
    protected virtual bool Guardar() => true;

    /// <summary>Registra una regla evaluada al escribir y al salir del campo.</summary>
    /// <param name="campo">Control que muestra el error.</param>
    /// <param name="regla">Devuelve el mensaje de error, o null si es válido.</param>
    protected void Validar(Control campo, Func<string?> regla)
    {
        _reglas.Add((campo, regla));
        void Evaluar(object? s, EventArgs e) => errorProvider.SetError(campo, regla() ?? "");
        campo.Validating += (s, e) => Evaluar(s, e);
        if (campo is TextBoxBase) campo.TextChanged += Evaluar;
        else if (campo is ComboBox) ((ComboBox)campo).SelectedIndexChanged += Evaluar;
        else if (campo is DateTimePicker) ((DateTimePicker)campo).ValueChanged += Evaluar;
        else if (campo is NumericUpDown) ((NumericUpDown)campo).ValueChanged += Evaluar;
    }

    /// <summary>Evalúa todas las reglas; enfoca el primer campo inválido.</summary>
    protected bool ValidarTodo()
    {
        Control? primero = null;
        foreach (var (campo, regla) in _reglas)
        {
            var mensaje = regla();
            errorProvider.SetError(campo, mensaje ?? "");
            if (mensaje is not null) primero ??= campo;
        }
        primero?.Focus();
        return primero is null;
    }

    /// <summary>Muestra el error del servicio; true si el resultado fue exitoso.</summary>
    protected bool Procesar(Resultado resultado)
    {
        if (!resultado.Exito) Mensajes.Error(this, resultado.Mensaje);
        return resultado.Exito;
    }

    /// <summary>Aplica estilo a los campos y muestra el título.</summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        lblTitulo.Text = Text;
        if (!DesignMode) Estilizador.AplicarCampos(pnlContenido);
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarTodo()) return;
        if (Guardar()) DialogResult = DialogResult.OK;
    }
}
