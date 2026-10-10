# PROMPT — Sistema de Reservas de Hotel «Hotel Aurora»
*C# · .NET · Windows Forms · JSON · xUnit · Proyecto Integrador Programación II (USPG)*

---

## 0. Cómo debes trabajar

Actúa como desarrollador senior de C# y arquitecto de software. Construirás, **por fases**, el sistema descrito en este documento. Responde siempre en español.

- **Desarrolla el sistema completo en una sola ejecución.** Recorre las fases 0 a 9 de la sección 12 en orden y **sin detenerte a pedir confirmación**. Al cerrar cada fase: compila, ejecuta las pruebas, corrige cualquier error y haz un **commit local** con el mensaje de la fase en formato Conventional Commits, por ejemplo `feat(reservas): validar solapamiento de fechas`. No hagas push. Entre fases escribe solo una línea de avance (`✔ Fase 3 completa — 42 pruebas en verde`).
- Detente únicamente si algo te bloquea de verdad (por ejemplo, un SDK faltante): explica qué necesitas y sigue en cuanto se resuelva. Si la sesión se interrumpe y recibes «continúa», retoma desde la primera fase incompleta sin rehacer lo terminado.
- Al terminar, entrega un **informe final** con:
  - cómo ejecutar la aplicación y las pruebas;
  - credenciales de prueba;
  - resultado de las pruebas (total y en verde);
  - extras implementados;
  - pendientes o limitaciones;
  - lista de commits realizados por fase.
- No agregues funcionalidades fuera de esta especificación. Si algo es ambiguo, pregunta antes de suponer.
- Mantén la **trazabilidad**: Problema → Requisitos → UML → Clases → Código C# → Interfaz → Archivos → Pruebas. Cada clase y método público lleva comentario XML que cita el requisito o regla que implementa (`/// <remarks>RF-12, RN-03</remarks>`). Si cambias un nombre, actualiza `docs/uml/clases.puml` y `docs/04-trazabilidad.md` en la misma fase.
- Prioriza código **claro, conciso y eficiente** sobre código ingenioso.

## 1. Contexto y evaluación

Ponderación: Análisis 10 % · UML 25 % · C# y POO 25 % · Interfaz 10 % · Archivos 10 % · Presentación 10 % · GitHub 10 %.
El objetivo no es solo que el programa funcione: el código debe ser consecuencia visible del análisis y del UML.

## 2. Stack y restricciones

- .NET 8 o .NET 10 (el LTS instalado), C# moderno, `Nullable` habilitado, `ImplicitUsings`, namespaces de archivo.
- Interfaz: **Windows Forms**. Persistencia: **archivos JSON** con `System.Text.Json`. Pruebas: **xUnit**.
- Sin base de datos, sin Entity Framework, sin paquetes NuGet de interfaz de terceros: los gráficos y controles se dibujan con GDI+. Solo los extras aprobados (sección 11) pueden añadir paquetes.
- Cultura `es-GT`: moneda en quetzales (`Q 1,250.00`), fechas `dd/MM/yyyy`.
- `decimal` para dinero, nunca `double`. Fechas de reserva comparadas con `.Date`.
- `<ApplicationHighDpiMode>SystemAware</ApplicationHighDpiMode>` en el proyecto de UI.

## 3. Estructura de la solución

```
HotelReservas.sln
├─ src/Hotel.Core/            Biblioteca de clases (net8.0) — sin referencias a WinForms
│  ├─ Comun/                  Resultado, ReglaNegocioException, Validador, Constantes
│  ├─ Dominio/                Entidades y enums
│  ├─ Interfaces/             IRepositorio<T>, IValidable, IReloj
│  ├─ Datos/                  JsonConfig, RepositorioJson<T>, ContextoDatos, DatosSemilla
│  └─ Servicios/              Lógica de negocio (un servicio por módulo)
├─ src/Hotel.UI/              Windows Forms (net8.0-windows) — referencia a Hotel.Core
│  ├─ Tema/                   Paleta, Tipografia, Estilizador
│  ├─ Controles/              Controles personalizados dibujados con GDI+
│  ├─ Vistas/                 Un UserControl por módulo (se cargan dentro de la ventana principal)
│  ├─ Formularios/            Diálogos de registro, edición y detalle
│  └─ Program.cs              Composición: crea ContextoDatos y servicios, los inyecta por constructor
├─ tests/Hotel.Tests/         xUnit — referencia a Hotel.Core
├─ docs/                      Análisis, requisitos, casos de uso, clases, trazabilidad
│  └─ uml/                    Diagramas PlantUML (.puml)
├─ datos-prueba/              Copia de los JSON generados por la semilla
├─ .gitignore                 Plantilla VisualStudio
└─ README.md
```

