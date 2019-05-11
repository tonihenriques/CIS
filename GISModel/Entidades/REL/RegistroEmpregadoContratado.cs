using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.REL
{
    [Table("RELRegistroEmpregadoContratado")]
    public class RegistroEmpregadoContratado : EntidadeBase
    {

        [Required(ErrorMessage = "Selecione um registro")]
        public string UKRegistro { get; set; }

        public virtual Incidente Incidente { get; set; }

        [Required(ErrorMessage = "Selecione um contratado")]
        public string UKEmpregadoContratado { get; set; }

        public virtual EmpregadoContratado EmpregadoContratado { get; set; }




        [Display(Name = "Função")]
        public string Funcao { get; set; }

        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string UKFornecedor { get; set; }




        public string UKCodificacao { get; set; }

        public string UKLesaoDoenca { get; set; }

        public string UKCAT { get; set; }

    }
}
