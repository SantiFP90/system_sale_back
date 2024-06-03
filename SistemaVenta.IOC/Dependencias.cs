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

namespace SistemaVenta.IOC
{
    public static class Dependencias
    {
        //La capa de dependencias en una aplicación se encarga de configurar los servicios que la aplicación necesita, asegurando que las diferentes partes del código tengan acceso a las implementaciones que necesitan. min 19 - parte 5
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QualitySantiagoFernandezContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("MyDatabaseConnection"));
            });

            //Inyeccion de dependencia para cualquier modelo del repository
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Inyeccion de dependencia particular del repository y el modelo
            services.AddScoped<IVentaRepository, VentaRepository>();

            //DEPENDENCIA DE AUTOMAPPER
            services.AddAutoMapper(typeof(AutoMapperProfile));

        }
    }
}
