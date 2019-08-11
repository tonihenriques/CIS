using GISCore.Business.Abstract;
using GISHelpers.Extensions.System;
using GISModel.DTO.Shared;
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
        public IIncidenteBusiness IncidenteBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMeusIncidentesBar() {
            try
            {
                List<string[]> model = new List<string[]>();

                string sqlTotalIncidentesPorMes = @"select 'Pessoa' as Tipo, concat(month(DataInclusao), '/', year(DataInclusao)) as  mesref, COUNT(*) as total
                                                from OBJIncidente
                                                where UsuarioInclusao = '" + CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper() + @"' and StatusWF in ('RS', 'SO') and Status = 'Em Edição'
                                                group by concat(month(DataInclusao), '/', year(DataInclusao))

                                                union

                                                select 'Veiculo' as Tipo, concat(month(DataInclusao), '/', year(DataInclusao)) as mesref, COUNT(*) as total
                                                from OBJIncidenteVeiculo
                                                where UsuarioInclusao = '" + CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper() + @"' and StatusWF in ('RS', 'SO') and Status = 'Em Edição'
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


        public ActionResult LoadMeusIncidentesPorTipoAcidente()
        {

            try
            {

                List<string[]> model = new List<string[]>();

                string sqlTotalIncidentesPorMes = @"select 'Pessoa' as tipo, ETipoAcidente, COUNT(*) as total from OBJIncidente where Status = 'Em Edição' and StatusWF in ('RS', 'SO') and UsuarioInclusao = '" + CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper() + @"'
	                                                group by ETipoAcidente
	                                                union all
	                                                select 'Veiculo' as tipo, ETipoAcidente, count(*) as total from OBJIncidenteVeiculo where Status = 'Em Edição' and StatusWF in ('RS', 'SO') and UsuarioInclusao = '" + CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper() + @"'
	                                                group by ETipoAcidente";
                DataTable result = IncidenteBusiness.GetDataTable(sqlTotalIncidentesPorMes);
                if (result.Rows.Count > 0)
                {
                    string chartData = "['Task', 'Hours per Day'],";

                    foreach (DataRow row in result.Rows)
                    {
                        string def = row["tipo"].ToString();
                        string tipo = row["ETipoAcidente"].ToString();
                        string total = row["total"].ToString();

                        string sTipo = string.Empty;
                        if (def == "Pessoa")
                        {
                            ETipoAcidente eTipo = (ETipoAcidente)Enum.Parse(typeof(ETipoAcidente), tipo, true);
                            sTipo = "Pessoa - " + EnumExtensions.GetDisplayName(eTipo);
                        }
                        else
                        {
                            ETipoAcidenteVeiculo eTipo = (ETipoAcidenteVeiculo)Enum.Parse(typeof(ETipoAcidenteVeiculo), tipo, true);
                            sTipo = "Veículo - " + EnumExtensions.GetDisplayName(eTipo);
                        }

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

        public ActionResult Sobre()
        {
            return View();
        }

    }
}