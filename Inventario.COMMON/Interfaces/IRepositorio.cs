using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IRepositorio<T> where T:Base
    {
        bool Create(T entidad);
        bool Update(string id, T entidadMod);
        bool Delete(T entidad);
        List<T> Read { get; }


    }
}
