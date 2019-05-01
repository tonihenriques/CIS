using GISCore.Business.Abstract;
using GISModel.Entidades.OBJ;
using System;

namespace GISCore.Business.Concrete
{
    public class LesaoEmpregadoBusiness : BaseBusiness<LesaoEmpregado>, ILesaoEmpregadoBusiness
    {

        public override void Inserir(LesaoEmpregado entidade)
        {
            if (string.IsNullOrEmpty(entidade.UniqueKey))
            {
                entidade.UniqueKey = Guid.NewGuid().ToString();
            }
            
            base.Inserir(entidade);
        }

    }
}
