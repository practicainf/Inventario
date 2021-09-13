using Inventario.COMMON.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManageIP : IManage<IP>
    {
        IEnumerable ListarIpDispo();
        object CambiarEstado(string v, IP ip);
    }
}
