using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    public class Acesso : EntidadeBase
    {

        public string CPFUsuario { get; set; }

        public string LoginUsuario { get; set; }

        public string EmailUsuario { get; set; }

        public string UKEmpresa { get; set; }

    }
}
