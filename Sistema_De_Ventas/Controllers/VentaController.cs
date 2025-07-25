using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using SistemaVenta.Utility.Utilidad;
using static System.Net.WebRequestMethods;
using System;
using SistemaVentas.BLL.Servicios;
using Microsoft.AspNetCore.Authorization;

namespace Sistema_De_Ventas.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador, Empleado")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        public VentaController(IVentaService productoService)
        {
            _ventaService = productoService;
        }


        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            var response = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta ?? string.Empty;
            fechaInicio = fechaInicio ?? string.Empty;
            fechaFin = fechaFin ?? string.Empty;

            try
            {
                response.status = true;
                response.value = await _ventaService.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
                response.message = "Historial de ventas obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener el hisotrial de ventas: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var response = new Response<List<ReporteDTO>>();
            fechaInicio = fechaInicio ?? string.Empty;
            fechaFin = fechaFin ?? string.Empty;

            try
            {
                response.status = true;
                response.value = await _ventaService.Reporte(fechaInicio, fechaFin);
                response.message = "Reporte de ventas obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener el reporte de ventas: {ex.Message}";
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO producto)
        {
            var response = new Response<VentaDTO>();
            try
            {
                response.status = true;
                response.value = await _ventaService.Registrar(producto);
                response.message = "Venta registrada exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al registrar la venta: {ex.Message}";
            }
            return Ok(response);
        }


    }
}
