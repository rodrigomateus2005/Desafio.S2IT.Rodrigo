using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Desafio.S2IT.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Desafio.S2IT.Data.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Desafio.S2IT.Web.Stores;
using Desafio.S2IT.Data.Domain.Repository;
using Desafio.S2IT.Data.Application.Business;

namespace Desafio.S2IT.Web.Controllers
{
    public class LoginController : Controller
    {

        private readonly DesafioUserMananger _userManager;
        private readonly SignInManager<Login> _signInManager;
        private readonly IOptions<SqlConnectionOptions> _configuration;

        public LoginController(
            IOptions<SqlConnectionOptions> configuration,
            DesafioUserMananger userManager,
            SignInManager<Login> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this._configuration == null)
            {
                return RedirectToAction(nameof(MigrateController.Index), "Migrate");
            }

            if (!RepositoryBase.IsValidOptions(this._configuration.Value))
            {
                return RedirectToAction(nameof(MigrateController.Index), "Migrate");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Index(LoginModel login)
        {
            var resultado = await _signInManager.PasswordSignInAsync(login.Usuario, login.Senha, false, false);
            
            if (resultado.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                login.LoginIncorreto = true;
                return View(login);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}