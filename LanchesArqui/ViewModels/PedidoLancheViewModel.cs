using LanchesArqui.Models;
using System.Collections.Generic;

namespace LanchesArqui.ViewModels
{
    //ViewModel serve para representar os dados de uma view e pode ser composta de uma ou mais models
    public class PedidoLancheViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
