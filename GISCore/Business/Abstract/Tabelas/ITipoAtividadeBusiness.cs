using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface ITipoAtividadeBusiness : IBaseBusiness<TipoAtividade>
    {

        List<TipoAtividade> ListarTodos();

    }
}
