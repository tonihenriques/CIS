using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IAtoInseguroBusiness : IBaseBusiness<AtoInseguro>
    {

        List<AtoInseguro> ListarTodos();

    }
}
