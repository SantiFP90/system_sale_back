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
    public class MenuController : ControllerBase
    {
        private readonly IMenu _menuService;
        public MenuController(IMenu menuService)
        {
            _menuService = menuService;
        }


        [HttpGet]
        [Route("Menu/{id:int}")]
        public async Task<IActionResult> Menu(int id)
        {
            var response = new Response<MenuDTO>();
            try
            {
                response.status = true;
                response.value = await _menuService.Lista(id);
                response.message = "Menu obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener el menu: {ex.Message}";
            }
            return Ok(response);
        }
    }
}
