using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class RTicket : IRepositorio<Ticket>
    {
        private string DBName = "Inventario.db";
        private string TableName = "Tickets";

        public List<Ticket> Read
        {
            get
            {
                List<Ticket> datos = new List<Ticket>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Ticket>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Ticket entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ticket>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Ticket entidad, string id)
        {
            try
            {

                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ticket>(TableName);
                    coleccion.Delete(entidad.Id == id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Ticket entidadMod)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Ticket>(TableName);
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
