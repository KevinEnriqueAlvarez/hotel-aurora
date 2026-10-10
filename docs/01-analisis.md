# 01 · Análisis — Sistema de Reservas «Hotel Aurora»

## Situación actual
Hotel Aurora es un hotel mediano (20 habitaciones en 4 pisos) que hoy gestiona sus reservas en cuadernos y hojas de cálculo. La recepción anota entradas y salidas a mano, los cargos de consumo (restaurante, minibar, lavandería) se apuntan en papeles sueltos y la factura se arma al final de la estadía.

Consecuencias observadas:

- **Dobles reservas:** dos huéspedes con la misma habitación en fechas que se cruzan.
- **Cargos sin cobrar:** consumos que no llegan a la factura final.
- **Sin historial de huéspedes:** no se sabe cuántas veces ha venido un cliente ni cuánto ha facturado.
- **Estado de habitaciones desactualizado:** se ofrecen habitaciones en limpieza o mantenimiento.
- **Cobros sin trazabilidad:** pagos parciales sin registro formal ni saldo pendiente claro.

## Problema identificado
La gestión manual y dispersa de reservas, estadías, cargos y pagos produce errores de disponibilidad, pérdidas por consumos no facturados y falta de información histórica para decidir.

## Propuesta de solución
Una aplicación de escritorio (Windows Forms, C#/.NET) que centraliza:

1. El catálogo de habitaciones y su estado operativo.
2. El registro de huéspedes con su historial.
3. Reservas con validación automática de disponibilidad y solapamiento.
4. Check-in / check-out con cargos adicionales.
5. Facturación con descuento e impuestos, pagos parciales y anulación.
6. Un tablero de inicio con indicadores de ocupación e ingresos.

Los datos se guardan en archivos JSON legibles, sin base de datos.

## Objetivo general
Construir un sistema local de reservas y recepción que elimine las dobles reservas y los cargos sin cobrar del Hotel Aurora, aplicando análisis, modelado UML y programación orientada a objetos en C#.

## Objetivos específicos
1. Levantar y documentar requisitos funcionales, no funcionales y reglas de negocio.
2. Modelar el sistema con diagramas UML (casos de uso, clases, secuencia, actividad y estados).
3. Implementar un dominio con encapsulamiento, herencia, polimorfismo e interfaces.
4. Persistir la información en JSON con escritura atómica y recuperación ante archivos dañados.
5. Construir una interfaz Windows Forms consistente, sin lógica de negocio en los formularios.
6. Verificar las reglas de negocio con pruebas automatizadas xUnit y un plan de pruebas manuales.

## Alcance
Incluye: acceso por rol, habitaciones, huéspedes, reservas, check-in/out, cargos, facturación, pagos, anulación, historial, tablero de inicio, integración continua.

## Limitaciones
- Aplicación **local y monousuario**, sin red ni sincronización entre equipos.
- **Sin pagos en línea** (el registro de tarjeta guarda solo los últimos 4 dígitos y la autorización).
- **Sin factura electrónica FEL** de la SAT; la factura es un comprobante interno.
- Sin canal de reservas web ni integración con terceros.
