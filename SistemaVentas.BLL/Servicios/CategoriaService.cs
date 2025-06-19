using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualBasic;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    //Herencia: Implementa ICategoriaService(cumple con contrato de interfaz)
    public class CategoriaService : ICategoriaService
    {
        //Inyección de Dependencias:

        //IGenericRepository<Categoria>: Patrón Repository genérico para operaciones CRUD

        //IMapper: Instancia de AutoMapper para conversión de objetos

        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;
        //readonly: Las dependencias no pueden cambiarse después de la construcción

        //Inyección por constructor: Mejor práctica para dependencias
        
        //Razón 1: Inversión de Control(IoC)
        //Principio SOLID: Depender de abstracciones(IGenericRepository), no de implementaciones.
        //Beneficio: Puedes cambiar fácilmente el repositorio real(SQL → MongoDB) sin modificar el servicio.
        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategorias = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());

            }
            catch
            {
                throw;
            }
        }
    }
}
