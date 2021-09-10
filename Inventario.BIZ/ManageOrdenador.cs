
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageOrdenador : IManageOrdenador
    {
        IRepositorio<Ordenador> repositorio;
        public ManageOrdenador(IRepositorio<Ordenador> repo)
        {
            repositorio = repo;
        }

        public List<Ordenador> List => repositorio.Read;

        public bool Create(Ordenador entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Ordenador entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Ordenador Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Ordenador entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
