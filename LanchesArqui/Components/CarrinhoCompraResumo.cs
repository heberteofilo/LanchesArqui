using LanchesArqui.Models;
using LanchesArqui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesArqui.Components
{
    public class CarrinhoCompraResumo : ViewComponent
    {
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraResumo(CarrinhoCompra carrinhoCompra) // injeção
        {
            _carrinhoCompra = carrinhoCompra;
        }

        public IViewComponentResult Invoke()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();// obter itens do carrinho

            //var itens = new List<CarrinhoCompraItem>() { new CarrinhoCompraItem(), new CarrinhoCompraItem() };


            _carrinhoCompra.CarrinhoCompraItens = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(carrinhoCompraVM); // retorna o carrinho e o total do carrinho
        }
    }
}
