using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using CafePedidosApp.Common;
using CafePedidosApp.Domain;


namespace CafePedidosApp.Services;

public class PedidoService : IPedidoService
{
    private readonly IRepositorioPedidos _repoPedidos;
    private readonly IRepositorioProductos _repoProductos;
    private readonly IConfirmadorPedido _confirmador;
    private readonly IInventarioService _inventario;

    public PedidoService(IRepositorioPedidos repoPedidos,
                         IRepositorioProductos repoProductos,
                         IConfirmadorPedido confirmador,
                         IInventarioService inventario)
    {
        _repoPedidos = repoPedidos;
        _repoProductos = repoProductos;
        _confirmador = confirmador;
        _inventario = inventario;
    }

    public Pedido Crear(string pedidoId)
    {
        var p = new Pedido(pedidoId);
        _repoPedidos.Agregar(p);
        return p;
    }

    public void AgregarItem(string pedidoId, string productoId, int cantidad)
    {
        var ped = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        var prod = _repoProductos.ObtenerPorId(productoId) ?? throw new Exception("Producto no existe");
        if (cantidad <= 0) throw new ArgumentException("Cantidad > 0");
        ped.AgregarItem(prod, cantidad);
        _repoPedidos.Agregar(ped);
    }

    public void Confirmar(string pedidoId)
    {
        var ped = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        _confirmador.Confirmar(ped);
        _repoPedidos.Agregar(ped);
    }

    public void PasarAEnPreparacion(string pedidoId)
    {
        var p = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        p.PasarAEnPreparacion();
        _repoPedidos.Agregar(p);
    }

    public void MarcarListo(string pedidoId)
    {
        var p = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        p.MarcarListo();
        _repoPedidos.Agregar(p);
    }

    public void Cancelar(string pedidoId)
    {
        var p = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        if (p.Estado is EstadoPedido.Confirmado or EstadoPedido.EnPreparacion)
            _inventario.RevertirStock(p); // 👈 reversión por insumos
        p.Cancelar();
        _repoPedidos.Agregar(p);
    }

    public void CobrarEfectivo(string pedidoId, decimal recibido)
    {
        var ped = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        if (ped.Estado != EstadoPedido.Listo) throw new InvalidOperationException("El pedido debe estar Listo");
        var pago = new PagoEfectivo(recibido);
        pago.Validar(ped.Total);
        ped.MarcarPagado();
        _repoPedidos.Agregar(ped);
    }

    public void CobrarTarjeta(string pedidoId, decimal recibido)
    {
        var ped = _repoPedidos.ObtenerPorId(pedidoId) ?? throw new Exception("Pedido no existe");
        if (ped.Estado != EstadoPedido.Listo) throw new InvalidOperationException("El pedido debe estar Listo");
        var pago = new PagoTarjeta(recibido);
        pago.Validar(ped.Total);
        ped.MarcarPagado();
        _repoPedidos.Agregar(ped);
    }
}
