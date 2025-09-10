using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Common;

public class ReporteDia
{
    public DateOnly Fecha { get; }
    public decimal TotalVentas { get; }
    public int PedidosEntregados { get; }
    public int PedidosCancelados { get; }
    public decimal TicketPromedio { get; }
    public IReadOnlyDictionary<string, int> CantidadPorProducto { get; }

    public ReporteDia(
        DateOnly fecha,
        decimal totalVentas,
        int pedidosEntregados,
        int pedidosCancelados,
        decimal ticketPromedio,
        IReadOnlyDictionary<string, int> cantidadPorProducto
    )
    {
        Fecha = fecha;
        TotalVentas = totalVentas;
        PedidosEntregados = pedidosEntregados;
        PedidosCancelados = pedidosCancelados;
        TicketPromedio = ticketPromedio;
        CantidadPorProducto = cantidadPorProducto;
    }
}
