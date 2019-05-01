using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface INaturezaLesaoBusiness : IBaseBusiness<NaturezaLesao>
    {

        List<NaturezaLesao> ListarTodos();

    }
}
