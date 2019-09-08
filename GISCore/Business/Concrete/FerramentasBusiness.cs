using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.OBJ.Tabelas;
using Ninject;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class FerramentasBusiness : BaseBusiness<EntidadeBase>, IFerramentasBusiness
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Municipio> MunicipioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<EspecieAcidenteImpessoal> EspecieAcidenteImpessoalBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Natureza> NaturezaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FuncaoGrids> FuncaoGridsBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoAcidentePessoal> TipoAcidentePessoalBusiness { get; set; }

        [Inject]
        public IBaseBusiness<AgenteAcidente> AgenteAcidenteBusiness { get; set; }

        [Inject]
        public IBaseBusiness<CondicaoAmbientalInseg> CondicaoAmbientalInsegBusiness { get; set; }

        [Inject]
        public IBaseBusiness<PrejuizoMaterial> PrejuizoMaterialBusiness { get; set; }

        [Inject]
        public IBaseBusiness<AtoInseguro> AtoInseguroBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FatorPessoalInseguranca> FatorPessoalInsegurancaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<NaturezaLesao> NaturezaLesaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<LocalizacaoLesao> LocalizacaoLesaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<ESocial> ESocialBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteLesao> FonteLesaoBusiness { get; set; }

        #endregion

        public List<string> BuscarSheetsExcel()
        {
            List<string> lista = new List<string>();

            try
            {
                FileInfo fileExcel = new FileInfo(Path.Combine(ConfigurationManager.AppSettings["DiretorioRaiz"], "TabelasAuxiliares.xlsx"));
                using (ExcelPackage xlPackage = new ExcelPackage(fileExcel))
                {
                    foreach (ExcelWorksheet ws in xlPackage.Workbook.Worksheets)
                    {
                        if (!ws.Name.Equals("SESMT 1050") && !ws.Name.Equals("Senha"))
                        {
                            lista.Add(ws.Name);
                        }
                    }
                }
            }
            catch { }

            return lista;
        }

        public List<string[]> CarregarDadosTabelasAuxiliaresFromExcel(string UsuarioLogado)
        {

            List<string[]> result = new List<string[]>();

            FileInfo fileExcel = new FileInfo(Path.Combine(ConfigurationManager.AppSettings["DiretorioRaiz"], "TabelasAuxiliares.xlsx"));
            using (ExcelPackage xlPackage = new ExcelPackage(fileExcel))
            {
                foreach (ExcelWorksheet ws in xlPackage.Workbook.Worksheets)
                {
                    if (ws.Name.Equals("CodMunicipio"))
                    {
                        result.Add(LoaderMunicipio(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("Natureza"))
                    {
                        result.Add(LoaderNatureza(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("FunçaoGrids"))
                    {
                        result.Add(LoaderFuncaoGrid(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("EspecieAcidenteImpessoal"))
                    {
                        result.Add(LoaderEspecieAcidenteImpessoal(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("TipoAcidentePessoal"))
                    {
                        result.Add(LoaderTipoAcidentePessoal(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("AgenteAcidente"))
                    {
                        result.Add(LoaderAgenteAcidente(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("CondicaoAmbientalInseg"))
                    {
                        result.Add(LoaderCondicaoAmbientalInseg(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("PrejuizoMaterial"))
                    {
                        result.Add(LoaderPrejuizoMaterial(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("AtoInseguro"))
                    {
                        result.Add(LoaderAtoInseguro(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("FatorPessoalInseguranca"))
                    {
                        result.Add(LoaderFatorPessoalInseguranca(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("NaturezaDaLesaoPrincipal"))
                    {
                        result.Add(LoaderNaturezaLesao(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("LocalizaoDaLesaoPrincipal"))
                    {
                        result.Add(LoaderLocalizacaoLesao(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("FonteDaLesao"))
                    {
                        result.Add(LoaderFonteLesao(ws, UsuarioLogado));
                    }
                    else if (ws.Name.Equals("esDescrição"))
                    {
                        result.Add(LoaderESocial(ws, UsuarioLogado));
                    }
                }
            }

            return result;

        }

        private string[] LoaderMunicipio(ExcelWorksheet ws, string UsuarioLogado)
        {

            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (MunicipioBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {

                                MunicipioBusiness.Inserir(new Municipio()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;

                            }
                        }
                        catch (Exception ex)
                        {
                            ex.GetBaseException();
                        }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "Municipio", found.ToString(), created.ToString() };
        }

        private string[] LoaderNatureza(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (NaturezaBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                NaturezaBusiness.Inserir(new Natureza()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "Natureza", found.ToString(), created.ToString() };
        }

        private string[] LoaderFuncaoGrid(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (FuncaoGridsBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                FuncaoGridsBusiness.Inserir(new FuncaoGrids()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "FunçãoGrids", found.ToString(), created.ToString() };
        }

        private string[] LoaderEspecieAcidenteImpessoal(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (EspecieAcidenteImpessoalBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                EspecieAcidenteImpessoalBusiness.Inserir(new EspecieAcidenteImpessoal()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "EspecieAcidenteImpessoal", found.ToString(), created.ToString() };
        }

        private string[] LoaderTipoAcidentePessoal(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (TipoAcidentePessoalBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                TipoAcidentePessoalBusiness.Inserir(new TipoAcidentePessoal()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "TipoAcidentePessoal", found.ToString(), created.ToString() };
        }

        private string[] LoaderAgenteAcidente(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (AgenteAcidenteBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                AgenteAcidenteBusiness.Inserir(new AgenteAcidente()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "AgenteAcidente", found.ToString(), created.ToString() };
        }

        private string[] LoaderCondicaoAmbientalInseg(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (CondicaoAmbientalInsegBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                CondicaoAmbientalInsegBusiness.Inserir(new CondicaoAmbientalInseg()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "CondicaoAmbientalInseg", found.ToString(), created.ToString() };
        }

        private string[] LoaderPrejuizoMaterial(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (PrejuizoMaterialBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                PrejuizoMaterialBusiness.Inserir(new PrejuizoMaterial()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "PrejuizoMaterial", found.ToString(), created.ToString() };
        }

        private string[] LoaderAtoInseguro(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (AtoInseguroBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                AtoInseguroBusiness.Inserir(new AtoInseguro()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "AtoInseguro", found.ToString(), created.ToString() };
        }

        private string[] LoaderFatorPessoalInseguranca(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (FatorPessoalInsegurancaBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                FatorPessoalInsegurancaBusiness.Inserir(new FatorPessoalInseguranca()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "FatorPessoalInseguranca", found.ToString(), created.ToString() };
        }

        private string[] LoaderNaturezaLesao(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (NaturezaLesaoBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                NaturezaLesaoBusiness.Inserir(new NaturezaLesao()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "NaturezaLesao", found.ToString(), created.ToString() };
        }

        private string[] LoaderLocalizacaoLesao(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (LocalizacaoLesaoBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                LocalizacaoLesaoBusiness.Inserir(new LocalizacaoLesao()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "LocalizacaoLesao", found.ToString(), created.ToString() };
        }

        private string[] LoaderFonteLesao(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (FonteLesaoBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                FonteLesaoBusiness.Inserir(new FonteLesao()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "FonteLesao", found.ToString(), created.ToString() };
        }

        private string[] LoaderESocial(ExcelWorksheet ws, string UsuarioLogado)
        {
            int found = 0;
            int created = 0;

            int i = 0;
            for (i = 1; i < 50000; i++)
            {
                try
                {
                    if (ws.Cells[i, 1] != null && ws.Cells[i, 1].Value != null && !string.IsNullOrEmpty(ws.Cells[i, 1].Value.ToString()))
                    {
                        found += 1;
                        try
                        {
                            string xCodigo = ws.Cells[i, 1].Value.ToString();
                            if (ESocialBusiness.Consulta.Where(a => a.Codigo.Equals(xCodigo) && string.IsNullOrEmpty(a.UsuarioExclusao)).Count() == 0)
                            {
                                ESocialBusiness.Inserir(new ESocial()
                                {
                                    Codigo = ws.Cells[i, 1].Value.ToString(),
                                    Descricao = ws.Cells[i, 2].Value.ToString(),
                                    NomeCompleto = ws.Cells[i, 3].Value.ToString(),
                                    UsuarioInclusao = UsuarioLogado
                                });

                                created += 1;
                            }
                        }
                        catch { }
                    }
                    else
                        break;
                }
                catch { }
            }

            return new string[] { "ESocial", found.ToString(), created.ToString() };
        }

    }
}