**Regla de dependencias:** `UI → Core`. Los formularios **no contienen lógica de negocio**: validan formato, llaman a un servicio y muestran su `Resultado`.

## 4. Actores

| Actor | Descripción |
|---|---|
| Recepcionista | Gestiona huéspedes, reservas, check-in/check-out, cargos adicionales y pagos. |
| Administrador | Todo lo del recepcionista + habitaciones, usuarios y anulación de facturas. |
| Huésped (secundario) | No usa el sistema; es el sujeto de reservas, estadías y facturas. |

## 5. Requisitos funcionales

| ID | Módulo | Requisito |
|---|---|---|
| RF-01 | Acceso | Iniciar y cerrar sesión con usuario y contraseña; el menú se adapta al rol. |
| RF-02 | Acceso | Gestionar usuarios (registrar, editar, desactivar, restablecer contraseña). Solo Administrador. |
| RF-03 | Habitaciones | Registrar habitación: número único, piso, tipo, capacidad, tarifa por noche, descripción. |
| RF-04 | Habitaciones | Editar habitación. |
| RF-05 | Habitaciones | Eliminar habitación sin reservas, o desactivarla si tiene historial. |
| RF-06 | Habitaciones | Consultar y filtrar por tipo, estado, piso y texto. |
| RF-07 | Habitaciones | Cambiar estado operativo (Disponible ↔ Mantenimiento) y liberar (Limpieza → Disponible). |
| RF-08 | Huéspedes | Registrar huésped con documento único (DPI o pasaporte), datos personales y NIT. |
| RF-09 | Huéspedes | Editar huésped. |
| RF-10 | Huéspedes | Eliminar huésped sin reservas, o desactivarlo si tiene historial. |
| RF-11 | Huéspedes | Buscar por nombre o documento. |
| RF-12 | Huéspedes | Consultar historial de estadías (fechas, habitación, total facturado). |
| RF-13 | Reservas | Consultar disponibilidad por rango de fechas, cantidad de personas y tipo. |
| RF-14 | Reservas | Crear reserva (Pendiente o Confirmada) validando reglas RN-02 a RN-04. |
| RF-15 | Reservas | Modificar fechas, habitación o personas de una reserva Pendiente/Confirmada. |
| RF-16 | Reservas | Confirmar reserva. |
| RF-17 | Reservas | Cancelar reserva con motivo obligatorio. |
| RF-18 | Reservas | Listar y filtrar reservas por estado, rango de fechas y huésped. |
| RF-19 | Check-in/out | Realizar check-in con asignación o reasignación de habitación. |
| RF-20 | Check-in/out | Registrar cargos adicionales (catálogo de servicios) durante la estadía. |
| RF-21 | Check-in/out | Realizar check-out: genera la factura y deja la habitación en Limpieza. |
| RF-22 | Facturación | Generar factura: alojamiento + servicios adicionales − descuento + impuestos. |
| RF-23 | Facturación | Registrar pagos (efectivo, tarjeta, transferencia), incluidos pagos parciales. |
| RF-24 | Facturación | Anular factura con motivo (solo Administrador). |
| RF-25 | Facturación | Consultar historial de facturación con filtros y totales del periodo. |
| RF-26 | Facturación | Vista previa e impresión de factura. |

## 6. Requisitos no funcionales

| ID | Requisito |
|---|---|
| RNF-01 | Persistencia en archivos JSON legibles (indentados, enums como texto, tildes sin escapar). |
| RNF-02 | Los datos permanecen al cerrar y volver a abrir la aplicación. |
| RNF-03 | Escritura atómica; un archivo dañado no cierra la aplicación (se respalda y se avisa). |
| RNF-04 | Respuesta menor a 1 s con cientos de registros. |
| RNF-05 | Interfaz consistente con la paleta de la sección 10; usable a 1280×720. |
| RNF-06 | Contraseñas almacenadas con hash PBKDF2 (SHA-256, sal aleatoria, 100 000 iteraciones). |
| RNF-07 | Arquitectura en capas; formularios sin lógica de negocio. |
| RNF-08 | Código comentado en español con referencias a RF/RN. |
| RNF-09 | Ningún error inesperado muestra trazas técnicas: mensaje amigable y registro en `Datos/errores.log`. |

## 7. Modelo de dominio (base del diagrama de clases)

