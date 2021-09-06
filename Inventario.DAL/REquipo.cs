using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.DAL
{
    public class REquipo : IRepositorio<Equipo>
    {
        private string DBName = "Inventario.db";
        private string TableName = "Equipos";

        public List<Equipo> Read
        {
            get
            {
                List<Equipo> datos = new List<Equipo>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Equipo>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Create(Equipo entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Equipo>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Equipo entidad)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Equipo>(TableName);
                    coleccion.Delete(entidad.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(string id, Equipo entidadModificada)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Equipo>(TableName);
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
