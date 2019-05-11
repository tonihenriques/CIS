using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.REL
{
    [Table("RELRegistroEmpregadoProprio")]
    public class RegistroEmpregadoProprio : EntidadeBase
    {
        
        [Required(ErrorMessage = "Selecione um registro")]
        public string UKRegistro { get; set; }

        public virtual Incidente Registro { get; set; }


        [Required(ErrorMessage = "Selecione um empregado")]
        public string UKEmpregadoProprio { get; set; }

        public virtual EmpregadoProprio EmpregadoProprio { get; set; }




        [Display(Name = "Função")]
        public string Funcao { get; set; }




        public string UKCodificacao { get; set; }

        public string UKLesaoDoenca { get; set; }

        public string UKCAT { get; set; }

    }
}