**Enums:** `TipoDocumento {Dpi, Pasaporte}` · `RolUsuario {Administrador, Recepcionista}` · `TipoHabitacion {Sencilla, Doble, Suite, Familiar}` · `EstadoHabitacion {Disponible, Ocupada, Limpieza, Mantenimiento}` · `EstadoReserva {Pendiente, Confirmada, EnCurso, Finalizada, Cancelada}` · `CategoriaServicio {Restaurante, Minibar, Lavanderia, Spa, Transporte, Otro}` · `EstadoFactura {Emitida, Pagada, Anulada}`

```
EntidadBase (abstracta) : IValidable
  int Id · DateTime FechaCreacion · bool Activo = true
  abstract IEnumerable<string> Validar()

Persona : EntidadBase (abstracta)
  Nombres · Apellidos · TipoDocumento · NumeroDocumento · Telefono · Correo
  NombreCompleto (calculada)
  override Validar()  → validaciones comunes; las clases hijas llaman a base.Validar()

Huesped : Persona
  Nacionalidad · FechaNacimiento · Nit (por defecto "CF") · Notas
  override Validar()  → + mayor de 18 años, NIT válido

Usuario : Persona
  NombreUsuario · Rol · HashContrasena [private set] · Sal [private set]
  EstablecerContrasena(texto) · VerificarContrasena(texto) : bool

Habitacion : EntidadBase
  Numero · Piso · Tipo · Capacidad · TarifaNoche · Descripcion
  Estado [private set] · CambiarEstado(nuevo)  → valida transición (RN-10)

Reserva : EntidadBase
  Codigo · HuespedId · HabitacionId · FechaEntrada · FechaSalida · CantidadPersonas
  TarifaPactada (copia de la tarifa al reservar) · Estado [private set] · MotivoCancelacion
  FechaCheckIn? · FechaCheckOut? · Cargos : IReadOnlyList<CargoAdicional>
  Noches · TotalAlojamiento · TotalCargos (calculadas)
  Confirmar() · Cancelar(motivo) · RegistrarCheckIn(fecha) · RegistrarCheckOut(fecha)
  AgregarCargo(cargo) · CambiarFechas(entrada, salida, personas) · ReasignarHabitacion(id, tarifa)
  SeSolapaCon(entrada, salida) : bool

ServicioAdicional : EntidadBase      Nombre · Categoria · Precio
CargoAdicional (objeto de valor)     ServicioId · Descripcion · Cantidad · PrecioUnitario · Fecha · Subtotal
LineaFactura (objeto de valor)       Concepto · Cantidad · PrecioUnitario · Subtotal

Factura : EntidadBase
  Numero · ReservaId · HuespedId · NitCliente · NombreCliente · FechaEmision
  Lineas · PorcentajeDescuento · Subtotal · Descuento · Iva · ImpuestoTurismo · Total
  Estado [private set] · MotivoAnulacion · Pagos : IReadOnlyList<Pago> · TotalPagado · Saldo
  static Generar(reserva, habitacion, huesped, configuracion, porcentajeDescuento, fecha) : Factura
  RegistrarPago(pago) · Anular(motivo)

Pago (abstracta) : IValidable        Monto · Fecha · abstract string Metodo · abstract string Detalle()
  PagoEfectivo        MontoRecibido · Cambio (calculado)
  PagoTarjeta         UltimosCuatroDigitos · NumeroAutorizacion   (nunca guardar el número completo)
  PagoTransferencia   Banco · NumeroReferencia
  JSON polimórfico: [JsonPolymorphic(TypeDiscriminatorPropertyName = "tipo")] + [JsonDerivedType]

ConfiguracionHotel
  NombreHotel · Nit · Direccion · Telefono · PorcentajeIva (12) · PorcentajeTurismo (10)
  MaxNochesPorReserva (30) · DescuentoMaximo (30)
```

**Interfaces**

```
IValidable            IEnumerable<string> Validar()
IRepositorio<T>       where T : EntidadBase
  IReadOnlyList<T> ObtenerTodos(bool incluirInactivos = false)
  T? ObtenerPorId(int id)
  IEnumerable<T> Buscar(Func<T, bool> criterio)
  void Agregar(T entidad)      // asigna Id y persiste
  void Actualizar(T entidad)   // persiste
  void Eliminar(int id)        // físico; el servicio decide si desactiva en su lugar
IReloj                DateTime Ahora { get; } · DateTime Hoy => Ahora.Date
  RelojSistema (producción) · RelojFijo (pruebas y semilla, Ahora modificable)
```

**Servicios** (reciben repositorios, `IReloj` y `ConfiguracionHotel` por constructor; devuelven `Resultado` / `Resultado<T>` con `Exito`, `Mensaje`, `Errores`, `Valor`; capturan `ReglaNegocioException` y la convierten en `Resultado.Fallo`):

