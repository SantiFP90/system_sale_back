using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IRolService
    {
        //Task<List<RolDTO>>: Especifica que el método Lista es asincrónico y devuelve un Task que, cuando se completa, produce una List<RolDTO>.
        Task<List<RolDTO>> Lista();
    }
}
