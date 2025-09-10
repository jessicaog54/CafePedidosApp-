using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CafePedidosApp.Common;

namespace CafePedidosApp.Services;

public interface IReporteService
{
    ReporteDia Generar(DateOnly fecha);
}
