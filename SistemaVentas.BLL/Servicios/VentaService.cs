using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
namespace SistemaVentas.BLL.Servicios
{
    public class VentaService : IVentaService
    {

        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository ,IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepository;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(modelo));

                if(ventaGenerada.IdVenta == 0)
                {
                    throw new Exception("No se pudo registrar la venta.");
                }

                return _mapper.Map<VentaDTO>(ventaGenerada);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {


            //Es una interfaz que representa una consulta que puede ser ejecutada contra una base de datos. Tiene estas características clave:

            //Ejecución diferida(lazy loading): La consulta no se ejecuta inmediatamente, solo cuando realmente necesitas los datos(al llamar a.ToList(), .FirstOrDefault(), etc.)

            //Composición de consultas: Puedes ir agregando más condiciones(Where, OrderBy, etc.) antes de ejecutarla

            //Traducción a SQL: Entity Framework traduce esta consulta a SQL optimizado para tu proveedor de base de datos

            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var ListaResultado = new List<Venta>();

            try
            {
                if(buscarPor == "fecha")
                {
                    DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));

                    ListaResultado = await query.Where(v =>
                        v.FechaRegistro.Value.Date >= fecha_inicio.Date && v.FechaRegistro.Value.Date <= fecha_fin.Date)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                } else
                {
                    ListaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
            }
            catch
            {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(ListaResultado);
        }



        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepositorio.Consultar();
            var ListaResultado = new List<DetalleVenta>();

            try
            {
                DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));

                ListaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_fin.Date
                    ).ToListAsync();

            }
            catch
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListaResultado);
        }
    }
}