- `AutenticacionServicio` — IniciarSesion, CerrarSesion, UsuarioActual
- `UsuarioServicio` — Registrar, Editar, Desactivar, RestablecerContrasena
- `HabitacionServicio` — Registrar, Editar, Eliminar, Buscar(filtro), CambiarEstado, Liberar
- `HuespedServicio` — Registrar, Editar, Eliminar, Buscar(texto), HistorialEstadias
- `ReservaServicio` — BuscarDisponibles, Crear, Modificar, Confirmar, Cancelar, Buscar(filtro), LlegadasDelDia, HuespedesEnCasa
- `EstadiaServicio` — CheckIn(reservaId, habitacionId?), AgregarCargo(reservaId, servicioId, cantidad), CheckOut(reservaId, porcentajeDescuento) → `Resultado<Factura>`
- `FacturacionServicio` — RegistrarPago, Anular, Buscar(filtro), ResumenPeriodo(desde, hasta)

## 8. Reglas de negocio

| ID | Regla |
|---|---|
| RN-01 | Únicos: número de habitación, documento de huésped, nombre de usuario. |
| RN-02 | Reserva: entrada ≥ hoy (al crear), salida > entrada, máximo `MaxNochesPorReserva` noches, personas ≤ capacidad. |
| RN-03 | Solapamiento: una reserva activa (Pendiente, Confirmada, EnCurso) bloquea la habitación si `entradaA < salidaB && entradaB < salidaA`. La salida de una reserva el mismo día de la entrada de otra **no** se solapa. |
| RN-04 | Habitaciones en Mantenimiento o inactivas no se ofrecen para reservar. |
| RN-05 | Transiciones de reserva: Pendiente → Confirmada → EnCurso → Finalizada; Pendiente/Confirmada → Cancelada (motivo obligatorio). Cualquier otra transición lanza `ReglaNegocioException`. |
| RN-06 | Modificar solo reservas Pendiente/Confirmada, revalidando RN-02 a RN-04 y excluyéndose a sí misma. |
| RN-07 | Check-in: reserva Confirmada y hoy dentro de [entrada, salida); habitación Disponible. Puede reasignarse a otra habitación disponible con capacidad suficiente (se actualiza la tarifa pactada). Habitación → Ocupada. |
| RN-08 | Cargos adicionales solo en reservas EnCurso, cantidad > 0. |
| RN-09 | Check-out: solo EnCurso; noches cobradas = máx(1, días reales entre check-in y check-out); genera factura; reserva → Finalizada; habitación → Limpieza. |
| RN-10 | Estado de habitación: Ocupada solo cambia por check-in/check-out; Limpieza → Disponible (liberar); Disponible ↔ Mantenimiento (manual). «Reservada hoy» no se guarda: se calcula a partir de las reservas. |
| RN-11 | Factura: Subtotal = alojamiento + cargos; Descuento = Subtotal × %desc (0 – `DescuentoMaximo`); Base = Subtotal − Descuento; IVA = Base × %IVA; Impuesto de turismo = Alojamiento × (1 − %desc) × %Turismo; Total = Base + IVA + Impuesto de turismo. Redondeo a 2 decimales (`MidpointRounding.AwayFromZero`). |
| RN-12 | Pagos: monto > 0 y ≤ saldo. Efectivo exige monto recibido ≥ monto (calcula cambio); tarjeta exige 4 dígitos y autorización; transferencia exige banco y referencia. Saldo 0 → Pagada. |
| RN-13 | Anulación: solo Administrador, motivo obligatorio; una factura anulada no admite pagos y nunca se elimina. |
| RN-14 | No se elimina físicamente un huésped o una habitación con reservas: se desactiva. |
| RN-15 | Correlativos: `RES-000001`, `FAC-000001`. |

**Validaciones (`Validador`, estático):**
- DPI: 13 dígitos. Pasaporte: 6–20 caracteres alfanuméricos.
- NIT: `CF` o dígitos + verificador (0–9 o K). Cálculo módulo 11: multiplicar los dígitos sin el verificador, de izquierda a derecha, por factores que van de (n + 1) hasta 2; `r = (11 − suma % 11) % 11`; si `r = 10` el verificador es `K`.
- Correo con `MailAddress.TryCreate`. Teléfono: 8–15 dígitos, `+` inicial opcional. Nombres: 2–60 caracteres, letras (con tildes y ñ) y espacios.
- Contraseña: mínimo 8 caracteres, una mayúscula y un número.

## 9. Persistencia en JSON

