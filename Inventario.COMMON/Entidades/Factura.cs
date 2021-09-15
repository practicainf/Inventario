using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Factura : Base
    {
        public String FacturaNumero { get; set; }
        public String FacturaTipoAdquisicion { get; set; }

        public List<Equipo> EquiposEnFactura { get; set; }

        public Proveedor FacturaProveedor { get; set; }

    }
}
