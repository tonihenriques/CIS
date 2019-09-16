using GISCore.Business.Abstract;
using GISModel.DTO.Populacao;
using GISModel.DTO.Shared;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.REL;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PopulacaoController : BaseController
    {

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IBaseBusiness<Populacao> PopulacaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<IncidenteVeiculoPopulacao> IncidenteVeiculoPopulacaoBusiness { get; set; }




        [RestritoAAjax]
        [HttpPost]
        public ActionResult Novo(string UKIncidenteVeiculo)
        {
            return PartialView("_Novo", new VMPopulacao() { UKIncidenteVeiculo = UKIncidenteVeiculo });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(VMPopulacao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dataNasc = DateTime.ParseExact(entidade.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    Populacao obj = PopulacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(entidade.Nome) && a.DataNascimento.Equals(dataNasc));
                    if (obj == null)
                    {
                        obj = new Populacao()
                        {
                            UniqueKey = Guid.NewGuid().ToString(),
                            DataNascimento = dataNasc,
                            Nome = entidade.Nome,
                            Sexo = entidade.Sexo,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };

                        PopulacaoBusiness.Inserir(obj);
                    }

                    IncidenteVeiculoPopulacao rel = new IncidenteVeiculoPopulacao()
                    {
                        UKIncidenteVeiculo = entidade.UKIncidenteVeiculo,
                        UKPopulacao = obj.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        AgenteCausador = entidade.AgenteCausador,
                        Atividade = entidade.Atividade,
                        Causa = entidade.Causa,
                        Custo = entidade.Custo,
                        Lesao = entidade.Lesao,
                        Natureza = entidade.Natureza,
                        NivelTensao = entidade.NivelTensao,
                        SituacaoRede = entidade.SituacaoRede,
                        TipoAcidente = entidade.TipoAcidente
                    };

                    IncidenteVeiculoPopulacaoBusiness.Inserir(rel);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Pessoa cadastrada com sucesso" } });

                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException() == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    }
                }

            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }

        }

        [RestritoAAjax]
        [HttpPost]
        public ActionResult Excluir(string UKRel)
        {
            try
            {
                if (string.IsNullOrEmpty(UKRel))
                {
                    throw new Exception("Não foi possível localizar a identificação da pessoa a ser excluída.");
                }
                else
                {

                    IncidenteVeiculoPopulacao rel = IncidenteVeiculoPopulacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKRel));
                    if (rel == null)
                    {
                        throw new Exception("Não foi possível localizar a pessoa a ser excluída.");
                    }
                    else
                    {
                        rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        IncidenteVeiculoPopulacaoBusiness.Terminar(rel);
                        return Json(new { resultado = new RetornoJSON() { Sucesso = "Pessoa excluída com sucesso." } });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }
        }




    }
}