using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IAgenteAcidenteBusiness : IBaseBusiness<AgenteAcidente>
    {

        List<AgenteAcidente> ListarTodos();

    }
}