- Carpeta `Datos/` junto al ejecutable (`AppContext.BaseDirectory`), creada si no existe. La ruta se recibe en `ContextoDatos`, de modo que las pruebas usen una carpeta temporal.
- Un archivo por colección: `usuarios.json`, `habitaciones.json`, `huespedes.json`, `servicios.json`, `reservas.json`, `facturas.json`, `configuracion.json`.
- `JsonConfig`: `WriteIndented`, camelCase, `JsonStringEnumConverter`, `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` (tildes legibles).
- `RepositorioJson<T>`: carga una vez en memoria; Id = máximo + 1; cada escritura persiste de inmediato de forma atómica (serializar a `archivo.json.tmp` y `File.Move(tmp, archivo, overwrite: true)`).
- Archivo inexistente → colección vacía. JSON dañado → renombrar a `archivo.json.danado-yyyyMMddHHmmss`, iniciar vacío y avisar al usuario sin cerrar la aplicación.
- Encapsulamiento compatible con la serialización: propiedades `private set` llevan `[JsonInclude]`; colecciones expuestas como `IReadOnlyList` y modificadas solo por métodos. Cada entidad tiene una prueba de ida y vuelta.
- `DatosSemilla`: si no hay usuarios, genera datos de prueba **usando los servicios** (con `RelojFijo` avanzando en el tiempo) para que todo sea consistente:
  - Usuarios `admin / Admin123*` y `recepcion / Recepcion123*`.
  - 20 habitaciones en 4 pisos, todos los tipos, tarifas entre Q350 y Q1,500.
  - 15 huéspedes con nombres guatemaltecos y DPI de 13 dígitos, más algunos extranjeros con pasaporte.
  - 10 servicios adicionales (desayuno, lavandería, spa, minibar, traslado al aeropuerto, etc.).
  - ~40 reservas repartidas en los últimos 8 meses y las próximas 2 semanas, en todos los estados, con check-in/out, cargos, facturas y pagos (incluye pagos parciales y una factura anulada).

## 10. Interfaz gráfica

### 10.1 Estilo de referencia
Estilo neumórfico suave, como el dashboard de referencia (imagen en `docs/referencia/dashboard.png`; ábrela antes de diseñar la interfaz): fondo gris azulado claro, tarjetas claras con esquinas muy redondeadas y sombra difusa, barras en azul marino con las destacadas en degradado teal, gráfico de dona con segmentos marino, pizarra, azul y cian, pestaña activa en azul claro, tipografía limpia y mucho espacio en blanco.

### 10.2 Paleta (`Tema/Paleta.cs`)

| Token | Hex | Uso |
|---|---|---|
| Fondo | `#E6EBF1` | Fondo de la aplicación |
| Tarjeta | `#F4F7FA` | Tarjetas y paneles |
| Superficie | `#FFFFFF` | Campos, tabla, chips |
| Sombra | `#A3B1C6` (alfa 35–60) | Sombra difusa de tarjetas |
| Marino | `#2E3A50` | Barra lateral, títulos, barras de gráfico |
| MarinoHover | `#3B4963` | Hover y botón activo de la barra lateral |
| Teal | `#1FB2B5` | Acento principal, botón primario, éxito |
| TealClaro | `#4FD1D3` | Degradado de barras destacadas, hover primario |
| Cian | `#38D3E0` | Serie de gráfico |
| Azul | `#3B8FD9` | Serie de gráfico, información |
| AzulPestana | `#86B9EA` | Pestaña o segmento activo |
| Pizarra | `#5A6E8C` | Serie de gráfico, texto terciario |
| TextoPrincipal | `#1F2A3C` | Texto principal |
| TextoSecundario | `#7A8699` | Etiquetas y ayudas |
| Borde | `#D5DCE5` | Bordes de campos |
| SeleccionTabla | `#D6F3F4` | Fila seleccionada |
| Advertencia | `#F2A93B` | Advertencias |
| Error | `#E25C5C` | Errores y acciones destructivas |

**Colores de estado (insignias tipo «pill»):** Habitación: Disponible = Teal, Ocupada = Marino, Reservada hoy = Azul, Limpieza = Pizarra, Mantenimiento = Advertencia. Reserva: Pendiente = Advertencia, Confirmada = Azul, EnCurso = Teal, Finalizada = Pizarra, Cancelada = Error. Factura: Emitida = Azul, Pagada = Teal, Anulada = Error.

**Tipografía:** Segoe UI 10 (texto), Segoe UI Semibold 11 (etiquetas), 16 (título de tarjeta), 20 (título de vista), 22 Bold (valores destacados). Íconos con la fuente `Segoe MDL2 Assets` (sin archivos de imagen). Radio de esquinas 14 px, padding de tarjeta 16 px, separación 12 px.

