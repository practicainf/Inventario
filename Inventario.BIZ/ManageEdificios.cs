using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageEdificios : IManageEdificios
    {
        IRepositorio<Edificio> repositorio;
        public ManageEdificios(IRepositorio<Edificio> repo)
        {
            repositorio = repo;
        }

        public List<Edificio> List => repositorio.Read;

        public bool Create(Edificio entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Edificio entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Edificio Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Edificio entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
