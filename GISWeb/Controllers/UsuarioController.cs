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

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class UsuarioController : BaseController
    {

        #region Inject

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        
        #endregion

        public ActionResult Index()
        {
            ViewBag.Usuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        public ActionResult Novo()
        {
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Inserir(Usuario);

                    TempData["MensagemSucesso"] = "O usuário '" + Usuario.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
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

        public ActionResult Edicao(string id)
        {
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            List<Usuario> usuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(id)).ToList();

            if (usuarios.Count == 1)
            {
                Usuario oUsuario = usuarios[0];

                if (oUsuario.UKEmpresa != null && !string.IsNullOrEmpty(oUsuario.UKEmpresa))
                {
                    ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(oUsuario.UKEmpresa)).ToList();
                }
                
                return View(oUsuario);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Usuario entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Alterar(entidade);

                    TempData["MensagemSucesso"] = "O usuário '" + entidade.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
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
        public ActionResult BuscarUsuarioPorParteLoginOuNome(string filtro)
        {
            try
            {
                List<Usuario> usuarios = UsuarioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && (a.Login.ToUpper().Contains(filtro.ToUpper()) || a.Nome.ToUpper().Contains(filtro.ToUpper()))).ToList();
                
                return Json(new { resultado = usuarios });
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