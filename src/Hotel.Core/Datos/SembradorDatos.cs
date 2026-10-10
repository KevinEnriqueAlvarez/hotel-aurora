using System.Globalization;
using System.Text;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Core.Datos;

/// <summary>Simula día a día ocho meses de operación avanzando un <see cref="RelojFijo"/>.</summary>
/// <remarks>RNF-02; sección 9</remarks>
internal sealed class SembradorDatos
{
    private enum Destino { Pagada, Parcial, Anulada, SinPago, Cancelada, EnCurso, Pendiente, Confirmada }

    private sealed class Plan
    {
        public required Destino Destino { get; init; }
        public required int HuespedId { get; init; }
        public required DateTime Entrada { get; init; }
        public required int Noches { get; init; }
        public required DateTime Creacion { get; init; }
        public int Descuento { get; init; }
        public int Indice { get; init; }
        public int ReservaId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime Salida => Entrada.AddDays(Noches);
    }

    private static readonly string[] Nombres =
        ["Carlos", "María", "José", "Ana", "Luis", "Sofía", "Jorge", "Lucía", "Pedro", "Carmen",
         "Miguel", "Andrea", "Diego", "Gabriela", "Fernando"];
    private static readonly string[] Apellidos =
        ["López", "García", "Hernández", "Pérez", "Martínez", "Morales", "Ramírez", "Castillo", "Méndez", "Cifuentes",
         "Barrios", "Orellana", "Alvarado", "Estrada", "Velásquez"];

    private readonly ContextoDatos _contexto;
    private readonly DateTime _ahora;
    private readonly DateTime _hoy;
    private readonly RelojFijo _reloj;
    private readonly ServiciosHotel _s;
    private readonly Random _azar = new(20260101);
    private readonly Dictionary<int, DateTime> _enLimpieza = [];

    public SembradorDatos(ContextoDatos contexto, DateTime ahora)
    {
        _contexto = contexto;
        _ahora = ahora;
        _hoy = ahora.Date;
        _reloj = new RelojFijo(_hoy.AddMonths(-8));
        _s = new ServiciosHotel(contexto, _reloj);
    }

    public void Ejecutar()
    {
        CrearUsuarios();
        CrearCatalogo();
        var habitaciones = CrearHabitaciones();
        var huespedes = CrearHuespedes();
        Simular(PlanearReservas(huespedes), habitaciones);
        PonerUnaEnMantenimiento();
        _s.Autenticacion.CerrarSesion();
    }

    // ---------- Datos maestros ----------

    private void CrearUsuarios()
    {
        _s.Usuarios.Registrar(NuevoUsuario("Administrador", "Aurora", "admin", RolUsuario.Administrador, "1000000000001"), "Admin123*");
        _s.Autenticacion.IniciarSesion("admin", "Admin123*");
        _s.Usuarios.Registrar(NuevoUsuario("Recepcionista", "Principal", "recepcion", RolUsuario.Recepcionista, "1000000000002"), "Recepcion123*");
    }

    private static Usuario NuevoUsuario(string nombres, string apellidos, string usuario, RolUsuario rol, string dpi) => new()
    {
        Nombres = nombres, Apellidos = apellidos, NombreUsuario = usuario, Rol = rol,
        TipoDocumento = TipoDocumento.Dpi, NumeroDocumento = dpi, Telefono = "22345678",
        Correo = $"{usuario}@hotelaurora.gt"
    };

    private void CrearCatalogo()
    {
        (string, CategoriaServicio, decimal)[] servicios =
        [
            ("Desayuno buffet", CategoriaServicio.Restaurante, 65m),
            ("Almuerzo ejecutivo", CategoriaServicio.Restaurante, 95m),
            ("Cena a la carta", CategoriaServicio.Restaurante, 120m),
            ("Minibar", CategoriaServicio.Minibar, 35m),
            ("Lavandería por docena", CategoriaServicio.Lavanderia, 80m),
            ("Planchado de prenda", CategoriaServicio.Lavanderia, 20m),
            ("Masaje relajante", CategoriaServicio.Spa, 350m),
            ("Traslado al aeropuerto", CategoriaServicio.Transporte, 180m),
            ("Tour Antigua Guatemala", CategoriaServicio.Transporte, 250m),
            ("Cóctel de bienvenida", CategoriaServicio.Otro, 45m)
        ];
        foreach (var (nombre, categoria, precio) in servicios)
            _contexto.Servicios.Agregar(new ServicioAdicional
                { Nombre = nombre, Categoria = categoria, Precio = precio, FechaCreacion = _reloj.Ahora });
    }

