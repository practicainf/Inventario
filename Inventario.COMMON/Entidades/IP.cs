using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class IP : Base
    {
        public String DireccionIP { get; set; }
        public String Estado { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Estado, DireccionIP);
        }
    }
}
