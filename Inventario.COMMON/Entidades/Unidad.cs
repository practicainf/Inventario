using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Unidad:Base
    {
        public String NombreUnidad { get; set; }
        public List<Funcionario> FuncionariosEnUnidad { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", NombreUnidad);
        }
    }
}
