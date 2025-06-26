using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using SistemaVenta.Utility;
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
        private readonly Jwt _jwt;

        public UsarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper, Jwt jwt)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _jwt = jwt;
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
                if (!EsCorreoValido(correo))
                    throw new ArgumentException("Correo inválido.");

                if (!EsClaveSegura(clave))
                    throw new ArgumentException("La clave debe tener al menos 8 caracteres, una mayúscula y un número.");

                var queryUsuario = await _usuarioRepositorio.Consultar(u => 
                u.Correo == correo &&
                u.Clave == _jwt.encriptarSHA256(clave)
                );

                if(queryUsuario.FirstOrDefault() == null)
                {
                    throw new TaskCanceledException("El usuario no existe.");
                }

                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();

                var usuarioLogueado = _mapper.Map<SesionDTO>(devolverUsuario);

                usuarioLogueado.Token = _jwt.GenerarToken(usuarioLogueado);

                return  usuarioLogueado;
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
                modelo.Clave = _jwt.encriptarSHA256(modelo.Clave!);

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

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);

                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if(usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("No se ha encontrado el usuario a editar.");
                }

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el usuario.");
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

                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);

                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("No se ha encontrado el usuario a eliminar.");
                }


                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar el usuario.");
                }

                return respuesta;

            }
            catch
            {
                throw;
            }
        }

        bool EsCorreoValido(string correo)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool EsClaveSegura(string clave)
        {
            return clave.Length >= 8 &&
                   clave.Any(char.IsUpper) &&
                   clave.Any(char.IsLower) &&
                   clave.Any(char.IsDigit);
        }


    }
}
