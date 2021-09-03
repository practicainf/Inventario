using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManageTickets : IManageTickets
    {
        IRepositorio<Ticket> repositorio;
        public ManageTickets(IRepositorio<Ticket> repo)
        {
            this.repositorio = repo;
        }
        public List<Ticket> List => repositorio.Read;

        public bool Create(Ticket entidad)
        {
            return repositorio.Create(entidad);
        }

        public bool Delete(Ticket entidad, string id)
        {
            return repositorio.Delete(entidad, id);
        }

        public Ticket Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Ticket entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }
    }
}
