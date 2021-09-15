using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{

    public class ManageFactura : IManageFacturas
    {
        IRepositorio<Factura> repositorio;
        public ManageFactura(IRepositorio<Factura> repo)
        {
            repositorio = repo;
        }

        public List<Factura> List => repositorio.Read;

        public bool Create(Factura entidad)
        {
            return repositorio.Create(entidad);
        }



        public bool Delete(Factura entidad)
        {
            return repositorio.Delete(entidad);
        }



        public Factura Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Factura entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }




    }

}
