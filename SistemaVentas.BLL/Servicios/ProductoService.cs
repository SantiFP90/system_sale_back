using System;
using System.Collections.Generic;
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
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }


        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProduto = await _productoRepositorio.Consultar();

                var listaProductos = queryProduto.Include(cat => cat.IdCategoriaNavigation).ToList();

                //Este es un mapeo explícito
                List<ProductoDTO> listadoProductosDto = _mapper.Map<List<Producto>, List<ProductoDTO>>(listaProductos);

                return listadoProductosDto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));

                if(productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }

                //Este es un mapeo por inferencia
                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);

                var productoEncontrado = await _productoRepositorio.Obtener(u => u.IdProducto == productoModelo.IdProducto);

                if(productoEncontrado == null)
                {
                    throw new TaskCanceledException("El producto no existe");
                }

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio; 

                bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el producto.");
                }

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.Obtener(u => u.IdProducto == id);

                if (productoEncontrado == null)
                {
                    throw new TaskCanceledException("No se ha encontrado el producto a eliminar.");
                }

                bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar el producto.");
                }

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

    }
}
