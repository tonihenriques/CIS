using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum Zona
    {
        [Display(Name = "1 - ZONA URBANA")]
        URBANA = 1,

        [Display(Name = "2 - ZONA RURAL")]
        RURAL = 2,

        [Display(Name = "3 - RODOVIA")]
        RODOVIA = 3,

        [Display(Name = "4 - PATIO CEMIG")]
        PATIO_CEMIG = 4,

        [Display(Name = "5 - OUTROS")]
        OUTROS = 5
    }
}
