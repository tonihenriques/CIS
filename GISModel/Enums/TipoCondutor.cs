using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum TipoCondutor
    {

        [Display(Name = "1 - MOTORISTA")]
        Motorista = 1,

        [Display(Name = "2 - ELETRICISTA MOTORISTA")]
        Eletricista_Motorista = 2,

        [Display(Name = "3 - CREDENCIADO")]
        Credenciado = 3,

        [Display(Name = "4 - MOTOCICLISTA")]
        Motociclista = 4

    }
}
