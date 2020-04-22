using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJPopulacao")]
    public class Populacao : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o nome da pessoa a ser cadastrada")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Sexo' é obrigatório")]
        public ESexo Sexo { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo 'Data de Nascimento' é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime DataNascimento { get; set; }

    }
}
