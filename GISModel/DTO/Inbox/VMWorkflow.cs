using System.Collections.Generic;

namespace GISModel.DTO.Inbox
{
    public class VMWorkflow
    {

        public string NomeDocumento { get; set; }

        public string Status { get; set; }

        public int MajorVersion { get; set; }

        public List<VMWorkflowStep> Passos { get; set; }
        
    }
}
