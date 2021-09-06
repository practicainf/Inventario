
using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManageFuncionarios : IManage<Funcionario>
    {
        List<Funcionario> FuncionariosPorArea(string area);
        
    }
}