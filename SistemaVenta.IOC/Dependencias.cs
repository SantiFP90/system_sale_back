using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.DAL.DBContext;
using SistemaVentas.DAL.Repositorios;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVenta.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.BLL.Servicios;

namespace SistemaVenta.IOC
{
    public static class Dependencias
    {
        //La capa de dependencias en una aplicación se encarga de configurar los servicios que la aplicación necesita, asegurando que las diferentes partes del código tengan acceso a las implementaciones que necesitan.
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
                        services.AddDbContext<QualitySantiagoFernandezContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("MyDatabaseConnection"));
            });


            services.AddSingleton<Jwt>();

            //Inyeccion de dependencia para cualquier modelo del repository
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Inyeccion de dependencia particular del repository y el modelo
            services.AddScoped<IVentaRepository, VentaRepository>();

            //DEPENDENCIA DE AUTOMAPPER
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IRolService, RolService>();
            //"Cuando alguien pida IUsuarioService, dales UsuarioService"
            services.AddScoped<IUsuarioService, UsarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IMenu, MenuService>();
            services.AddScoped<IDashboardService, DashboardService>();

        }
    }
}
