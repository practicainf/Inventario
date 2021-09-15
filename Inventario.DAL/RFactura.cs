using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class RFactura : IRepositorio<Factura>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "Facturas";


        public List<Factura> Read
        {
            get
            {
                List<Factura> datos = new List<Factura>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Factura>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Factura entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Factura>(TableName);
                    coleccion.Insert(entidad);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Factura entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Factura>(TableName);
                    coleccion.Update(entidadMod);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




        public bool Delete(Factura entidad)
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
    }
}
