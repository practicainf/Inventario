using Inventario.COMMON.Interfaces;
using Inventario.COMMON.Entidades;

using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using System.Linq;

namespace Inventario.DAL
{
    public class RPantalla : IRepositorio<Pantalla>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Pantallas";

        public List<Pantalla> Read
        {
            get
            {
                List<Pantalla> datos = new List<Pantalla>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Pantalla>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Pantalla entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Pantalla>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Pantalla entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Pantalla>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Pantalla entidadModificada)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Pantalla>(TableName);
                    coleccion.Update(entidadModificada);
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
