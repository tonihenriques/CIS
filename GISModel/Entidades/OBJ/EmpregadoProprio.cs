using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("OBJEmpregadoProprio")]
    public class EmpregadoProprio : EntidadeBase
    {

        [Display(Name = "Matrícula")]
        public string NumeroPessoal { get; set; }

        [Display(Name ="Nome do Empregado")]
        public string Nome { get; set; }

        

        

        

    }
}