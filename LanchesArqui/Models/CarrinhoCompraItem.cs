using System.ComponentModel.DataAnnotations;

namespace LanchesArqui.Models
{
    public class CarrinhoCompraItem
    {
        [Key]
        public int Id { get; set; }
        public Lanche Lanche { get; set; }
        public int Quantidade { get; set; }

        [StringLength(200)]
        public string Id_CarrinhoCompra { get; set; }

    }
}
