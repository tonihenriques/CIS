using GISModel.DTO;
using GISModel.Entidades;
using System.Collections.Generic;

namespace GISCore.Business.Abstract
{
    public interface IOperacaoBusiness : IBaseBusiness<EntidadeBase>
    {

        OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, Incidente entidade);

        string RecuperarProximoStatus(string statusAtual);

        string RecuperarStatusAnterior(string statusAtual);

        List<string> RecuperarResponsavelPorStatus(string status);

    }
}
