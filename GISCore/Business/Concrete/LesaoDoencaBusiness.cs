using GISCore.Business.Abstract;
using GISModel.Entidades.OBJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class LesaoDoencaBusiness : BaseBusiness<LesaoDoenca>, ILesaoDoencaBusiness
    {

        public override void Inserir(LesaoDoenca entidade)
        {
            if (string.IsNullOrEmpty(entidade.UniqueKey))
            {
                entidade.UniqueKey = Guid.NewGuid().ToString();
            }

            base.Inserir(entidade);
        }

    }
}
