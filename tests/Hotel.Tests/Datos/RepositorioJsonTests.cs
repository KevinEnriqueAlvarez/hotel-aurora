using Hotel.Core.Datos;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Datos;

/// <summary>Persistencia, Id autoincremental, aislamiento y archivos dañados (RNF-01..RNF-04).</summary>
public sealed class RepositorioJsonTests : IDisposable
{
    private readonly CarpetaTemporal _carpeta = new();

    public void Dispose() => _carpeta.Dispose();

    private static Habitacion Hab(string numero) =>
        new() { Numero = numero, Piso = 1, Capacidad = 2, TarifaNoche = 400m, Descripcion = "Vista al jardín" };

    private RepositorioJson<Habitacion> Abrir(Action<string>? aviso = null) =>
        new(_carpeta.Archivo("habitaciones.json"), aviso);

    [Fact]
    public void Agregar_VariasEntidades_AsignaIdMaximoMasUno()
    {
        var repo = Abrir();
        var a = Hab("101");
        var b = Hab("102");
        repo.Agregar(a);
        repo.Agregar(b);
        Assert.Equal(1, a.Id);
        Assert.Equal(2, b.Id);
    }

    [Fact]
    public void Agregar_TrasEliminarLaUltima_ReutilizaMaximoMasUno()
    {
        var repo = Abrir();
        repo.Agregar(Hab("101"));
        repo.Agregar(Hab("102"));
        repo.Eliminar(2);
        var c = Hab("103");
        repo.Agregar(c);
        Assert.Equal(2, c.Id);
    }

    [Fact]
    public void ObtenerTodos_ReabrirArchivo_ConservaLosDatos()
    {
        var repo = Abrir();
        repo.Agregar(Hab("101"));
        repo.Agregar(Hab("102"));

        var reabierto = Abrir();

        Assert.Equal(2, reabierto.ObtenerTodos().Count);
        Assert.Equal("Vista al jardín", reabierto.ObtenerPorId(1)!.Descripcion);
    }

    [Fact]
    public void ObtenerTodos_ExcluyeInactivasPorDefecto()
    {
        var repo = Abrir();
        var h = Hab("101");
        repo.Agregar(h);
        h.Activo = false;
        repo.Actualizar(h);

        Assert.Empty(repo.ObtenerTodos());
        Assert.Single(repo.ObtenerTodos(incluirInactivos: true));
    }

    [Fact]
    public void ObtenerPorId_ModificarCopia_NoAfectaAlRepositorio()
    {
        var repo = Abrir();
        repo.Agregar(Hab("101"));

        var copia = repo.ObtenerPorId(1)!;
        copia.Numero = "999";

        Assert.Equal("101", repo.ObtenerPorId(1)!.Numero);
    }

    [Fact]
    public void Actualizar_CambiaElRegistroYPersiste()
    {
        var repo = Abrir();
        var h = Hab("101");
        repo.Agregar(h);
        h.TarifaNoche = 777m;
        repo.Actualizar(h);

        Assert.Equal(777m, Abrir().ObtenerPorId(1)!.TarifaNoche);
    }

    [Fact]
    public void Actualizar_IdInexistente_LanzaKeyNotFound()
    {
        var repo = Abrir();
        Assert.Throws<KeyNotFoundException>(() => repo.Actualizar(new Habitacion { Id = 99 }));
    }

    [Fact]
    public void Buscar_AplicaElCriterio()
    {
        var repo = Abrir();
        repo.Agregar(Hab("101"));
        repo.Agregar(Hab("202"));
        Assert.Single(repo.Buscar(h => h.Numero.StartsWith('2')));
    }

    [Fact]
    public void Eliminar_QuitaElRegistroDelArchivo()
    {
        var repo = Abrir();
        repo.Agregar(Hab("101"));
        repo.Eliminar(1);
        Assert.Empty(Abrir().ObtenerTodos(incluirInactivos: true));
    }

    [Fact]
    public void ArchivoInexistente_IniciaVacio() => Assert.Empty(Abrir().ObtenerTodos());

    [Fact]
    public void ArchivoDanado_NoLanzaYSeRespaldaYAvisa()
    {
        File.WriteAllText(_carpeta.Archivo("habitaciones.json"), "{ esto no es json ]");
        var avisos = new List<string>();

        var repo = Abrir(avisos.Add);

        Assert.Empty(repo.ObtenerTodos());
        Assert.Single(avisos);
        Assert.Single(Directory.GetFiles(_carpeta.Ruta, "habitaciones.json.danado-*"));
        Assert.False(File.Exists(_carpeta.Archivo("habitaciones.json")));
    }

    [Fact]
    public void ArchivoDanado_DespuesDeRespaldarSePuedeSeguirGuardando()
    {
        File.WriteAllText(_carpeta.Archivo("habitaciones.json"), "");
        var repo = Abrir(_ => { });
        repo.Agregar(Hab("101"));
        Assert.Single(Abrir().ObtenerTodos());
    }

    [Fact]
    public void Persistir_NoDejaArchivoTemporal()
    {
        Abrir().Agregar(Hab("101"));
        Assert.False(File.Exists(_carpeta.Archivo("habitaciones.json.tmp")));
    }

    [Fact]
    public void Archivo_UsaTildesLegiblesYEnumsComoTexto()
    {
        var h = Hab("101");
        h.Tipo = TipoHabitacion.Suite;
        Abrir().Agregar(h);

        var texto = File.ReadAllText(_carpeta.Archivo("habitaciones.json"));

        Assert.Contains("Vista al jardín", texto);
        Assert.Contains("\"Suite\"", texto);
        Assert.Contains("\"tipo\"", texto);
        Assert.Contains(Environment.NewLine, texto);
    }

    [Fact]
    public void ContextoDatos_CarpetaNueva_CreaConfiguracionPorDefecto()
    {
        var ctx = new ContextoDatos(Path.Combine(_carpeta.Ruta, "Datos"));
        Assert.Equal(12m, ctx.Configuracion.PorcentajeIva);
        Assert.True(File.Exists(Path.Combine(ctx.Carpeta, "configuracion.json")));
    }
}
