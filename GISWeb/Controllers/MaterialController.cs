using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades.OBJ;
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
    public class MaterialController : BaseController
    {

        #region Inject 

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IBaseBusiness<Material> MaterialBusiness { get; set; }

        #endregion

        public ActionResult Novo(string UKIncidenteVeiculo)
        {
            Material obj = new Material();
            obj.UKIncidente = UKIncidenteVeiculo;

            return PartialView("_Novo", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Material entidade)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    MaterialBusiness.Inserir(entidade);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Material cadastrado com sucesso" } });
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



        [RestritoAAjax]
        [HttpPost]
        public ActionResult Excluir(string UK)
        {
            try
            {
                if (string.IsNullOrEmpty(UK))
                {
                    throw new Exception("Não foi possível localizar a identificação do material a ser excluído.");
                }
                else
                {

                    Material obj = MaterialBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UK));
                    if (obj == null)
                    {
                        throw new Exception("Não foi possível localizar o material a ser excluído.");
                    }
                    else
                    {
                        obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        MaterialBusiness.Terminar(obj);
                        return Json(new { resultado = new RetornoJSON() { Sucesso = "Material excluído com sucesso." } });
                    }
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

    }
}