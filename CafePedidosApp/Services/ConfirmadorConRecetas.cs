using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;

using System;
using CafePedidosApp.Domain;

namespace CafePedidosApp.Services
{
    // Valida stock por INSUMOS (recetas) y descuenta si alcanza.
    public class ConfirmadorConRecetas : IConfirmadorPedido
    {
        private readonly IInventarioService _inventario;

        public ConfirmadorConRecetas(IInventarioService inventario)
        {
            _inventario = inventario;
        }

        public void Confirmar(Pedido pedido)
        {
            if (pedido.Items.Count == 0)
                throw new InvalidOperationException("Pedido vacío");

            if (!_inventario.HayStockSuficiente(pedido, out var faltante))
                throw new InvalidOperationException($"No hay stock suficiente: {faltante}");

            _inventario.DescontarStock(pedido);
            pedido.Confirmar();
        }
    }
}


