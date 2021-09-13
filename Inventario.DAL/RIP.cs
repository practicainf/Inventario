using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class RIP : IRepositorio<IP>
    {
        private string DBName = @"C:\InventarioDB\Inventario.db";
        private string TableName = "IPs";

        public List<IP> Read
        {
            get
            {
                List<IP> datos = new List<IP>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<IP>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(IP entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<IP>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(IP entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<IP>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, IP entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<IP>(TableName);
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
