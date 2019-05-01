using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IEspecieAcidenteImpessoalBusiness : IBaseBusiness<EspecieAcidenteImpessoal>
    {

        List<EspecieAcidenteImpessoal> ListarTodos();

    }
}
