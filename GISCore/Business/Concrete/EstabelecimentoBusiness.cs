using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GISCore.Business.Concrete
{
    public class EstabelecimentoBusiness : BaseBusiness<Estabelecimento>, IEstabelecimentoBusiness
    {
        public override void Inserir(Estabelecimento pTEstabelecimento)
        {
            pTEstabelecimento.UniqueKey = Guid.NewGuid().ToString();
            base.Inserir(pTEstabelecimento);
        }
        
        public override void Alterar(Estabelecimento pTEstabelecimento)
        {

            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pTEstabelecimento.UniqueKey));
            if (tempEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento.");
            }
            
            tempEstabelecimento.UniqueKey = pTEstabelecimento.UniqueKey;
            
            base.Alterar(tempEstabelecimento);
            
        }

    }



}

