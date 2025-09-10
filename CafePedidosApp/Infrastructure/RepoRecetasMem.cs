using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;


namespace CafePedidosApp.Services; // o tu proyecto Infrastructure

public class RepoRecetasMem : IRepositorioRecetas
{
    private readonly List<Receta> _db = new();
    public void Agregar(Receta r) => _db.Add(r);
    public IEnumerable<Receta> Listar() => _db;
}
