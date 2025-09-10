using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using CafePedidosApp.Domain;

namespace CafePedidosApp.Services;

public class InventarioService : IInventarioService
{
    private readonly IRepositorioRecetas _repoRecetas;
    private readonly IRepositorioInsumos _repoInsumos;

    public InventarioService(IRepositorioRecetas repoRecetas, IRepositorioInsumos repoInsumos)
    {
        _repoRecetas = repoRecetas;
        _repoInsumos = repoInsumos;
    }

    private Dictionary<string, decimal> CalcularRequerimientos(Pedido pedido)
    {
        var reqs = new Dictionary<string, decimal>();
        var recetas = _repoRecetas.Listar().ToList();

        foreach (var item in pedido.Items)
        {
            var recetasProd = recetas.Where(r => r.ProductoId == item.ProductoId);
            foreach (var r in recetasProd)
            {
                var total = r.CantidadRequerida * item.Cantidad;
                reqs[r.InsumoId] = reqs.TryGetValue(r.InsumoId, out var curr) ? curr + total : total;
            }
        }
        return reqs;
    }

    public bool HayStockSuficiente(Pedido pedido, out string? faltante)
    {
        var reqs = CalcularRequerimientos(pedido);
        foreach (var kv in reqs)
        {
            var ins = _repoInsumos.ObtenerPorId(kv.Key);
            if (ins is null)
            {
                faltante = $"Insumo desconocido: {kv.Key}";
                return false;
            }
            if (ins.StockDisponible < kv.Value)
            {
                faltante = $"{ins.Nombre} insuficiente (req {kv.Value} {ins.Unidad}, disp {ins.StockDisponible} {ins.Unidad})";
                return false;
            }
        }
        faltante = null;
        return true;
    }

    public void DescontarStock(Pedido pedido)
    {
        var reqs = CalcularRequerimientos(pedido);
        foreach (var kv in reqs)
        {
            var ins = _repoInsumos.ObtenerPorId(kv.Key) ?? throw new System.Exception("Insumo no existe");
            ins.AjustarStock(-kv.Value);
            _repoInsumos.Actualizar(ins);
        }
    }

    public void RevertirStock(Pedido pedido)
    {
        var reqs = CalcularRequerimientos(pedido);
        foreach (var kv in reqs)
        {
            var ins = _repoInsumos.ObtenerPorId(kv.Key) ?? throw new System.Exception("Insumo no existe");
            ins.AjustarStock(+kv.Value);
            _repoInsumos.Actualizar(ins);
        }
    }
}