### 10.3 Controles personalizados (`Controles/`, `DoubleBuffered`, `SmoothingMode.AntiAlias`)
- `PanelTarjeta`: esquinas redondeadas y sombra difusa dibujada **dentro** de sus límites (margen interno de 6 px) para evitar artefactos.
- `BotonModerno`: variantes Primario (Teal), Secundario (Superficie + borde), Peligro (Error) y Fantasma; esquinas redondeadas, estados hover/presionado, cursor de mano, ícono opcional.
- `InsigniaEstado`: pill de color según estado (también pintada en tablas mediante `CellPainting`).
- `Estilizador.AplicarTabla(DataGridView)`: sin bordes ni encabezados de fila, alto de fila 36, encabezado con texto Marino semibold, filas alternas, selección `SeleccionTabla` con texto oscuro, `FullRowSelect`, solo lectura.
- `Estilizador.AplicarCampos(...)`: TextBox, ComboBox y DateTimePicker planos y consistentes.
- Los controles deben aparecer en el cuadro de herramientas y los formularios deben abrirse en el Diseñador de Visual Studio (disposición en `.Designer.cs`, lógica en `.cs`).

### 10.4 Ventanas y navegación
- **FormLogin** (420×520, centrado): tarjeta con nombre del hotel, usuario, contraseña con mostrar/ocultar, botón Ingresar, Enter para enviar, error en línea.
- **FormPrincipal** (mínimo 1280×720): barra lateral Marino de 220 px con nombre del hotel, usuario y rol, y botones de navegación con ícono (activo: fondo MarinoHover + indicador Teal de 4 px a la izquierda). Encabezado con título de la vista y fecha de hoy. Área de contenido (Fondo) donde se carga el UserControl de la vista activa, sin abrir ventanas nuevas.
- **Menú:** Inicio · Habitaciones · Huéspedes · Reservas · Recepción · Facturación · Usuarios (solo Admin) · Cerrar sesión.

### 10.5 Vistas
- **`VistaListadoBase`** (UserControl base que heredan las vistas de listado): buscador que filtra al escribir, zona de filtros, botones Nuevo / Editar / Eliminar o Desactivar / Ver detalle, tabla estilizada, contador de registros, doble clic = editar.
- **`FormularioBase`** (Form base de los diálogos): estilo común, botones Guardar y Cancelar (`AcceptButton`/`CancelButton`), `ErrorProvider`.
- **Inicio:** si el extra E1 no se aprueba, mostrar 4 tarjetas de resumen (ocupación de hoy, llegadas de hoy, huéspedes en casa, saldo por cobrar).
- **Habitaciones:** filtros por tipo, estado y piso; acciones registrar, editar, eliminar/desactivar, poner en mantenimiento, liberar.
- **Huéspedes:** búsqueda por nombre o documento; detalle con pestaña «Historial de estadías».
- **Reservas:** filtros por estado y rango de fechas. `FormReserva` en cuatro secciones: (1) fechas, personas y tipo → «Buscar disponibilidad»; (2) habitaciones disponibles con total estimado; (3) huésped: buscar o «Nuevo huésped»; (4) resumen y guardar como Pendiente o Confirmada. Acciones: modificar, confirmar, cancelar con motivo.
- **Recepción:** tres listas — «Llegadas de hoy» (check-in), «Huéspedes en casa» (agregar cargo, check-out) y «Por limpiar» (liberar). El check-out muestra el estado de cuenta, permite aplicar descuento y abre la factura generada.
- **Facturación:** historial con filtros (fechas, estado, huésped/NIT) y totales del periodo; detalle con líneas, totales, pagos y saldo; `FormPago` cambia sus campos según el método elegido; anular con motivo; vista previa e impresión con `PrintDocument` + `PrintPreviewDialog`.
- **Usuarios** (solo Admin): registrar, editar, desactivar y restablecer contraseña.

### 10.6 Mensajes y validación
- `DialogoMensaje` con el tema de la aplicación (Información, Éxito, Advertencia, Error, Confirmación), expuesto mediante un helper `Mensajes`.
- Confirmación antes de eliminar, desactivar, cancelar, anular, hacer check-out y cerrar sesión.
- Validación en tiempo real con `ErrorProvider`; al guardar se muestran también los errores devueltos por el servicio.
- Manejador global de excepciones en `Program.cs` (`Application.ThreadException` y `AppDomain.UnhandledException`) con mensaje amigable y registro en `Datos/errores.log`.

## 11. Extras (implementa solo los marcados con [x])

