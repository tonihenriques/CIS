using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IFatorPessoalInsegurancaBusiness : IBaseBusiness<FatorPessoalInseguranca>
    {

        List<FatorPessoalInseguranca> ListarTodos();

    }
}
