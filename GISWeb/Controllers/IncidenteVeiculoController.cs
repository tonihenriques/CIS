using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISHelpers.Utils;
using GISModel.DTO.IncidenteVeiculo;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IncidenteVeiculoController : BaseController
    {


        #region Inject 

        [Inject]
        public IESocialBusiness ESocialBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Municipio> MunicipioBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IIncidenteVeiculoBusiness IncidenteVeiculoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Novo()
        {
            ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(IncidenteVeiculo entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UniqueKey = Guid.NewGuid().ToString();

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    entidade.Status = "Em Edição";
                    entidade.Responsavel = entidade.UsuarioInclusao;
                    entidade.Codigo = "I-" + DateTime.Now.Year.ToString() + "-" + IncidenteVeiculoBusiness.GetNextNumber("IncidenteVeiculo", "select max(SUBSTRING(codigo, 8, 6)) from objincidenteveiculo").ToString().PadLeft(6, '0');
                    entidade.StatusWF = "RS";
                    entidade.DataAtualizacao = DateTime.Now;
                    IncidenteVeiculoBusiness.Inserir(entidade);

                    Severino.GravaCookie("MensagemSucesso", "O incidente foi cadastrado com sucesso.", 10);
                    Severino.GravaCookie("FuncaoInboxAChamar", "IncidentesVeiculo", 10);
                    Severino.GravaCookie("ObjRecemCriado", entidade.UniqueKey, 10);

                    ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Inbox") } });
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

        [HttpPost]
        [RestritoAAjax]
        public ActionResult Detalhes(string uniquekey)
        {
            try
            {
                if (string.IsNullOrEmpty(uniquekey))
                    throw new Exception("Não foi possível localizar o parâmetro que identifica o incidente.");
                else
                {
                    //List<Incidente> lista = IncidenteBusiness.Consulta.Where(a => a.UniqueKey.Equals(uniquekey) && string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                    //Incidente registro = lista[0];

                    VMIncidenteVeiculo vm = new VMIncidenteVeiculo();
                    //vm.UniqueKey = registro.UniqueKey;
                    //vm.Codigo = registro.Codigo;
                    //vm.Status = registro.Status;
                    //vm.Descricao = registro.Descricao;
                    //vm.AcidenteFatal = registro.AcidenteFatal ? "Sim" : "Não";
                    //vm.AcidenteGraveIP102 = registro.AcidenteGraveIP102 ? "Sim" : "Não";
                    //vm.Centro = registro.Centro.GetDisplayName();
                    //vm.Regional = registro.Regional.ToString();
                    //vm.NumeroSmart = registro.NumeroSmart;

                    //if (registro.ETipoEntrada != 0)
                    //    vm.TipoEntrada = registro.ETipoEntrada.GetDisplayName();




                    //Municipio mun = MunicipioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKMunicipio));
                    //if (mun != null)
                    //    vm.Municipio = mun.NomeCompleto;

                    //ESocial eso = ESocialBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKESocial));
                    //if (eso != null)
                    //{
                    //    vm.ESocial = eso.Codigo;
                    //    ViewBag.ESocialDesc = eso.Descricao;
                    //}


                    //vm.Estado = registro.Estado;
                    //vm.Logradouro = registro.Logradouro;
                    //vm.NumeroLogradouro = registro.NumeroLogradouro;
                    //vm.ETipoAcidente = registro.ETipoAcidente.GetDisplayName();
                    //vm.LocalAcidente = registro.LocalAcidente;


                    //Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKOrgao));
                    //if (dep != null)
                    //    vm.Orgao = dep.Sigla;

                    //if (registro.TipoLocalAcidente != 0)
                    //    vm.TipoLocalAcidente = registro.TipoLocalAcidente.GetDisplayName();


                    //vm.DataIncidente = registro.DataIncidente.ToString("dd/MM/yyyy");
                    //vm.HoraIncidente = registro.HoraIncidente;
                    //vm.DataInclusao = registro.DataInclusao.ToString("dd/MM/yyyy HH:mm");

                    //if (registro.DataAtualizacao != null)
                    //{
                    //    vm.DataAtualizacao = ((DateTime)registro.DataAtualizacao).ToString("dd/MM/yyyy HH:mm");
                    //}

                    //vm.UsuarioInclusao = registro.UsuarioInclusao;

                    //vm.Arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(uniquekey)).ToList();

                    //vm.EnvolvidosProprio = (from rel in RegistroEmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                    //                        join envol in EmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoProprio equals envol.UniqueKey
                    //                        select new VMProprio()
                    //                        {
                    //                            Funcao = rel.Funcao,
                    //                            NumeroPessoal = envol.NumeroPessoal,
                    //                            Nome = envol.Nome,
                    //                            UKEmpregado = envol.UniqueKey,
                    //                            UKRel = rel.UniqueKey,
                    //                            UKCodificacao = rel.UKCodificacao,
                    //                            UKCAT = rel.UKCAT,
                    //                            UKLesaoDoenca = rel.UKLesaoDoenca
                    //                        }).ToList();

                    //vm.EnvolvidosTerceiro = (from rel in RegistroEmpregadoContratadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                    //                         join envol in EmpregadoTerceiroBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoContratado equals envol.UniqueKey
                    //                         select new VMTerceiro()
                    //                         {
                    //                             Funcao = rel.Funcao,
                    //                             CPF = envol.CPF,
                    //                             Nome = envol.Nome,
                    //                             UKEmpregado = envol.UniqueKey,
                    //                             UKRel = rel.UniqueKey,
                    //                             UKCodificacao = rel.UKCodificacao,
                    //                             UKCAT = rel.UKCAT,
                    //                             UKLesaoDoenca = rel.UKLesaoDoenca
                    //                         }).ToList();


                    //vm.Operacoes = OperacaoBusiness.RecuperarTodasPermitidas(CustomAuthorizationProvider.UsuarioAutenticado.Login, CustomAuthorizationProvider.UsuarioAutenticado.Permissoes, lista);

                    //registro.Operacoes = vm.Operacoes;

                    //ViewBag.Incidente = registro;

                    return PartialView("_Detalhes", vm);
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