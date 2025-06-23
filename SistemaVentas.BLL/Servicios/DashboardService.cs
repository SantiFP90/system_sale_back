using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.DAL.Repositorios;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.BLL.Servicios
{
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepositorio, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(
                v => v.FechaRegistro
                ).Select(v => v.FechaRegistro).First();

            if (!ultimaFecha.HasValue)
                throw new InvalidOperationException("No hay ventas registradas");

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date); 
        }

        private async Task<int> TotalVentasUltmaSemana()
        {
            int total = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                total = tablaVenta.Count();
            }
            return total;
        }


        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-AR"));
        }

        private async Task<string> TotalIngresos()
        {
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                resultado = _ventaQuery.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-AR"));
        }

        private async Task<int> TotalProductos(){
            IQueryable<Producto> _ventaQuery = await _productoRepositorio.Consultar();
            int total = _ventaQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if(_ventaQuery.Count() > 0) {

                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta.GroupBy(v => v.FechaRegistro.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(dv => new {fecha = dv.Key.ToString("dd/MM/yyyy"),total = dv.Count()}).ToDictionary(keySelector : r => r.fecha, elementSelector : r => r.total);
            }
            return resultado;
        }

        public async Task<DashboardDTO> Resumen()
        {
            DashboardDTO vmDashBoard = new DashboardDTO();
            try
            {
                vmDashBoard.TotalVentas = await TotalVentasUltmaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalIngresosCompleto = await TotalIngresos();
                vmDashBoard.TotalProductos = await TotalProductos();
                vmDashBoard.VentasUltimaSemana = _mapper.Map<List<VentaSemanaDTO>>(await VentasUltimaSemana());
            }
            catch
            {
                throw;
            }
            return vmDashBoard;
        }
    }
}
