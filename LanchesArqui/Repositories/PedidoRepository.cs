using LanchesArqui.Context;
using LanchesArqui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesArqui.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(AppDbContext appDbContext, CarrinhoCompra carrinhoCompra) //acesso ao carrinho de compra
        {
            _appDbContext = appDbContext;
            _carrinhoCompra = carrinhoCompra;
        }
        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now;
            _appDbContext.Pedidos.Add(pedido); // envia para o banco
            _appDbContext.SaveChanges(); // persistir as informações no DB

            var carrinhoCompraItens = _carrinhoCompra.CarrinhoCompraItens; //obtem itens do carrinho

            foreach (var carrinhoItem in carrinhoCompraItens)
            {
                var pedidoDetalhe = new PedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    LancheId = carrinhoItem.Lanche.Id,
                    PedidoId = pedido.Id,
                    Preco = carrinhoItem.Lanche.Preco
                };
                _appDbContext.PedidoDetalhes.Add(pedidoDetalhe);
            }
            _appDbContext.SaveChanges(); // persistir as informações no DB
        }
    }
}
