using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Registro y edición de una habitación.</summary>
/// <remarks>RF-03, RF-04, RN-01; CU-02</remarks>
public partial class FormHabitacion : FormularioBase
{
    private readonly ServiciosHotel? _s;
    private readonly Habitacion? _original;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormHabitacion()
    {
        InitializeComponent();
        cmbTipo.DataSource = Enum.GetValues<TipoHabitacion>();
    }

    /// <summary>Crea el formulario para registrar (<paramref name="existente"/> nulo) o editar.</summary>
    public FormHabitacion(ServiciosHotel servicios, Habitacion? existente = null) : this()
    {
        _s = servicios;
        _original = existente;
        Text = existente is null ? "Nueva habitación" : $"Editar habitación {existente.Numero}";
        if (existente is not null)
        {
            txtNumero.Text = existente.Numero;
            numPiso.Value = existente.Piso;
            cmbTipo.SelectedItem = existente.Tipo;
            numCapacidad.Value = existente.Capacidad;
            numTarifa.Value = existente.TarifaNoche;
            txtDescripcion.Text = existente.Descripcion;
        }

        Validar(txtNumero, () => string.IsNullOrWhiteSpace(txtNumero.Text) ? "El número es obligatorio." : null);
        Validar(numTarifa, () => numTarifa.Value <= 0 ? "La tarifa debe ser mayor que cero." : null);
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null) return false;
        var habitacion = _original ?? new Habitacion();
        habitacion.Numero = txtNumero.Text.Trim();
        habitacion.Piso = (int)numPiso.Value;
        habitacion.Tipo = (TipoHabitacion)cmbTipo.SelectedItem!;
        habitacion.Capacidad = (int)numCapacidad.Value;
        habitacion.TarifaNoche = numTarifa.Value;
        habitacion.Descripcion = txtDescripcion.Text.Trim();

        return Procesar(_original is null
            ? _s.Habitaciones.Registrar(habitacion)
            : _s.Habitaciones.Editar(habitacion));
    }
}
