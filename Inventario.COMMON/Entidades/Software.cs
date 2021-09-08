using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Software:Base
    {
        public String Nombre { get; set; }
        public String Tipo { get; set; }
        public DateTime Vencimiento { get; set; }
    }
}
