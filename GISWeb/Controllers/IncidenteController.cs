using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISHelpers.Extensions.System;
using GISHelpers.Utils;
using GISModel.DTO.Envolvidos;
using GISModel.DTO.Incidente;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ.Tabelas;
using GISModel.Entidades.REL;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IncidenteController : BaseController
    {

        #region

            [Inject]
            public IArquivoBusiness ArquivoBusiness { get; set; }

            [Inject]
            public IIncidenteBusiness IncidenteBusiness { get; set; }

            [Inject]
            public IESocialBusiness ESocialBusiness { get; set; }

            [Inject]
            public IBaseBusiness<Municipio> MunicipioBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public IOperacaoBusiness OperacaoBusiness { get; set; }

            [Inject]
            public IEmpregadoProprioBusiness EmpregadoProprioBusiness { get; set; }

            [Inject]
            public IEmpregadoContratadoBusiness EmpregadoTerceiroBusiness { get; set; }

            [Inject]
            public IBaseBusiness<RegistroEmpregadoProprio> RegistroEmpregadoProprioBusiness { get; set; }

            [Inject]
            public IBaseBusiness<RegistroEmpregadoContratado> RegistroEmpregadoContratadoBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.TRegistros = IncidenteBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }
        
        public ActionResult ListaAcidentes()
        {
            ViewBag.ListaAcidentes = IncidenteBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        public ActionResult BuscarRegistroPorID(string RegistroID)
        {
            try
            {
                Incidente oRegistro = IncidenteBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(RegistroID));
                if (oRegistro == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "registro não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oRegistro) });
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

        public ActionResult AnexarArquivo(string id)
        {
            ViewBag.Registro = id;
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incidente registro = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(id) && string.IsNullOrEmpty(a.UsuarioExclusao));
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
        }

        public ActionResult Novo()
        {
            ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

            return View();
        }

        public ActionResult Edicao(string id)
        {
            return View(IncidenteBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Incidente entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UniqueKey = Guid.NewGuid().ToString();

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    entidade.Status = "Em Edição";
                    entidade.Responsavel = entidade.UsuarioInclusao;
                    entidade.Codigo = "I-" + DateTime.Now.Year.ToString() + "-" + IncidenteBusiness.GetNextNumber("Incidente", "select max(SUBSTRING(codigo, 8, 6)) from objincidente").ToString().PadLeft(6, '0');
                    entidade.StatusWF = "RS";
                    IncidenteBusiness.Inserir(entidade);

                    Severino.GravaCookie("MensagemSucesso", "O incidente foi cadastrado com sucesso.", 10);
                    Severino.GravaCookie("FuncaoInboxAChamar", "Incidentes", 10);
                    Severino.GravaCookie("ObjRecemCriado", entidade.UniqueKey, 10);

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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Incidente TRegistro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IncidenteBusiness.Alterar(TRegistro);

                    TempData["MensagemSucesso"] = "Registro Atualizado com Sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Registro") } });
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
        public ActionResult Terminar(string RegistroID)
        {
            try
            {
                Incidente oRegistro = IncidenteBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(RegistroID));
                if (oRegistro == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Empregado, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oRegistro.DataExclusao = DateTime.Now;
                    oRegistro.UsuarioExclusao = "LoginTeste";
                    IncidenteBusiness.Alterar(oRegistro);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Registro:'" + oRegistro.UniqueKey + "' foi excluído com sucesso." } });
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

        [HttpPost]
        public ActionResult TerminarComRedirect(string RegistroID)
        {
            try
            {
                Incidente oRegistro = IncidenteBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(RegistroID));
                if (oRegistro == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Registro, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oRegistro.DataExclusao = DateTime.Now;
                    oRegistro.UsuarioExclusao = "LoginTeste";

                    IncidenteBusiness.Alterar(oRegistro);

                    TempData["MensagemSucesso"] = "O Registro de Nº: '" + oRegistro.UniqueKey + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Registro") } });
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
        public ActionResult _Upload()
        {
            try
            {
                return PartialView("_Upload");
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
        public ActionResult Upload()
        {
            try
            {
                string fName = string.Empty;
                string msgErro = string.Empty;
                foreach (string fileName in Request.Files.AllKeys)
                {
                    HttpPostedFileBase oFile = Request.Files[fileName];
                    fName = oFile.FileName;
                    if (oFile != null)
                    {
                        string sExtensao = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
                        if (sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("GIF"))
                        {
                            //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
                            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                            sLocalFile = Path.Combine(sLocalFile, "Relatorios");
                            sLocalFile = Path.Combine(sLocalFile, "LoginTeste");

                            if (!System.IO.Directory.Exists(sLocalFile))
                                Directory.CreateDirectory(sLocalFile);
                            else
                            {
                                //Tratamento de limpar arquivos da pasta, pois o usuário pode estar apenas alterando o arquivo.
                                //Limpar para não ficar lixo.
                                //O arquivo que for salvo abaixo será limpado após o cadastro.
                                //Se o usuário cancelar o cadastro, a rotina de limpar diretórios ficará responsável por limpá-lo.
                                foreach (string iFile in System.IO.Directory.GetFiles(sLocalFile))
                                {
                                    System.IO.File.Delete(iFile);
                                }
                            }

                            sLocalFile = Path.Combine(sLocalFile, oFile.FileName);

                            oFile.SaveAs(sLocalFile);

                        }
                        else
                        {
                            throw new Exception("Extensão do arquivo não permitida.");
                        }

                    }
                }
                if (string.IsNullOrEmpty(msgErro))
                    return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", arquivo = fName, erro = msgErro });
                else
                    return Json(new { erro = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }
        
        public ActionResult listar()
        {
            ViewBag.RegistroID = new SelectList(IncidenteBusiness.Consulta.ToList());
            return View();
        }

        // Gera o Relatorio do acidente
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incidente registro = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(id) && string.IsNullOrEmpty(a.UsuarioExclusao));
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
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
                    Incidente registro = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(uniquekey) && string.IsNullOrEmpty(a.UsuarioExclusao));

                    VMIncidente vm = new VMIncidente();
                    vm.UniqueKey = registro.UniqueKey;
                    vm.Codigo = registro.Codigo;
                    vm.Status = registro.Status;
                    vm.Descricao = registro.Descricao;
                    vm.AcidenteFatal = registro.AcidenteFatal ? "Sim" : "Não";
                    
                    Municipio mun = MunicipioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Codigo.Equals(registro.UKMunicipio));
                    if (mun != null)
                        vm.Municipio = mun.NomeCompleto;

                    ESocial eso = ESocialBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Codigo.Equals(registro.UKESocial));
                    if (eso != null)
                        vm.ESocial = eso.NomeCompleto;

                    vm.Estado = registro.Estado;
                    vm.Logradouro = registro.Logradouro;
                    vm.NumeroLogradouro = registro.NumeroLogradouro;
                    vm.ETipoAcidente =  registro.ETipoAcidente.GetDisplayName();

                    if (registro.TipoLocalAcidente != 0)
                        vm.TipoLocalAcidente = registro.TipoLocalAcidente.GetDisplayName();


                    vm.DataIncidente = registro.DataIncidente.ToString("dd/MM/yyyy");
                    vm.DataInclusao = registro.DataInclusao.ToString();
                    vm.UsuarioInclusao = registro.UsuarioInclusao;

                    vm.Arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(uniquekey)).ToList();

                    vm.EnvolvidosProprio = (from rel in RegistroEmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                                            join envol in EmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoProprio equals envol.UniqueKey
                                            select new VMProprio() {
                                                Funcao = rel.Funcao,
                                                NumeroPessoal = envol.NumeroPessoal,
                                                Nome = envol.Nome,
                                                UKEmpregado = envol.UniqueKey,
                                                UKRel = rel.UniqueKey
                                            }).ToList();

                    vm.EnvolvidosTerceiro = (from rel in RegistroEmpregadoContratadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                                             join envol in EmpregadoTerceiroBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoContratado equals envol.UniqueKey
                                             select new VMTerceiro()
                                             {
                                                 Funcao = rel.Funcao,
                                                 CPF = envol.CPF,
                                                 Nome = envol.Nome,
                                                 UKEmpregado = envol.UniqueKey,
                                                 UKRel = rel.UniqueKey
                                             }).ToList();

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


        [RestritoAAjax]
        [HttpPost]
        public ActionResult MontarMenuDeOperacoes(string uk)
        {
            try
            {
                Incidente fichaPersistida = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                if (fichaPersistida == null)
                    throw new Exception("As informações fornecidas para montagem do menu de operações não são válidas.");

                fichaPersistida.Operacoes = OperacaoBusiness.RecuperarTodasPermitidas(CustomAuthorizationProvider.UsuarioAutenticado.Login, fichaPersistida);

                return PartialView("_MenuOperacoes", fichaPersistida);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

    }
}