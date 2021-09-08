using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class RDepartamento : IRepositorio<Departamento>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Departamentos";

        public List<Departamento> Read
        {
            get
            {
                List<Departamento> datos = new List<Departamento>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Departamento>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Departamento entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Departamento>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Departamento entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Departamento>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Departamento entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Departamento>(TableName);
                    coleccion.Update(entidadMod);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
