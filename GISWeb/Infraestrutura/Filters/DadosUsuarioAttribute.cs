using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Filters
{
    public class DadosUsuarioAttribute : ActionFilterAttribute
    {
        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            if (AutorizacaoProvider.UsuarioAutenticado != null)
            {
                filterContext.Controller.ViewBag.NomeUsuario = AutorizacaoProvider.UsuarioAutenticado.Nome;
                filterContext.Controller.ViewBag.MatriculaUsuario = AutorizacaoProvider.UsuarioAutenticado.Login;
                filterContext.Controller.ViewBag.Permissoes = AutorizacaoProvider.UsuarioAutenticado.Permissoes;
            }
        }

    }
}