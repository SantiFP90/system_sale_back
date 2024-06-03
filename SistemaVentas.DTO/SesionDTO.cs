using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    public  class SesionDTO
    {
        //ESTE DTO NOS PERMITE OBTENER/ENVIAR LA INFORMACION DEL USUARIO LOGUEADO
         public int IdUsuario { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Correo { get; set; }

        public string? RolDescripcion { get; set; }
    }
}
