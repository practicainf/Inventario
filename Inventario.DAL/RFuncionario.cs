using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Inventario.DAL
{
    public class RFuncionario : IRepositorio<Funcionario>
    {
        private string DBName = "Inventario.db";
        private string TableName = "Funcionarios";
        public List<Funcionario> Leer {
            get
            {
                List<Funcionario> datos = new List<Funcionario>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Funcionario>(TableName).FindAll().ToList();
                    
                }
                return datos;  
            }

        };
        
        public bool Crear(Funcionario entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Funcionario>(TableName);
                    coleccion.Insert(entidad);

                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Editar(string id, Funcionario entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Funcionario>(TableName);
                    coleccion.Update(entidadMod);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Eliminar(string id)
        {
            try
            {
                int r;
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Funcionario>(TableName);
                    r = coleccion.Delete(e => e.Id == id);
                }
                return r > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
