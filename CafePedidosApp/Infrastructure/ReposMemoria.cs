using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;
using CafePedidosApp.Common;
using CafePedidosApp.Services;

public class RepoProductosMem : IRepositorioProductos
{
    private readonly Dictionary<string, Producto> _db = new();
    public void Agregar(Producto p) => _db[p.Id] = p;
    public Producto? ObtenerPorId(string id) => _db.TryGetValue(id, out var p) ? p : null;
    public IEnumerable<Producto> Listar() => _db.Values;
}

public class RepoPedidosMem : IRepositorioPedidos
{
    private readonly Dictionary<string, Pedido> _db = new();
    public void Agregar(Pedido p) => _db[p.Id] = p;
    public Pedido? ObtenerPorId(string id) => _db.TryGetValue(id, out var p) ? p : null;
    public IEnumerable<Pedido> Listar() => _db.Values;
}
