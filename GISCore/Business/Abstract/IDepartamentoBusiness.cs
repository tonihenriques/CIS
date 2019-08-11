using GISModel.DTO.Departamento;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Abstract
{
    public interface IDepartamentoBusiness : IBaseBusiness<Departamento>
    {


        List<NivelHierarquico> BuscarNiveis();

    }
}
