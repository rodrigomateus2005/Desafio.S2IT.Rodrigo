using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Desafio.S2IT.Web.Models;
using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Application.Business;
using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.Extensions.Options;

namespace Desafio.S2IT.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SqlConnectionOptions _configuration;

        public HomeController(
            IOptions<SqlConnectionOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        public IActionResult Index()
        {
            HomeModel model = new HomeModel();

            model.JogosDisponiveis = new JogoBusiness(this._configuration).ObterTodosDisponiveis();
            model.JogosEmprestados = new JogoBusiness(this._configuration).ObterTodosEmprestados();

            return View(model);
        }

        [HttpPost]
        public IActionResult Devolver(string Id)
        {
            new EmprestimoBusiness(this._configuration).DevolverJogo(int.Parse(Id));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}