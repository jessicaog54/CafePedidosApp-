using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedidoItem
{
    public string ProductoId { get; }
    public string ProductoNombre { get; }
    public decimal PrecioUnitario { get; }
    public int Cantidad { get; private set; }
    public decimal Subtotal => PrecioUnitario * Cantidad;

    public PedidoItem(string productoId, string nombre, decimal precioUnitario, int cantidad)
    {
        if (string.IsNullOrWhiteSpace(productoId)) throw new ArgumentException("ProductoId requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre requerido");
        if (precioUnitario <= 0) throw new ArgumentException("Precio > 0");
        if (cantidad <= 0) throw new ArgumentException("Cantidad > 0");
        ProductoId = productoId;
        ProductoNombre = nombre;
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }
}