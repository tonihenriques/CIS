using GISModel.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("OBJEmpregadoContratado")]
    public class EmpregadoContratado : EntidadeBase
    {

        [Display(Name = "Nome do Empregado")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF obrigatório")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Display(Name = "Data de Nascimento")]
        public string Nascimento { get; set; }

    }
}