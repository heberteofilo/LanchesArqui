using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LanchesArqui.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string CategoriaNome { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }
        //public List<Lanche> Lanches { get; set; } = new List<Lanche>();

    }
}
