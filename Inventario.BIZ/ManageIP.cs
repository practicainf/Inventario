using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManageIP : IManageIP
    {
        IRepositorio<IP> repositorio;
        public ManageIP(IRepositorio<IP> repo)
        {
            repositorio = repo;
        }

        public List<IP> List => repositorio.Read;

        public object CambiarEstado(string v, IP entidadMod)
        {
            return repositorio.Update(v, entidadMod);
        }

        public bool Create(IP entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(IP entidad)
        {
            return repositorio.Delete(entidad);
        }

        public IEnumerable ListarIpDispo()
        {
            return repositorio.Read.Where(p => p.Estado == "DISPONIBLE");
        }

        

        public IP Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, IP entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }
    }
}
