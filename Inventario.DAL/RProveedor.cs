using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
     public class RProveedor : IRepositorio<Proveedor>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Proveedores";

        public List<Proveedor> Read
        {
            get
            {
                List<Proveedor> datos = new List<Proveedor>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Proveedor>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Proveedor entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Proveedor>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Proveedor entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Proveedor>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Proveedor entidadModificada)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Proveedor>(TableName);
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
