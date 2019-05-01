using GISCore.Business.Abstract;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FerramentasController : Controller
    {

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IFerramentasBusiness FerramentasBusiness { get; set; }

        public ActionResult Index()
        {
            List<string> sheets = FerramentasBusiness.BuscarSheetsExcel();

            return View(sheets);
        }

        public ActionResult LoaderAllSheetsFromTabelasAuxiliares()
        {
            List<string[]> result = FerramentasBusiness.CarregarDadosTabelasAuxiliaresFromExcel(CustomAuthorizationProvider.UsuarioAutenticado.Login);

            return PartialView("_LoaderAllSheetsFromTabelasAuxiliares", result);
        }
        
    }
}