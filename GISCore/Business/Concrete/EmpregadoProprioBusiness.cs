using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EmpregadoProprioBusiness : BaseBusiness<EmpregadoProprio>, IEmpregadoProprioBusiness
    {
        public override void Inserir(EmpregadoProprio pEmpProprio)
        {

            if (string.IsNullOrEmpty(pEmpProprio.UniqueKey))
            {
                pEmpProprio.UniqueKey = Guid.NewGuid().ToString();
            }

            base.Inserir(pEmpProprio);
        }


        public override void Alterar(EmpregadoProprio pEmpProprio)
        {

            EmpregadoProprio tempEmpProprio = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pEmpProprio.UniqueKey));
            if (tempEmpProprio == null)
            {
                throw new Exception("Não foi possível encontrar o Empregado com este CPF.");
            }

            tempEmpProprio.UniqueKey = pEmpProprio.UniqueKey;

            base.Alterar(tempEmpProprio);

        }

    }

}

