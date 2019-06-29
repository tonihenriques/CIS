using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoEntradaVeiculo
    {

        [Display(Name = "Acidente de Trabalho")]
        Acidente_de_Trabalho = 1,

        [Display(Name = "Acidente de Trajeto")]
        Acidente_de_Trajeto = 2,

        [Display(Name = "Acidente Potencial")]
        Acidente_Potencial = 3,

        [Display(Name = "Acidente com danos materiais")]
        Acidente_Danos_Materiais = 4,

        [Display(Name = "Acidente com população")]
        Acidente_Populacao = 5,

        [Display(Name = "Acidente de Trânsito")]
        Acidente_Transito = 5,

    }
}
