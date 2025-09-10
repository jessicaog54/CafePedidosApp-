using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;

namespace CafePedidosApp.Services; // o tu proyecto Infrastructure

public class RepoInsumosMem : IRepositorioInsumos
{
    private readonly Dictionary<string, Insumo> _db = new();

    public void Agregar(Insumo i) => _db[i.Id] = i;
    public Insumo? ObtenerPorId(string id) => _db.TryGetValue(id, out var x) ? x : null;
    public IEnumerable<Insumo> Listar() => _db.Values;
    public void Actualizar(Insumo i) => _db[i.Id] = i;
}
