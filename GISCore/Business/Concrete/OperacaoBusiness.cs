using GISCore.Business.Abstract;
using GISModel.DTO;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class OperacaoBusiness : BaseBusiness<EntidadeBase>, IOperacaoBusiness
    {

        public OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, Incidente entidade)
        {
            var operacoes = new OperacaoCollection();

            if (entidade.Responsavel.ToUpper().Equals(usuarioLogado.ToUpper()))
            {
                operacoes.Add(GISModel.Enums.Operacao.Aprovar);
                //operacoes.Add(GISModel.Enums.Operacao.Encaminhar);
                operacoes.Add(GISModel.Enums.Operacao.AlterarDados);
                operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);

                if (entidade.Status.ToUpper().Equals("EM EDIÇÃO"))
                {
                    operacoes.Add(GISModel.Enums.Operacao.Excluir);
                }
                else
                {
                    operacoes.Add(GISModel.Enums.Operacao.Reprovar);
                }
            }
            else 
            {
                operacoes.Add(GISModel.Enums.Operacao.Assumir);
                operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);
            }
            return operacoes;

        }

        public string RecuperarProximoStatus(string statusAtual)
        {
            switch (statusAtual.ToUpper())
            {
                case "EM EDIÇÃO":
                    return "Em Análise";

                case "EM ANÁLISE":
                    return "Em processamento";

                case "EM PROCESSAMENTO":
                    return "Em Aprovação";

                case "EM APROVAÇÃO":
                    return "Concluído";

                default:
                    throw new Exception("Status não reconhecido.");

            }
        }

        public string RecuperarStatusAnterior(string statusAtual)
        {
            switch (statusAtual.ToUpper())
            {
                case "EM ANÁLISE":
                    return "Em Edição";

                case "EM PROCESSAMENTO":
                    return "Em Análise";

                case "EM APROVAÇÃO":
                    return "Em processamento";

                default:
                    throw new Exception("Status não reconhecido.");

            }
        }

        public List<string> RecuperarResponsavelPorStatus(string status)
        {
            switch (status.ToUpper())
            {
                case "EM EDIÇÃO":
                    return new List<string>() { "Submitter" };

                case "EM ANÁLISE":
                    return new List<string>() { "Medico" };

                case "EM PROCESSAMENTO":
                    return new List<string>() { "Técnico de Segurança" };

                case "EM APROVAÇÃO":
                    return new List<string>() { "Gerente", "Engenheiro Assistente" };

                case "CONCLUÍDO":
                    return new List<string>() { "" };

                default:
                    throw new Exception("Status não reconhecido.");

            }
        }

    }
}
