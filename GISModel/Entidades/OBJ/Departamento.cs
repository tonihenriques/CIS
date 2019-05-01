using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("OBJDepartamento")]
    public class Departamento : EntidadeBase
    {

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Informe o código do departamento")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe a sigla do departamento")]
        public string Sigla { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        


        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Selecione uma empresa")]
        public string UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }



        [Display(Name = "Departamento Vinculado")]
        public string UKDepartamentoVinculado { get; set; }



        [Display(Name = "Nível Hierarquico")]
        [Required(ErrorMessage = "Selecione um nível")]
        public string UKNivelHierarquico { get; set; }

    }
}
