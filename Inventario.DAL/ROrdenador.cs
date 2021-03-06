using Inventario.COMMON.Interfaces;
using Inventario.COMMON.Entidades;

using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using System.Linq;

namespace Inventario.DAL
{
    public class ROrdenador : IRepositorio<Ordenador>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Ordenadores";

        public List<Ordenador> Read
        {
            get
            {
                List<Ordenador> datos = new List<Ordenador>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Ordenador>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Ordenador entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ordenador>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Ordenador entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ordenador>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Ordenador entidadModificada)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ordenador>(TableName);
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