    private List<Habitacion> CrearHabitaciones()
    {
        (string, int, TipoHabitacion, int, decimal)[] datos =
        [
            ("101", 1, TipoHabitacion.Sencilla, 1, 350m), ("102", 1, TipoHabitacion.Sencilla, 1, 380m),
            ("103", 1, TipoHabitacion.Doble, 2, 520m), ("104", 1, TipoHabitacion.Doble, 2, 540m),
            ("105", 1, TipoHabitacion.Familiar, 4, 750m),
            ("201", 2, TipoHabitacion.Sencilla, 1, 400m), ("202", 2, TipoHabitacion.Doble, 2, 580m),
            ("203", 2, TipoHabitacion.Doble, 3, 620m), ("204", 2, TipoHabitacion.Familiar, 5, 850m),
            ("205", 2, TipoHabitacion.Familiar, 4, 800m),
            ("301", 3, TipoHabitacion.Doble, 2, 650m), ("302", 3, TipoHabitacion.Doble, 2, 680m),
            ("303", 3, TipoHabitacion.Suite, 3, 1050m), ("304", 3, TipoHabitacion.Familiar, 5, 900m),
            ("305", 3, TipoHabitacion.Suite, 2, 1100m),
            ("401", 4, TipoHabitacion.Suite, 3, 1200m), ("402", 4, TipoHabitacion.Suite, 4, 1300m),
            ("403", 4, TipoHabitacion.Suite, 2, 1250m), ("404", 4, TipoHabitacion.Suite, 4, 1400m),
            ("405", 4, TipoHabitacion.Suite, 4, 1500m)
        ];
        var lista = new List<Habitacion>();
        foreach (var (numero, piso, tipo, capacidad, tarifa) in datos)
        {
            var r = _s.Habitaciones.Registrar(new Habitacion
            {
                Numero = numero, Piso = piso, Tipo = tipo, Capacidad = capacidad, TarifaNoche = tarifa,
                Descripcion = $"Habitación {tipo.ToString().ToLowerInvariant()} del piso {piso}"
            });
            if (r.Valor is not null) lista.Add(r.Valor);
        }
        return lista;
    }

    private List<int> CrearHuespedes()
    {
        var ids = new List<int>();
        for (var i = 0; i < Nombres.Length; i++)
        {
            var digitos = $"{_azar.Next(1000000, 9999999)}";
            var nit = i % 3 == 0 ? $"{digitos}{Validador.CalcularVerificadorNit(digitos)}" : Constantes.NitConsumidorFinal;
            ids.Add(Registrar(new Huesped
            {
                Nombres = Nombres[i], Apellidos = $"{Apellidos[i]} {Apellidos[(i + 5) % Apellidos.Length]}",
                TipoDocumento = TipoDocumento.Dpi,
                NumeroDocumento = $"{_azar.NextInt64(1_000_000_000_000, 3_000_000_000_000)}",
                Nacionalidad = "Guatemalteca", Nit = nit, Telefono = $"{_azar.Next(2, 6)}{_azar.Next(1000000, 9999999)}"
            }, i));
        }

        (string, string, string, string)[] extranjeros =
        [
            ("John", "Smith", "Estadounidense", "US4455123"), ("Laura", "Fernández", "Española", "ES7788991"),
            ("Pierre", "Dubois", "Francesa", "FR3322114"), ("Hiroshi", "Tanaka", "Japonesa", "JP9911220")
        ];
        for (var j = 0; j < extranjeros.Length; j++)
        {
            var (nombres, apellidos, nacionalidad, pasaporte) = extranjeros[j];
            ids.Add(Registrar(new Huesped
            {
                Nombres = nombres, Apellidos = apellidos, TipoDocumento = TipoDocumento.Pasaporte,
                NumeroDocumento = pasaporte, Nacionalidad = nacionalidad, Nit = Constantes.NitConsumidorFinal,
                Telefono = $"+1{_azar.Next(20000000, 99999999)}"
            }, 100 + j));
        }
        return ids.Where(id => id > 0).ToList();
    }

    private int Registrar(Huesped h, int indice)
    {
        h.FechaNacimiento = new DateTime(_azar.Next(1955, 2003), _azar.Next(1, 13), _azar.Next(1, 28));
        h.Correo = $"{Simplificar(h.Nombres)}.{Simplificar(h.Apellidos.Split(' ')[0])}{indice}@correo.com";
        return _s.Huespedes.Registrar(h).Valor?.Id ?? 0;
    }

