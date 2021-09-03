using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManageFuncionarios : IManageFuncionarios
    {
        IRepositorio<Funcionario> repositorio;
        public ManageFuncionarios(IRepositorio<Funcionario> repo)
        {
            repositorio = repo;
        }
        public List<Funcionario> List => repositorio.Read;

        public bool Create(Funcionario entidad)
        {
            return repositorio.Create(entidad);
        }

        public bool Delete(Funcionario entidad, string id)
        {
            return repositorio.Delete(entidad, id);
        }        

        public Funcionario Search(string id)
        {
            return List.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Update(string id, Funcionario entidadMod)
        {
            return repositorio.Update(id, entidadMod);
        }
    }
}