- [x] **E1 · Dashboard «Inicio» con el estilo de la imagen de referencia:** 4 tarjetas KPI (ocupación de hoy, ingresos del mes, reservas activas, llegadas de hoy); tarjeta «Ingresos» con selector segmentado (Ingresos | Reservas | Ocupación) y gráfico de barras de 12 meses (meses anteriores en Marino, los 3 últimos en degradado Teal → TealClaro con etiqueta de valor encima); tarjeta «Habitaciones» con gráfico de dona (total al centro, leyenda con conteo y %) y barras de progreso «Cobrado vs. Pendiente»; tarjeta «Saldo por cobrar». Controles `GraficoBarras`, `GraficoDona`, `TarjetaKpi` y `BarraProgreso` con GDI+. Datos desde un `ReporteServicio`.
- [ ] **E2 · Mapa de habitaciones** en Recepción: mosaico por piso, cada habitación es una tarjeta del color de su estado; menú contextual con check-in, liberar y mantenimiento.
- [ ] **E3 · Exportación:** factura en PDF (QuestPDF) y tablas a CSV mediante `IExportador` → `ExportadorCsv`, `ExportadorPdf`.
- [ ] **E4 · Bitácora de auditoría:** `BitacoraServicio` registra usuario, fecha, acción y entidad en `bitacora.json`; vista de solo lectura para Admin.
- [x] **E5 · Integración continua:** GitHub Actions (`windows-latest`) que compila y ejecuta las pruebas en cada push y pull request; insignia de estado en el README.
- [x] **E6 · Diagramas de estados UML** de la Reserva y de la Habitación.
- [ ] **E7 · Respaldo y restauración:** comprimir la carpeta `Datos/` en `.zip` y restaurar desde uno.
- [ ] **E8 · Notificaciones «toast»** no bloqueantes para mensajes de éxito.

## 12. Fases de desarrollo (todas en una sola ejecución)

| Fase | Contenido | Entregable verificable |
|---|---|---|
| 0 | Análisis y UML (sección 13) **antes de escribir código** | Documentos en `docs/` y `.puml` en `docs/uml/` |
| 1 | Solución, 3 proyectos, `.gitignore`, README inicial; Core: Comun, Interfaces, Dominio | Compila; pruebas de dominio en verde |
| 2 | Datos: JsonConfig, RepositorioJson, ContextoDatos | Pruebas de persistencia en verde |
| 3 | Servicios, reglas de negocio y DatosSemilla | Pruebas de servicios en verde; JSON de semilla generados |
| 4 | UI base: Tema, controles, DialogoMensaje, VistaListadoBase, FormularioBase, FormLogin, FormPrincipal | Login y navegación funcionando con el tema |
| 5 | Habitaciones, Huéspedes y Usuarios | CRUD completo con validación |
| 6 | Reservas y Recepción (check-in, cargos, check-out, liberar) | Flujo completo de una estadía |
| 7 | Facturación (detalle, pagos, anulación, impresión) | Factura pagada e impresa en vista previa |
| 8 | Extras aprobados | Según sección 11 |
| 9 | Cierre: UML y trazabilidad sincronizados con el código final, plan de pruebas, JSON semilla copiados a `datos-prueba/`, README completo, checklist de la sección 16, informe final | Todo el checklist en verde |

## 13. Documentación (Fase 0)

- `docs/01-analisis.md` — Situación actual (hotel mediano que gestiona reservas en cuadernos y hojas de cálculo: dobles reservas, cargos sin cobrar, sin historial de huéspedes), problema identificado, propuesta de solución, objetivo general, objetivos específicos, alcance y limitaciones (aplicación local monousuario, sin red, sin pagos en línea, sin factura electrónica FEL de la SAT).
- `docs/02-requisitos.md` — Tablas de RF, RNF y reglas de negocio (secciones 5, 6 y 8).
- `docs/03-casos-de-uso.md` — Actores y especificación de: CU-01 Iniciar sesión, CU-02 Gestionar habitaciones, CU-03 Gestionar huéspedes, CU-04 Crear reserva, CU-05 Modificar/cancelar reserva, CU-06 Realizar check-in, CU-07 Registrar cargo adicional, CU-08 Realizar check-out, CU-09 Registrar pago, CU-10 Anular factura, CU-11 Consultar historiales, CU-12 Gestionar usuarios. Cada uno con actor, precondición, flujo principal, flujos alternos, postcondición y RF relacionados.
- `docs/04-trazabilidad.md` — Matriz: RF | CU | Clases de dominio | Servicio.Método | Vista/Formulario | Archivo JSON | Prueba(s).
- `docs/05-clases.md` — Descripción de las principales clases: responsabilidad, atributos clave y relaciones.
- `docs/06-plan-de-pruebas.md` (se completa en la fase 9 con los nombres definitivos):
  - **(a) Pruebas automatizadas:** tabla con clase de prueba, qué verifica y RF/RN relacionados.
  - **(b) Casos de prueba manuales** CP-01, CP-02… que cubran todos los RF. Columnas:
    - ID;
    - RF;
    - precondición;
    - pasos;
    - datos de entrada;
    - resultado esperado;
    - «Resultado obtenido» y «Evidencia», vacías para completar con capturas.
  - Incluye casos de error: validaciones, solapamiento de reservas, pago mayor al saldo, anulación sin permiso.
