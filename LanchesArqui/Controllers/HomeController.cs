using LanchesArqui.Repositories;
using LanchesArqui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesArqui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILancheRepository _lancheRepository;

        public HomeController(ILancheRepository lancheRepository) // injeção no construtor
        {
            _lancheRepository = lancheRepository;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                LanchesPreferidos = _lancheRepository.LanchesPreferidos
            };
            return View(homeViewModel);
        } 
        public ViewResult AccessDenied()
        {
            return View();
        }

    }
}