    private static string Simplificar(string texto)
    {
        var sb = new StringBuilder();
        foreach (var c in texto.Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetter(c))
                sb.Append(char.ToLowerInvariant(c));
        return sb.ToString();
    }

    // ---------- Planeación y simulación ----------

    private List<Plan> PlanearReservas(List<int> huespedes)
    {
        var inicio = _hoy.AddMonths(-8);
        var planes = new List<Plan>();
        int Huesped() => huespedes[_azar.Next(huespedes.Count)];

        for (var i = 0; i < 24; i++)
        {
            var paso = ((_hoy - inicio).Days - 12) / 23.0;   // reparte las estadías a lo largo de los 8 meses
            var entrada = inicio.AddDays(1 + (int)(i * paso) + _azar.Next(0, 3));
            var destino = i switch { 23 => Destino.Anulada, 22 => Destino.SinPago, >= 17 => Destino.Parcial, _ => Destino.Pagada };
            planes.Add(Nuevo(destino, entrada, _azar.Next(1, 6), i % 5 == 0 ? 10 : 0, i));
        }
        planes.Add(Nuevo(Destino.Pagada, _hoy.AddDays(-2), 2, 0, 24));   // sale hoy
        planes.Add(Nuevo(Destino.Pagada, _hoy.AddDays(-4), 3, 5, 25));   // sale ayer
        for (var i = 0; i < 5; i++)
        {
            var entrada = inicio.AddDays(_azar.Next(0, (_hoy - inicio).Days - 3));
            planes.Add(Nuevo(Destino.Cancelada, entrada, _azar.Next(1, 5), 0, 30 + i));
        }
        for (var i = 0; i < 4; i++)
            planes.Add(Nuevo(Destino.EnCurso, _hoy.AddDays(-_azar.Next(0, 3)), _azar.Next(3, 7), 0, 40 + i));
        for (var i = 0; i < 2; i++)
            planes.Add(Nuevo(Destino.Confirmada, _hoy, _azar.Next(1, 4), 0, 50 + i));
        for (var i = 0; i < 5; i++)
            planes.Add(Nuevo(i < 2 ? Destino.Pendiente : Destino.Confirmada, _hoy.AddDays(_azar.Next(1, 14)), _azar.Next(1, 5), 0, 60 + i));
        planes.Add(Nuevo(Destino.Cancelada, _hoy.AddDays(5), 2, 0, 70));
        return planes;

        Plan Nuevo(Destino destino, DateTime entrada, int noches, int descuento, int indice) => new()
        {
            Destino = destino, HuespedId = Huesped(), Entrada = entrada, Noches = noches,
            Creacion = Max(inicio, Min(entrada.AddDays(-_azar.Next(0, 8)), _hoy.AddDays(-_azar.Next(0, 6)))),
            Descuento = descuento, Indice = indice
        };
    }

    private void Simular(List<Plan> planes, List<Habitacion> habitaciones)
    {
        var inicio = _hoy.AddMonths(-8);
        for (var dia = inicio; dia <= _hoy; dia = dia.AddDays(1))
        {
            Reloj(dia.AddHours(7));
            LiberarLimpias(dia);

            foreach (var p in planes.Where(p => p.Creacion == dia))
            {
                Reloj(dia.AddHours(9).AddMinutes(p.Indice));
                CrearReserva(p, habitaciones);
            }
            foreach (var p in planes.Where(p => p.ReservaId > 0 && CancelaEn(p) == dia))
            {
                Reloj(dia.AddHours(10).AddMinutes(p.Indice));
                _s.Reservas.Cancelar(p.ReservaId, MotivoCancelacion(p.Indice));
            }
            foreach (var p in planes.Where(p => p.ReservaId > 0 && EsEstadia(p) && p.Salida == dia && p.Destino != Destino.EnCurso))
            {
                Reloj(dia.AddHours(11).AddMinutes(p.Indice));
                HacerCheckOut(p);
            }
            foreach (var p in planes.Where(p => p.ReservaId > 0 && EsEstadia(p) && p.Entrada == dia))
            {
                Reloj(dia.AddHours(14).AddMinutes(p.Indice));
                HacerCheckIn(p);
            }
        }
    }

    private static bool EsEstadia(Plan p) =>
        p.Destino is Destino.Pagada or Destino.Parcial or Destino.Anulada or Destino.SinPago or Destino.EnCurso;

