using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility.Utilidad
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string message { get; set; }

    }
}
