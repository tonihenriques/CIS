using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISHelpers.Utils;
using GISModel.DTO.Incidente;
using GISHelpers.Extensions.System;
using GISModel.DTO.Inbox;
using System.Data;
using GISModel.Enums;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class InboxController : BaseController
    {

        #region

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public IIncidenteBusiness IncidenteBusiness { get; set; }

            [Inject]
            public IOperacaoBusiness OperacaoBusiness { get; set; }

            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

            [Inject]
            public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.FuncaoInbox = Severino.RecuperaCookie("FuncaoInboxAChamar", true);

            if (ViewBag.FuncaoInbox == null || string.IsNullOrEmpty(ViewBag.FuncaoInbox))
            {
                ViewBag.FuncaoInbox = "Incidentes";
            }

            ViewBag.ObjRecemCriado = Severino.RecuperaCookie("ObjRecemCriado", true);

            return View();
        }

        public ActionResult MeusGrupos()
        {
            ViewBag.FuncaoInbox = Severino.RecuperaCookie("FuncaoInboxAChamar", true);

            if (ViewBag.FuncaoInbox == null || string.IsNullOrEmpty(ViewBag.FuncaoInbox))
            {
                ViewBag.FuncaoInbox = "Incidentes";
            }

            ViewBag.ObjRecemCriado = Severino.RecuperaCookie("ObjRecemCriado", true);

            return View();
        }

        public ActionResult CarregarInboxPessoal(string tab)
        {
            try
            {
                switch (tab)
                {
                    case "Incidentes":

                        List<VMIncidente> incidentes = (from pro in IncidenteBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                                                        join org in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on pro.UKOrgao equals org.UniqueKey
                                                        orderby pro.DataInclusao descending
                                                        select new VMIncidente() {
                                                           UniqueKey = pro.UniqueKey,
                                                           Codigo = pro.Codigo,
                                                           Status = pro.Status,
                                                           ETipoAcidente = pro.ETipoAcidente.GetDisplayName(),
                                                           DataIncidente = pro.DataIncidente.ToString("dd/MM/yyyy"),
                                                           AcidenteFatal = pro.AcidenteFatal ? "Sim" : "Não",
                                                           AcidenteGraveIP102 = pro.AcidenteGraveIP102 ? "Sim" : "Não",
                                                           Orgao = org.Sigla,
                                                           MensagemPasso = pro.MensagemPasso
                                                        }).ToList();

                        return PartialView("_ListarIncidente", incidentes);

                    case "IncidentesVeiculos":
                        return PartialView("_ListarIncidenteVeiculo");

                    case "QuaseIncidentes":
                        return PartialView("_ListarQuaseIncidente");

                    case "QuaseIncidentesVeiculos":
                        return PartialView("_ListarQuaseIncidenteVeiculo");

                    default:
                        throw new Exception("Tab não reconhecida.");
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

        public ActionResult CarregarInboxGrupos(string tab) {

            try
            {
                List<string> perfis = (from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                      where usuarioperfil.UKUsuario.Equals(CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey)
                                      select "'" + perfil.Nome + "'").ToList();

                switch (tab)
                {
                    case "Incidentes":

                        List<VMIncidente> lista = new List<VMIncidente>();

                        string sql = @"select i.UniqueKey, i.Codigo, i.DataIncidente, i.ETipoAcidente, d.Sigla, i.AcidenteFatal, i.AcidenteGraveIP102, i.Status, i.MensagemPasso
                                       from OBJIncidente i, OBJDepartamento d
                                       where i.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and
		                                     i.UKOrgao = d.UniqueKey and d.UsuarioExclusao is null
                                       order by i.datainclusao desc ";

                        DataTable dtInbox = PerfilBusiness.GetDataTable(sql);
                        if (dtInbox.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtInbox.Rows)
                            {
                                lista.Add(new VMIncidente()
                                {
                                    UniqueKey = row[0].ToString(),
                                    Codigo = row[1].ToString(),
                                    DataIncidente = ((DateTime)row[2]).ToString("dd/MM/yyyy"),
                                    ETipoAcidente = ((ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), row[3].ToString())).GetDisplayName(),
                                    Orgao = row[4].ToString(),
                                    AcidenteFatal = ((bool)row[5]) ? "Sim" : "Não",
                                    AcidenteGraveIP102 = ((bool)row[6]) ? "Sim" : "Não",
                                    Status = row[7].ToString(),
                                    MensagemPasso = row[8].ToString()
                                });
                            }

                        }

                        return PartialView("_ListarIncidente", lista);

                    case "IncidentesVeiculos":
                        return PartialView("_ListarIncidenteVeiculo");

                    case "QuaseIncidentes":
                        return PartialView("_ListarQuaseIncidente");

                    case "QuaseIncidentesVeiculos":
                        return PartialView("_ListarQuaseIncidenteVeiculo");

                    default:
                        throw new Exception("Tab não reconhecida.");
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

        [RestritoAAjax]
        public ActionResult AprovarIncidente(string uk)
        {
            try
            {

                Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                if (fichaPersistida != null)
                {


                    //if (fichaPersistida.Configuration.UID.Contains(ConfigurationManager.AppSettings["Custom:Submodulo:Web:RH:Contexto"]))
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("RelacionadosComFicha", "PESQUISA", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos == null || fichaPersistida.Arquivos.Count == 0)
                    //        throw new Exception("Não foi encontrado nenhum arquivo anexado ao documento, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}
                    //else
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("ObrigatóriosNãoAnexados", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos != null && fichaPersistida.Arquivos.Count > 0)
                    //        throw new Exception("Existem arquivos obrigatórios ainda não anexados e, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}

                    ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                    ViewBag.Codigo = fichaPersistida.Codigo;
                    ViewBag.Status = fichaPersistida.Status;

                    return PartialView("_AprovarIncidente");
                }
                else
                    throw new Exception("As informações fornecidas para aprovação do documento não são válidas.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult AprovarIncidente(ItemInbox item)
        {
            try
            {
                Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(a => 
                    string.IsNullOrEmpty(a.UsuarioExclusao) && 
                    a.UniqueKey.Equals(item.UniqueKey) && 
                    a.Responsavel.ToUpper().Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper()));

                if (obj == null)
                {
                    throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                }
                else
                {
                    obj.StatusWF = "SO";
                    obj.DataExclusao = DateTime.Now;
                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    IncidenteBusiness.Alterar(obj);

                    string novoStatus = OperacaoBusiness.RecuperarProximoStatus(obj.Status);
                    List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);
                    string sUniqueKey = Guid.NewGuid().ToString();
                    foreach (string resp in Responsaveis)
                    {
                        Incidente obj2 = new Incidente();
                        PropertyCopy.Copy(obj, obj2);

                        obj2.ID = Guid.NewGuid().ToString();
                        obj2.UniqueKey = sUniqueKey;

                        if (novoStatus.Equals("Concluído"))
                        {
                            obj2.StatusWF = "SO";
                        }
                        else
                        {
                            obj2.StatusWF = "RS";
                        }
                        
                        obj2.MensagemPasso = item.Comentarios;
                        obj2.Status = novoStatus;
                        obj2.Responsavel = resp;
                        obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        obj2.UsuarioExclusao = null;
                        obj2.DataExclusao = DateTime.MaxValue;

                        IncidenteBusiness.Inserir(obj2);

                        Severino.GravaCookie("MensagemSucesso", "O incidente " + obj2.Codigo + " foi aprovado com sucesso.", 10);
                    }
                }

                return Json(new { sucesso = "O passo do workflow associado ao documento foi aprovado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }



        [RestritoAAjax]
        public ActionResult Assumir(string uk)
        {
            try
            {
                Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                if (fichaPersistida != null)
                {

                    //if (fichaPersistida.Configuration.UID.Contains(ConfigurationManager.AppSettings["Custom:Submodulo:Web:RH:Contexto"]))
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("RelacionadosComFicha", "PESQUISA", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos == null || fichaPersistida.Arquivos.Count == 0)
                    //        throw new Exception("Não foi encontrado nenhum arquivo anexado ao documento, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}
                    //else
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("ObrigatóriosNãoAnexados", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos != null && fichaPersistida.Arquivos.Count > 0)
                    //        throw new Exception("Existem arquivos obrigatórios ainda não anexados e, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}

                    ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                    ViewBag.Codigo = fichaPersistida.Codigo;
                    ViewBag.Status = fichaPersistida.Status;
                    ViewBag.Responsavel = fichaPersistida.Responsavel;

                    return PartialView("_AssumirIncidente");
                }
                else
                    throw new Exception("As informações fornecidas para aprovação do documento não são válidas.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult Assumir(ItemInbox item)
        {
            try
            {
                List<Incidente> lista = IncidenteBusiness.Consulta.Where(a => 
                    string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey)).ToList();

                if (lista == null || lista.Count == 0)
                {
                    throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                }
                else
                {
                    bool first = true;
                    foreach (Incidente obj in lista)
                    {
                        obj.StatusWF = "SO";
                        obj.DataExclusao = DateTime.Now;
                        obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        IncidenteBusiness.Alterar(obj);

                        if (first)
                        {
                            first = false;
                            Incidente obj2 = new Incidente();
                            PropertyCopy.Copy(obj, obj2);

                            obj2.ID = Guid.NewGuid().ToString();
                            obj2.StatusWF = "RS";

                            if (!string.IsNullOrEmpty(item.Comentarios))
                            {
                                obj2.MensagemPasso = item.Comentarios;
                            }
                            
                            obj2.Responsavel = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioExclusao = null;
                            obj2.DataExclusao = DateTime.MaxValue;

                            IncidenteBusiness.Inserir(obj2);

                            Severino.GravaCookie("MensagemSucesso", "O incidente " + obj2.Codigo + " foi redirecionado para a sua caixa pessoal.", 10);
                        }
                        
                    }

                    
                }

                return Json(new { sucesso = "O passo do workflow associado ao documento foi aprovado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }



        [RestritoAAjax]
        public ActionResult RejeitarIncidente(string uk)
        {
            try
            {

                Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                if (fichaPersistida != null)
                {


                    //if (fichaPersistida.Configuration.UID.Contains(ConfigurationManager.AppSettings["Custom:Submodulo:Web:RH:Contexto"]))
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("RelacionadosComFicha", "PESQUISA", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos == null || fichaPersistida.Arquivos.Count == 0)
                    //        throw new Exception("Não foi encontrado nenhum arquivo anexado ao documento, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}
                    //else
                    //{
                    //    fichaPersistida.Arquivos = ArquivoBusiness.RecuperarPorParamsMultiple("ObrigatóriosNãoAnexados", fichaPersistida, AutorizacaoProvider.UsuarioAutenticado);
                    //    if (fichaPersistida.Arquivos != null && fichaPersistida.Arquivos.Count > 0)
                    //        throw new Exception("Existem arquivos obrigatórios ainda não anexados e, portanto, não é possível caminhar com o documento em seu fluxo de aprovação.");
                    //}

                    ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                    ViewBag.Codigo = fichaPersistida.Codigo;
                    ViewBag.Status = fichaPersistida.Status;

                    return PartialView("_RejeitarIncidente");
                }
                else
                    throw new Exception("As informações fornecidas para aprovação do documento não são válidas.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult RejeitarIncidente(ItemInbox item)
        {
            try
            {
                Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(a =>
                    string.IsNullOrEmpty(a.UsuarioExclusao) &&
                    a.UniqueKey.Equals(item.UniqueKey) &&
                    a.Responsavel.ToUpper().Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper()));

                if (obj == null)
                {
                    throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                }
                else
                {
                    obj.StatusWF = "RJ";
                    obj.DataExclusao = DateTime.Now;
                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    IncidenteBusiness.Alterar(obj);

                    string novoStatus = OperacaoBusiness.RecuperarStatusAnterior(obj.Status);
                    List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);
                    string sUniqueKey = Guid.NewGuid().ToString();
                    foreach (string resp in Responsaveis)
                    {
                        Incidente obj2 = new Incidente();
                        PropertyCopy.Copy(obj, obj2);

                        obj2.ID = Guid.NewGuid().ToString();
                        obj2.UniqueKey = sUniqueKey;
                        obj2.StatusWF = "RS";
                        obj2.MensagemPasso = item.Comentarios;
                        obj2.Status = novoStatus;

                        if (resp.Equals("Submitter"))
                        {
                            string sql = "select top 1 UsuarioInclusao from OBJIncidente where Codigo = '" + obj2.Codigo + "' order by DataInclusao";
                            string submitter = IncidenteBusiness.ExecuteQuery(sql);
                            if (string.IsNullOrEmpty(submitter))
                            {
                                throw new Exception("Não foi possível recuperar o usuário que criou o incidente.");
                            }

                            obj2.Responsavel = submitter;
                        }
                        else
                        {
                            obj2.Responsavel = resp;
                        }
                        
                        obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        obj2.UsuarioExclusao = null;
                        obj2.DataExclusao = DateTime.MaxValue;

                        IncidenteBusiness.Inserir(obj2);

                        Severino.GravaCookie("MensagemSucesso", "O incidente " + obj2.Codigo + " foi rejeitado com sucesso.", 10);
                    }
                }

                return Json(new { sucesso = "O passo do workflow associado ao documento foi aprovado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }


    }
}