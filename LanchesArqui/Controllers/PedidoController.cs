using LanchesArqui.Models;
using LanchesArqui.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LanchesArqui.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost] // se não colocar annotation fica com Get
        [Authorize]
        public IActionResult Checkout(Pedido pedido)
        {
            decimal precoTotalPedido = 0.0m;
            int totalItensPedido = 0;

            var items = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItens = items;

            //verifica se tem itens no pedido
            if (_carrinhoCompra.CarrinhoCompraItens.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho está vazio, inclua um lanche...");
            }

            //calcula o total do pedido
            foreach (var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribui o total de itens do pedido
            pedido.TotalItensPedido = totalItensPedido;

            //atribui o total do pedido ao pedido
            pedido.PedidoTotal = precoTotalPedido;

            if (ModelState.IsValid)
            {
                _pedidoRepository.CriarPedido(pedido);

                // TempData persiste os dados de um Controller para outro.
                //TempData["Nome"] = pedido.Nome;
                //TempData["NumeroPedido"] = pedido.Id;
                //TempData["DataPedido"] = pedido.PedidoEnviado;


                ViewBag.TotalPedido = pedido.PedidoTotal = _carrinhoCompra.GetCarrinhoCompraTotal();
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :) ";

                _carrinhoCompra.LimparCarrinho();
                //redireciona para a action informada
                //return RedirectToAction("CheckoutCompleto");
                //Em vez de redirecionar pode chamar diretamente a View, tipando com o pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }

        public IActionResult CheckoutCompleto()
        {
            // ViewBag manda dados do Controller para a View.
            ViewBag.Cliente = TempData["Cliente"];
            ViewBag.NumeroPedido = TempData["NumeroPedido"];
            ViewBag.DataPedido = TempData["DataPedido"];
            ViewBag.TotalPedido = TempData["TotalPedido"];
            ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :) ";
            return View();
        }
    }
}