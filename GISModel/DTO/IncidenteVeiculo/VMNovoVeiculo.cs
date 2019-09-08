using GISModel.CustomAttributes;
using GISModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.IncidenteVeiculo
{
    public class VMNovoVeiculo
    {

        [Display(Name = "Nº Pessoal Condutor")]
        public string NPCondutor { get; set; }

        [Display(Name = "Nome Condutor")]
        public string NomeCondutor { get; set; }

        [Display(Name = "Tipo de Veículo")]
        [RequiredEnum(ErrorMessage = "O campo 'Tipo de Veículo' é obrigatório.")]
        public TipoDeVeiculo TipoVeiculo { get; set; }

        [RequiredEnum(ErrorMessage = "O campo 'Zona' é obrigatório.")]
        public Zona Zona { get; set; }

        [RequiredEnum(ErrorMessage = "O campo 'Natureza' é obrigatório.")]
        public NaturezaVeiculo Natureza { get; set; }

        public int Custo { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Tipo de Frota")]
        [RequiredEnum(ErrorMessage = "O campo 'Tipo Frota' é obrigatório.")]
        public TipoFrota TipoFrota { get; set; }


        [Display(Name = "Tipo de Condutor")]
        [RequiredEnum(ErrorMessage = "O campo 'Tipo Condutor' é obrigatório.")]
        public TipoCondutor TipoCondutor { get; set; }

        [Display(Name = "Ação do Condutor")]
        [RequiredEnum(ErrorMessage = "O campo 'Ação do Condutor' é obrigatório.")]
        public AcaoCondutor AcaoCondutor { get; set; }


        public string UKIncidenteVeiculo { get; set; }

    }
}
