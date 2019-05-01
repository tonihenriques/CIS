using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CatController : BaseController
    {
        
        #region

            [Inject]
            public ICatBusiness CatBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {

            ViewBag.TCat = CatBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }


        public ActionResult BuscarCatPorID(string CatID)
        {

            try
            {
                CAT oCat = CatBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(BuscarCatPorID(CatID)));
                if (oCat == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Cat não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oCat) });
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

        public ActionResult Novo(string id)
        {


            ViewBag.RegistroID = id;

            return View();
        }

        public ActionResult Edicao(string id)
        {
            return View(CatBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(CAT Cat)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    CatBusiness.Inserir(Cat);

                    TempData["MensagemSucessoCat"] = "A codificação da Cat foi cadastrada com sucesso.";

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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(CAT TCat)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CatBusiness.Alterar(TCat);

                    TempData["MensagemSucessoCatAtulizado"] = "A CAT foi atualizado com sucesso.";

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
        public ActionResult Terminar(string IdCat)
        {

            try
            {
                CAT oCat = CatBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IdCat));
                if (oCat == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a CAT, pois a mesma não foi localizado." } });
                }
                else
                {

                    oCat.DataExclusao = DateTime.Now;
                    oCat.UsuarioExclusao = "LoginTeste";
                    CatBusiness.Alterar(oCat);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Esta Cat foi excluído com sucesso." } });
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
        public ActionResult TerminarComRedirect(string IdCat)
        {

            try
            {
                CAT oCat = CatBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IdCat));
                if (oCat == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta codificação, pois a mesma não foi localizada." } });
                }
                else
                {
                    oCat.DataExclusao = DateTime.Now;
                    oCat.UsuarioExclusao = "LoginTeste";

                    CatBusiness.Alterar(oCat);

                    TempData["MensagemSucessoTerminar"] = "A codificação foi foi excluída com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EmpProprio") } });
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

        public ActionResult listar()
        {
            ViewBag.CatID = new SelectList(CatBusiness.Consulta.ToList());
            return View();
        }
        //[RestritoAAjax]
        //public ActionResult _Upload()
        //{
        //    try
        //    {
        //        return PartialView("_Upload");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 500;
        //        return Content(ex.Message, "text/html");
        //    }
        //}

        //[HttpPost]
        //[RestritoAAjax]
        //[ValidateAntiForgeryToken]
        //public ActionResult Upload()
        //{
        //    try
        //    {
        //        string fName = string.Empty;
        //        string msgErro = string.Empty;
        //        foreach (string fileName in Request.Files.AllKeys)
        //        {
        //            HttpPostedFileBase oFile = Request.Files[fileName];
        //            fName = oFile.FileName;
        //            if (oFile != null)
        //{
        //    string sExtensao = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
        //    if (sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("GIF"))
        //    {
        //        //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
        //        string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
        //        sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
        //        sLocalFile = Path.Combine(sLocalFile, "Empresa");
        //        sLocalFile = Path.Combine(sLocalFile, "LoginTeste");

        //        if (!System.IO.Directory.Exists(sLocalFile))
        //            Directory.CreateDirectory(sLocalFile);
        //        else
        //        {
        //            //Tratamento de limpar arquivos da pasta, pois o usuário pode estar apenas alterando o arquivo.
        //            //Limpar para não ficar lixo.
        //            //O arquivo que for salvo abaixo será limpado após o cadastro.
        //            //Se o usuário cancelar o cadastro, a rotina de limpar diretórios ficará responsável por limpá-lo.
        //            foreach (string iFile in System.IO.Directory.GetFiles(sLocalFile))
        //            {
        //                System.IO.File.Delete(iFile);
        //            }
        //        }

        //    sLocalFile = Path.Combine(sLocalFile, oFile.FileName);

        //    oFile.SaveAs(sLocalFile);

        //}
        //        else
        //        {
        //            throw new Exception("Extensão do arquivo não permitida.");
        //        }

        //    }
        //}
        //        if (string.IsNullOrEmpty(msgErro))
        //            return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", arquivo = fName, erro = msgErro });
        //        else
        //            return Json(new { erro = msgErro });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { erro = ex.Message });
        //    }
        //}
        
    }
}