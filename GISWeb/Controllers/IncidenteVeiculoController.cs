using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IncidenteVeiculoController : BaseController
    {
        [Inject]
        public IESocialBusiness ESocialBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Municipio> MunicipioBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        public ActionResult Novo()
        {
            ViewBag.ESocial = ESocialBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.Municipios = MunicipioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Descricao);

            return View();
        }


    }
}