﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJEstado")]
    public class Estado : EntidadeBase
    {

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

    }
}