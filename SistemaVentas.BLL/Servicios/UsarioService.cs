using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    //siempre implementamos la interfaz 
    public class UsarioService : IUsuarioService
    {
        //agregamos el repositorio, y le indicamos el modelo con el cual vamos a trabajar
        //y agregamos la variable mapper que viene  Imapper
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public UsarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                //Siempre vamos a devolver el DTO, ya que son los datos que vamos a consumir desde el front
                return _mapper.Map<List<UsuarioDTO>>(listUsuario); 
            } 
            catch { 
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar(u => 
                u.Correo == correo &&
                u.Clave == clave
                );

                if(queryUsuario.FirstOrDefault() == null)
                {
                    throw new TaskCanceledException("El usuario no existe.");
                }
                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(devolverUsuario);
            }
            catch
            {
                throw; 
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if(usuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("Error al crear el usuario.");
                }
                var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                return;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Eliminar(UsuarioDTO modelo)
        {
            try
            {
                return;
            }
            catch
            {
                throw;
            }
        }

       
    }
}
