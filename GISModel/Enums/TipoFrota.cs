using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum TipoFrota
    {

        [Display(Name = "CEMIG (Nº frota)")]
        CEMIG = 1,

        [Display(Name = "Contratada (Nº placa)")]
        Contratada = 2,

    }
}
