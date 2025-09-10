using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Common;  
using CafePedidosApp.Domain;

using System.Collections.Generic;

namespace CafePedidosApp.Domain;

public interface IConfirmadorPedido
{
    void Confirmar(Pedido pedido);
}


public interface IRepositorioProductos
{
    void Agregar(Producto p);
    Producto? ObtenerPorId(string id);
    IEnumerable<Producto> Listar();
}

public interface IRepositorioPedidos
{
    void Agregar(Pedido p);
    Pedido? ObtenerPorId(string id);
    IEnumerable<Pedido> Listar();
}

public interface IRepositorioInsumos
{
    void Agregar(Insumo i);
    Insumo? ObtenerPorId(string id);
    IEnumerable<Insumo> Listar();
    void Actualizar(Insumo i);
}

public interface IRepositorioRecetas
{
    void Agregar(Receta r);
    IEnumerable<Receta> Listar();
}

// Inventario (por insumo/receta)
public interface IInventarioService
{
    bool HayStockSuficiente(Pedido pedido, out string? faltante);
    void DescontarStock(Pedido pedido);
    void RevertirStock(Pedido pedido);
}
