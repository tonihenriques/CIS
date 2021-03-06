﻿using GISModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Envolvidos
{
    public class VMProprio
    {

        public string UKRel { get; set; }

        public string UKIncidente { get; set; }

        public string UKEmpregado { get; set; }

        public string UKCodificacao { get; set; }

        public string UKLesaoDoenca { get; set; }

        public string UKCAT { get; set; }



        [Display(Name = "Matrícula")]
        public string NumeroPessoal { get; set; }

        [Display(Name = "Nome do Empregado")]
        public string Nome { get; set; }

        [Display(Name = "Função")]
        public string Funcao { get; set; }



        [Display(Name = "Descrição da Lesão/Doença")]
        [Required(ErrorMessage = "Favor Inserir uma Descrição")]
        public string DescricaoLesao { get; set; }

        [Display(Name = "Natureza da Lesão Principal")]
        public string UKNaturezaLesaoPrincipal { get; set; }

        [Display(Name = "Local da Lesão Principal")]
        public string UKLocalizacaoLesaoPrincipal { get; set; }

        [Display(Name = "Natureza da Lesão Secundária")]
        public string UKNaturezaLesaoSecundaria { get; set; }

        [Display(Name = "Local da Lesão Secundária")]
        public string UKLocalizacaoLesaoSecundaria { get; set; }

    }
}
