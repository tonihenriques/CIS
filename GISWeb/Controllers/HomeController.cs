using GISCore.Business.Abstract;
using GISHelpers.Extensions.System;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : BaseController
    {

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        [Inject]
        public IIncidenteBusiness IncidenteBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadIncidentesBar() {
            try
            {
                List<string[]> model = new List<string[]>();

                string sqlTotalIncidentesPorMes = @"select 'Pessoa' as Tipo, concat(month(DataInclusao), '/', year(DataInclusao)) as  mesref, COUNT(*) as total
                                                from OBJIncidente
                                                where UsuarioExclusao is null 
                                                group by concat(month(DataInclusao), '/', year(DataInclusao))

                                                union

                                                select 'Veiculo' as Tipo, concat(month(DataInclusao), '/', year(DataInclusao)) as mesref, COUNT(*) as total
                                                from OBJIncidenteVeiculo
                                                where UsuarioExclusao is null
                                                group by concat(month(DataInclusao), '/', year(DataInclusao))
                                                order by Tipo";

                DataTable result = IncidenteBusiness.GetDataTable(sqlTotalIncidentesPorMes);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        string tipo = row["Tipo"].ToString();
                        string mesref = row["mesref"].ToString();
                        string total = row["total"].ToString();

                        string[] item = model.FirstOrDefault(a => a[0].Equals(mesref));

                        if (item == null)
                        {
                            if (tipo == "Pessoa")
                            {
                                model.Add(new string[] { mesref, total, "0" });
                            }
                            else
                            {
                                model.Add(new string[] { mesref, "0", total, "0" });
                            }
                        }
                        else
                        {
                            item[2] = total;
                        }
                    }


                    string chartData = "['Mês', 'Pessoa', { role: 'annotation' }, 'Veículo', { role: 'annotation' }],";
                    foreach (string[] item in model)
                    {
                        string NomeMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(item[0].Substring(0, item[0].IndexOf("/"))));
                        chartData += "['" + char.ToUpper(NomeMes[0]) + NomeMes.Substring(1) + "', " + item[1] + ", '" + item[1] + "', " + item[2] + ", '" + item[2] + "'],";
                    }

                    if (chartData.EndsWith(","))
                        chartData = chartData.Substring(0, chartData.Length - 1);

                    //Padrão
                    //    ['Janeiro', 9, '9', 6, '6'],
                    //    ['Fevereiro', 10, '10', 2, '2'],
                    //    ['Março', 11, '11', 4, '4'],
                    //    ['Abril', 12, '12', 4, '4'],
                    //    ['Maio', 15, '15', 5, '5'],
                    //    ['Junho', 12, '12', 4, '4']

                    return Json(new { resultado = new RetornoJSON() { Conteudo = chartData } });

                }

                return Json(new { resultado = new RetornoJSON() { Conteudo = "" } });
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

        public ActionResult LoadIncidentesPessoasPorTipoAcidente()
        {
            try
            {
                List<string[]> model = new List<string[]>();

                string sqlTotalIncidentesPorMes = @"select ETipoAcidente, COUNT(*) as total from OBJIncidente 
                                                    where UsuarioExclusao is null
	                                                group by ETipoAcidente";

                DataTable result = IncidenteBusiness.GetDataTable(sqlTotalIncidentesPorMes);
                if (result.Rows.Count > 0)
                {
                    string chartData = "['Task', 'Hours per Day'],";

                    foreach (DataRow row in result.Rows)
                    {
                        string tipo = row["ETipoAcidente"].ToString();
                        string total = row["total"].ToString();

                        ETipoAcidente eTipo = (ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), tipo, true);
                        string sTipo = EnumExtensions.GetDisplayName(eTipo);

                        chartData += "['" + sTipo + "', " + total + "],";
                    }

                    if (chartData.EndsWith(","))
                        chartData = chartData.Substring(0, chartData.Length - 1);

                    //Padrão
                    //['Task', 'Hours per Day'],
                    //['Contratado', 11],
                    //['Empregado', 2],
                    //['Novos Negócio', 2],
                    //['Obra PART', 2],
                    //['Doença Ocupacional', 7]

                    return Json(new { resultado = new RetornoJSON() { Conteudo = chartData } });
                }

                return Json(new { resultado = new RetornoJSON() { Conteudo = "" } });
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

        public ActionResult LoadIncidentesVeiculosPorTipoAcidente()
        {

            try
            {

                List<string[]> model = new List<string[]>();

                string sqlTotalIncidentesPorMes = @"select ETipoAcidente, count(*) as total from OBJIncidenteVeiculo 
                                                    where UsuarioExclusao is null
	                                                group by ETipoAcidente";

                DataTable result = IncidenteBusiness.GetDataTable(sqlTotalIncidentesPorMes);
                if (result.Rows.Count > 0)
                {
                    string chartData = "['Task', 'Hours per Day'],";

                    foreach (DataRow row in result.Rows)
                    {
                        string tipo = row["ETipoAcidente"].ToString();
                        string total = row["total"].ToString();

                        ETipoAcidenteVeiculo eTipo = (ETipoAcidenteVeiculo)Enum.Parse(typeof(ETipoAcidenteVeiculo), tipo, true);
                        string sTipo = EnumExtensions.GetDisplayName(eTipo);

                        chartData += "['" + sTipo + "', " + total + "],";
                    }

                    if (chartData.EndsWith(","))
                        chartData = chartData.Substring(0, chartData.Length - 1);

                    //Padrão
                    //['Task', 'Hours per Day'],
                    //['Contratado', 11],
                    //['Empregado', 2],
                    //['Novos Negócio', 2],
                    //['Obra PART', 2],
                    //['Doença Ocupacional', 7]

                    return Json(new { resultado = new RetornoJSON() { Conteudo = chartData } });
                }

                return Json(new { resultado = new RetornoJSON() { Conteudo = "" } });
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


        public ActionResult Dashboard() {
            return View();
        }

        public ActionResult Suporte() {

            var lUsuariosPerfis = (from perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.Nome.Equals("Administrador") || p.Nome.Equals("Medico"))).ToList()
                          join usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on perfil.UniqueKey equals usuarioperfil.UKPerfil
                          join usuario in UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKUsuario equals usuario.UniqueKey
                          select new UsuarioPerfil { Perfil = new Perfil() { Nome = perfil.Nome },
                                                     Usuario = new Usuario() { Login = usuario.Login, Nome = usuario.Nome }
                                                   }).ToList();

            return View(lUsuariosPerfis);
        }

        public ActionResult Sobre()
        {
            return View();
        }

    }
}