# CafePedidosApp

Aplicación de consola en **.NET 8 (C#)** que gestiona pedidos de una cafetería aplicando los **cuatro pilares de POO**: encapsulamiento, herencia, polimorfismo y abstracción.  

## Requisitos
- [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git instalado

## Instalación y ejecución
Clonar el repositorio:

```bash
git clone https://github.com/<TU_USUARIO>/CafePedidosApp.git
cd CafePedidosApp/src/CafePedidosApp.Console


Ejecutar la aplicación:

dotnet run

Estructura del proyecto
/CafePedidosApp
 ├── docs/analisis.pdf          # Documento de análisis (caso, HU, reqs, pruebas, etc.)
 ├── src/
 │   ├── CafePedidosApp.Domain/ # Entidades (Producto, Pedido, Insumo, Receta, Pago…)
 │   ├── CafePedidosApp.Services/ # Servicios de negocio (PedidoService, InventarioService…)
 │   ├── CafePedidosApp.Infrastructure/ # Repositorios (memoria)
 │   └── CafePedidosApp.Console/ # Menú de consola (Program.cs)
 └── 

Funcionalidades principales

Crear productos, insumos y recetas.

Crear y administrar pedidos.

Confirmar pedidos validando stock por insumos.

Flujo de estados: Creado → Confirmado → En Preparación → Listo → Entregado/Cancelado.

Pago en efectivo o tarjeta.

Reporte diario (ventas totales, cancelados, ticket promedio, consumo por producto).# CafePedidosApp-
