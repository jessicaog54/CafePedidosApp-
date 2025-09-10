// See https://aka.ms/new-console-template for more information
using System.Globalization;
using CafePedidosApp.Domain;   
using CafePedidosApp.Services;  
using CafePedidosApp.Common;


// Repos
var repoProd = new RepoProductosMem();
var repoPed = new RepoPedidosMem();
var repoInsumos = new RepoInsumosMem();
var repoRecetas = new RepoRecetasMem();

// Semilla de insumos
repoInsumos.Agregar(new Insumo("CAF", "Café molido", "g", 2000));
repoInsumos.Agregar(new Insumo("LEC", "Leche", "ml", 5000));
repoInsumos.Agregar(new Insumo("AGU", "Agua", "ml", 20000));

// Semilla de productos
repoProd.Agregar(new Producto("CAF-ESP", "Expresso", 5000m));
repoProd.Agregar(new Producto("CAF-LAT", "Latte", 7500m));

// Recetas
repoRecetas.Agregar(new Receta("CAF-ESP", "CAF", 8));
repoRecetas.Agregar(new Receta("CAF-ESP", "AGU", 30));
repoRecetas.Agregar(new Receta("CAF-LAT", "CAF", 8));
repoRecetas.Agregar(new Receta("CAF-LAT", "LEC", 200));
repoRecetas.Agregar(new Receta("CAF-LAT", "AGU", 20));

// Servicios
var inventario = new InventarioService(repoRecetas, repoInsumos);
var confirmador = new ConfirmadorConRecetas(inventario);
var pedidosSrv = new PedidoService(repoPed, repoProd, confirmador, inventario);
var reporteSrv = new ReporteService(repoPed);


//correcto con recetas/insumos
repoProd.Agregar(new Producto("CAF-ESP", "Espresso", 5000m));
repoProd.Agregar(new Producto("CAF-LAT", "Latte", 7500m));

while (true)
{
    Console.WriteLine(@"
1) Crear producto
2) Crear pedido
3) Agregar ítem a pedido
4) Confirmar pedido (descuenta stock)
5) Pasar a En Preparación
6) Marcar Listo
7) Cobrar (E=efectivo / T=tarjeta)
8) Cancelar pedido
9) Reporte diario
10) Listar productos
11) Listar pedidos
12) Listar insumos
13) Crear insumo
14) Ajustar stock de insumo
15) Crear receta (Producto ↔ Insumo)
16) Listar recetas de un producto

