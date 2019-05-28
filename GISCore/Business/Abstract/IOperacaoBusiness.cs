using GISModel.DTO;
using GISModel.DTO.Permissoes;
using GISModel.Entidades;
using System.Collections.Generic;

namespace GISCore.Business.Abstract
{
    public interface IOperacaoBusiness : IBaseBusiness<EntidadeBase>
    {

        OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, List<Incidente> entidades);

        OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, Incidente entidade);

        string RecuperarProximoStatus(string statusAtual);

        string RecuperarStatusAnterior(string statusAtual);

        List<string> RecuperarResponsavelPorStatus(string status);

    }
}
