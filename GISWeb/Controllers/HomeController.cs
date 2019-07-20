using GISWeb.Infraestrutura.Filters;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard() {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

    }
}