    private DateTime? CancelaEn(Plan p) =>
        p.Destino == Destino.Cancelada ? Min(p.Creacion.AddDays(1), _hoy) : null;

    private void CrearReserva(Plan p, List<Habitacion> habitaciones)
    {
        var confirmar = p.Destino switch
        {
            Destino.Pendiente => false,
            Destino.Cancelada => p.Indice % 2 == 0,
            _ => true
        };
        foreach (var hab in habitaciones.OrderBy(_ => _azar.Next()))
        {
            var personas = _azar.Next(1, Math.Min(hab.Capacidad, 3) + 1);
            var r = _s.Reservas.Crear(p.HuespedId, hab.Id, p.Entrada, p.Salida, personas, confirmar);
            if (!r.Exito || r.Valor is null) continue;
            p.ReservaId = r.Valor.Id;
            p.HabitacionId = hab.Id;
            return;
        }
    }

    private void HacerCheckIn(Plan p)
    {
        if (_enLimpieza.ContainsKey(p.HabitacionId))
        {
            _s.Habitaciones.Liberar(p.HabitacionId);
            _enLimpieza.Remove(p.HabitacionId);
        }
        if (!_s.Estadias.CheckIn(p.ReservaId).Exito) { p.ReservaId = 0; return; }

        var servicios = _s.Estadias.ObtenerServicios();
        for (var k = 0; k < _azar.Next(1, 4); k++)
        {
            Reloj(_reloj.Ahora.AddMinutes(30));
            _s.Estadias.AgregarCargo(p.ReservaId, servicios[_azar.Next(servicios.Count)].Id, _azar.Next(1, 4));
        }
    }

    private void HacerCheckOut(Plan p)
    {
        var r = _s.Estadias.CheckOut(p.ReservaId, p.Descuento);
        if (!r.Exito || r.Valor is null) return;
        _enLimpieza[p.HabitacionId] = _reloj.Ahora.Date;

        var factura = r.Valor;
        Reloj(_reloj.Ahora.AddMinutes(15));
        switch (p.Destino)
        {
            case Destino.Pagada or Destino.Anulada:
                _s.Facturacion.RegistrarPago(factura.Id, CrearPago(p.Indice, factura.Total));
                if (p.Destino == Destino.Anulada) _s.Facturacion.Anular(factura.Id, "Datos de facturación incorrectos");
                break;
            case Destino.Parcial:
                _s.Facturacion.RegistrarPago(factura.Id, CrearPago(p.Indice, Constantes.Redondear(factura.Total * 0.4m)));
                break;
        }
    }

    private static Pago CrearPago(int indice, decimal monto) => (indice % 3) switch
    {
        0 => new PagoEfectivo { Monto = monto, MontoRecibido = Math.Ceiling(monto / 50m) * 50m },
        1 => new PagoTarjeta { Monto = monto, UltimosCuatroDigitos = $"{4000 + indice * 37 % 6000:D4}", NumeroAutorizacion = $"AUT{100000 + indice * 911}" },
        _ => new PagoTransferencia { Monto = monto, Banco = indice % 2 == 0 ? "Banrural" : "Banco Industrial", NumeroReferencia = $"REF{500000 + indice * 313}" }
    };

    private static string MotivoCancelacion(int indice) => (indice % 3) switch
    {
        0 => "El huésped canceló por cambio de planes.",
        1 => "Viaje cancelado por el cliente.",
        _ => "No se recibió confirmación de pago."
    };

    private void LiberarLimpias(DateTime dia)
    {
        if (dia >= _hoy) return;
        foreach (var id in _enLimpieza.Where(kv => kv.Value <= dia.AddDays(-1)).Select(kv => kv.Key).ToList())
        {
            _s.Habitaciones.Liberar(id);
            _enLimpieza.Remove(id);
        }
    }

    private void PonerUnaEnMantenimiento()
    {
        var conReserva = _contexto.Reservas.Buscar(r => r.EstaActiva).Select(r => r.HabitacionId).ToHashSet();
        var libre = _s.Habitaciones.Buscar().FirstOrDefault(h =>
            h.Estado == EstadoHabitacion.Disponible && !conReserva.Contains(h.Id) && h.Tipo == TipoHabitacion.Doble);
        if (libre is not null) _s.Habitaciones.CambiarEstado(libre.Id, EstadoHabitacion.Mantenimiento);
    }

    private void Reloj(DateTime momento) => _reloj.Ahora = momento > _ahora ? _ahora : momento;

    private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
}
