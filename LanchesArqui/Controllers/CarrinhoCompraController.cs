using LanchesArqui.Models;
using LanchesArqui.Repositories;
using LanchesArqui.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LanchesArqui.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(ILancheRepository lancheRepository, 
                                        CarrinhoCompra carrinhoCompra) //para ter acesso aos lanches e ao carrinho de compra
        {
            _lancheRepository = lancheRepository;
            _carrinhoCompra = carrinhoCompra;
        }
        
        
        [Authorize]
        public IActionResult Index() // exibe o carrinho com base na viewmodel
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            var carrinhoCompraViewModel = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()

            };

            return View(carrinhoCompraViewModel);
        }
        [Authorize]
        //[Authorize(Roles ="Member")]
        //[AllowAnonymous]
        public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = _lancheRepository.Lanches.FirstOrDefault(p => p.Id == lancheId);
            // localiza o lanche pelo id passado

            if (lancheSelecionado != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado, 1); // passa o lanche com quantidade
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult RemoverItemNoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = _lancheRepository.Lanches.FirstOrDefault(p => p.Id == lancheId);
            // localiza o lanche pelo id passado

            if (lancheSelecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);
            }
            return RedirectToAction("Index");
        }
    }
}