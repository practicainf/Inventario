using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Edificio:Base
    {
        public String NombreEdificio { get; set; }

        public String DireccionEdificio { get; set; }

        public List<Departamento> DepartamentosEnEdificio { get; set; }
    }
}
