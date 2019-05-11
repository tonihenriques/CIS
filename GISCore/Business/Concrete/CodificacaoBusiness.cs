using GISCore.Business.Abstract;
using GISModel.Entidades.OBJ;
using System;

namespace GISCore.Business.Concrete
{
    public class CodificacaoBusiness : BaseBusiness<Codificacao>, ICodificacaoBusiness
    {

        public override void Inserir(Codificacao entidade)
        {
            if (string.IsNullOrEmpty(entidade.UniqueKey))
            {
                entidade.UniqueKey = Guid.NewGuid().ToString();
            }
            
            base.Inserir(entidade);
        }

    }
}
