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
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<ProductoDTO>>();
            try
            {
                response.status = true;
                response.value = await _productoService.Lista();
                response.message = "Lista de productos obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al obtener la lista de productos: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] ProductoDTO producto)
        {
            var response = new Response<ProductoDTO>();
            try
            {
                response.status = true;
                response.value = await _productoService.Crear(producto);
                response.message = "Producto creado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al crear el producto: {ex.Message}";
            }
            return Ok(response);
        }


        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProductoDTO producto)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _productoService.Editar(producto);
                response.message = "Producto creado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al crear el producto: {ex.Message}";
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
                response.value = await _productoService.Eliminar(id);
                response.message = "Producto eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error al eliminar el producto: {ex.Message}";
            }
            return Ok(response);
        }

    }
}