- `docs/uml/` (PlantUML):
  - `casos-de-uso.puml` — actores, CU y relaciones include/extend.
  - `clases.puml` — paquetes Dominio, Interfaces, Datos y Servicios; herencia, implementación, asociación y composición con multiplicidades. Debe coincidir con el código.
  - `secuencia-crear-reserva.puml` — Recepcionista → FormReserva → ReservaServicio → IRepositorio → archivo JSON, con el caso alterno de solapamiento.
  - `secuencia-check-out.puml` — check-out con generación de factura y cambio de estados.
  - `actividad-estadia.puml` — con carriles Recepcionista / Sistema: reserva → check-in → cargos → check-out → pago.

## 14. Pruebas (xUnit)

Carpetas `Dominio/`, `Datos/`, `Servicios/`. Nombres `Metodo_Escenario_ResultadoEsperado`. Usar `RelojFijo` y carpeta temporal (limpieza con `IDisposable`). Mínimo:
- Reserva: transiciones válidas e inválidas; cálculo de noches; `SeSolapaCon` con casos borde (salida = entrada no se solapa).
- ReservaServicio: rechaza solapamiento, capacidad excedida, fecha pasada y habitación en mantenimiento; al modificar se excluye a sí misma.
- EstadiaServicio: check-in cambia estados; check-out genera la factura correcta y deja la habitación en Limpieza.
- Factura: cálculos de RN-11 con y sin descuento; pagos parciales; pago mayor al saldo rechazado; anulación bloquea pagos.
- Validador: DPI, NIT (verificador numérico y K), correo, teléfono, contraseña.
- RepositorioJson: ida y vuelta de cada entidad (incluido `Pago` polimórfico y propiedades `private set`), Id autoincremental, archivo dañado sin excepción y con respaldo.
- Usuario: hash y verificación de contraseña.

## 15. Estándares de código

- Nombres del dominio en español; PascalCase para tipos y miembros; `_camelCase` para campos privados; una clase por archivo con el mismo nombre.
- Métodos cortos (≈ 25 líneas), cláusulas de guarda, sin duplicación, sin números mágicos (usar `Constantes` o `ConfiguracionHotel`).
- Comentarios XML en español en clases y miembros públicos, con `<remarks>` que citen RF/RN.
- Excepciones solo para reglas de negocio violadas (`ReglaNegocioException`) y errores inesperados; la UI nunca muestra trazas técnicas.
- LINQ legible; no repetir consultas dentro de bucles.
- Eventos con nombres descriptivos (`BtnGuardar_Click`).

## 16. Checklist de aceptación final

- [ ] La solución compila sin errores y todas las pruebas pasan.
- [ ] Al borrar `Datos/` y ejecutar, la semilla se regenera; los datos persisten al cerrar y reabrir.
- [ ] Cada RF aparece en la matriz con su CU, clase, vista, archivo JSON y prueba.
- [ ] `clases.puml` coincide con el código (nombres, propiedades, métodos y relaciones).
- [ ] Cada módulo tiene menú, registro, consulta, edición, eliminación/desactivación, navegación, confirmaciones, mensajes de error y validación.
- [ ] POO visible y con propósito: encapsulamiento (`private set` + métodos de transición), herencia (`EntidadBase`, `Persona`, `Pago`, `VistaListadoBase`, `FormularioBase`), polimorfismo (`Validar`, `Pago.Detalle`), interfaces (`IRepositorio<T>`, `IValidable`, `IReloj`).
- [ ] Paleta aplicada en todas las pantallas; ventana usable a 1280×720.
- [ ] README con descripción, cómo ejecutar la aplicación y `dotnet test`, estructura, credenciales de prueba, espacio para capturas, insignia de GitHub Actions y tabla «Equipo» (nombre y rol, por completar).
- [ ] `docs/06-plan-de-pruebas.md` cubre todos los RF.
- [ ] Informe final entregado (sección 0).
