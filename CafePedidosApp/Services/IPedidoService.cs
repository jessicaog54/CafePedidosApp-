using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Domain;

namespace CafePedidosApp.Services;

public interface IPedidoService
{
    Pedido Crear(string pedidoId);
    void AgregarItem(string pedidoId, string productoId, int cantidad);
    void Confirmar(string pedidoId);
    void PasarAEnPreparacion(string pedidoId);
    void MarcarListo(string pedidoId);
    void Cancelar(string pedidoId);
    void CobrarEfectivo(string pedidoId, decimal recibido);
    void CobrarTarjeta(string pedidoId, decimal recibido);
}
