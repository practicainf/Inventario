using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageEquipos : IManageEquipos
    {
        IRepositorio<Equipo> repositorio;
        public ManageEquipos(IRepositorio<Equipo> repo)
        {
            repositorio = repo;
        }

        public List<Equipo> List => repositorio.Read;

        public bool Create(Equipo entidad)
        {
            return repositorio.Create(entidad);
        }

        

        public bool Delete(Equipo entidad)
        {
            return repositorio.Delete(entidad);
        }

        

        public Equipo Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Equipo entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }

        

        
    }

}
