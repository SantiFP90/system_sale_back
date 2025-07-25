using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using SistemaVenta.Utility.Utilidad;
using static System.Net.WebRequestMethods;
using System;
using SistemaVentas.BLL.Servicios;

namespace Sistema_De_Ventas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //Principio de Inversión de Dependencias
        private readonly IUsuarioService _usuarioService; // Depende de la abstracción (interfaz)
        //El controlador usa los métodos definidos en la interfaz, sin conocer la implementación concreta


        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<UsuarioDTO>>();
            try
            {
                response.status = true;
                response.value = await _usuarioService.Lista();
                response.message = "Lista de usuarios obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener la lista de usuarios: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new Response<SesionDTO>();
            try
            {
                response.status = true;
                response.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
                response.message = "Inicio de sesión exitoso.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al iniciar sesión: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] UsuarioDTO usario)
        {
            var response = new Response<UsuarioDTO>();
            try
            {
                response.status = true;
                response.value = await _usuarioService.Crear(usario);
                response.message = "Usuario creado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al crear el usuario: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usario)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _usuarioService.Editar(usario);
                response.message = "Usuario creado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al crear el usuario: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _usuarioService.Eliminar(id);
                response.message = "Usuario eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al eliminar el usuario: {ex.Message}";
            }
            return Ok(response);
        }


    }
}
