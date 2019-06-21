using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISHelpers.Extensions.System;
using GISHelpers.Utils;
using GISModel.DTO.Envolvidos;
using GISModel.DTO.Incidente;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.OBJ.Tabelas;
using GISModel.Entidades.REL;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            public ITipoAtividadeBusiness TipoAtividadeBusiness { get; set; }

            [Inject]
            public INaturezaBusiness NaturezaBusiness { get; set; }

            [Inject]
            public IFuncaoGridsBusiness FuncaoGridsBusiness { get; set; }

            [Inject]
            public IEspecieAcidenteImpessoalBusiness EspecieAcidenteImpessoalBusiness { get; set; }

            [Inject]
            public ITipoAcidentePessoalBusiness TipoAcidentePessoalBusiness { get; set; }

            [Inject]
            public IAgenteAcidenteBusiness AgenteAcidenteBusiness { get; set; }

            [Inject]
            public IFonteLesaoBusiness FonteLesaoBusiness { get; set; }

            [Inject]
            public IFatorPessoalInsegurancaBusiness FatorPessoalInsegurancaBusiness { get; set; }

            [Inject]
            public IAtoInseguroBusiness AtoInseguroBusiness { get; set; }

            [Inject]
            public ICondicaoAmbientalInsegBusiness CondicaoAmbientalInsegBusiness { get; set; }

            [Inject]
            public IPrejuizoMaterialBusiness PrejuizoMaterialBusiness { get; set; }


            [Inject]
            public ICodificacaoBusiness CodificacaoBusiness { get; set; }






            [Inject]
            public ICatBusiness CATBusiness { get; set; }

            [Inject]
            public ILesaoDoencaBusiness LesaoDoencaBusiness { get; set; }

            [Inject]
            public ICodificacaoBusiness LesaoEmpregadoBusiness { get; set; }

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

            [Inject]
            public INivelHierarquicoBusiness NivelHierarquicoBusiness { get; set; }


            [Inject]
            public INaturezaLesaoBusiness NaturezaLesaoBusiness { get; set; }

            [Inject]
            public ILocalizacaoLesaoBusiness LocalizacaoLesaoBusiness { get; set; }

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

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

        public ActionResult Edicao(string uniquekey)
        {
            try
            {
                Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(uniquekey));

                if (obj == null)
                {
                    throw new Exception("Não foi possível encontrar o incidente a partir da identificação.");
                }

                ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

                if (!string.IsNullOrEmpty(obj.UKOrgao))
                {
                    obj.UKDiretoria = BuscarDiretoriaPorOrgao(obj.UKOrgao);
                }

                return PartialView(obj);
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

        public string BuscarDiretoriaPorOrgao(string ukDepartamento)
        {
           
            Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ukDepartamento));
            if (dep == null)
            {
                throw new Exception("Não foi possível encontrar o departamento a partir da identificação.");
            }
            else
            {
                NivelHierarquico nh = NivelHierarquicoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(dep.UKNivelHierarquico));
                if (nh == null)
                {
                    throw new Exception("Não foi possível recuperar o nível hierarquico do departamento selecionado.");
                }
                else
                {
                    if (nh.Nome.Equals("Diretoria"))
                    {
                        return dep.Sigla;
                    }
                    else if (nh.Nome.Equals("Superintendência"))
                    {
                        Departamento depDir = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(dep.UKDepartamentoVinculado));
                        if (depDir == null)
                        {
                            throw new Exception("Não foi possível encontrar o órgão vinculado a Superintendência selecionada.");
                        }
                        else
                        {
                            return depDir.Sigla;
                        }
                    }
                    else if (nh.Nome.Equals("Gerência"))
                    {
                        Departamento depDir = (from dSup in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(dep.UKDepartamentoVinculado)).ToList()
                                                join dDir in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on dSup.UKDepartamentoVinculado equals dDir.UniqueKey
                                                select dDir).ToList().FirstOrDefault();
                        if (depDir == null)
                        {
                            throw new Exception("Não foi possível encontrar o órgão vinculado a Gerência selecionada.");
                        }
                        else
                        {
                            return depDir.Sigla;
                        }
                    }
                    else
                    {
                        throw new Exception("Nível hierarquico do departamento selecionado não conhecido.");
                    }
                }
            }

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
                    entidade.DataAtualizacao = DateTime.Now;
                    IncidenteBusiness.Inserir(entidade);

                    Severino.GravaCookie("MensagemSucesso", "O incidente foi cadastrado com sucesso.", 10);
                    Severino.GravaCookie("FuncaoInboxAChamar", "Incidentes", 10);
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Incidente entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Incidente obj = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (obj == null)
                    {
                        throw new Exception("Não foi possível localizar o incidente a ser atualizado.");
                    }

                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    IncidenteBusiness.Terminar(obj);

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    entidade.DataInclusao = obj.DataInclusao;
                    entidade.DataAtualizacao = DateTime.Now;

                    entidade.UKDiretoria = null;
                    entidade.Codigo = obj.Codigo;
                    entidade.Status = obj.Status;
                    entidade.StatusWF = obj.StatusWF;
                    entidade.Responsavel = obj.Responsavel;
                    IncidenteBusiness.Inserir(entidade);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Incidente " + entidade.Codigo + " atualizado com Sucesso." } });
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
        public ActionResult Terminar(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("A identificação do incidente não foi localizado entre os parâmetros.");

                Incidente oRegistro = IncidenteBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(id));
                if (oRegistro == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o incidente, pois o mesmo não foi localizado." } });
                }
                else
                {
                    //Registro
                    oRegistro.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    IncidenteBusiness.Terminar(oRegistro);

                    //Arquivos
                    List<Arquivo> arqs = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(id)).ToList();
                    if (arqs?.Count > 0)
                    {
                        foreach (Arquivo arq in arqs)
                        {
                            arq.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            ArquivoBusiness.Terminar(arq);
                        }
                    }

                    //Envolvidos
                    List<RegistroEmpregadoContratado> empTerc = RegistroEmpregadoContratadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(id)).ToList();
                    if (empTerc?.Count > 0)
                    {
                        foreach (RegistroEmpregadoContratado regContratado in empTerc)
                        {
                            //CAT
                            if (!string.IsNullOrEmpty(regContratado.UKCAT))
                            {
                                CAT catTemp = CATBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regContratado.UKCAT));
                                if (catTemp != null)
                                {
                                    catTemp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    CATBusiness.Terminar(catTemp);
                                }
                            }

                            //Lesao Doença
                            if (!string.IsNullOrEmpty(regContratado.UKLesaoDoenca))
                            {
                                LesaoDoenca lesao = LesaoDoencaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regContratado.UKLesaoDoenca));
                                if (lesao != null)
                                {
                                    lesao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    LesaoDoencaBusiness.Terminar(lesao);
                                }
                            }

                            //Lesao Empregado
                            if (!string.IsNullOrEmpty(regContratado.UKCodificacao))
                            {
                                Codificacao lesaoEmp = LesaoEmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regContratado.UKCodificacao));
                                if (lesaoEmp != null)
                                {
                                    lesaoEmp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    LesaoEmpregadoBusiness.Terminar(lesaoEmp);
                                }
                            }

                            regContratado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoContratadoBusiness.Terminar(regContratado);
                        }
                    }

                    List<RegistroEmpregadoProprio> empProprio = RegistroEmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(id)).ToList();
                    if (empProprio?.Count > 0)
                    {
                        foreach (RegistroEmpregadoProprio regProprio in empProprio)
                        {
                            //CAT
                            if (!string.IsNullOrEmpty(regProprio.UKCAT))
                            {
                                CAT catTemp = CATBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regProprio.UKCAT));
                                if (catTemp != null)
                                {
                                    catTemp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    CATBusiness.Terminar(catTemp);
                                }
                            }

                            //Lesao Doença
                            if (!string.IsNullOrEmpty(regProprio.UKLesaoDoenca))
                            {
                                LesaoDoenca lesao = LesaoDoencaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regProprio.UKLesaoDoenca));
                                if (lesao != null)
                                {
                                    lesao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    LesaoDoencaBusiness.Terminar(lesao);
                                }
                            }

                            //Lesao Empregado
                            if (!string.IsNullOrEmpty(regProprio.UKCodificacao))
                            {
                                Codificacao lesaoEmp = LesaoEmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(regProprio.UKCodificacao));
                                if (lesaoEmp != null)
                                {
                                    lesaoEmp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    LesaoEmpregadoBusiness.Terminar(lesaoEmp);
                                }
                            }

                            regProprio.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoProprioBusiness.Terminar(regProprio);
                        }
                    }

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O incidente " + oRegistro.Codigo + " foi excluído com sucesso." } });
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
                    List<Incidente> lista = IncidenteBusiness.Consulta.Where(a => a.UniqueKey.Equals(uniquekey) && string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                    Incidente registro = lista[0];

                    VMIncidente vm = new VMIncidente();
                    vm.UniqueKey = registro.UniqueKey;
                    vm.Codigo = registro.Codigo;
                    vm.Status = registro.Status;
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
                    vm.ETipoAcidente =  registro.ETipoAcidente.GetDisplayName();
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

                    vm.EnvolvidosProprio = (from rel in RegistroEmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                                            join envol in EmpregadoProprioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoProprio equals envol.UniqueKey
                                            select new VMProprio() {
                                                Funcao = rel.Funcao,
                                                NumeroPessoal = envol.NumeroPessoal,
                                                Nome = envol.Nome,
                                                UKEmpregado = envol.UniqueKey,
                                                UKRel = rel.UniqueKey,
                                                UKCodificacao = rel.UKCodificacao,
                                                UKCAT = rel.UKCAT,
                                                UKLesaoDoenca = rel.UKLesaoDoenca
                                            }).ToList();

                    vm.EnvolvidosTerceiro = (from rel in RegistroEmpregadoContratadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRegistro.Equals(registro.UniqueKey)).ToList()
                                             join envol in EmpregadoTerceiroBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on rel.UKEmpregadoContratado equals envol.UniqueKey
                                             select new VMTerceiro()
                                             {
                                                 Funcao = rel.Funcao,
                                                 CPF = envol.CPF,
                                                 Nome = envol.Nome,
                                                 UKEmpregado = envol.UniqueKey,
                                                 UKRel = rel.UniqueKey,
                                                 UKCodificacao = rel.UKCodificacao,
                                                 UKCAT = rel.UKCAT,
                                                 UKLesaoDoenca = rel.UKLesaoDoenca
                                             }).ToList();


                    vm.Operacoes = OperacaoBusiness.RecuperarTodasPermitidas(CustomAuthorizationProvider.UsuarioAutenticado.Login, CustomAuthorizationProvider.UsuarioAutenticado.Permissoes, lista);

                    registro.Operacoes = vm.Operacoes;

                    ViewBag.Incidente = registro;

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

                fichaPersistida.Operacoes = OperacaoBusiness.RecuperarTodasPermitidas(CustomAuthorizationProvider.UsuarioAutenticado.Login, CustomAuthorizationProvider.UsuarioAutenticado.Permissoes, fichaPersistida);

                return PartialView("_MenuOperacoes", fichaPersistida);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }


        public ActionResult NovaCodificacao(string UKRelEnvolvido, string Tipo)
        {

            ViewBag.TiposAtividades = TipoAtividadeBusiness.ListarTodos();
            ViewBag.Naturezas = NaturezaBusiness.ListarTodos();
            ViewBag.Funcoes = FuncaoGridsBusiness.ListarTodos();
            ViewBag.Especies = EspecieAcidenteImpessoalBusiness.ListarTodos();
            ViewBag.TiposAcidentes = TipoAcidentePessoalBusiness.ListarTodos();
            ViewBag.Agentes = AgenteAcidenteBusiness.ListarTodos();
            ViewBag.Fontes = FonteLesaoBusiness.ListarTodos();
            ViewBag.Fatores = FatorPessoalInsegurancaBusiness.ListarTodos();
            ViewBag.Atos = AtoInseguroBusiness.ListarTodos();
            ViewBag.Condicoes = CondicaoAmbientalInsegBusiness.ListarTodos();
            ViewBag.Prejuizos = PrejuizoMaterialBusiness.ListarTodos();

            VMNovaCodificacao obj = new VMNovaCodificacao()
            {
                UKRelEnvolvido = UKRelEnvolvido,
                Tipo = Tipo
            };

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarCodificacao(VMNovaCodificacao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Codificacao cod = new Codificacao()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        TipoAcidente = entidade.TipoAcidente,
                        Atividade = entidade.Atividade,
                        UKTipoAtividade = entidade.UKTipoAtividade,
                        UKNatureza = entidade.UKNatureza,
                        ConsequenciaLesao = entidade.ConsequenciaLesao,
                        UKFuncaoGRIDS = entidade.UKFuncaoGRIDS,
                        UKEspecieAcidImpessoal = entidade.UKEspecieAcidImpessoal,
                        UKTipoAcidPessoal = entidade.UKTipoAcidPessoal,
                        UKAgenteAcidente = entidade.UKAgenteAcidente,
                        UKFonteLesao = entidade.UKFonteLesao,
                        UKFatorPessoalInseg = entidade.UKFatorPessoalInseg,
                        UKAtoInseguro = entidade.UKAtoInseguro,
                        UKCondAmbientalInseg = entidade.UKCondAmbientalInseg,
                        UKPrejMaterial = entidade.UKPrejMaterial,
                        Custo = entidade.Custo,
                        DiasPerdidos = entidade.DiasPerdidos,
                        DiasDebitados = entidade.DiasDebitados,
                        DataObito = entidade.DataObito
                    };

                    CodificacaoBusiness.Inserir(cod);



                    //Atualizar rel com ukcodificacao
                    if (entidade.Tipo.Equals("Proprio"))
                    {
                        RegistroEmpregadoProprio rel = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoProprioBusiness.Terminar(rel);

                            RegistroEmpregadoProprio rel2 = new RegistroEmpregadoProprio();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoProprio = rel.UKEmpregadoProprio;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = cod.UniqueKey;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = rel.UKCAT;
                            
                            rel2.UniqueKey = rel.UniqueKey;
                            
                            RegistroEmpregadoProprioBusiness.Inserir(rel2);
                        }
                    }
                    else
                    {
                        RegistroEmpregadoContratado rel = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null) {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoContratadoBusiness.Terminar(rel);

                            RegistroEmpregadoContratado rel2 = new RegistroEmpregadoContratado();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoContratado = rel.UKEmpregadoContratado;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = cod.UniqueKey;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = rel.UKCAT;

                            rel2.UniqueKey = rel.UniqueKey;

                            rel2.UKFornecedor = rel.UKFornecedor;

                            RegistroEmpregadoContratadoBusiness.Inserir(rel2);
                        }
                    }
                    
                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Codificação cadastrada com sucesso" } });
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




        public ActionResult EditarCodificacao(string UKIncidente, string UKRelEnvolvido, string Tipo, string UKCodificacao)
        {

            try {

                if (string.IsNullOrEmpty(UKCodificacao))
                    throw new Exception("Parâmetro que identifica a codifição não encontrado.");

                if (string.IsNullOrEmpty(UKIncidente))
                    throw new Exception("Não foi possível encontrar a identificação do incidente nos parâmetros.");

                Incidente objIncidente = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(UKIncidente) && string.IsNullOrEmpty(a.UsuarioExclusao));
                if (objIncidente == null)
                {
                    throw new Exception("Não foi possível encontrar o incidente.");
                }

                if (objIncidente.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login) && !objIncidente.Status.Equals("Em Aprovação"))
                {
                    ViewBag.PodeEditar = true;
                }

                Codificacao cod = CodificacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKCodificacao));
                if (cod == null)
                    throw new Exception("Não foi possível encontrar a codificação.");

                ViewBag.TiposAtividades = TipoAtividadeBusiness.ListarTodos();
                ViewBag.Naturezas = NaturezaBusiness.ListarTodos();
                ViewBag.Funcoes = FuncaoGridsBusiness.ListarTodos();
                ViewBag.Especies = EspecieAcidenteImpessoalBusiness.ListarTodos();
                ViewBag.TiposAcidentes = TipoAcidentePessoalBusiness.ListarTodos();
                ViewBag.Agentes = AgenteAcidenteBusiness.ListarTodos();
                ViewBag.Fontes = FonteLesaoBusiness.ListarTodos();
                ViewBag.Fatores = FatorPessoalInsegurancaBusiness.ListarTodos();
                ViewBag.Atos = AtoInseguroBusiness.ListarTodos();
                ViewBag.Condicoes = CondicaoAmbientalInsegBusiness.ListarTodos();
                ViewBag.Prejuizos = PrejuizoMaterialBusiness.ListarTodos();

                VMNovaCodificacao obj = new VMNovaCodificacao()
                {
                    UKRelEnvolvido = UKRelEnvolvido,
                    Tipo = Tipo,
                    UniqueKey = cod.UniqueKey,
                    TipoAcidente = cod.TipoAcidente,
                    Atividade = cod.Atividade,
                    UKTipoAtividade = cod.UKTipoAtividade,
                    UKNatureza = cod.UKNatureza,
                    ConsequenciaLesao = cod.ConsequenciaLesao,
                    UKFuncaoGRIDS = cod.UKFuncaoGRIDS,
                    UKEspecieAcidImpessoal = cod.UKEspecieAcidImpessoal,
                    UKTipoAcidPessoal = cod.UKTipoAcidPessoal,
                    UKAgenteAcidente = cod.UKAgenteAcidente,
                    UKFonteLesao = cod.UKFonteLesao,
                    UKFatorPessoalInseg = cod.UKFatorPessoalInseg,
                    UKAtoInseguro = cod.UKAtoInseguro,
                    UKCondAmbientalInseg = cod.UKCondAmbientalInseg,
                    UKPrejMaterial = cod.UKPrejMaterial,
                    Custo = cod.Custo,
                    DiasPerdidos = cod.DiasPerdidos,
                    DiasDebitados = cod.DiasDebitados,
                    DataObito = cod.DataObito
                };

                return PartialView(obj);

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
        [ValidateAntiForgeryToken]
        public ActionResult AtualizarCodificacao(VMNovaCodificacao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.UniqueKey))
                        throw new Exception("Parâmetro que identifica a codificação a ser editada não encontrado.");

                    Codificacao codAntiga = CodificacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (codAntiga == null) 
                        throw new Exception("Não foi possível encontrar a codificação a ser atualizada.");

                    codAntiga.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    CodificacaoBusiness.Terminar(codAntiga);


                    Codificacao cod = new Codificacao()
                    {
                        UniqueKey = entidade.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        TipoAcidente = entidade.TipoAcidente,
                        Atividade = entidade.Atividade,
                        UKTipoAtividade = entidade.UKTipoAtividade,
                        UKNatureza = entidade.UKNatureza,
                        ConsequenciaLesao = entidade.ConsequenciaLesao,
                        UKFuncaoGRIDS = entidade.UKFuncaoGRIDS,
                        UKEspecieAcidImpessoal = entidade.UKEspecieAcidImpessoal,
                        UKTipoAcidPessoal = entidade.UKTipoAcidPessoal,
                        UKAgenteAcidente = entidade.UKAgenteAcidente,
                        UKFonteLesao = entidade.UKFonteLesao,
                        UKFatorPessoalInseg = entidade.UKFatorPessoalInseg,
                        UKAtoInseguro = entidade.UKAtoInseguro,
                        UKCondAmbientalInseg = entidade.UKCondAmbientalInseg,
                        UKPrejMaterial = entidade.UKPrejMaterial,
                        Custo = entidade.Custo,
                        DiasPerdidos = entidade.DiasPerdidos,
                        DiasDebitados = entidade.DiasDebitados,
                        DataObito = entidade.DataObito
                    };

                    CodificacaoBusiness.Inserir(cod);



                    //Atualizar rel com ukcodificacao
                    if (entidade.Tipo.Equals("Proprio"))
                    {
                        RegistroEmpregadoProprio rel = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoProprioBusiness.Terminar(rel);

                            RegistroEmpregadoProprio rel2 = new RegistroEmpregadoProprio();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoProprio = rel.UKEmpregadoProprio;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = cod.UniqueKey;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = rel.UKCAT;

                            rel2.UniqueKey = rel.UniqueKey;

                            RegistroEmpregadoProprioBusiness.Inserir(rel2);
                        }
                    }
                    else
                    {
                        RegistroEmpregadoContratado rel = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoContratadoBusiness.Terminar(rel);

                            RegistroEmpregadoContratado rel2 = new RegistroEmpregadoContratado();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoContratado = rel.UKEmpregadoContratado;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = cod.UniqueKey;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = rel.UKCAT;

                            rel2.UniqueKey = rel.UniqueKey;

                            rel2.UKFornecedor = rel.UKFornecedor;

                            RegistroEmpregadoContratadoBusiness.Inserir(rel2);
                        }
                    }

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Codificação atualizada com sucesso" } });
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




        public ActionResult NovaCAT(string UKRelEnvolvido, string Tipo)
        {

            VMNovaCAT obj = new VMNovaCAT()
            {
                UKRelEnvolvido = UKRelEnvolvido,
                Tipo = Tipo,
                DataAtendimento = DateTime.Now.ToString("dd/MM/yyyy"),
                DataRegistro = DateTime.Now.ToString("dd/MM/yyyy")
            };

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarCAT(VMNovaCAT entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CAT cat = new CAT()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        NumeroCat = entidade.NumeroCat,
                        DataRegistro = entidade.DataRegistro,
                        ETipoRegistrador = entidade.ETipoRegistrador,
                        ETipoCAT = entidade.ETipoCAT,
                        ETipoIniciativa = entidade.ETipoIniciativa,
                        CodigoCNS = entidade.CodigoCNS,
                        DataAtendimento = entidade.DataAtendimento,
                        HoraAtendimento = entidade.HoraAtendimento,
                        Internacao = entidade.Internacao,
                        DuracaoTratamento = entidade.DuracaoTratamento,
                        Afatamento = entidade.Afatamento,
                        Diagnostico = entidade.Diagnostico,
                        Observacao = entidade.Observacao,
                        CID = entidade.CID,
                        NomeMedico = entidade.NomeMedico,
                        EOrgaoClasse = entidade.EOrgaoClasse,
                        NumOrgClasse = entidade.NumOrgClasse,
                        EUnidadeFederacao = entidade.EUnidadeFederacao
                    };

                    CATBusiness.Inserir(cat);



                    //Atualizar rel com ukcodificacao
                    if (entidade.Tipo.Equals("Proprio"))
                    {
                        RegistroEmpregadoProprio rel = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoProprioBusiness.Terminar(rel);

                            RegistroEmpregadoProprio rel2 = new RegistroEmpregadoProprio();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoProprio = rel.UKEmpregadoProprio;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = rel.UKCodificacao;                            
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = cat.UniqueKey;

                            rel2.UniqueKey = rel.UniqueKey;

                            RegistroEmpregadoProprioBusiness.Inserir(rel2);
                        }
                    }
                    else
                    {
                        RegistroEmpregadoContratado rel = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoContratadoBusiness.Terminar(rel);

                            RegistroEmpregadoContratado rel2 = new RegistroEmpregadoContratado();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoContratado = rel.UKEmpregadoContratado;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = rel.UKCodificacao;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = cat.UniqueKey;

                            rel2.UniqueKey = rel.UniqueKey;

                            rel2.UKFornecedor = rel.UKFornecedor;

                            RegistroEmpregadoContratadoBusiness.Inserir(rel2);
                        }
                    }

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "CAT cadastrada com sucesso" } });
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



        public ActionResult EditarCAT(string UKIncidente, string UKRelEnvolvido, string Tipo, string UKCAT)
        {

            try
            {

                if (string.IsNullOrEmpty(UKCAT))
                    throw new Exception("Parâmetro que identifica a CAT não encontrado.");

                if (string.IsNullOrEmpty(UKIncidente))
                    throw new Exception("Não foi possível encontrar a identificação do incidente nos parâmetros.");

                CAT cat = CATBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKCAT));
                if (cat == null)
                    throw new Exception("Não foi possível encontrar a codificação.");

                Incidente objIncidente = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(UKIncidente) && string.IsNullOrEmpty(a.UsuarioExclusao));
                if (objIncidente == null)
                {
                    throw new Exception("Não foi possível encontrar o incidente.");
                }

                if (objIncidente.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login) && !objIncidente.Status.Equals("Em Aprovação"))
                {
                    ViewBag.PodeEditar = true;
                }

                VMNovaCAT obj = new VMNovaCAT()
                {
                    UKRelEnvolvido = UKRelEnvolvido,
                    Tipo = Tipo,
                    UniqueKey = cat.UniqueKey,
                    NumeroCat = cat.NumeroCat,
                    DataRegistro = cat.DataRegistro,
                    ETipoRegistrador = cat.ETipoRegistrador,
                    ETipoCAT = cat.ETipoCAT,
                    ETipoIniciativa = cat.ETipoIniciativa,
                    CodigoCNS = cat.CodigoCNS,
                    DataAtendimento = cat.DataAtendimento,
                    HoraAtendimento = cat.HoraAtendimento,
                    Internacao = cat.Internacao,
                    DuracaoTratamento = cat.DuracaoTratamento,
                    Afatamento = cat.Afatamento,
                    Diagnostico = cat.Diagnostico,
                    Observacao = cat.Observacao,
                    CID = cat.CID,
                    NomeMedico = cat.NomeMedico,
                    EOrgaoClasse = cat.EOrgaoClasse,
                    NumOrgClasse = cat.NumOrgClasse,
                    EUnidadeFederacao = cat.EUnidadeFederacao
                };

                return PartialView(obj);

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
        [ValidateAntiForgeryToken]
        public ActionResult AtualizarCAT(VMNovaCAT entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.UniqueKey))
                        throw new Exception("Parâmetro que identifica a CAT a ser editada não encontrado.");

                    CAT catAntiga = CATBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (catAntiga == null)
                        throw new Exception("Não foi possível encontrar a CAT a ser atualizada.");

                    catAntiga.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    CATBusiness.Terminar(catAntiga);


                    CAT cat = new CAT()
                    {
                        UniqueKey = entidade.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        NumeroCat = entidade.NumeroCat,
                        DataRegistro = entidade.DataRegistro,
                        ETipoRegistrador = entidade.ETipoRegistrador,
                        ETipoCAT = entidade.ETipoCAT,
                        ETipoIniciativa = entidade.ETipoIniciativa,
                        CodigoCNS = entidade.CodigoCNS,
                        DataAtendimento = entidade.DataAtendimento,
                        HoraAtendimento = entidade.HoraAtendimento,
                        Internacao = entidade.Internacao,
                        DuracaoTratamento = entidade.DuracaoTratamento,
                        Afatamento = entidade.Afatamento,
                        Diagnostico = entidade.Diagnostico,
                        Observacao = entidade.Observacao,
                        CID = entidade.CID,
                        NomeMedico = entidade.NomeMedico,
                        EOrgaoClasse = entidade.EOrgaoClasse,
                        NumOrgClasse = entidade.NumOrgClasse,
                        EUnidadeFederacao = entidade.EUnidadeFederacao
                    };

                    CATBusiness.Inserir(cat);



                    //Atualizar rel com ukcodificacao
                    if (entidade.Tipo.Equals("Proprio"))
                    {
                        RegistroEmpregadoProprio rel = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoProprioBusiness.Terminar(rel);

                            RegistroEmpregadoProprio rel2 = new RegistroEmpregadoProprio();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoProprio = rel.UKEmpregadoProprio;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = rel.UKCodificacao;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = cat.UniqueKey;

                            rel2.UniqueKey = rel.UniqueKey;

                            RegistroEmpregadoProprioBusiness.Inserir(rel2);
                        }
                    }
                    else
                    {
                        RegistroEmpregadoContratado rel = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRelEnvolvido));
                        if (rel != null)
                        {
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            RegistroEmpregadoContratadoBusiness.Terminar(rel);

                            RegistroEmpregadoContratado rel2 = new RegistroEmpregadoContratado();

                            rel2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            rel2.DataInclusao = DateTime.Now;

                            rel2.UKRegistro = rel.UKRegistro;
                            rel2.UKEmpregadoContratado = rel.UKEmpregadoContratado;
                            rel2.Funcao = rel.Funcao;
                            rel2.UKCodificacao = rel.UKCodificacao;
                            rel2.UKLesaoDoenca = rel.UKLesaoDoenca;
                            rel2.UKCAT = cat.UniqueKey;

                            rel2.UniqueKey = rel.UniqueKey;

                            rel2.UKFornecedor = rel.UKFornecedor;

                            RegistroEmpregadoContratadoBusiness.Inserir(rel2);
                        }
                    }

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "CAT atualizada com sucesso" } });
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





        public ActionResult PesquisaDadosBase()
        {
            ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaDadosBase(VMPesquisaIncidenteBase entidade)
        {
            try
            {
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.NumeroSmart) &&
                    string.IsNullOrEmpty(entidade.DataIncidente) &&
                    string.IsNullOrEmpty(entidade.HoraIncidente) &&
                    entidade.AcidenteFatal.Equals("Todos") &&
                    entidade.AcidenteGraveIP102.Equals("Todos") &&
                    entidade.ETipoEntrada == 0 &&
                    entidade.ETipoAcidente == 0 &&
                    entidade.Centro == 0 &&
                    entidade.Regional == 0 &&
                    string.IsNullOrEmpty(entidade.LocalAcidente) &&
                    entidade.TipoLocalAcidente == 0 &&
                    string.IsNullOrEmpty(entidade.Logradouro) &&
                    string.IsNullOrEmpty(entidade.NumeroLogradouro) &&
                    string.IsNullOrEmpty(entidade.UKMunicipio) &&
                    string.IsNullOrEmpty(entidade.Estado) &&
                    string.IsNullOrEmpty(entidade.NumeroBoletimOcorrencia) &&
                    string.IsNullOrEmpty(entidade.DataBoletimOcorrencia) &&
                    string.IsNullOrEmpty(entidade.UKESocial) &&
                    string.IsNullOrEmpty(entidade.Descricao) &&
                    string.IsNullOrEmpty(entidade.UKOrgao))
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

                if (entidade.ETipoEntrada != 0)
                    sWhere += " and o.ETipoEntrada = " + ((int)entidade.ETipoEntrada).ToString();

                if (entidade.ETipoAcidente != 0)
                    sWhere += " and o.ETipoAcidente = " + ((int)entidade.ETipoAcidente).ToString();

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

                if (!string.IsNullOrEmpty(entidade.UKMunicipio))
                    sWhere += " and o.UKMunicipio = '" + entidade.UKMunicipio + "'";

                if (!string.IsNullOrEmpty(entidade.Estado))
                    sWhere += " and o.Estado = '" + entidade.Estado + "'";

                if (!string.IsNullOrEmpty(entidade.NumeroBoletimOcorrencia))
                    sWhere += " and o.NumeroBoletimOcorrencia like '" + entidade.NumeroBoletimOcorrencia.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.DataBoletimOcorrencia))
                    sWhere += " and o.DataBoletimOcorrencia = '" + DateTime.ParseExact(entidade.DataBoletimOcorrencia, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'";

                if (!string.IsNullOrEmpty(entidade.UKESocial))
                    sWhere += " and o.UKESocial = '" + entidade.UKESocial + "'";

                if (!string.IsNullOrEmpty(entidade.Descricao))
                    sWhere += " and o.Descricao like '" + entidade.Descricao.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.UKOrgao))
                    sWhere += " and o.UKOrgao = '" + entidade.UKOrgao + "'";



                string sql = @"select top 100 UniqueKey, Codigo, DataIncidente, AcidenteFatal, AcidenteGraveIP102, 
                                          ETipoEntrada, ETipoAcidente, Status,
                                          (select Sigla from OBJDepartamento where UniqueKey = o.UKOrgao and UsuarioExclusao is null) as Orgao,
                                          (select Descricao from OBJMunicipio where UniqueKey = o.UKMunicipio and UsuarioExclusao is null) as Municipio
                           from OBJIncidente o
                           where o.UsuarioExclusao is null " + sWhere + @"
                           order by Codigo";

                List<VMIncidente> lista = new List<VMIncidente>();
                DataTable result = IncidenteBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMIncidente()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            Codigo = row["Codigo"].ToString(),
                            DataIncidente = ((DateTime)row["DataIncidente"]).ToString("dd/MM/yyyy"),
                            TipoEntrada = row["ETipoEntrada"].ToString(),
                            ETipoAcidente = ((ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), row["ETipoAcidente"].ToString(), true)).GetDisplayName(),
                            AcidenteFatal = ((int)row["ETipoAcidente"]).Equals(0) ? "Não" : "Sim",
                            AcidenteGraveIP102 = ((bool)row["AcidenteGraveIP102"]) ? "Sim" : "Não",
                            Status = row["Status"].ToString(),
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



        public ActionResult PesquisaEnvolvidosProprio() {

            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaEnvolvidosProprio(VMPesqIncidenteEnvProprio entidade)
        {
            try
            {
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.Nome) &&
                    string.IsNullOrEmpty(entidade.Matricula) &&
                    string.IsNullOrEmpty(entidade.Funcao) &&
                    entidade.UKLocalizacaoLesaoPrincipal.Equals("Todos") &&
                    entidade.UKLocalizacaoLesaoSecundaria.Equals("Todos") &&
                    entidade.UKNaturezaLesaoPrincipal.Equals("Todos") &&
                    entidade.UKNaturezaLesaoSecundaria.Equals("Todos") &&
                    string.IsNullOrEmpty(entidade.DescricaoLesao))
                    throw new Exception("Informe pelo menos um filtro para prossegui na pesquisa.");

                if (!string.IsNullOrEmpty(entidade.Nome))
                    sWhere += " and upper(ep.Nome) like '" + entidade.Nome.ToUpper().Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.Matricula))
                    sWhere += " and upper(ep.NumeroPessoal) like '" + entidade.Matricula.ToUpper().Replace("*", "%") + "'";

                string sql = @"select top 100 o.UniqueKey, o.Codigo, o.DataIncidente, o.AcidenteFatal, o.AcidenteGraveIP102, 
                                          o.ETipoEntrada, o.ETipoAcidente, o.Status,
                                          (select Sigla from OBJDepartamento where UniqueKey = o.UKOrgao and UsuarioExclusao is null) as Orgao,
                                          (select Descricao from OBJMunicipio where UniqueKey = o.UKMunicipio and UsuarioExclusao is null) as Municipio
                           from OBJIncidente o, RELRegistroEmpregadoProprio re, OBJEmpregadoProprio ep
                           where o.UsuarioExclusao is null and 
	                             o.UniqueKey = re.UKRegistro and re.UKEmpregadoProprio = ep.UniqueKey " + sWhere + @"
                           order by Codigo";

                List<VMIncidente> lista = new List<VMIncidente>();
                DataTable result = IncidenteBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMIncidente()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            Codigo = row["Codigo"].ToString(),
                            DataIncidente = ((DateTime)row["DataIncidente"]).ToString("dd/MM/yyyy"),
                            TipoEntrada = row["ETipoEntrada"].ToString(),
                            ETipoAcidente = ((ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), row["ETipoAcidente"].ToString(), true)).GetDisplayName(),
                            AcidenteFatal = ((int)row["ETipoAcidente"]).Equals(0) ? "Não" : "Sim",
                            AcidenteGraveIP102 = ((bool)row["AcidenteGraveIP102"]) ? "Sim" : "Não",
                            Status = row["Status"].ToString(),
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



        public ActionResult PesquisaEnvolvidosTerceiro()
        {

            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaEnvolvidosTerceiro(VMPesqIncidenteEnvTerceiro entidade)
        {
            try
            {
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.CPF) &&
                    string.IsNullOrEmpty(entidade.Nome) &&
                    string.IsNullOrEmpty(entidade.Nascimento) &&
                    string.IsNullOrEmpty(entidade.Funcao) &&
                    entidade.UKLocalizacaoLesaoPrincipal.Equals("Todos") &&
                    entidade.UKLocalizacaoLesaoSecundaria.Equals("Todos") &&
                    entidade.UKNaturezaLesaoPrincipal.Equals("Todos") &&
                    entidade.UKNaturezaLesaoSecundaria.Equals("Todos") &&
                    string.IsNullOrEmpty(entidade.DescricaoLesao))
                    throw new Exception("Informe pelo menos um filtro para prossegui na pesquisa.");

                if (!string.IsNullOrEmpty(entidade.CPF))
                    sWhere += " and ec.CPF like '" + entidade.CPF.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.Nome))
                    sWhere += " and upper(ec.Nome) like '" + entidade.Nome.ToUpper().Replace("*", "%") + "'";

                string sql = @"select top 100 o.UniqueKey, o.Codigo, o.DataIncidente, o.AcidenteFatal, o.AcidenteGraveIP102, 
                                          o.ETipoEntrada, o.ETipoAcidente, o.Status,
                                          (select Sigla from OBJDepartamento where UniqueKey = o.UKOrgao and UsuarioExclusao is null) as Orgao,
                                          (select Descricao from OBJMunicipio where UniqueKey = o.UKMunicipio and UsuarioExclusao is null) as Municipio
                           from OBJIncidente o, RELRegistroEmpregadoContratado rc, OBJEmpregadoContratado ec
                           where o.UsuarioExclusao is null and 
	                             o.UniqueKey = rc.UKRegistro and rc.UKEmpregadoContratado = ec.UniqueKey  " + sWhere + @"
                           order by o.Codigo";

                List<VMIncidente> lista = new List<VMIncidente>();
                DataTable result = IncidenteBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMIncidente()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            Codigo = row["Codigo"].ToString(),
                            DataIncidente = ((DateTime)row["DataIncidente"]).ToString("dd/MM/yyyy"),
                            TipoEntrada = row["ETipoEntrada"].ToString(),
                            ETipoAcidente = ((ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), row["ETipoAcidente"].ToString(), true)).GetDisplayName(),
                            AcidenteFatal = ((int)row["ETipoAcidente"]).Equals(0) ? "Não" : "Sim",
                            AcidenteGraveIP102 = ((bool)row["AcidenteGraveIP102"]) ? "Sim" : "Não",
                            Status = row["Status"].ToString(),
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



        public ActionResult PesquisaCodificacao()
        {
            ViewBag.TiposAtividades = TipoAtividadeBusiness.ListarTodos();
            ViewBag.Naturezas = NaturezaBusiness.ListarTodos();
            ViewBag.Funcoes = FuncaoGridsBusiness.ListarTodos();
            ViewBag.Especies = EspecieAcidenteImpessoalBusiness.ListarTodos();
            ViewBag.TiposAcidentes = TipoAcidentePessoalBusiness.ListarTodos();
            ViewBag.Agentes = AgenteAcidenteBusiness.ListarTodos();
            ViewBag.Fontes = FonteLesaoBusiness.ListarTodos();
            ViewBag.Fatores = FatorPessoalInsegurancaBusiness.ListarTodos();
            ViewBag.Atos = AtoInseguroBusiness.ListarTodos();
            ViewBag.Condicoes = CondicaoAmbientalInsegBusiness.ListarTodos();
            ViewBag.Prejuizos = PrejuizoMaterialBusiness.ListarTodos();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaCodificacao(VMNovaCodificacao entidade)
        {
            return PartialView();
        }





        public ActionResult PesquisaCAT()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaCAT(VMNovaCAT entidade)
        {
            return PartialView();
        }

    }
}