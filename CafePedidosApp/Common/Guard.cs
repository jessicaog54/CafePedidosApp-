using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafePedidosApp.Common;

public static class Guard
{
    public static void NotNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} es requerido");
    }

    public static void Positive(decimal value, string paramName)
    {
        if (value <= 0) throw new ArgumentException($"{paramName} debe ser > 0");
    }

    public static void NonNegative(int value, string paramName)
    {
        if (value < 0) throw new ArgumentException($"{paramName} no puede ser negativo");
    }

    public static void PositiveInt(int value, string paramName)
    {
        if (value <= 0) throw new ArgumentException($"{paramName} debe ser > 0");
    }
}

