﻿using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISCore.Business.Concrete;
using GISCore.Business.Concrete.Tabelas;
using GISHelpers.Extensions.System;
using GISHelpers.Utils;
using GISModel.DTO.Incidente;
using GISModel.DTO.IncidenteVeiculo;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.OBJ.Tabelas;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        public IArquivoBusiness ArquivoBusiness { get; set; }

        [Inject]
        public IESocialBusiness ESocialBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Municipio> MunicipioBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IIncidenteVeiculoBusiness IncidenteVeiculoBusiness { get; set; }

        [Inject]
        public IOperacaoBusiness OperacaoBusiness { get; set; }

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
                    entidade.Codigo = "IV-" + DateTime.Now.Year.ToString() + "-" + IncidenteVeiculoBusiness.GetNextNumber("IncidenteVeiculo", "select max(SUBSTRING(codigo, 9, 6)) from objincidenteveiculo").ToString().PadLeft(6, '0');
                    entidade.StatusWF = "RS";
                    entidade.DataAtualizacao = DateTime.Now;
                    IncidenteVeiculoBusiness.Inserir(entidade);

                    Severino.GravaCookie("MensagemSucesso", "O incidente com veículo foi cadastrado com sucesso.", 10);
                    Severino.GravaCookie("FuncaoInboxAChamar", "IncidentesVeiculos", 10);
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
                    throw new Exception("Não foi possível localizar o parâmetro que identifica o incidente com veículo.");
                else
                {
                    List<IncidenteVeiculo> lista = IncidenteVeiculoBusiness.Consulta.Where(a => a.UniqueKey.Equals(uniquekey) && string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                    IncidenteVeiculo registro = lista[0];

                    VMIncidenteVeiculo vm = new VMIncidenteVeiculo();
                    vm.UniqueKey = registro.UniqueKey;
                    vm.Codigo = registro.Codigo;
                    //vm.Status = registro.Status;
                    vm.Descricao = registro.Descricao;
                    vm.AcidenteFatal = registro.AcidenteFatal ? "Sim" : "Não";
                    vm.AcidenteGraveIP102 = registro.AcidenteGraveIP102 ? "Sim" : "Não";
                    vm.Centro = registro.Centro.GetDisplayName();
                    vm.Regional = registro.Regional.ToString();
                    vm.NumeroSmart = registro.NumeroSmart;

                    if (registro.ETipoEntrada != 0)
                        vm.TipoEntrada = registro.ETipoEntrada.GetDisplayName();




                    Municipio mun = MunicipioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKMunicipio));
                    if (mun != null)
                        vm.Municipio = mun.NomeCompleto;

                    ESocial eso = ESocialBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKESocial));
                    if (eso != null)
                    {
                        vm.ESocial = eso.Codigo;
                        ViewBag.ESocialDesc = eso.Descricao;
                    }


                    vm.Estado = registro.Estado;
                    vm.Logradouro = registro.Logradouro;
                    vm.NumeroLogradouro = registro.NumeroLogradouro;
                    vm.ETipoAcidente = registro.ETipoAcidente.GetDisplayName();
                    vm.LocalAcidente = registro.LocalAcidente;


                    Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(registro.UKOrgao));
                    if (dep != null)
                        vm.Orgao = dep.Sigla;

                    if (registro.TipoLocalAcidente != 0)
                        vm.TipoLocalAcidente = registro.TipoLocalAcidente.GetDisplayName();


                    vm.DataIncidente = registro.DataIncidente.ToString("dd/MM/yyyy");
                    vm.HoraIncidente = registro.HoraIncidente;
                    vm.DataInclusao = registro.DataInclusao.ToString("dd/MM/yyyy HH:mm");

                    if (registro.DataAtualizacao != null)
                    {
                        vm.DataAtualizacao = ((DateTime)registro.DataAtualizacao).ToString("dd/MM/yyyy HH:mm");
                    }

                    vm.UsuarioInclusao = registro.UsuarioInclusao;

                    vm.Arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(uniquekey)).ToList();

                    vm.Operacoes = OperacaoBusiness.RecuperarTodasPermitidas(CustomAuthorizationProvider.UsuarioAutenticado.Login, CustomAuthorizationProvider.UsuarioAutenticado.Permissoes, lista);

                    registro.Operacoes = vm.Operacoes;

                    ViewBag.Incidente = registro;

                    return PartialView("_DetalhesVeiculo", vm);
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





        public ActionResult PesquisaDadosBase()
        {
            ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaDadosBase(VMPesquisaIncidenteVeiculoBase entidade)
        {
            try
            {
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.NumeroSmart) &&
                    string.IsNullOrEmpty(entidade.DataIncidente) &&
                    string.IsNullOrEmpty(entidade.HoraIncidente) &&
                    entidade.AcidenteFatal.Equals("Todos") &&
                    entidade.AcidenteGraveIP102.Equals("Todos") &&
                    //entidade.ETipoEntrada == 0 &&
                    //entidade.ETipoAcidente == 0 &&
                    entidade.Centro == 0 &&
                    entidade.Regional == 0 &&
                    string.IsNullOrEmpty(entidade.LocalAcidente) &&
                    entidade.TipoLocalAcidente == 0 &&
                    string.IsNullOrEmpty(entidade.Logradouro) &&
                    string.IsNullOrEmpty(entidade.NumeroLogradouro) &&
                    //string.IsNullOrEmpty(entidade.UKMunicipio) &&
                    string.IsNullOrEmpty(entidade.Estado))// &&
                   // string.IsNullOrEmpty(entidade.NumeroBoletimOcorrencia) &&
                    //string.IsNullOrEmpty(entidade.DataBoletimOcorrencia) &&
                    //string.IsNullOrEmpty(entidade.UKESocial) &&
                    //string.IsNullOrEmpty(entidade.Descricao) &&
                    //string.IsNullOrEmpty(entidade.UKOrgao))
                    throw new Exception("Informe pelo menos um filtro para prossegui na pesquisa.");

                if (!string.IsNullOrEmpty(entidade.NumeroSmart))
                    sWhere += " and o.NumeroSmart like '" + entidade.NumeroSmart.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.DataIncidente))
                    sWhere += " and o.DataIncidente = '" + DateTime.ParseExact(entidade.DataIncidente, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'";

                if (!string.IsNullOrEmpty(entidade.HoraIncidente))
                    sWhere += " and o.HoraIncidente = '" + entidade.HoraIncidente + "'";

                if (!string.IsNullOrEmpty(entidade.AcidenteFatal) && !entidade.AcidenteFatal.Equals("Todos"))
                {
                    if (entidade.AcidenteFatal.Equals("Sim"))
                    {
                        sWhere += " and o.AcidenteFatal = 1";
                    }
                    else
                    {
                        sWhere += " and o.AcidenteFatal = 0";
                    }
                }

                if (!string.IsNullOrEmpty(entidade.AcidenteGraveIP102) && !entidade.AcidenteGraveIP102.Equals("Todos"))
                {
                    if (entidade.AcidenteGraveIP102.Equals("Sim"))
                    {
                        sWhere += " and o.AcidenteGraveIP102 = 1";
                    }
                    else
                    {
                        sWhere += " and o.AcidenteGraveIP102 = 0";
                    }
                }

                //if (entidade.ETipoEntrada != 0)
                //    sWhere += " and o.ETipoEntrada = " + ((int)entidade.ETipoEntrada).ToString();

                //if (entidade.ETipoAcidente != 0)
                //    sWhere += " and o.ETipoAcidente = " + ((int)entidade.ETipoAcidente).ToString();

                if (entidade.Centro != 0)
                    sWhere += " and o.Centro = " + ((int)entidade.Centro).ToString();

                if (entidade.Regional != 0)
                    sWhere += " and o.Regional = " + ((int)entidade.Regional).ToString();

                if (!string.IsNullOrEmpty(entidade.LocalAcidente))
                    sWhere += " and o.LocalAcidente like '" + entidade.LocalAcidente.Replace("*", "%") + "'";

                if (entidade.TipoLocalAcidente != 0)
                    sWhere += " and o.TipoLocalAcidente = " + ((int)entidade.TipoLocalAcidente).ToString();

                if (!string.IsNullOrEmpty(entidade.Logradouro))
                    sWhere += " and o.Logradouro like '" + entidade.Logradouro.Trim().Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.NumeroLogradouro))
                    sWhere += " and o.NumeroLogradouro like '" + entidade.NumeroLogradouro.Replace("*", "%") + "'";

                //if (!string.IsNullOrEmpty(entidade.UKMunicipio))
                //    sWhere += " and o.UKMunicipio = '" + entidade.UKMunicipio + "'";

                if (!string.IsNullOrEmpty(entidade.Estado))
                    sWhere += " and o.Estado = '" + entidade.Estado + "'";

                //if (!string.IsNullOrEmpty(entidade.NumeroBoletimOcorrencia))
                //    sWhere += " and o.NumeroBoletimOcorrencia like '" + entidade.NumeroBoletimOcorrencia.Replace("*", "%") + "'";

                //if (!string.IsNullOrEmpty(entidade.DataBoletimOcorrencia))
                //    sWhere += " and o.DataBoletimOcorrencia = '" + DateTime.ParseExact(entidade.DataBoletimOcorrencia, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'";

                //if (!string.IsNullOrEmpty(entidade.UKESocial))
                //    sWhere += " and o.UKESocial = '" + entidade.UKESocial + "'";

                if (!string.IsNullOrEmpty(entidade.Descricao))
                    sWhere += " and o.Descricao like '" + entidade.Descricao.Replace("*", "%") + "'";

                //if (!string.IsNullOrEmpty(entidade.UKOrgao))
                //    sWhere += " and o.UKOrgao = '" + entidade.UKOrgao + "'";



                string sql = @"select top 100 UniqueKey, Codigo, DataIncidente, AcidenteFatal, AcidenteGraveIP102, 
                                          ETipoEntrada, ETipoAcidente, 
                                          (select Sigla from OBJDepartamento where UniqueKey = o.UKOrgao and UsuarioExclusao is null) as Orgao,
                                          (select Descricao from OBJMunicipio where UniqueKey = o.UKMunicipio and UsuarioExclusao is null) as Municipio
                           from OBJIncidenteVeiculo o
                           where o.UsuarioExclusao is null " + sWhere + @"
                           order by Codigo";

                List<VMIncidenteVeiculo> lista = new List<VMIncidenteVeiculo>();
                DataTable result = IncidenteVeiculoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMIncidenteVeiculo()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            Codigo = row["Codigo"].ToString(),
                            DataIncidente = ((DateTime)row["DataIncidente"]).ToString("dd/MM/yyyy"),
                            TipoEntrada = row["ETipoEntrada"].ToString(),
                            ETipoAcidente = ((ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), row["ETipoAcidente"].ToString(), true)).GetDisplayName(),
                            AcidenteFatal = ((int)row["ETipoAcidente"]).Equals(0) ? "Não" : "Sim",
                            AcidenteGraveIP102 = ((bool)row["AcidenteGraveIP102"]) ? "Sim" : "Não",
                            //Status = row["Status"].ToString(),
                            Orgao = row["Orgao"].ToString(),
                            Municipio = row["Municipio"].ToString()
                        });
                    }
                }

                return PartialView("_ResultadoPesquisa", lista);
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