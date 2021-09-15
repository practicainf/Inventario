using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManageProveedor : IManageProveedores
    {
        IRepositorio<Proveedor> repositorio;
        public ManageProveedor(IRepositorio<Proveedor> repo)
        {
            repositorio = repo;
        }

        public List<Proveedor> List => repositorio.Read;

        public bool Create(Proveedor entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Proveedor entidad)
        {
            return repositorio.Delete(entidad);
        }

        

        

        public Proveedor Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Proveedor entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }
}
