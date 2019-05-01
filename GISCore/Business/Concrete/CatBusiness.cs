using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class CatBusiness : BaseBusiness<CAT>, ICatBusiness
    {
        public override void Inserir(CAT pCat)
        {
            pCat.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(pCat);
        }


        public override void Alterar(CAT pCat)
        {
            
            CAT tempCat = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pCat.UniqueKey));
            if (tempCat == null)
            {
                throw new Exception("Não foi possível encontrar esta CAT.");
            }
            
            tempCat.UniqueKey = pCat.UniqueKey;
            
            base.Alterar(tempCat);
            
        }

    }

}

