using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Common;
using CafePedidosApp.Domain;

namespace CafePedidosApp.Services;

public class ReporteService : IReporteService
{
    private readonly IRepositorioPedidos _repoPedidos;

    public ReporteService(IRepositorioPedidos repoPedidos)
    {
        _repoPedidos = repoPedidos;
    }

    public ReporteDia Generar(DateOnly fecha)
    {
        bool mismoDia(DateTime dt) => dt.Date == fecha.ToDateTime(TimeOnly.MinValue).Date;

        var pedidosDia = _repoPedidos.Listar().Where(p => mismoDia(p.CreadoEn)).ToList();

        var entregados = pedidosDia.Where(p => p.Estado == EstadoPedido.Entregado).ToList();
        var cancelados = pedidosDia.Count(p => p.Estado == EstadoPedido.Cancelado);
        var total = entregados.Sum(p => p.Total);
        var n = entregados.Count;
        var ticket = n > 0 ? total / n : 0m;

        var porProducto = new Dictionary<string, int>();
        foreach (var p in entregados)
            foreach (var it in p.Items)
                porProducto[it.ProductoId] = (porProducto.TryGetValue(it.ProductoId, out var curr) ? curr : 0) + it.Cantidad;

        return new ReporteDia(fecha, total, n, cancelados, ticket, porProducto);
    }
}
