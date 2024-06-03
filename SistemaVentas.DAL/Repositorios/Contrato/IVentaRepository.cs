using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.Model;

namespace SistemaVentas.DAL.Repositorios.Contrato
    //Aqui creo la interfaz, y especifico que voy a utlizar los modelos, luego agrego la interfaz de Igeneric y le indico que trabajaremos con el modelo de venta
{
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);
    }
}
