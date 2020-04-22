using GISCore.Business.Abstract;
using GISModel.DTO.Account;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace GISWeb.Controllers
{
    public class AccountController : BaseController
    {

        #region

            [Inject]
            public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        #endregion

        public ActionResult Login()
        {
            return View();
        }

        [Autorizador]
        [DadosUsuario]
        public ActionResult Perfil()
        {

            Usuario usr = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(AutorizacaoProvider.UsuarioAutenticado.UniqueKey));
            if (usr != null)
            {
                Departamento dp = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(usr.UKDepartamento));
                if (dp != null)
                {
                    ViewBag.Departamento = dp.Sigla + ", " + dp.Descricao;
                }

                Empresa emp = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(usr.UKEmpresa));
                if (emp != null)
                {
                    ViewBag.Empresa = emp.NomeFantasia;
                }

                ViewBag.DataInclusao = usr.DataInclusao.ToString("dd/MM/yyyy HH:mm");

                return View(AutorizacaoProvider.UsuarioAutenticado);
            }

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
            try
            {
                ReiniciarCache(AutorizacaoProvider.UsuarioAutenticado.Login);
            }
            catch { }
            

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



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefinirSenha(NovaSenhaViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                if (entidade.NovaSenha.Equals(entidade.ConfirmarNovaSenha))
                {
                    try
                    {
                        Usuario user = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Login.Equals(AutorizacaoProvider.UsuarioAutenticado.Login));

                        if (user == null)
                            return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível localizar o usuário logado na base de dados. Favor acionar o administrador." } });

                        if (!user.Senha.Equals(UsuarioBusiness.CreateHashFromPassword(entidade.SenhaAtual)))
                            return Json(new { resultado = new RetornoJSON() { Alerta = "A senha atual não confere com a senha da base de dados." } });

                        entidade.IDUsuario = user.ID;

                        UsuarioBusiness.DefinirSenha(entidade);

                        return Json(new { resultado = new RetornoJSON() { Sucesso = "Senha alterada com sucesso." } });
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
                    return Json(new { resultado = new RetornoJSON() { Erro = "As duas senhas devem ser identicas." } });
                }
            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }


    }
}