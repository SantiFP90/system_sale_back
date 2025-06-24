using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using Sistema_De_Ventas.Utilidad;
using static System.Net.WebRequestMethods;
using System;
using SistemaVentas.BLL.Servicios;

namespace Sistema_De_Ventas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var response = new Response<DashboardDTO>();
            try
            {
                response.status = true;
                response.value = await _dashboardService.Resumen();
                response.message = "Dashboard obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener el dashboard: {ex.Message}";
            }
            return Ok(response);
        }

    }
}
