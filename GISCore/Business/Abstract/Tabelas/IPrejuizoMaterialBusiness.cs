using GISModel.Entidades.OBJ.Tabelas;
using System.Collections.Generic;

namespace GISCore.Business.Abstract.Tabelas
{
    public interface IPrejuizoMaterialBusiness : IBaseBusiness<PrejuizoMaterial>
    {

        List<PrejuizoMaterial> ListarTodos();

    }
}
