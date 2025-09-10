using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;
using CafePedidosApp.Common;
using CafePedidosApp.Services;


namespace CafePedidosApp.Services
{
    public static class Cobrador
    {
        public static void Cobrar(Pedido pedido, Pago pago)
        {
            
            if (pedido.Estado != EstadoPedido.Listo)
                throw new InvalidOperationException("El pedido debe estar 'Listo' para poder cobrar.");

            pago.Validar(pedido.Total);
            pedido.MarcarPagado();
        }
    }
}
