using GISModel.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJEmpresa")]
    public class Empresa : EntidadeBase
    {
        
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe um CNPJ")]
        [CustomValidationCNPJ(ErrorMessage = "CPNJ inválido")]
        public string CNPJ { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "Informe o nome da Empresa")]
        public string NomeFantasia { get; set; }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Display(Name = "URL do Site")]
        public string URL_Site { get; set; }
        
        [Display(Name = "URL do WebService (DMZ)")]
        public string URL_WS { get; set; }

        [Display(Name = "URL do AD na Intranet")]
        public string URL_AD { get; set; }

    }
}
