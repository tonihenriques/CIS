using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace GISModel.Entidades
{
    [Table("OBJCAT")]
    public class CAT : EntidadeBase
    {
        
        [Display(Name ="Número da CAT")]
        public string NumeroCat { get; set; }

        [Display(Name ="Data do Registro")]
        [DataType(DataType.Date)]
        public string DataRegistro { get; set; }

        [Display(Name = "Tipo de Registrador")]
        public ETipoRegistrador ETipoRegistrador { get; set; }

        [Display(Name = "Tipo de CAT")]
        public ETipoCAT ETipoCAT { get; set; }

        [Display(Name = "CAT iniciada por:")]
        public ETipoIniciativa ETipoIniciativa { get; set; }

        [Display(Name = "Cód. Atentdimento CNS")]
        public string CodigoCNS { get; set; }

        [Display(Name = "Data do Atendimento")]
        [DataType(DataType.Date)]
        public string DataAtendimento { get; set; }

        [Display(Name = "Hora do Atendimento")]
        [DataType(DataType.Date)]
        public string HoraAtendimento { get; set; }

        [Display(Name = "Indicação de Internação")]
        public YesNo Internacao { get; set; }

        [Display(Name = "Duração do Tratamento")]
        public string DuracaoTratamento { get; set; }

        [Display(Name = "Haverá Afastamento")]
        public YesNo Afatamento { get; set; }

        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Display(Name = "CID")]
        public string CID { get; set; }

        [Display(Name = "Nome do Médico")]
        public string NomeMedico { get; set; }

        [Display(Name = "Órgão de Classe")]
        public EOrgaoClasse EOrgaoClasse { get; set; }

        [Display(Name = "Num. Insc. Órgão de Classe")]
        public string NumOrgClasse { get; set; }

        [Display(Name = "Un. da Fed. do Órgão de Classe ")]
        public EUnidadeFederacao EUnidadeFederacao { get; set; }

        [Display(Name = "Incidente")]
        public string UKIncidente { get; set; }
        
        public virtual Incidente Incidente { get; set; }

    }
}