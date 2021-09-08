using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageUnidads : IManageUnidad
    {
        IRepositorio<Unidad> repositorio;
        public ManageUnidads(IRepositorio<Unidad> repo)
        {
            repositorio = repo;
        }

        public List<Unidad> List => repositorio.Read;

        public bool Create(Unidad entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Unidad entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Unidad Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Unidad entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
