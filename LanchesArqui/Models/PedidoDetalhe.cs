using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesArqui.Models
{
    public class PedidoDetalhe
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int LancheId { get; set; }
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        //prop de navegacão
        public virtual Lanche Lanche { get; set; }
        public virtual Pedido Pedido { get; set; }

    }
}
