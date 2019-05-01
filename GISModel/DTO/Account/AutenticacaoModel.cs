using GISModel.DTO.Permissoes;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Account
{
    [Serializable]
    public class AutenticacaoModel
    {

        [Required(ErrorMessage = "Login é obrigatório")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        
        public string Nome { get; set; }

        public string Email { get; set; }

        public string UniqueKey { get; set; }

        public List<VMPermissao> Permissoes { get; set; }
        
    }
}
