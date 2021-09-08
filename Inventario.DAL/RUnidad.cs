using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class RUnidad : IRepositorio<Unidad>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Unidades";

        public List<Unidad> Read
        {
            get
            {
                List<Unidad> datos = new List<Unidad>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Unidad>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Unidad entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Unidad>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Unidad entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Unidad>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Unidad entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Unidad>(TableName);
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
