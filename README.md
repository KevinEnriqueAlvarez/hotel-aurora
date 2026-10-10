# Hotel Aurora — Sistema de Reservas

[![CI](https://github.com/USUARIO/hotel-aurora/actions/workflows/ci.yml/badge.svg)](https://github.com/USUARIO/hotel-aurora/actions/workflows/ci.yml)

Aplicación de escritorio para gestionar las **habitaciones, huéspedes, reservas, check-in/check-out, cargos y facturación** de un hotel mediano.
Proyecto Integrador de Programación II (USPG) — C# · .NET 10 · Windows Forms · JSON · xUnit.

> Reemplace `USUARIO` en la insignia por su usuario u organización de GitHub.

## Características

- Acceso por rol (Administrador y Recepcionista) con contraseñas PBKDF2.
- Habitaciones, huéspedes y usuarios con validación en tiempo real.
- Reservas con control de disponibilidad y solapamiento; confirmación y cancelación con motivo.
- Recepción: check-in con reasignación, cargos adicionales, check-out con factura y limpieza.
- Facturación con IVA, impuesto de turismo, descuento, pagos parciales, anulación e impresión.
- Tablero «Inicio» con indicadores y gráficos dibujados con GDI+ (extra E1).
- Persistencia en archivos JSON legibles con escritura atómica y recuperación de archivos dañados.

## Requisitos

- Windows 10/11.
- [.NET SDK 10](https://dotnet.microsoft.com/download) (el LTS instalado).

## Cómo ejecutar

```powershell
dotnet run --project src/Hotel.UI
```

En el primer arranque se crea la carpeta `Datos/` junto al ejecutable y se generan datos de prueba. Para regenerarlos, cierre la aplicación y borre `Datos/`.

## Pruebas

```powershell
dotnet test
```

Resultado actual: **255 pruebas, todas en verde** (ver `docs/06-plan-de-pruebas.md`).

## Credenciales de prueba

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `Admin123*` | Administrador |
| `recepcion` | `Recepcion123*` | Recepcionista |

## Estructura

```
HotelReservas.sln
├─ src/Hotel.Core/     Dominio, datos JSON y servicios (sin referencias a WinForms)
│  ├─ Comun/ Dominio/ Interfaces/ Datos/ Servicios/
├─ src/Hotel.UI/       Windows Forms: Tema/, Controles/, Vistas/, Formularios/, Program.cs
├─ tests/Hotel.Tests/  xUnit: Dominio/, Datos/, Servicios/
├─ docs/               Análisis, requisitos, casos de uso, trazabilidad, clases, plan de pruebas
│  └─ uml/             Diagramas PlantUML (casos de uso, clases, secuencia, actividad, estados)
├─ datos-prueba/       Copia de los JSON generados por la semilla
└─ .github/workflows/  Integración continua
```

**Regla de dependencias:** `UI → Core`. Los formularios solo validan formato, llaman a un servicio y muestran su `Resultado`.

## Documentación

| Documento | Contenido |
|---|---|
| `docs/01-analisis.md` | Situación actual, problema, solución, objetivos, alcance |
| `docs/02-requisitos.md` | RF, RNF y reglas de negocio |
| `docs/03-casos-de-uso.md` | CU-01 a CU-12 |
| `docs/04-trazabilidad.md` | Matriz RF → CU → clases → servicio → vista → JSON → prueba |
| `docs/05-clases.md` | Descripción de las clases |
| `docs/06-plan-de-pruebas.md` | Pruebas automatizadas y casos manuales |
| `docs/uml/*.puml` | Diagramas UML (renderizar con PlantUML) |

## Capturas

| Inicio | Habitaciones | Recepción |
|---|---|---|
| _(pendiente)_ | _(pendiente)_ | _(pendiente)_ |

| Reservas | Facturación | Factura |
|---|---|---|
| _(pendiente)_ | _(pendiente)_ | _(pendiente)_ |

## Equipo

| Nombre | Rol |
|---|---|
| _(por completar)_ | _(por completar)_ |
| _(por completar)_ | _(por completar)_ |

## Limitaciones

Aplicación local y monousuario, sin red, sin pagos en línea y sin factura electrónica FEL de la SAT (la factura es un comprobante interno).
