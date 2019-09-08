using System;

namespace GISModel.DTO.Inbox
{
    public class VMWorkflowStep
    {

        public string Nome { get; set; }

        public string Status { get; set; }

        public string MensagemPasso { get; set; }

        public string Responsavel { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime DataExclusao { get; set; }

        public string UsuarioExclusao { get; set; }

        public string NomeUsuarioExclusao { get; set; }

        public string NomeResponsavel { get; set; }

    }
}
