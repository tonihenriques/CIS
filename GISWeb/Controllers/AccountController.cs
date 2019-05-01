using GISCore.Business.Abstract;
using GISModel.DTO.Account;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.UI;

namespace GISWeb.Controllers
{
    public class AccountController : Controller
    {

        #region

            [Inject]
            public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

        #endregion

        public ActionResult Login()
        {
            return View();
        }

        [Autorizador]
        [DadosUsuario]
        public ActionResult Perfil()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AutenticacaoModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string msgErro = string.Empty;
                    AutorizacaoProvider.LogIn(usuario, out msgErro);
                    return Json(new { url = Url.Action(ConfigurationManager.AppSettings["Web:DefaultAction"], ConfigurationManager.AppSettings["Web:DefaultController"]) });
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
            }
        }
        
        public ActionResult Logout()
        {
            AutorizacaoProvider.LogOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [OutputCache(Duration = 604800, Location = OutputCacheLocation.Client, VaryByParam = "login")]
        public ActionResult FotoPerfil(string login)
        {
            byte[] avatar = null;

            try
            {
                avatar = UsuarioBusiness.RecuperarAvatar(login);
            }
            catch { }

            if (avatar == null || avatar.Length == 0)
                avatar = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Ace/avatars/unknown.png"));

            return File(avatar, "image/jpeg");
        }

        [HttpPost]
        [Autorizador]
        public ActionResult AtualizarFoto(string imagemStringBase64)
        {
            UsuarioBusiness.SalvarAvatar(AutorizacaoProvider.UsuarioAutenticado.Login, imagemStringBase64);

            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
                //Severino.GravaCookie("MensagemErro", ex.Message, 2);
            //}
            return Json(new { url = Url.Action("Perfil") });
        }

    }
}