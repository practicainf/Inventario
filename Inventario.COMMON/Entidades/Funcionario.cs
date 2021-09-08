
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Funcionario : Base
    {

        public String Rut { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Area { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Nombre, Apellido);
        }
    }
}
