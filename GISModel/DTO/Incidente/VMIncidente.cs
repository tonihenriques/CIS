using GISModel.DTO.Envolvidos;
using GISModel.Entidades;
using GISModel.Enums;
using System.Collections.Generic;

namespace GISModel.DTO.Incidente
{
    public class VMIncidente
    {

        public string UniqueKey { get; set; }




        public string UKWorkflow { get; set; }
        
        public string NomeWF { get; set; }

        public string StatusWF { get; set; }

        public string MensagemPasso { get; set; }




        public string Codigo { get; set; }

        public StatusIncidente Status { get; set; }

        public string NumeroSmart { get; set; }

        public string ETipoAcidente { get; set; }

        public string Orgao { get; set; }

        public string Diretoria { get; set; }

        public string DataIncidente { get; set; }

        public string HoraIncidente { get; set; }

        public string Municipio { get; set; }

        public string Estado { get; set; }


        public string ESocial { get; set; }

        public string Descricao { get; set; }


        public string LocalAcidente { get; set; }

        public string Logradouro { get; set; }

        public string NumeroLogradouro { get; set; }

        public string TipoLocalAcidente { get; set; }

        public string AcidenteGraveIP102 { get; set; }

        public string AcidenteFatal { get; set; }

        public string TipoEntrada { get; set; }

        public string Centro { get; set; }

        public string Regional { get; set; }



        public string UsuarioInclusao { get; set; }

        public string DataInclusao { get; set; }

        public string DataAtualizacao { get; set; }


        public List<Arquivo> Arquivos { get; set; }

        public List<VMProprio> EnvolvidosProprio { get; set; }

        public List<VMTerceiro> EnvolvidosTerceiro { get; set; }

        public OperacaoCollection Operacoes { get; set; }

    }
}
