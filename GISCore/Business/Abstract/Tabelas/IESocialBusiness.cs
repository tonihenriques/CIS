using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IESocialBusiness : IBaseBusiness<ESocial>
    {

        List<ESocial> ListarTodos();

    }
}
