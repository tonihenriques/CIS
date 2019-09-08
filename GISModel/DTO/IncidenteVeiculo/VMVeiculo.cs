using GISModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.IncidenteVeiculo
{
    public class VMVeiculo
    {

        [Display(Name = "Nº Pessoal Condutor")]
        public string NPCondutor { get; set; }

        [Display(Name = "Nome Condutor")]
        public string NomeCondutor { get; set; }

        [Display(Name = "Tipo de Veículo")]
        public TipoDeVeiculo TipoVeiculo { get; set; }

        public Zona Zona { get; set; }

        public NaturezaVeiculo Natureza { get; set; }

        public int Custo { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Tipo de Frota")]
        public TipoFrota TipoFrota { get; set; }


        [Display(Name = "Tipo de Condutor")]
        public TipoCondutor TipoCondutor { get; set; }

        [Display(Name = "Ação do Condutor")]
        public AcaoCondutor AcaoCondutor { get; set; }


        public string UKRel { get; set; }

    }
}
