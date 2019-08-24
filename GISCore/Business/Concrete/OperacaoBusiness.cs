using GISCore.Business.Abstract;
using GISModel.DTO;
using GISModel.DTO.Permissoes;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class OperacaoBusiness : BaseBusiness<EntidadeBase>, IOperacaoBusiness
    {

        public OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, List<Incidente> entidades)
        {
            var operacoes = new OperacaoCollection();

            foreach (Incidente entidade in entidades)
            {
                if (entidade.PassoAtual.Responsavel.ToUpper().Equals(usuarioLogado.ToUpper()))
                {
                    if (!operacoes.Contains(GISModel.Enums.Operacao.Aprovar))
                        operacoes.Add(GISModel.Enums.Operacao.Aprovar);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.AlterarDados))
                        operacoes.Add(GISModel.Enums.Operacao.AlterarDados);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);

                    if (!entidade.PassoAtual.Nome.ToUpper().Equals("EM APROVAÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.AnexarArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.AnexarArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.ExcluirArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.ExcluirArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCAT))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCAT);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCodificacao))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCodificacao);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirEnvolvidos))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirEnvolvidos);
                    }

                    if (entidade.PassoAtual.Nome.ToUpper().Equals("EM EDIÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Excluir))
                            operacoes.Add(GISModel.Enums.Operacao.Excluir);
                    }
                    else
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Reprovar))
                            operacoes.Add(GISModel.Enums.Operacao.Reprovar);
                    }
                }
                else
                {
                    if (permissoes.Where(a => a.Perfil.Equals(entidade.PassoAtual.Responsavel)).Count() > 0)
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Assumir))
                            operacoes.Add(GISModel.Enums.Operacao.Assumir);
                    }

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);
                }
            }
            
            return operacoes;

        }

        public OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, List<IncidenteVeiculo> entidades)
        {
            var operacoes = new OperacaoCollection();

            foreach (IncidenteVeiculo entidade in entidades)
            {
                if (entidade.PassoAtual.Responsavel.ToUpper().Equals(usuarioLogado.ToUpper()))
                {
                    if (!operacoes.Contains(GISModel.Enums.Operacao.Aprovar))
                        operacoes.Add(GISModel.Enums.Operacao.Aprovar);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.AlterarDados))
                        operacoes.Add(GISModel.Enums.Operacao.AlterarDados);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);

                    if (!entidade.PassoAtual.Nome.ToUpper().Equals("EM APROVAÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.AnexarArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.AnexarArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.ExcluirArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.ExcluirArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCAT))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCAT);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCodificacao))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCodificacao);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirEnvolvidos))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirEnvolvidos);
                    }

                    if (entidade.PassoAtual.Nome.ToUpper().Equals("EM EDIÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Excluir))
                            operacoes.Add(GISModel.Enums.Operacao.Excluir);
                    }
                    else
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Reprovar))
                            operacoes.Add(GISModel.Enums.Operacao.Reprovar);
                    }
                }
                else
                {
                    if (permissoes.Where(a => a.Perfil.Equals(entidade.PassoAtual.Responsavel)).Count() > 0)
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Assumir))
                            operacoes.Add(GISModel.Enums.Operacao.Assumir);
                    }

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);
                }
            }

            return operacoes;

        }



        public OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, List<Workflow> entidades)
        {
            var operacoes = new OperacaoCollection();

            foreach (Workflow PassoAtual in entidades)
            {
                if (PassoAtual.Responsavel.ToUpper().Equals(usuarioLogado.ToUpper()))
                {
                    if (!operacoes.Contains(GISModel.Enums.Operacao.Aprovar))
                        operacoes.Add(GISModel.Enums.Operacao.Aprovar);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.AlterarDados))
                        operacoes.Add(GISModel.Enums.Operacao.AlterarDados);

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);

                    if (!PassoAtual.Nome.ToUpper().Equals("EM APROVAÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.AnexarArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.AnexarArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.ExcluirArquivos))
                            operacoes.Add(GISModel.Enums.Operacao.ExcluirArquivos);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCAT))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCAT);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirCodificacao))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirCodificacao);

                        if (!operacoes.Contains(GISModel.Enums.Operacao.IncluirEnvolvidos))
                            operacoes.Add(GISModel.Enums.Operacao.IncluirEnvolvidos);
                    }

                    if (PassoAtual.Nome.ToUpper().Equals("EM EDIÇÃO"))
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Excluir))
                            operacoes.Add(GISModel.Enums.Operacao.Excluir);
                    }
                    else
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Reprovar))
                            operacoes.Add(GISModel.Enums.Operacao.Reprovar);
                    }
                }
                else
                {
                    if (permissoes.Where(a => a.Perfil.Equals(PassoAtual.Responsavel)).Count() > 0)
                    {
                        if (!operacoes.Contains(GISModel.Enums.Operacao.Assumir))
                            operacoes.Add(GISModel.Enums.Operacao.Assumir);
                    }

                    if (!operacoes.Contains(GISModel.Enums.Operacao.HistoricoWorkflow))
                        operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);
                }
            }

            return operacoes;

        }

        public OperacaoCollection RecuperarTodasPermitidas(string usuarioLogado, List<VMPermissao> permissoes, Workflow PassoAtual)
        {
            var operacoes = new OperacaoCollection();

            if (PassoAtual.Responsavel.ToUpper().Equals(usuarioLogado.ToUpper()))
            {
                operacoes.Add(GISModel.Enums.Operacao.Aprovar);
                //operacoes.Add(GISModel.Enums.Operacao.Encaminhar);
                operacoes.Add(GISModel.Enums.Operacao.AlterarDados);
                operacoes.Add(GISModel.Enums.Operacao.HistoricoWorkflow);

                if (!PassoAtual.Nome.ToUpper().Equals("EM APROVAÇÃO"))
                {
                    operacoes.Add(GISModel.Enums.Operacao.AnexarArquivos);
                    operacoes.Add(GISModel.Enums.Operacao.ExcluirArquivos);
                    operacoes.Add(GISModel.Enums.Operacao.IncluirCAT);
                    operacoes.Add(GISModel.Enums.Operacao.IncluirCodificacao);
                    operacoes.Add(GISModel.Enums.Operacao.IncluirEnvolvidos);
                }

                if (PassoAtual.Nome.ToUpper().Equals("EM EDIÇÃO"))
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
                if (permissoes.Where(a => a.Perfil.Equals(PassoAtual.Responsavel)).Count() > 0)
                {
                    operacoes.Add(GISModel.Enums.Operacao.Assumir);
                }

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
                    return "Em Processamento";

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
                    return "Em Processamento";

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
