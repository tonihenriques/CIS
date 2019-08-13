﻿using GISModel.CustomAttributes;
using GISModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Incidente
{
    public class VMNovaCAT
    {
        public string UniqueKey { get; set; }

        public string UKRelEnvolvido { get; set; }

        public string Tipo { get; set; }


        [Display(Name = "Número da CAT")]
        public string NumeroCat { get; set; }

        [Display(Name = "Data do Registro")]
        public string DataRegistro { get; set; }

        [Display(Name = "Tipo de Registrador")]
        public ETipoRegistrador ETipoRegistrador { get; set; }

        [Display(Name = "Tipo de CAT")]
        public ETipoCAT ETipoCAT { get; set; }

        [Display(Name = "CAT iniciada por:")]
        [RequiredEnum(ErrorMessage = "O campo 'Tipo de Iniciativa' é obrigatório.")]
        public ETipoIniciativa ETipoIniciativa { get; set; }

        [Display(Name = "Cód. Atendimento CNES")]
        public string CodigoCNS { get; set; }

        [Display(Name = "Data do Atendimento")]
        public string DataAtendimento { get; set; }

        [Display(Name = "Hora do Atendimento")]
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
        [RequiredEnum(ErrorMessage = "O órgão de classe é obrigatório.")]
        public EOrgaoClasse EOrgaoClasse { get; set; }

        [Display(Name = "Num. Insc. Órgão de Classe")]
        public string NumOrgClasse { get; set; }

        [Display(Name = "Un. da Fed. do Órgão de Classe ")]
        [RequiredEnum(ErrorMessage = "A Unidade da Federação é obrigatório.")]
        public EUnidadeFederacao EUnidadeFederacao { get; set; }

    }
}
