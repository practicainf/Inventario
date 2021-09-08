using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Departamento:Base
    {
        public String NombreDepartamento { get; set; }
        public List<Unidad> UnidadesEnDepartamento { get; set; }
    }
}
