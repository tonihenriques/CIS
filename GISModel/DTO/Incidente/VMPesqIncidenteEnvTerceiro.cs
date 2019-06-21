using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Incidente
{
    public class VMPesqIncidenteEnvTerceiro
    {

        public string CPF { get; set; }

        public string Nome { get; set; }

        public string Nascimento { get; set; }

        [Display(Name = "Fornecedor")]
        public string UKFornecedor { get; set; }
        
        [Display(Name = "Função")]
        public string Funcao { get; set; }

        [Display(Name = "Descrição da Lesão/Doença")]
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
