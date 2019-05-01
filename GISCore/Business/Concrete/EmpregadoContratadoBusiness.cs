using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EmpregadoContratadoBusiness : BaseBusiness<EmpregadoContratado>, IEmpregadoContratadoBusiness
    {
        public override void Inserir(EmpregadoContratado pEmpContratado)
        {
            pEmpContratado.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(pEmpContratado);
        }


        public override void Alterar(EmpregadoContratado pEmpContratado)
        {
            
            EmpregadoContratado tempEmpContratado = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pEmpContratado.UniqueKey));
            if (tempEmpContratado == null)
            {
                throw new Exception("Não foi possível encontrar o Empregado com este CPF.");
            }
            
            tempEmpContratado.UniqueKey = pEmpContratado.UniqueKey;
            
            base.Alterar(tempEmpContratado);
            
        }

    }

}

