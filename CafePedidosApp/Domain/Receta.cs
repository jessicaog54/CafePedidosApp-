using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Domain;

public class Receta
{
    public string ProductoId { get; }
    public string InsumoId { get; }
    public decimal CantidadRequerida { get; } // por 1 unidad del producto

    public Receta(string productoId, string insumoId, decimal cantidadRequerida)
    {
        if (string.IsNullOrWhiteSpace(productoId)) throw new ArgumentException("ProductoId requerido");
        if (string.IsNullOrWhiteSpace(insumoId)) throw new ArgumentException("InsumoId requerido");
        if (cantidadRequerida <= 0) throw new ArgumentException("Cantidad > 0");
        ProductoId = productoId; InsumoId = insumoId; CantidadRequerida = cantidadRequerida;
    }
}
