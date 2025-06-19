using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public Task<ProductoDTO> Registrar(VentaDTO modelo)
        {
            try
            {

            }
            catch
            {

            }
        }

        public Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            try
            {

            }
            catch
            {

            }
        }



        public Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
