using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Domain;

public class Insumo
{
    public string Id { get; }
    public string Nombre { get; private set; }
    public string Unidad { get; private set; } // "g", "ml", "u", etc.
    public decimal StockDisponible { get; private set; } // >= 0

    public Insumo(string id, string nombre, string unidad, decimal stockInicial)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre requerido");
        if (string.IsNullOrWhiteSpace(unidad)) throw new ArgumentException("Unidad requerida");
        if (stockInicial < 0) throw new ArgumentException("Stock no puede ser negativo");
        Id = id; Nombre = nombre; Unidad = unidad; StockDisponible = stockInicial;
    }

    public void AjustarStock(decimal delta)
    {
        var nuevo = StockDisponible + delta;
        if (nuevo < 0) throw new InvalidOperationException("Stock insuficiente");
        StockDisponible = nuevo;
    }
}
