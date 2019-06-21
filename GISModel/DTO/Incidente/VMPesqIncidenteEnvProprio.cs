using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Incidente
{
    public class VMPesqIncidenteEnvProprio
    {

        public string Matricula { get; set; }

        public string Nome { get; set; }

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
