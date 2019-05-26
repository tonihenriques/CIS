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

            [Inject]
            public INaturezaLesaoBusiness NaturezaLesaoBusiness { get; set; }

            [Inject]
            public ILocalizacaoLesaoBusiness LocalizacaoLesaoBusiness { get; set; }


            [Inject]
            public ILesaoDoencaBusiness LesaoDoencaBusiness { get; set; }




            [Inject]
            public ICodificacaoBusiness LesaoEmpregadoBusiness { get; set; }

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

            

            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.NaturezaLesao = NaturezaLesaoBusiness.ListarTodos();
            ViewBag.LocalizacaoLesao = LocalizacaoLesaoBusiness.ListarTodos();

            return PartialView(obj);
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

                        

                    RegistroEmpregadoContratadoBusiness.Inserir(new RegistroEmpregadoContratado()
                    {
                        UniqueKey = Guid.NewGuid().ToString(),
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        Funcao = entidade.Funcao,
                        UKLesaoDoenca = lesaodoencaTemp?.UniqueKey,
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



        public ActionResult Edicao(string id)
        {
            try
            {
                VMTerceiro obj = new VMTerceiro();

                RegistroEmpregadoContratado relEmpTerceiro = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(id));
                if (relEmpTerceiro == null)
                {
                    throw new Exception("Não foi possível buscar o relacionamento entre o empregado e o incidente.");
                }
                else
                {
                    EmpregadoContratado empTerceiro = EmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(relEmpTerceiro.UKEmpregadoContratado));
                    if (empTerceiro == null)
                    {
                        throw new Exception("Não foi possível localicar o empregado próprio através do relacionamento.");
                    }
                    else
                    {
                        LesaoDoenca lesao = LesaoDoencaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(relEmpTerceiro.UKLesaoDoenca));
                        if (lesao != null)
                        {
                            obj.UKLocalizacaoLesaoPrincipal = lesao.UKLocalizacaoLesaoPrincipal;
                            obj.UKLocalizacaoLesaoSecundaria = lesao.UKLocalizacaoLesaoSecundaria;
                            obj.UKNaturezaLesaoPrincipal = lesao.UKNaturezaLesaoPrincipal;
                            obj.UKNaturezaLesaoSecundaria = lesao.UKNaturezaLesaoSecundaria;
                            obj.DescricaoLesao = lesao.DescricaoLesao;
                            obj.UKLesaoDoenca = lesao.UniqueKey;
                        }

                        obj.UKRel = relEmpTerceiro.UniqueKey;
                        obj.UKIncidente = relEmpTerceiro.UKRegistro;
                        obj.UKEmpregado = relEmpTerceiro.UKEmpregadoContratado;
                        obj.UKFornecedor = relEmpTerceiro.UKFornecedor;

                        obj.Funcao = relEmpTerceiro.Funcao;

                        obj.Nome = empTerceiro.Nome;
                        obj.CPF = empTerceiro.CPF;
                        obj.Nascimento = empTerceiro.Nascimento;

                        ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
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
        public ActionResult Atualizar(VMTerceiro entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoContratado empTerceiro = EmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKEmpregado));
                    if (empTerceiro == null)
                    {
                        throw new Exception("Não foi possível encontrar o envolvido.");
                    }
                    else
                    {
                        if (!empTerceiro.Nome.ToUpper().Equals(entidade.Nome.ToUpper().Trim()) ||
                            !empTerceiro.Nascimento.Equals(entidade.Nascimento))
                        {
                            empTerceiro.DataExclusao = DateTime.Now;
                            empTerceiro.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            EmpregadoContratadoBusiness.Alterar(empTerceiro);

                            EmpregadoContratado empTerceiro2 = new EmpregadoContratado();
                            empTerceiro2.UniqueKey = empTerceiro.UniqueKey;
                            empTerceiro2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            empTerceiro2.CPF = empTerceiro.CPF;
                            empTerceiro2.Nome = entidade.Nome;
                            empTerceiro2.Nascimento = entidade.Nascimento;

                            EmpregadoContratadoBusiness.Inserir(empTerceiro2);
                        }
                    }


                    //##############################################################################################################################################


                    RegistroEmpregadoContratado relEmpTerceiro = RegistroEmpregadoContratadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKRel));
                    if (relEmpTerceiro == null)
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


                    if (!relEmpTerceiro.Funcao.ToUpper().Trim().Equals(entidade.Funcao.ToUpper().Trim()) ||
                        !relEmpTerceiro.UKLesaoDoenca.Equals(entidade.UKLesaoDoenca) ||
                        !relEmpTerceiro.UKFornecedor.Equals(entidade.UKFornecedor))
                    {

                        relEmpTerceiro.DataExclusao = DateTime.Now;
                        relEmpTerceiro.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        RegistroEmpregadoContratadoBusiness.Alterar(relEmpTerceiro);

                        RegistroEmpregadoContratado relEmpTerceiro2 = new RegistroEmpregadoContratado();
                        relEmpTerceiro2.UniqueKey = relEmpTerceiro.UniqueKey;
                        relEmpTerceiro2.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        relEmpTerceiro2.Funcao = entidade.Funcao;
                        relEmpTerceiro2.UKLesaoDoenca = entidade.UKLesaoDoenca;
                        relEmpTerceiro2.UKEmpregadoContratado = entidade.UKEmpregado;
                        relEmpTerceiro2.UKCodificacao = entidade.UKCodificacao;
                        relEmpTerceiro2.UKCAT = entidade.UKCAT;
                        relEmpTerceiro2.UKRegistro = entidade.UKIncidente;
                        relEmpTerceiro2.UKFornecedor = entidade.UKFornecedor;

                        RegistroEmpregadoContratadoBusiness.Inserir(relEmpTerceiro2);

                    }

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Empregado terceiro '" + entidade.Nome + "' atualizado com sucesso." } });

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