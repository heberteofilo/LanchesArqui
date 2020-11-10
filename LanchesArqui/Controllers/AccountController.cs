using LanchesArqui.Models;
using LanchesArqui.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LanchesArqui.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CarrinhoCompra _carrinhoCompra;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 CarrinhoCompra carrinhoCompra)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _carrinhoCompra = carrinhoCompra;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            }
            );
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,
                                                                      loginVM.Password,
                                                                      false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Usuário/Senha inválidos ou não localizados");
            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = register.UserName };
                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    // Adiciona o usuário padrão ao perfil Member
                    await _userManager.AddToRoleAsync(user, "Member");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("LoggedIn", "Account");
                    //return RedirectToAction("Index", "Home");
                }
            }
            return View(register);
        }

        public ViewResult LoggedIn() => View();

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //limpa o carrinho antes de fazer logoff, para não exibir carrinho a outros usuários
            _carrinhoCompra.LimparCarrinho();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}