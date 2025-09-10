using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Common;

namespace CafePedidosApp.Domain;

public class Pedido
{
    private readonly List<PedidoItem> _items = new();

    public string Id { get; }
    public DateTime CreadoEn { get; }
    public EstadoPedido Estado { get; private set; } = EstadoPedido.Creado;
    public bool Pagado { get; private set; }
    public IReadOnlyCollection<PedidoItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Subtotal);

    public Pedido(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id requerido");
        Id = id;
        CreadoEn = DateTime.Now;
    }

    public void AgregarItem(Producto p, int cantidad)
    {
        if (Estado != EstadoPedido.Creado)
            throw new InvalidOperationException("Solo se pueden editar ítems en estado Creado");
        if (cantidad <= 0) throw new ArgumentException("Cantidad > 0");
        _items.Add(new PedidoItem(p.Id, p.Nombre, p.Precio, cantidad));
    }

    public void Confirmar() => CambiarEstado(EstadoPedido.Confirmado);
    public void PasarAEnPreparacion() => CambiarEstado(EstadoPedido.EnPreparacion);
    public void MarcarListo() => CambiarEstado(EstadoPedido.Listo);
    public void MarcarEntregado() => CambiarEstado(EstadoPedido.Entregado);
    public void Cancelar() => CambiarEstado(EstadoPedido.Cancelado);

    public void MarcarPagado()
    {
        if (Estado != EstadoPedido.Listo)
            throw new InvalidOperationException("El pedido debe estar Listo para registrar pago");
        Pagado = true;
        MarcarEntregado();
    }

    private void CambiarEstado(EstadoPedido nuevo)
    {
        if (!TransicionValida(Estado, nuevo))
            throw new InvalidOperationException($"Transición inválida de {Estado} a {nuevo}");
        Estado = nuevo;
    }

    private static bool TransicionValida(EstadoPedido actual, EstadoPedido nuevo) =>
        (actual, nuevo) switch
        {
            (EstadoPedido.Creado, EstadoPedido.Confirmado) => true,
            (EstadoPedido.Confirmado, EstadoPedido.EnPreparacion) => true,
            (EstadoPedido.EnPreparacion, EstadoPedido.Listo) => true,
            (EstadoPedido.Listo, EstadoPedido.Entregado) => true,
            (EstadoPedido.Creado or EstadoPedido.Confirmado or EstadoPedido.EnPreparacion or EstadoPedido.Listo, EstadoPedido.Cancelado) => true,
            _ => false
        };
}

