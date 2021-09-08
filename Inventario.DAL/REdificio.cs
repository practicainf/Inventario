using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class REdificio : IRepositorio<Edificio>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Edificios";

        public List<Edificio> Read
        {
            get
            {
                List<Edificio> datos = new List<Edificio>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Edificio>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Edificio entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Edificio>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Edificio entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Edificio>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Edificio entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Edificio>(TableName);
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
