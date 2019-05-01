using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface ILocalizacaoLesaoBusiness : IBaseBusiness<LocalizacaoLesao>
    {

        List<LocalizacaoLesao> ListarTodos();

    }
}
