using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Domain;

public class Producto
{
    public string Id { get; }
    public string Nombre { get; private set; }
    public decimal Precio { get; private set; }

    public Producto(string id, string nombre, decimal precio)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre requerido");
        if (precio <= 0) throw new ArgumentException("Precio debe ser > 0");
        Id = id; Nombre = nombre; Precio = precio;
    }

    public void Renombrar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre requerido");
        Nombre = nombre;
    }

    public void CambiarPrecio(decimal nuevo)
    {
        if (nuevo <= 0) throw new ArgumentException("Precio debe ser > 0");
        Precio = nuevo;
    }
}
