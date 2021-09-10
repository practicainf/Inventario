
using Inventario.COMMON.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManageEquipos : IManage<Equipo>
    {
        IEnumerable ListarPantallas(string v);
        IEnumerable ListarOrdenador(string v);
    }
}
