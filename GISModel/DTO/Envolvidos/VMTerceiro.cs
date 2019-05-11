using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Envolvidos
{
    public class VMTerceiro
    {

        public string UKRel { get; set; }

        public string UKEmpregado { get; set; }

        public string UKCodificacao { get; set; }

        public string UKIncidente { get; set; }

        public string UKLesaoDoenca { get; set; }

        public string UKCAT { get; set; }



        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string UKFornecedor { get; set; }

        [Display(Name = "Nome do Empregado")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Data de Nascimento")]
        public string Nascimento { get; set; }

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
