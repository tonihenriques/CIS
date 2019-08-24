using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum StatusIncidente
    {

        [Display(Name = "Em andamento")]
        Em_andamento = 1,

        [Display(Name = "Concluído")]
        Concluido = 2,

        Pendente = 3

    }
}
