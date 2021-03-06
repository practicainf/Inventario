using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Ordenador : Equipo
    {

        public String Host { get; set; }
        public IP? DireccionIP { get; set; }
        public String TipoOrdenador { get; set; }
        public String Procesador { get; set; }
        public String Nucleos { get; set; }
        public String Ram { get; set; }
        public String Almacenamiento { get; set; }
        public String MACLAN { get; set; }
        public String? MACWIFI { get; set; }
        public List<SistOp> SistemaOperativo { get; set; }
        public List<Software> SoftwareInstalados { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", Tipo, TipoOrdenador, Host);
        }
    }
}