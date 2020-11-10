using LanchesArqui.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LanchesArqui.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        [Key]
        public string IdCarrinhoCompra { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public CarrinhoCompra(AppDbContext contexto)
        {
            _context = contexto;
        }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            //define uma sessão acessando o contexto atual(tem que registrar em IServicesCollections)
            ISession session =
                services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session; // permite criar uma sessão no contexto

            // obtem um serviço do tipo nosso contexto
            var context = services.GetService<AppDbContext>();

            //obtem ou gera o Id do carrinho
            string idCarrinho = session.GetString("IdCarrinho") ?? Guid.NewGuid().ToString(); //newguid gera um numero unico para carrinho na sessão

            //atribui o id do carrinho na Sessão
            session.SetString("IdCarrinho", idCarrinho);

            //retorna o carrinho com o contexto atual e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                IdCarrinhoCompra = idCarrinho
            };
        }

        public void AdicionarAoCarrinho(Lanche lanche, int quantidade)
        {
            var carrinhoCompraItem =
                _context.CarrinhoCompraItens.SingleOrDefault(
                    s => s.Lanche.Id == lanche.Id && s.Id_CarrinhoCompra == IdCarrinhoCompra);

            //verifica se o carrinho existe e senao existir cria um
            if (carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    Id_CarrinhoCompra = IdCarrinhoCompra,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else //se existir o carrinho com o item então incrementa a quantidade
            {
                carrinhoCompraItem.Quantidade++;
            }
            _context.SaveChanges();          
        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem =
                _context.CarrinhoCompraItens.SingleOrDefault(
                    s => s.Lanche.Id == lanche.Id && s.Id_CarrinhoCompra == IdCarrinhoCompra);

            var quantidadeLocal = 0;

            if (carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.Quantidade > 1) // se for maior que 1 diminui -1
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else // se for igual a 1 remove do carrinho o item
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
            }

            _context.SaveChanges();

            return quantidadeLocal;
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItems() // retorna os itens do carrinho pela id
        {
            return CarrinhoCompraItens ??
                (CarrinhoCompraItens =
                _context.CarrinhoCompraItens.Where(c => c.Id_CarrinhoCompra == IdCarrinhoCompra)
                .Include(s => s.Lanche)
                .ToList());
        }

        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                        .Where(carrinho => carrinho.Id_CarrinhoCompra == IdCarrinhoCompra);

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens); // remove todos os itens

            _context.SaveChanges();
        }

        public decimal GetCarrinhoCompraTotal() // soma todos os itens do carrinho
        {
            var total = _context.CarrinhoCompraItens.Where(c => c.Id_CarrinhoCompra == IdCarrinhoCompra)
                                                    .Select(c => c.Lanche.Preco * c.Quantidade).Sum();

            return total;
        }
    }
}
