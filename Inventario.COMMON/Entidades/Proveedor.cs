using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Proveedor : Base
    {
        public String NombreProveedor { get; set; }
        public String FonoProveedor { get; set; }
        public String Rut { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", NombreProveedor);
        }
    }
}
