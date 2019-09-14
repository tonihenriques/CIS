using GISModel.CustomAttributes;
using GISModel.Entidades.OBJ;
using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.REL
{
    [Table("RELIncidenteVeiculoPopulacao")]
    public class IncidenteVeiculoPopulacao : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o incidente com veículo")]
        public string UKIncidenteVeiculo { get; set; }

        public virtual IncidenteVeiculo IncidenteVeiculo { get; set; }



        [Required(ErrorMessage = "Informe a pessoa envolvida no incidente")]
        public string UKPopulacao { get; set; }

        public virtual Populacao Populacao { get; set; }



        [Display(Name = "Agente Causador")]
        [RequiredEnum(ErrorMessage = "O campo 'Agente Causador' é obrigatório.")]
        public EAgenteCausador AgenteCausador { get; set; }

        
        [Display(Name = "Tipo de Acidente")]
        [RequiredEnum(ErrorMessage = "O campo 'Tipo de Acidente' é obrigatório.")]
        public EPopulacaoTipoAcidente TipoAcidente { get; set; }


        [Display(Name = "Situação da Rede")]
        [RequiredEnum(ErrorMessage = "O campo 'Situação da Rede' é obrigatório.")]
        public ESituacaoRede SituacaoRede { get; set; }


        [RequiredEnum(ErrorMessage = "O campo 'Atividade' é obrigatório.")]
        public EPopulacaoAtividade Atividade { get; set; }


        [RequiredEnum(ErrorMessage = "O campo 'Causa' é obrigatório.")]
        public EPopulacaoCausa Causa { get; set; }


        [Display(Name = "Lesão")]
        [RequiredEnum(ErrorMessage = "O campo 'Lesão' é obrigatório.")]
        public EPopulacaoLesao Lesao { get; set; }


        [Display(Name = "Nível Tensão")]
        [RequiredEnum(ErrorMessage = "O campo 'Nível Tensão' é obrigatório.")]
        public ENivelTensao NivelTensao { get; set; }


        [RequiredEnum(ErrorMessage = "O campo 'Natureza' é obrigatório.")]
        public ENatureza Natureza { get; set; }


        public string Custo { get; set; }

    }
}
