using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISModel.DTO.Envolvidos;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.REL;
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
    public class EmpregadoProprioController : BaseController
    {

        #region

            [Inject]
            public INaturezaLesaoBusiness NaturezaLesaoBusiness { get; set; }

            [Inject]
            public ILocalizacaoLesaoBusiness LocalizacaoLesaoBusiness { get; set; }

            [Inject]
            public ILesaoDoencaBusiness LesaoDoencaBusiness { get; set; }

            [Inject]
            public IEmpregadoProprioBusiness EmpregadoProprioBusiness { get; set; }

            [Inject]
            public ILesaoEmpregadoBusiness LesaoEmpregadoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<RegistroEmpregadoProprio> RegistroEmpregadoProprioBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.TEmpProprio = EmpregadoProprioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult BuscarEmpregadoPorID(string EmpregadoID)
        {

            try
            {
                EmpregadoProprio oEmpProprio = EmpregadoProprioBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(BuscarEmpregadoPorID(EmpregadoID)));
                if (oEmpProprio == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Empregado não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oEmpProprio) });
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
            VMProprio obj = new VMProprio();
            obj.UKIncidente = id;

            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(VMProprio entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    EmpregadoProprio empProprio = EmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NumeroPessoal.Equals(entidade.NumeroPessoal.ToUpper().Trim()));
                    if (empProprio == null)
                    {
                        empProprio = new EmpregadoProprio()
                        {
                            UniqueKey = Guid.NewGuid().ToString(),
                            Nome = entidade.Nome,
                            NumeroPessoal = entidade.NumeroPessoal.ToUpper().Trim(),
                            DataInclusao = DateTime.Now,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };

                        EmpregadoProprioBusiness.Inserir(empProprio);
                    }

                    LesaoDoenca lesaodoencaTemp = new LesaoDoenca()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        DescricaoLesao = entidade.DescricaoLesao,
                        UKNaturezaLesaoPrincipal = entidade.UKNaturezaLesaoPrincipal,
                        UKLocalizacaoLesaoPrincipal = entidade.UKLocalizacaoLesaoPrincipal,
                        UKNaturezaLesaoSecundaria = entidade.UKNaturezaLesaoSecundaria,
                        UKLocalizacaoLesaoSecundaria = entidade.UKLocalizacaoLesaoSecundaria,
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                    };

                    LesaoDoencaBusiness.Inserir(lesaodoencaTemp);

                    //LesaoEmpregado lesaoEmp = new LesaoEmpregado() {
                    //    UniqueKey = Guid.NewGuid().ToString(),
                    //    TipoAcidente = entidade.TipoAcidente,
                    //    Atividade = entidade.Atividade,
                    //    UKTipoAtividade = entidade.UKTipoAtividade,
                    //    UKNatureza = entidade.UKNatureza,
                    //    ConsequenciaLesao = entidade.ConsequenciaLesao,
                    //    UKFuncaoGRIDS = entidade.UKFuncaoGRIDS,
                    //    UKEspecieAcidImpessoal = entidade.UKEspecieAcidImpessoal,
                    //    UKTipoAcidPessoal = entidade.UKTipoAcidPessoal,
                    //    UKAgenteAcidente = entidade.UKAgenteAcidente,
                    //    UKFonteLesao = entidade.UKFonteLesao,
                    //    UKFatorPessoalInseg = entidade.UKFatorPessoalInseg,
                    //    UKAtoInseguro = entidade.UKAtoInseguro,
                    //    UKCondAmbientalInseg = entidade.UKCondAmbientalInseg,
                    //    UKPrejMaterial = entidade.UKPrejMaterial,
                    //    Custo = entidade.Custo,
                    //    DiasPerdidos = entidade.DiasPerdidos,
                    //    DiasDebitados = entidade.DiasDebitados,
                    //    DataObito = entidade.DataObito,
                    //    DataInclusao = DateTime.Now,
                    //    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                    //};

                    //LesaoEmpregadoBusiness.Inserir(lesaoEmp);

                    RegistroEmpregadoProprioBusiness.Inserir(new RegistroEmpregadoProprio()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        Funcao = entidade.Funcao,
                        //UKLesaoEmpregado = lesaoEmp.UniqueKey,
                        UKLesaoDoenca = lesaodoencaTemp.UniqueKey,
                        UKRegistro = entidade.UKIncidente,
                        UKEmpregadoProprio = empProprio.UniqueKey
                    });

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Empregado próprio '" + entidade.Nome + "' cadastrado com sucesso." } });
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
            VMProprio obj = new VMProprio();

            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(EmpregadoProprio TEmpregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoProprioBusiness.Alterar(TEmpregado);

                    TempData["MensagemSucesso"] = "Empregado de Matrícula:'" + TEmpregado.NumeroPessoal + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EmpProprio") } });
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
        public ActionResult Excluir(string UKRel, string Nome)
        {

            try
            {
                if (string.IsNullOrEmpty(UKRel))
                    throw new Exception("Identificação do envolvido não localizado.");

                RegistroEmpregadoProprio oEmpregado = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKRel));
                if (oEmpregado == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o envolvido, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oEmpregado.DataExclusao = DateTime.Now;
                    oEmpregado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    RegistroEmpregadoProprioBusiness.Alterar(oEmpregado);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O envolvido '" + Nome + "' foi excluído com sucesso." } });
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
            ViewBag.EmpregadoID = new SelectList(EmpregadoProprioBusiness.Consulta.ToList());
            return View();
        }
       
    }
}