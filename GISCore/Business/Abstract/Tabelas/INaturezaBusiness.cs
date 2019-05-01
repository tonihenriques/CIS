using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface INaturezaBusiness : IBaseBusiness<Natureza>
    {

        List<Natureza> ListarTodos();

    }
}
