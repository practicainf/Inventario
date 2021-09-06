
using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManage<T> where T : Base
    {
        bool Create(T entidad);
        List<T> List { get; }
        bool Delete(T entidad);
        bool Update(string id, T entidadMod);
        T Search(string id);
    }
}