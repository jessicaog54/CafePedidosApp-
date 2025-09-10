using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Common;

public enum EstadoPedido
{
    Creado = 0,
    Confirmado = 1,
    EnPreparacion = 2,
    Listo = 3,
    Entregado = 4,
    Cancelado = 5
}
