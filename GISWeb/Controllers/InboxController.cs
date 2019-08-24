using GISCore.Business.Abstract;
using GISHelpers.Extensions.System;
using GISHelpers.Utils;
using GISModel.DTO.Inbox;
using GISModel.DTO.Incidente;
using GISModel.DTO.IncidenteVeiculo;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

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
            public IIncidenteVeiculoBusiness IncidenteVeiculoBusiness { get; set; }

            [Inject]
            public IOperacaoBusiness OperacaoBusiness { get; set; }

            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

            [Inject]
            public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

            [Inject]
            public IBaseBusiness<Workflow> WorkflowBusiness { get; set; }

        #endregion

        public ActionResult BuscarTotalDocsInbox() {

            int iTotal = 0;
            int iPessoal = 0;
            int iGrupos = 0;
            int iPessoalVeiculo = 0;
            int iGruposVeiculo = 0;
            
            
            if (memoryCacheDefault.Contains(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"))
            {
                List<int> lista = (List<int>)memoryCacheDefault[CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"];
                iTotal = lista[0];
                iPessoal = lista[1];
                iGrupos = lista[2];
                iPessoalVeiculo = lista[3];
                iGruposVeiculo = lista[4];
            }
            else
            {
                List<string> perfis = (from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                       join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                       where usuarioperfil.UKUsuario.Equals(CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey)
                                       select "'" + perfil.Nome + "'").ToList();

                string sql = @"select 'Pessoal' as tipo, COUNT(*) as Total
                           from OBJIncidente i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in ('" + CustomAuthorizationProvider.UsuarioAutenticado.Login + @"') and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all

                           select 'PessoalVeiculo' as tipo, COUNT(*) as Total
                           from OBJIncidenteVeiculo i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in ('" + CustomAuthorizationProvider.UsuarioAutenticado.Login + @"') and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all
                           
                           select 'Grupos' as tipo, COUNT(*) as Total
                           from OBJIncidente i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all

                           select 'GruposVeiculo' as tipo, COUNT(*) as Total
                           from OBJIncidenteVeiculo i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and w.UsuarioExclusao is null ";

                DataTable dtInbox = PerfilBusiness.GetDataTable(sql);
                if (dtInbox.Rows.Count > 0)
                {
                    foreach (DataRow row in dtInbox.Rows)
                    {
                        if (row[0].ToString().Equals("Pessoal"))
                        {
                            iPessoal = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("Grupos"))
                        {
                            iGrupos = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("PessoalVeiculo"))
                        {
                            iPessoalVeiculo = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("GruposVeiculo"))
                        {
                            iGruposVeiculo = int.Parse(row[1].ToString());
                        }
                    }

                    iTotal = iPessoal + iGrupos + iPessoalVeiculo + iGruposVeiculo;
                }

                List<int> lista = new List<int>();
                lista.Add(iTotal);
                lista.Add(iPessoal);
                lista.Add(iGrupos);
                lista.Add(iPessoalVeiculo);
                lista.Add(iGruposVeiculo);

                memoryCacheDefault.Add(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal", lista, DateTime.Now.AddHours(2));
            }

            return Json(new { resultado = new { Total = iTotal, Pessoal = iPessoal, Grupos = iGrupos, PessoalVeiculo = iPessoalVeiculo, GruposVeiculo = iGruposVeiculo } });
        }

        public List<int> BuscarTotalInbox()
        {

            int iTotal = 0;
            int iPessoal = 0;
            int iGrupos = 0;
            int iPessoalVeiculo = 0;
            int iGruposVeiculo = 0;

            List<int> lista = new List<int>();

            if (memoryCacheDefault.Contains(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"))
            {
                lista = (List<int>)memoryCacheDefault[CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"];
                iTotal = lista[0];
                iPessoal = lista[1];
                iGrupos = lista[2];
                iPessoalVeiculo = lista[3];
                iGruposVeiculo = lista[4];
            }
            else
            {
                List<string> perfis = (from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                       join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                       where usuarioperfil.UKUsuario.Equals(CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey)
                                       select "'" + perfil.Nome + "'").ToList();

                string sql = @"select 'Pessoal' as tipo, COUNT(*) as Total
                           from OBJIncidente i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in ('" + CustomAuthorizationProvider.UsuarioAutenticado.Login + @"') and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all

                           select 'PessoalVeiculo' as tipo, COUNT(*) as Total
                           from OBJIncidenteVeiculo i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in ('" + CustomAuthorizationProvider.UsuarioAutenticado.Login + @"') and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all
                           
                           select 'Grupos' as tipo, COUNT(*) as Total
                           from OBJIncidente i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and w.UsuarioExclusao is null 

                           union all

                           select 'GruposVeiculo' as tipo, COUNT(*) as Total
                           from OBJIncidenteVeiculo i, OBJWorkflow w
                           where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and w.UsuarioExclusao is null ";

                DataTable dtInbox = PerfilBusiness.GetDataTable(sql);
                if (dtInbox.Rows.Count > 0)
                {
                    foreach (DataRow row in dtInbox.Rows)
                    {
                        if (row[0].ToString().Equals("Pessoal"))
                        {
                            iPessoal = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("Grupos"))
                        {
                            iGrupos = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("PessoalVeiculo"))
                        {
                            iPessoalVeiculo = int.Parse(row[1].ToString());
                        }
                        else if (row[0].ToString().Equals("GruposVeiculo"))
                        {
                            iGruposVeiculo = int.Parse(row[1].ToString());
                        }
                    }

                    iTotal = iPessoal + iGrupos + iPessoalVeiculo + iGruposVeiculo;
                }

                
                lista.Add(iTotal);
                lista.Add(iPessoal);
                lista.Add(iGrupos);
                lista.Add(iPessoalVeiculo);
                lista.Add(iGruposVeiculo);

                memoryCacheDefault.Add(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal", lista, DateTime.Now.AddHours(2));
            }

            return lista;
        }



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
                List<int> lista = BuscarTotalInbox();
                ViewBag.Pessoa = lista[1];
                ViewBag.Veiculo = lista[3];

                switch (tab)
                {
                    case "Incidentes":

                        List<VMIncidente> incidentes = (from wf in WorkflowBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                                                        join pro in IncidenteBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on wf.UKObject equals pro.UniqueKey
                                                        join org in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on pro.UKOrgao equals org.UniqueKey
                                                        orderby pro.DataInclusao descending
                                                        select new VMIncidente()
                                                        {
                                                            UniqueKey = pro.UniqueKey,
                                                            Codigo = pro.Codigo,
                                                            StatusWF = wf.Status,
                                                            ETipoAcidente = pro.ETipoAcidente.GetDisplayName(),
                                                            DataIncidente = pro.DataIncidente.ToString("dd/MM/yyyy"),
                                                            AcidenteFatal = pro.AcidenteFatal ? "Sim" : "Não",
                                                            AcidenteGraveIP102 = pro.AcidenteGraveIP102 ? "Sim" : "Não",
                                                            Orgao = org.Sigla,
                                                            MensagemPasso = wf.MensagemPasso,
                                                            UKWorkflow = wf.UniqueKey
                                                        }).ToList();


                        return PartialView("_ListarIncidente", incidentes);

                    case "IncidentesVeiculos":

                        List<VMIncidenteVeiculo> incidentesVeiculos =
                                                       (from wf in WorkflowBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                                                        join pro in IncidenteVeiculoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on wf.UKObject equals pro.UniqueKey
                                                        join org in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on pro.UKOrgao equals org.UniqueKey
                                                        orderby pro.DataInclusao descending
                                                        select new VMIncidenteVeiculo()
                                                        {
                                                            UniqueKey = pro.UniqueKey,
                                                            Codigo = pro.Codigo,
                                                            StatusWF = wf.Status,
                                                            ETipoAcidente = pro.ETipoAcidente.GetDisplayName(),
                                                            DataIncidente = pro.DataIncidente.ToString("dd/MM/yyyy"),
                                                            AcidenteFatal = pro.AcidenteFatal ? "Sim" : "Não",
                                                            AcidenteGraveIP102 = pro.AcidenteGraveIP102 ? "Sim" : "Não",
                                                            Orgao = org.Sigla,
                                                            MensagemPasso = wf.MensagemPasso,
                                                            UKWorkflow = wf.UniqueKey
                                                        }).ToList();

                        return PartialView("_ListarIncidenteVeiculo", incidentesVeiculos);

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

                List<int> listaTotal = BuscarTotalInbox();
                ViewBag.Pessoa = listaTotal[2];
                ViewBag.Veiculo = listaTotal[4];


                    List<string> perfis = (from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                          where usuarioperfil.UKUsuario.Equals(CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey)
                                          select "'" + perfil.Nome + "'").ToList();

                switch (tab)
                {
                    case "Incidentes":

                        List<VMIncidente> lista = new List<VMIncidente>();

                        string sql = @"select i.UniqueKey, i.Codigo, i.DataIncidente, i.ETipoAcidente, d.Sigla, i.AcidenteFatal, i.AcidenteGraveIP102, w.Status, w.MensagemPasso, w.Uniquekey
                                       from OBJIncidente i, OBJDepartamento d, OBJWorkflow w
                                       where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and
		                                     i.UKOrgao = d.UniqueKey and d.UsuarioExclusao is null and w.UsuarioExclusao is null
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
                                    StatusWF = row[7].ToString(),
                                    MensagemPasso = row[8].ToString(),
                                    UKWorkflow = row[9].ToString()
                                });
                            }

                        }

                        return PartialView("_ListarIncidente", lista);

                    case "IncidentesVeiculos":

                        List<VMIncidenteVeiculo> listaVeiculos = new List<VMIncidenteVeiculo>();

                        string sqlVeiculos = @"select i.UniqueKey, i.Codigo, i.DataIncidente, i.ETipoAcidente, d.Sigla, i.AcidenteFatal, i.AcidenteGraveIP102, w.Status, w.MensagemPasso, w.Uniquekey
                                       from OBJIncidenteVeiculo i, OBJDepartamento d, OBJWorkflow w
                                       where i.UniqueKey = w.ukobject and w.Responsavel in (" + string.Join(",", perfis) + @") and i.UsuarioExclusao is null and
		                                     i.UKOrgao = d.UniqueKey and d.UsuarioExclusao is null and w.UsuarioExclusao is null
                                       order by i.datainclusao desc ";

                        DataTable dtInboxVeiculos = PerfilBusiness.GetDataTable(sqlVeiculos);
                        if (dtInboxVeiculos.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtInboxVeiculos.Rows)
                            {
                                listaVeiculos.Add(new VMIncidenteVeiculo()
                                {
                                    UniqueKey = row[0].ToString(),
                                    Codigo = row[1].ToString(),
                                    DataIncidente = ((DateTime)row[2]).ToString("dd/MM/yyyy"),
                                    ETipoAcidente = ((ETipoAcidenteVeiculo)Enum.Parse(typeof(ETipoAcidenteVeiculo), row[3].ToString())).GetDisplayName(),
                                    Orgao = row[4].ToString(),
                                    AcidenteFatal = ((bool)row[5]) ? "Sim" : "Não",
                                    AcidenteGraveIP102 = ((bool)row[6]) ? "Sim" : "Não",
                                    StatusWF = row[7].ToString(),
                                    MensagemPasso = row[8].ToString(),
                                    UKWorkflow = row[9].ToString()
                                });
                            }
                        }

                        return PartialView("_ListarIncidenteVeiculo", listaVeiculos);
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
        public ActionResult AprovarIncidente(string uk, string tipo, string ukwf)
        {
            try
            {
                if (string.IsNullOrEmpty(ukwf))
                    throw new Exception("Não foi possível localizar a identificação do fluxo do incidente.");

                if (tipo.Equals("Veiculo"))
                {
                    IncidenteVeiculo fichaPersistidaVeiculo = IncidenteVeiculoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistidaVeiculo != null)
                    {
                        ViewBag.UniqueKey = fichaPersistidaVeiculo.UniqueKey;
                        ViewBag.Codigo = fichaPersistidaVeiculo.Codigo;
                        ViewBag.Status = fichaPersistidaVeiculo.Status;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Veiculo";
                        
                        return PartialView("_AprovarIncidente");
                    }
                }
                else
                {
                    Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistida != null)
                    {
                        ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                        ViewBag.Codigo = fichaPersistida.Codigo;
                        ViewBag.Status = fichaPersistida.Status;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Pessoa";
                        
                        return PartialView("_AprovarIncidente");
                    }
                }

                throw new Exception("Não foi possível encontrar as informações necessárias para prosseguir na aprovação.");

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
                if (string.IsNullOrEmpty(item?.UKWorkflow))
                    throw new Exception("Não foi possível localizar a identificação do fluxo do incidente.");

                Workflow objWF = WorkflowBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UKWorkflow));
                if (objWF == null)
                    throw new Exception("Não foi possível recuperar o incidente no fluxo de aprovação.");

                DateTime dtNow = DateTime.Now;

                if (item.Tipo.Equals("Pessoa"))
                {
                    Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey));
                    if (obj == null)
                    {
                        throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                    }

                    ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);


                    objWF.Status = "SO";
                    objWF.DataExclusao = dtNow;
                    objWF.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    WorkflowBusiness.Alterar(objWF);



                    string novoStatus = OperacaoBusiness.RecuperarProximoStatus(objWF.Nome);
                    List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);

                    foreach (string resp in Responsaveis)
                    {
                        Workflow obj2 = new Workflow();
                        PropertyCopy.Copy(objWF, obj2);

                        obj2.ID = Guid.NewGuid().ToString();
                        obj2.UniqueKey = item.UniqueKey;

                        if (novoStatus.Equals("Concluído"))
                        {
                            obj2.Status = "SO";
                        }
                        else
                        {
                            obj2.Status = "RS";
                        }

                        obj2.MensagemPasso = item.Comentarios;
                        obj2.Nome = novoStatus;
                        obj2.Responsavel = resp;
                        obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        obj2.UsuarioExclusao = null;
                        obj2.DataInclusao = dtNow;
                        obj2.DataExclusao = DateTime.MaxValue;

                        WorkflowBusiness.Inserir(obj2);

                        if (obj2.Status.Equals("SO"))
                        {
                            obj.Status = StatusIncidente.Concluido;
                            obj.DataAtualizacao = dtNow;
                            IncidenteBusiness.Alterar(obj);
                        }


                        Severino.GravaCookie("MensagemSucesso", "O incidente " + obj.Codigo + " foi aprovado com sucesso.", 10);
                    }

                }
                else
                {
                    IncidenteVeiculo obj = IncidenteVeiculoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey));
                    if (obj == null)
                    {
                        throw new Exception("Não foi possível recuperar o incidente com o veículo através da identificação recebida.");
                    }
                    else
                    {
                        ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                        objWF.Status = "SO";
                        objWF.DataExclusao = DateTime.Now;
                        objWF.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        WorkflowBusiness.Alterar(objWF);

                        string novoStatus = OperacaoBusiness.RecuperarProximoStatus(objWF.Nome);
                        List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);

                        foreach (string resp in Responsaveis)
                        {
                            Workflow obj2 = new Workflow();
                            PropertyCopy.Copy(objWF, obj2);

                            obj2.ID = Guid.NewGuid().ToString();
                            obj2.UniqueKey = item.UniqueKey;

                            if (novoStatus.Equals("Concluído"))
                            {
                                obj2.Status = "SO";
                            }
                            else
                            {
                                obj2.Status = "RS";
                            }

                            obj2.MensagemPasso = item.Comentarios;
                            obj2.Nome = novoStatus;
                            obj2.Responsavel = resp;
                            obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioExclusao = null;
                            obj2.DataInclusao = dtNow;
                            obj2.DataExclusao = DateTime.MaxValue;

                            WorkflowBusiness.Inserir(obj2);

                            if (obj2.Status.Equals("SO"))
                            {
                                obj.Status = StatusIncidente.Concluido;
                                obj.DataAtualizacao = dtNow;
                                IncidenteVeiculoBusiness.Alterar(obj);
                            }

                            Severino.GravaCookie("MensagemSucesso", "O incidente veículo " + obj.Codigo + " foi aprovado com sucesso.", 10);
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
        public ActionResult Assumir(string uk, string tipo, string ukwf)
        {
            try
            {
                if (string.IsNullOrEmpty(ukwf))
                    throw new Exception("Não foi possível localizar a identificação do fluxo do incidente.");
                
                Workflow objWF = WorkflowBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ukwf));
                if (objWF == null)
                    throw new Exception("Não foi possível encontrar o fluxo de aprovação deste incidente.");

                if (tipo.Equals("Veiculo"))
                {
                    IncidenteVeiculo fichaPersistida = IncidenteVeiculoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistida != null)
                    {
                        ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                        ViewBag.Codigo = fichaPersistida.Codigo;
                        ViewBag.Status = fichaPersistida.Status;
                        ViewBag.Responsavel = objWF.Responsavel;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Veiculo";
                        
                        return PartialView("_AssumirIncidente");
                    }
                }
                else
                {
                    Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistida != null)
                    {
                        ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                        ViewBag.Codigo = fichaPersistida.Codigo;
                        ViewBag.Status = fichaPersistida.Status;
                        ViewBag.Responsavel = objWF.Responsavel;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Pessoa";                        

                        return PartialView("_AssumirIncidente");
                    }
                }

                throw new Exception("Não foi possível encontrar as informações necessárias para prosseguir na atribuição.");
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

                List<Workflow> lista = WorkflowBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UKWorkflow)).ToList();

                

                if (lista == null || lista.Count == 0)
                {
                    throw new Exception("Não foi possível recuperar o fluxo de aprovação do incidente.");
                }
                else
                {
                    ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                    DateTime dtNow = DateTime.Now;

                    bool first = true;
                    foreach (Workflow obj in lista)
                    {
                        obj.Status = "CL";
                        obj.DataExclusao = dtNow;
                        obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        WorkflowBusiness.Alterar(obj);

                        if (first)
                        {
                            first = false;
                            Workflow obj2 = new Workflow();
                            PropertyCopy.Copy(obj, obj2);

                            obj2.ID = Guid.NewGuid().ToString();
                            obj2.Status = "RS";
                            obj2.Responsavel = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioExclusao = null;
                            obj2.DataInclusao = dtNow;
                            obj2.DataExclusao = DateTime.MaxValue;

                            if (!string.IsNullOrEmpty(item.Comentarios))
                                obj2.MensagemPasso = item.Comentarios;

                            WorkflowBusiness.Inserir(obj2);

                            Severino.GravaCookie("MensagemSucesso", "O incidente foi redirecionado para a sua caixa pessoal.", 10);
                        }
                    }
                }

                //if (item.Tipo.Equals("Pessoa"))
                //{
                //}
                //else
                //{
                //    List<IncidenteVeiculo> lista = IncidenteVeiculoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey)).ToList();

                //    if (lista == null || lista.Count == 0)
                //    {
                //        throw new Exception("Não foi possível recuperar o incidente com veículo através da identificação recebida.");
                //    }
                //    else
                //    {
                //        ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                //        bool first = true;
                //        foreach (IncidenteVeiculo obj in lista)
                //        {
                //            obj.StatusWF = "SO";
                //            obj.DataExclusao = DateTime.Now;
                //            obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                //            IncidenteVeiculoBusiness.Alterar(obj);

                //            if (first)
                //            {
                //                first = false;
                //                IncidenteVeiculo obj2 = new IncidenteVeiculo();
                //                PropertyCopy.Copy(obj, obj2);

                //                obj2.ID = Guid.NewGuid().ToString();
                //                obj2.StatusWF = "RS";

                                

                //                obj2.Responsavel = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                //                obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                //                obj2.UsuarioExclusao = null;
                //                obj2.DataInclusao = DateTime.Now;
                //                obj2.DataExclusao = DateTime.MaxValue;
                //                if (!string.IsNullOrEmpty(item.Comentarios))
                //                {
                //                    obj2.MensagemPasso = item.Comentarios;
                //                }

                //                IncidenteVeiculoBusiness.Inserir(obj2);

                //                Severino.GravaCookie("MensagemSucesso", "O incidente com veículo " + obj2.Codigo + " foi redirecionado para a sua caixa pessoal.", 10);
                //            }
                //        }
                //    }
                //}

                return Json(new { sucesso = "O passo do workflow associado ao incidente foi aprovado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }




        [RestritoAAjax]
        public ActionResult RejeitarIncidente(string uk, string tipo, string ukwf)
        {
            try
            {
                if (string.IsNullOrEmpty(ukwf))
                    throw new Exception("Não foi possível localizar a identificação do fluxo do incidente.");

                if (tipo.Equals("Veiculo"))
                {
                    IncidenteVeiculo fichaPersistida = IncidenteVeiculoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistida != null)
                    {
                        ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                        ViewBag.Codigo = fichaPersistida.Codigo;
                        ViewBag.Status = fichaPersistida.Status;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Veiculo";

                        return PartialView("_RejeitarIncidente");
                    }
                }
                else
                {
                    Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                    if (fichaPersistida != null)
                    {
                        ViewBag.UniqueKey = fichaPersistida.UniqueKey;
                        ViewBag.Codigo = fichaPersistida.Codigo;
                        ViewBag.Status = fichaPersistida.Status;
                        ViewBag.UKWF = ukwf;
                        ViewBag.Tipo = "Pessoa";

                        return PartialView("_RejeitarIncidente");
                    }
                }

                throw new Exception("Não foi possível encontrar as informações necessárias para prosseguir com a rejeição.");
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
                if (string.IsNullOrEmpty(item.UKWorkflow))
                    throw new Exception("Não foi possível localizar a identificação do fluxo do incidente.");

                Workflow objWF = WorkflowBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UKWorkflow));
                if (objWF == null)
                    throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");

                DateTime dtNow = DateTime.Now;

                if (item.Tipo.Equals("Pessoa"))
                {
                    Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey));

                    if (obj == null)
                    {
                        throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                    }
                    else
                    {
                        ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                        objWF.Status = "RJ";
                        objWF.DataExclusao = dtNow;
                        objWF.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        WorkflowBusiness.Alterar(objWF);

                        string novoStatus = OperacaoBusiness.RecuperarStatusAnterior(objWF.Nome);

                        string sql = "select top 1 Responsavel from objworkflow where ukObject = '" + item.UniqueKey + "' and UsuarioExclusao is not null and Nome = '" + novoStatus + "' and MajorVersion = " + objWF.MajorVersion + " order by DataExclusao desc";
                        string Responsavel = IncidenteBusiness.ExecuteQuery(sql);
                        if (string.IsNullOrEmpty(Responsavel))
                        {
                            List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);
                            foreach (string resp in Responsaveis)
                            {
                                Workflow obj2 = new Workflow();
                                PropertyCopy.Copy(objWF, obj2);

                                obj2.ID = Guid.NewGuid().ToString();
                                obj2.Status = "RS";
                                obj2.MensagemPasso = item.Comentarios;
                                obj2.Nome = novoStatus;
                                obj2.MinorVersion += 1;

                                if (resp.Equals("Submitter"))
                                {
                                    string sql2 = "select top 1 UsuarioInclusao from OBJIncidente where Codigo = '" + obj.Codigo + "' order by DataInclusao";
                                    string submitter = IncidenteBusiness.ExecuteQuery(sql2);
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
                                obj2.DataInclusao = dtNow;
                                obj2.DataExclusao = DateTime.MaxValue;

                                WorkflowBusiness.Inserir(obj2);

                                Severino.GravaCookie("MensagemSucesso", "O incidente " + obj.Codigo + " foi rejeitado com sucesso.", 10);
                            }
                        }
                        else
                        {
                            Workflow obj2 = new Workflow();
                            PropertyCopy.Copy(objWF, obj2);

                            obj2.ID = Guid.NewGuid().ToString();
                            obj2.Status = "RS";
                            obj2.MensagemPasso = item.Comentarios;
                            obj2.Nome = novoStatus;
                            obj2.Responsavel = Responsavel;
                            obj2.MinorVersion += 1;
                            obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioExclusao = null;
                            obj2.DataInclusao = dtNow;
                            obj2.DataExclusao = DateTime.MaxValue;

                            WorkflowBusiness.Inserir(obj2);

                            Severino.GravaCookie("MensagemSucesso", "O incidente " + obj.Codigo + " foi rejeitado com sucesso.", 10);
                        }

                    }
                }
                else
                {
                    IncidenteVeiculo obj = IncidenteVeiculoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(item.UniqueKey));
                    if (obj == null)
                    {
                        throw new Exception("Não foi possível recuperar o incidente através da identificação recebida.");
                    }
                    else
                    {
                        ReiniciarCache(CustomAuthorizationProvider.UsuarioAutenticado.Login);

                        objWF.Status = "RJ";

                        

                        objWF.DataExclusao = dtNow;
                        objWF.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        WorkflowBusiness.Alterar(objWF);

                        string novoStatus = OperacaoBusiness.RecuperarStatusAnterior(objWF.Nome);

                        string sql = "select top 1 Responsavel from objworkflow where UKObject = '" + item.UniqueKey + "' and UsuarioExclusao is not null and Nome = '" + novoStatus + "' and MajorVersion = " + objWF.MajorVersion + " order by DataExclusao desc";
                        string Responsavel = IncidenteBusiness.ExecuteQuery(sql);
                        if (string.IsNullOrEmpty(Responsavel))
                        {
                            List<string> Responsaveis = OperacaoBusiness.RecuperarResponsavelPorStatus(novoStatus);
                            foreach (string resp in Responsaveis)
                            {
                                Workflow obj2 = new Workflow();
                                PropertyCopy.Copy(objWF, obj2);

                                obj2.ID = Guid.NewGuid().ToString();
                                obj2.Status = "RS";
                                obj2.MensagemPasso = item.Comentarios;
                                obj2.Nome = novoStatus;
                                obj2.MinorVersion += 1;

                                if (resp.Equals("Submitter"))
                                {
                                    string sql2 = "select top 1 UsuarioInclusao from OBJWorkflow where Codigo = '" + obj.Codigo + "' order by DataInclusao";
                                    string submitter = IncidenteBusiness.ExecuteQuery(sql2);
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
                                obj2.DataInclusao = dtNow;
                                obj2.DataExclusao = DateTime.MaxValue;

                                WorkflowBusiness.Inserir(obj2);

                                Severino.GravaCookie("MensagemSucesso", "O incidente com o veículo " + obj.Codigo + " foi rejeitado com sucesso.", 10);
                            }
                        }
                        else
                        {
                            Workflow obj2 = new Workflow();
                            PropertyCopy.Copy(objWF, obj2);

                            obj2.ID = Guid.NewGuid().ToString();
                            obj2.Status = "RS";
                            obj2.MensagemPasso = item.Comentarios;
                            obj2.Nome = novoStatus;
                            obj2.Responsavel = Responsavel;
                            obj2.MinorVersion += 1;
                            obj2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            obj2.UsuarioExclusao = null;
                            obj2.DataInclusao = dtNow;
                            obj2.DataExclusao = DateTime.MaxValue;

                            WorkflowBusiness.Inserir(obj2);

                            Severino.GravaCookie("MensagemSucesso", "O incidente com o veículo " + obj.Codigo + " foi rejeitado com sucesso.", 10);
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


        

    }
}