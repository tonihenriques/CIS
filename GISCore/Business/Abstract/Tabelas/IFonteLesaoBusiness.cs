using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IFonteLesaoBusiness : IBaseBusiness<FonteLesao>
    {

        List<FonteLesao> ListarTodos();

    }
}