0) Salir");
    Console.Write("> ");
    var op = Console.ReadLine();
    if (op == "0") break;

    try
    {
        switch (op)
        {
            case "1": // Crear producto
                Console.Write("Id: "); var idp = Console.ReadLine()!;
                Console.Write("Nombre: "); var nom = Console.ReadLine()!;
                Console.Write("Precio: "); var precio = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
                repoProd.Agregar(new Producto(idp, nom, precio));
                Console.WriteLine("Producto OK");
                break;


            case "2": // Crear pedido
                Console.Write("Id pedido: "); var idPedido = Console.ReadLine()!;
                pedidosSrv.Crear(idPedido);
                Console.WriteLine("Pedido creado");
                break;

            case "3": // Agregar ítem
                Console.Write("Id pedido: "); var idp2 = Console.ReadLine()!;
                Console.Write("Id producto: "); var prodId = Console.ReadLine()!;
                Console.Write("Cantidad: "); var cant = int.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
                pedidosSrv.AgregarItem(idp2, prodId, cant);
                Console.WriteLine("Ítem agregado");
                break;

            case "4": // Confirmar (valida y descuenta stock)
                Console.Write("Id pedido: "); var idp3 = Console.ReadLine()!;
                pedidosSrv.Confirmar(idp3);
                Console.WriteLine("Pedido confirmado (stock descontado)");
                break;

            case "5": // En Preparación
                Console.Write("Id pedido: "); var idp4 = Console.ReadLine()!;
                pedidosSrv.PasarAEnPreparacion(idp4);
                Console.WriteLine("Pedido en preparación");
                break;

            case "6": // Listo
                Console.Write("Id pedido: "); var idp6 = Console.ReadLine()!;
                pedidosSrv.MarcarListo(idp6);
                Console.WriteLine("Pedido marcado como Listo");
                break;

            case "7": // Cobrar
                {
                    Console.Write("Id pedido: "); var idp5 = Console.ReadLine()!;
                    Console.Write("Método (E=efectivo / T=tarjeta): ");
                    var metodo = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
                    Console.Write("Monto recibido: ");
                    var monto = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

                    if (metodo == "E")
                    {
                        pedidosSrv.CobrarEfectivo(idp5, monto);
                        var pedCobro = repoPed.ObtenerPorId(idp5)!;
                        var cambio = monto - pedCobro.Total;
                        Console.WriteLine($"Pagado. Total: {pedCobro.Total}  Estado:{pedCobro.Estado}");
                        Console.WriteLine($"Cambio: {cambio}");
                    }
                    else
                    {
                        pedidosSrv.CobrarTarjeta(idp5, monto);
                        var pedCobro = repoPed.ObtenerPorId(idp5)!;
                        Console.WriteLine($"Pagado. Total: {pedCobro.Total}  Estado:{pedCobro.Estado}");
                        Console.WriteLine("Cambio: 0 (tarjeta)");
                    }
                    break;
                }


            case "8": // Cancelar (con reversión de stock si aplica)
                Console.Write("Id pedido: "); var idp8 = Console.ReadLine()!;
                pedidosSrv.Cancelar(idp8);
                Console.WriteLine("Pedido cancelado");
                break;

            case "9": // Reporte diario
                var hoy = DateOnly.FromDateTime(DateTime.Now);
                var rep = reporteSrv.Generar(hoy);
                Console.WriteLine($"\nREPORTE {rep.Fecha}");
                Console.WriteLine($"Total ventas: {rep.TotalVentas}");
                Console.WriteLine($"Entregados: {rep.PedidosEntregados} | Cancelados: {rep.PedidosCancelados} | Ticket Promedio: {rep.TicketPromedio}");
                Console.WriteLine("Cantidad por producto:");
                foreach (var kv in rep.CantidadPorProducto)
                    Console.WriteLine($" - {kv.Key}: {kv.Value}");
                break;

            case "10": // Listar productos
                foreach (var p in repoProd.Listar())
                    Console.WriteLine($"[{p.Id}] {p.Nombre}  ${p.Precio}");
                break;


            case "11": // Listar pedidos
                foreach (var p in repoPed.Listar())
                    Console.WriteLine($"[{p.Id}] Total:{p.Total}  Estado:{p.Estado}  Pagado:{p.Pagado}  Ítems:{p.Items.Count}");
                break;

            case "12": // Listar insumos
                foreach (var ins in repoInsumos.Listar())
                    Console.WriteLine($"[{ins.Id}] {ins.Nombre} - {ins.StockDisponible}{ins.Unidad}");
                break;

            case "13": // Crear insumo
                {
                    Console.Write("Id insumo: "); var id = Console.ReadLine()!;
                    Console.Write("Nombre: "); var nombre = Console.ReadLine()!;
                    Console.Write("Unidad (g/ml/u): "); var unidad = Console.ReadLine()!;
                    Console.Write("Stock inicial: "); var stockStr = Console.ReadLine()!;
                    if (!decimal.TryParse(stockStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var stock) || stock < 0)
                    {
                        Console.WriteLine("Valor inválido para stock.");
                        break;
                    }

                    var ins = new Insumo(id, nombre, unidad, stock);
                    repoInsumos.Agregar(ins);
                    Console.WriteLine("Insumo creado.");
                    break;
                }

            case "14": // Ajustar stock de insumo
                {
                    Console.Write("Id insumo: "); var id = Console.ReadLine()!;
                    var ins = repoInsumos.ObtenerPorId(id);
                    if (ins is null) { Console.WriteLine("Insumo no existe"); break; }

                    Console.Write($"Delta (positivo suma, negativo resta) [{ins.StockDisponible}{ins.Unidad}]: ");
                    var deltaStr = Console.ReadLine()!;
                    if (!decimal.TryParse(deltaStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var delta))
                    {
                        Console.WriteLine("Valor inválido.");
                        break;
                    }

                    try
                    {
                        ins.AjustarStock(delta);
                        repoInsumos.Actualizar(ins);
                        Console.WriteLine($"Nuevo stock: {ins.StockDisponible}{ins.Unidad}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                }

            case "15": // Crear receta (Producto ↔ Insumo)
                {
                    Console.Write("ProductoId: "); var prodIdR = Console.ReadLine()!; // 👈 renombrado
                    var prodR = repoProd.ObtenerPorId(prodIdR);
                    if (prodR is null) { Console.WriteLine("Producto no existe"); break; }

                    Console.Write("InsumoId: "); var insIdR = Console.ReadLine()!;
                    var insR = repoInsumos.ObtenerPorId(insIdR);
                    if (insR is null) { Console.WriteLine("Insumo no existe"); break; }

                    Console.Write($"Cantidad requerida por 1 '{prodR.Nombre}' en {insR.Unidad}: ");
                    var cantStr = Console.ReadLine()!;
                    if (!decimal.TryParse(cantStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var canti) || canti <= 0)
                    {
                        Console.WriteLine("Cantidad inválida.");
                        break;
                    }

                    repoRecetas.Agregar(new Receta(prodIdR, insIdR, canti));
                    Console.WriteLine("Receta agregada.");
                    break;
                }


            case "16": // Listar recetas de un producto
                {
                    Console.Write("ProductoId: "); var prodId1 = Console.ReadLine()!;
                    var prod = repoProd.ObtenerPorId(prodId1);
                    if (prod is null) { Console.WriteLine("Producto no existe"); break; }

                    var recetas = repoRecetas.Listar().Where(r => r.ProductoId == prodId1).ToList();
                    if (recetas.Count == 0)
                    {
                        Console.WriteLine("Este producto no tiene recetas configuradas.");
                        break;
                    }

                    Console.WriteLine($"Receta de {prod.Nombre}:");
                    foreach (var r in recetas)
                    {
                        var ins = repoInsumos.ObtenerPorId(r.InsumoId);
                        var nombre = ins?.Nombre ?? r.InsumoId;
                        var unidad = ins?.Unidad ?? "";
                        Console.WriteLine($" - {nombre}: {r.CantidadRequerida}{unidad}");
                    }
                    break;
                }


            default:
                Console.WriteLine("Opción inválida");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
