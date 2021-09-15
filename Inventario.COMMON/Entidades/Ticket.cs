using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Ticket:Base
    {
        
        public DateTime FechaIngreso{ get; set; }
        public DateTime FechaEntrega{ get; set; }
        public DateTime? FechaRetiro { get; set; }
        public List<Equipo> EquipoSolicitado{ get; set; }
        public Funcionario Empleado{ get; set; }
    }
}
