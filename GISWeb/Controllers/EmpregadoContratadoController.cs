using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISCore.Business.Concrete;
using GISModel.DTO.Envolvidos;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.REL;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EmpregadoContratadoController : BaseController
    {

        #region

            //[Inject]
            //public ITipoAtividadeBusiness TipoAtividadeBusiness { get; set; }

            //[Inject]
            //public INaturezaBusiness NaturezaBusiness { get; set; }

            //[Inject]
            //public IFuncaoGridsBusiness FuncaoGridsBusiness { get; set; }

            //[Inject]
            //public IEspecieAcidenteImpessoalBusiness EspecieAcidenteImpessoalBusiness { get; set; }

            //[Inject]
            //public ITipoAcidentePessoalBusiness TipoAcidentePessoalBusiness { get; set; }

            //[Inject]
            //public IAgenteAcidenteBusiness AgenteAcidenteBusiness { get; set; }

            //[Inject]
            //public IFonteLesaoBusiness FonteLesaoBusiness { get; set; }

            //[Inject]
            //public IFatorPessoalInsegurancaBusiness FatorPessoalInsegurancaBusiness { get; set; }

            //[Inject]
            //public IAtoInseguroBusiness AtoInseguroBusiness { get; set; }

            //[Inject]
            //public ICondicaoAmbientalInsegBusiness CondicaoAmbientalInsegBusiness { get; set; }

            //[Inject]
            //public IPrejuizoMaterialBusiness PrejuizoMaterialBusiness { get; set; }


            [Inject]
            public INaturezaLesaoBusiness NaturezaLesaoBusiness { get; set; }

            [Inject]
            public ILocalizacaoLesaoBusiness LocalizacaoLesaoBusiness { get; set; }


            [Inject]
            public ILesaoDoencaBusiness LesaoDoencaBusiness { get; set; }




            [Inject]
            public ILesaoEmpregadoBusiness LesaoEmpregadoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<RegistroEmpregadoContratado> RegistroEmpregadoContratadoBusiness { get; set; }

            [Inject]
            public IEmpregadoContratadoBusiness EmpregadoContratadoBusiness { get; set; }

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {

            ViewBag.TEmpContratado = EmpregadoContratadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }


        public ActionResult BuscarEmpregadoPorID(string EmpregadoID)
        {

            try
            {
                EmpregadoContratado oEmpContratado = EmpregadoContratadoBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(BuscarEmpregadoPorID(EmpregadoID)));
                if (oEmpContratado == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Empregado não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oEmpContratado) });
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
            VMTerceiro obj = new VMTerceiro();
            obj.UKIncidente = id;

            //ViewBag.TiposAtividades = TipoAtividadeBusiness.ListarTodos();
            //ViewBag.Naturezas = NaturezaBusiness.ListarTodos();
            //ViewBag.Funcoes = FuncaoGridsBusiness.ListarTodos();
            //ViewBag.Especies = EspecieAcidenteImpessoalBusiness.ListarTodos();
            //ViewBag.TiposAcidentes = TipoAcidentePessoalBusiness.ListarTodos();
            //ViewBag.Agentes = AgenteAcidenteBusiness.ListarTodos();
            //ViewBag.Fontes = FonteLesaoBusiness.ListarTodos();
            //ViewBag.Fatores = FatorPessoalInsegurancaBusiness.ListarTodos();
            //ViewBag.Atos = AtoInseguroBusiness.ListarTodos();
            //ViewBag.Condicoes = CondicaoAmbientalInsegBusiness.ListarTodos();
            //ViewBag.Prejuizos = PrejuizoMaterialBusiness.ListarTodos();

            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return PartialView(obj);
        }

        public ActionResult Edicao(string id)
        {
            return PartialView(EmpregadoContratadoBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(VMTerceiro entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    EmpregadoContratado empTerceiro = EmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.Equals(entidade.CPF.ToUpper().Trim()));
                    if (empTerceiro == null)
                    {
                        empTerceiro = new EmpregadoContratado()
                        {
                            UniqueKey = Guid.NewGuid().ToString(),
                            Nome = entidade.Nome,
                            CPF = entidade.CPF.ToUpper().Trim(),
                            DataInclusao = DateTime.Now,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            Nascimento = entidade.Nascimento
                        };

                        EmpregadoContratadoBusiness.Inserir(empTerceiro);
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

                    //LesaoEmpregado lesaoEmp = new LesaoEmpregado()
                    //{
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


                    RegistroEmpregadoContratadoBusiness.Inserir(new RegistroEmpregadoContratado()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        Funcao = entidade.Funcao,
                        //UKLesaoEmpregado = lesaoEmp.UniqueKey,
                        UKLesaoDoenca = lesaodoencaTemp.UniqueKey,
                        UKRegistro = entidade.UKIncidente,
                        UKEmpregadoContratado = empTerceiro.UniqueKey,
                        UKFornecedor = entidade.UKFornecedor
                    });


                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Empregado contratado '" + entidade.Nome + "' cadastrado com sucesso." } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(EmpregadoContratado TEmpregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoContratadoBusiness.Alterar(TEmpregado);

                    TempData["MensagemSucesso"] = "Empregado de CPF:'" + TEmpregado.CPF + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EmpContratado") } });
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

                RegistroEmpregadoContratado oEmpregado = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKRel));
                if (oEmpregado == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o envolvido, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oEmpregado.DataExclusao = DateTime.Now;
                    oEmpregado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    RegistroEmpregadoContratadoBusiness.Alterar(oEmpregado);

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
            ViewBag.EmpregadoID = new SelectList(EmpregadoContratadoBusiness.Consulta.ToList());
            return View();
        }


    }
}