using System;
using System.Collections.Generic;
using System.Text;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;

namespace Inventario.DAL
{
    public class RHost : IRepositorio<Host>
    {
        public List<Host> Leer => throw new NotImplementedException();

        public bool Crear(Host entidad)
        {
            throw new NotImplementedException();
        }

        public bool Editar(Host entidadOg, Host entidadMod)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(Host entidad)
        {
            throw new NotImplementedException();
        }
    }
}
