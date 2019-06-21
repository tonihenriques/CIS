using GISCore.Business.Abstract;
using GISCore.Business.Abstract.Tabelas;
using GISHelpers.Utils;
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
            public IIncidenteBusiness IncidenteBusiness { get; set; }

            [Inject]
            public INaturezaLesaoBusiness NaturezaLesaoBusiness { get; set; }

            [Inject]
            public ILocalizacaoLesaoBusiness LocalizacaoLesaoBusiness { get; set; }

            [Inject]
            public ILesaoDoencaBusiness LesaoDoencaBusiness { get; set; }

            [Inject]
            public IEmpregadoProprioBusiness EmpregadoProprioBusiness { get; set; }

            [Inject]
            public ICodificacaoBusiness LesaoEmpregadoBusiness { get; set; }

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

                    LesaoDoenca lesaodoencaTemp = null;
                    if (!string.IsNullOrEmpty(entidade.DescricaoLesao) ||
                        !string.IsNullOrEmpty(entidade.UKNaturezaLesaoPrincipal) ||
                        !string.IsNullOrEmpty(entidade.UKLocalizacaoLesaoPrincipal) ||
                        !string.IsNullOrEmpty(entidade.UKNaturezaLesaoSecundaria) ||
                        !string.IsNullOrEmpty(entidade.UKLocalizacaoLesaoSecundaria))
                    {
                        lesaodoencaTemp = new LesaoDoenca()
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
                    }

                   

                    RegistroEmpregadoProprioBusiness.Inserir(new RegistroEmpregadoProprio()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        Funcao = entidade.Funcao,
                        UKLesaoDoenca = lesaodoencaTemp?.UniqueKey,
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
            try
            {
                VMProprio obj = new VMProprio();

                RegistroEmpregadoProprio relEmpProprio = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(id));
                if (relEmpProprio == null)
                {
                    throw new Exception("Não foi possível buscar o relacionamento entre o empregado e o incidente.");
                }
                else
                {
                    EmpregadoProprio empProprio = EmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(relEmpProprio.UKEmpregadoProprio));
                    if (empProprio == null)
                    {
                        throw new Exception("Não foi possível localicar o empregado próprio através do relacionamento.");
                    }
                    else
                    {
                        LesaoDoenca lesao = LesaoDoencaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(relEmpProprio.UKLesaoDoenca));
                        if (lesao != null)
                        {
                            obj.UKLocalizacaoLesaoPrincipal = lesao.UKLocalizacaoLesaoPrincipal;
                            obj.UKLocalizacaoLesaoSecundaria = lesao.UKLocalizacaoLesaoSecundaria;
                            obj.UKNaturezaLesaoPrincipal = lesao.UKNaturezaLesaoPrincipal;
                            obj.UKNaturezaLesaoSecundaria = lesao.UKNaturezaLesaoSecundaria;
                            obj.DescricaoLesao = lesao.DescricaoLesao;
                            obj.UKLesaoDoenca = lesao.UniqueKey;
                        }

                        obj.UKRel = relEmpProprio.UniqueKey;
                        obj.UKIncidente = relEmpProprio.UKRegistro;
                        obj.UKEmpregado = relEmpProprio.UKEmpregadoProprio;




                        Incidente objIncidente = IncidenteBusiness.Consulta.FirstOrDefault(a => a.UniqueKey.Equals(obj.UKIncidente) && string.IsNullOrEmpty(a.UsuarioExclusao));
                        if (objIncidente == null)
                        {
                            throw new Exception("Não foi possível encontrar o incidente.");
                        }
                        if (objIncidente.Responsavel.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login) && !objIncidente.Status.Equals("Em Aprovação"))
                        {
                            ViewBag.PodeEditar = true;
                        }




                        obj.Funcao = relEmpProprio.Funcao;
                        
                        obj.Nome = empProprio.Nome;
                        obj.NumeroPessoal = empProprio.NumeroPessoal;

                        ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
                        ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();
                    }
                }

                return PartialView(obj);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(VMProprio entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoProprio empProprio = EmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKEmpregado));
                    if (empProprio == null)
                    {
                        throw new Exception("Não foi possível encontrar o envolvido.");
                    }
                    else
                    {
                        if (!empProprio.Nome.ToUpper().Equals(entidade.Nome.ToUpper().Trim()))
                        {
                            empProprio.DataExclusao = DateTime.Now;
                            empProprio.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            EmpregadoProprioBusiness.Alterar(empProprio);

                            EmpregadoProprio empProprio2 = new EmpregadoProprio();
                            empProprio2.UniqueKey = empProprio.UniqueKey;
                            empProprio2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            empProprio2.NumeroPessoal = empProprio.NumeroPessoal;
                            empProprio2.Nome = entidade.Nome;

                            EmpregadoProprioBusiness.Inserir(empProprio2);
                        }
                    }


                    //##############################################################################################################################################


                    RegistroEmpregadoProprio relEmpProprio = RegistroEmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRel));
                    if (relEmpProprio == null)
                    {
                        throw new Exception("Não foi possível encontrar o vínculo entre o envolvimento e o incidente.");
                    }
                    

                    if (!string.IsNullOrEmpty(entidade.UKLesaoDoenca))
                    {
                        LesaoDoenca lesao = LesaoDoencaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKLesaoDoenca));
                        if (lesao == null)
                        {
                            throw new Exception("Não foi possível encontrar as informações de lesão / doença.");
                        }
                        else
                        {

                            if (!entidade.DescricaoLesao.ToUpper().Trim().Equals(lesao.DescricaoLesao.ToUpper().Trim()) ||
                                !entidade.UKLocalizacaoLesaoPrincipal.Equals(lesao.UKLocalizacaoLesaoPrincipal) ||
                                !entidade.UKLocalizacaoLesaoSecundaria.Equals(lesao.UKLocalizacaoLesaoSecundaria) ||
                                !entidade.UKNaturezaLesaoPrincipal.Equals(lesao.UKNaturezaLesaoPrincipal) ||
                                !entidade.UKNaturezaLesaoSecundaria.Equals(lesao.UKNaturezaLesaoSecundaria))
                            {
                                lesao.DataExclusao = DateTime.Now;
                                lesao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                LesaoDoencaBusiness.Alterar(lesao);

                                LesaoDoenca lesao2 = new LesaoDoenca();
                                lesao2.UniqueKey = lesao.UniqueKey;
                                lesao2.DescricaoLesao = entidade.DescricaoLesao;
                                lesao2.UKLocalizacaoLesaoPrincipal = entidade.UKLocalizacaoLesaoPrincipal;
                                lesao2.UKLocalizacaoLesaoSecundaria = entidade.UKLocalizacaoLesaoSecundaria;
                                lesao2.UKNaturezaLesaoPrincipal = entidade.UKNaturezaLesaoPrincipal;
                                lesao2.UKNaturezaLesaoSecundaria = entidade.UKNaturezaLesaoSecundaria;
                                lesao2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                                LesaoDoencaBusiness.Inserir(lesao2);
                            }

                        }
                    }
                    else
                    {
                        LesaoDoenca lesaodoencaTemp = new LesaoDoenca()
                        {
                            UniqueKey = Guid.NewGuid().ToString(),
                            DescricaoLesao = entidade.DescricaoLesao,
                            UKNaturezaLesaoPrincipal = entidade.UKNaturezaLesaoPrincipal,
                            UKLocalizacaoLesaoPrincipal = entidade.UKLocalizacaoLesaoPrincipal,
                            UKNaturezaLesaoSecundaria = entidade.UKNaturezaLesaoSecundaria,
                            UKLocalizacaoLesaoSecundaria = entidade.UKLocalizacaoLesaoSecundaria,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };

                        LesaoDoencaBusiness.Inserir(lesaodoencaTemp);

                        entidade.UKLesaoDoenca = lesaodoencaTemp.UniqueKey;
                    }


                    if (!relEmpProprio.Funcao.ToUpper().Trim().Equals(entidade.Funcao.ToUpper().Trim()) ||
                        !relEmpProprio.UKLesaoDoenca.Equals(entidade.UKLesaoDoenca)) {

                        relEmpProprio.DataExclusao = DateTime.Now;
                        relEmpProprio.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        RegistroEmpregadoProprioBusiness.Alterar(relEmpProprio);

                        RegistroEmpregadoProprio relEmpProprio2 = new RegistroEmpregadoProprio();
                        relEmpProprio2.UniqueKey = relEmpProprio.UniqueKey;
                        relEmpProprio2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        relEmpProprio2.Funcao = entidade.Funcao;
                        relEmpProprio2.UKLesaoDoenca = entidade.UKLesaoDoenca;
                        relEmpProprio2.UKEmpregadoProprio = entidade.UKEmpregado;
                        relEmpProprio2.UKCodificacao = entidade.UKCodificacao;
                        relEmpProprio2.UKCAT = entidade.UKCAT;
                        relEmpProprio2.UKRegistro = entidade.UKIncidente;

                        RegistroEmpregadoProprioBusiness.Inserir(relEmpProprio2);

                    }



                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Empregado próprio '" + entidade.Nome + "' atualizado com sucesso." } });

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

        [HttpPost]
        public ActionResult BuscarNomePorMatricula(string Matricula)
        {
            try
            {
                if (!string.IsNullOrEmpty(Matricula))
                {
                    EmpregadoProprio emp = EmpregadoProprioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NumeroPessoal.Trim().ToUpper().Equals(Matricula.Trim().ToUpper()));
                    if (emp != null)
                        return Json(new { resultado = new RetornoJSON() { Conteudo = emp.Nome } });
                }


                return Json(new { resultado = new RetornoJSON() { } });
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