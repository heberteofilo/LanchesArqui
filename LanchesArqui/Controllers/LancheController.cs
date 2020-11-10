using LanchesArqui.Models;
using LanchesArqui.Repositories;
using LanchesArqui.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanchesArqui.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private ICategoriaRepository _categoriaRepository { get; }

        public LancheController(ILancheRepository lancheRepository, ICategoriaRepository categoriaRepository)
        {
            _lancheRepository = lancheRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult List(string categoria)
        {
            //ViewBag.Lanche = "Lanches";
            //ViewData["Categoria"] = "Categoria";

            //var lanches = _lancheRepository.Lanches;
            //return View(lanches);

            string _categoria = categoria;
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.Id);
                categoria = "Todos os lanches";
            }
            else
            {
                if(string.Equals("Normal", _categoria, StringComparison.OrdinalIgnoreCase))
                {
                   lanches = _lancheRepository.Lanches
                              .Where(p => p.Categoria.CategoriaNome.Equals("Normal")).OrderBy(p => p.Nome);
                }
                else
                {
                    lanches = _lancheRepository.Lanches
                              .Where(p => p.Categoria.CategoriaNome.Equals("Natural")).OrderBy(p => p.Nome);
                }
                categoriaAtual = _categoria;
            }

            var lanchesListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            //var lanchelistViewModel = new LancheListViewModel();
            //lanchelistViewModel.Lanches = _lancheRepository.Lanches;
            //lanchelistViewModel.CategoriaAtual = "Categoria Atual";
            return View(lanchesListViewModel);
        }

        public IActionResult Details(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l => l.Id == lancheId);
            if (lanche == null)
            {
                return View("~/Views/Error/Error.cshtml");
            }
            return View(lanche);
        }
        public IActionResult Search(string searchString)
        {
            string _searchString = searchString;
            IEnumerable<Lanche> lanches;
            string _categoryAtual = string.Empty;

            if (string.IsNullOrEmpty(_searchString))
            {
                lanches = _lancheRepository.Lanches.OrderBy(p => p.Id);
            }
            else
            {
                lanches = _lancheRepository.Lanches.Where(p => p.Nome.ToLower()
                                                   .Contains(_searchString.ToLower()));
            }

            return View("~/Views/Lanche/List.cshtml", new LancheListViewModel { Lanches = lanches, CategoriaAtual = "Todos os lanches" });
        }

    }
}