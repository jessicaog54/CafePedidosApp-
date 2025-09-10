using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Abstracción
public abstract class Pago
{
    public decimal MontoRecibido { get; }
    protected Pago(decimal montoRecibido)
    {
        if (montoRecibido <= 0) throw new ArgumentException("Monto recibido > 0");
        MontoRecibido = montoRecibido;
    }

    public abstract void Validar(decimal total);
}

public sealed class PagoEfectivo : Pago
{
    public PagoEfectivo(decimal montoRecibido) : base(montoRecibido) { }
    public override void Validar(decimal total)
    {
        if (MontoRecibido < total) throw new InvalidOperationException("Pago insuficiente");
        // cambio = MontoRecibido - total (si quieres imprimirlo)
    }
}

public sealed class PagoTarjeta : Pago
{
    public PagoTarjeta(decimal montoRecibido) : base(montoRecibido) { }
    public override void Validar(decimal total)
    {
        // Validación simple (aprobación simulada). Si total <= 0 ya lanzó antes.
    }
}
