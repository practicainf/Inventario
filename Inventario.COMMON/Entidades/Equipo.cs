using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Equipo:Base
    {
        
        public String Nombre{ get; set; }   
        public String Tipo { get; set; }
        public String Marca { get; set; }
        public String Estado { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Tipo, Nombre);
        }
    }
}
