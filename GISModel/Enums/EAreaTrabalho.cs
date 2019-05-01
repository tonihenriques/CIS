using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EAreaTrabalho
    {
        [Display(Name = "CENTRO I")]
        CENTRO_I = 1,

        [Display(Name = "CENTRO II")]
        CENTRO_II= 2,

        [Display(Name = "LESTE")]
        LESTE = 3,

        [Display(Name = "MANTIQUEIRA")]
        MANTIQUEIRA = 4,

        [Display(Name = "NORTE")]
        NORTE = 5,

        [Display(Name = "OESTE")]
        OESTE = 6,

        [Display(Name = "SUL")]
        SUL = 7,

        [Display(Name = "TRIÂNGULO")]
        TRIÂNGULO = 8,

    }
}

