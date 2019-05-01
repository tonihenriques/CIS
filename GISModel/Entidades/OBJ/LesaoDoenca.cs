using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJLesaoDoenca")]
    public class LesaoDoenca : EntidadeBase
    {

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
