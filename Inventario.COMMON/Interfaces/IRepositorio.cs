using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IRepositorio<T> where T:Base
    {
        bool Crear(T entidad);
        bool Editar(string id, T entidadMod);
        bool Eliminar(string id);
        List<T> Leer { get; }



    }
}
