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
    public class EstabelecimentoController : BaseController
    {

        #region Inject

            [Inject]
            public IEstabelecimentoBusiness TEstabelecimentoBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.Estabelecimento = TEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }


        public ActionResult Novo()
        {
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");
            //ViewBag.Contratos = new SelectList(ContratoBusiness.Consulta.ToList(), "IDContrato", "Numero");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Estabelecimento TEstabelecimento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TEstabelecimentoBusiness.Inserir(TEstabelecimento);
                    TempData["MensagemSucesso"] = "O Estabelecimento '" + TEstabelecimento.Descricao + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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
            return View(TEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Estabelecimento TEstabelecimento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TEstabelecimentoBusiness.Alterar(TEstabelecimento);

                    TempData["MensagemSucesso"] = "O Estabelecimento '" + TEstabelecimento.Descricao + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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
        
    }
}