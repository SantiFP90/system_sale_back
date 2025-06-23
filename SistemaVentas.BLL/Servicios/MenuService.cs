using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.DAL.Repositorios;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.BLL.Servicios
{
    public class MenuService : IMenu
    {
        private readonly IGenericRepository<Menu> _menuRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IGenericRepository<MenuRol> _menuRolRepositorio;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Menu> menuRepositorio, IGenericRepository<Usuario> usuarioRepositorio, IGenericRepository<MenuRol> menuRolRepositorio, IMapper mapper)
        {
            _menuRepositorio = menuRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _menuRolRepositorio = menuRolRepositorio;
            _mapper = mapper;
        }

        public async Task<MenuDTO> Lista(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepositorio.Consultar();
            IQueryable<Menu> tbMenu = await _menuRepositorio.Consultar();
            try
            {
                IQueryable<Menu> tbResultado = (from u in tbUsuario join mr in tbMenuRol on u.IdRol equals mr.IdRol 
                  join m in tbMenu on mr.IdMenu equals m.IdMenu
                  select m
                  ).AsQueryable();

                var listMenu = tbResultado.ToList();

                return _mapper.Map<MenuDTO>(listMenu);
            }
            catch
            {
                throw;
            }
        }
    }
}
