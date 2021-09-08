using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageDepartamentos : IManageDepartamentos
    {
        IRepositorio<Departamento> repositorio;
        public ManageDepartamentos(IRepositorio<Departamento> repo)
        {
            repositorio = repo;
        }

        public List<Departamento> List => repositorio.Read;

        public bool Create(Departamento entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Departamento entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Departamento Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Departamento entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
