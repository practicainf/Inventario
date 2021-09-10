using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManagePantallas : IManagePantallas
    {
        IRepositorio<Pantalla> repositorio;
        public ManagePantallas(IRepositorio<Pantalla> repo)
        {
            repositorio = repo;
        }

        public List<Pantalla> List => repositorio.Read;

        public bool Create(Pantalla entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Pantalla entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Pantalla Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Pantalla entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
