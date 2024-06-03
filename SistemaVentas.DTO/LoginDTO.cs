using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    public class LoginDTO
    {
        //ESTE DTO NOS PERMITE OBTNER LA INFORMACIÓN CORRESPONDIENTE AL LOGUEO
        public string? Correo { get; set; }
        public string? Clave { get; set; }

    }
}
