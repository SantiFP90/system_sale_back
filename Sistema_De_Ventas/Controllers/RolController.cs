 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using Sistema_De_Ventas.Utilidad;
using static System.Net.WebRequestMethods;
using System;


namespace Sistema_De_Ventas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;
        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }
        [HttpGet]
        [Route("Lista")]
        //IActionResult es una interfaz que representa el resultado de una acción en un controlador de ASP.NET Core.Es el tipo de retorno más flexible para los endpoints de tu API porque puede representar diferentes tipos de respuestas HTTP.
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<RolDTO>>();
            try
            {
                response.status = true;
                response.value = await _rolService.Lista();
                response.message = "Lista de roles obtenida correctamente.";
            }
            catch(Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener la lista de roles: {ex.Message}";
            }
            return Ok(response);
        }
    }
}
