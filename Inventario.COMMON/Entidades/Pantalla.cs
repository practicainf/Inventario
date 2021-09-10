using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Pantalla:Equipo
    {
        public String SN { get; set; }
        public String Pulgadas { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", Tipo, Nombre, Pulgadas);
        }
    }
}
