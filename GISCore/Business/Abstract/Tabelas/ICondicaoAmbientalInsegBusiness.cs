using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface ICondicaoAmbientalInsegBusiness : IBaseBusiness<CondicaoAmbientalInseg>
    {

        List<CondicaoAmbientalInseg> ListarTodos();

    }
}
