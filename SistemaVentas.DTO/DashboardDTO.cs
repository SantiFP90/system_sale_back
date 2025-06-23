using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    public class DashboardDTO
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set;}

        public string? TotalIngresosCompleto { get; set; }

        public int TotalProductos { get; set; }

        public List<VentaSemanaDTO>VentasUltimaSemana { get; set; }

    }
}
