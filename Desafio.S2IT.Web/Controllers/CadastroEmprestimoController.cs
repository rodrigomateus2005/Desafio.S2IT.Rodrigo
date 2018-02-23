using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.Extensions.Options;
using Desafio.S2IT.Web.Models;
using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Application.Business;

namespace Desafio.S2IT.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class CadastroEmprestimoController : Controller
    {
        private readonly SqlConnectionOptions _configuration;

        public CadastroEmprestimoController(
            IOptions<SqlConnectionOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CadastroEmprestimoModel model = new CadastroEmprestimoModel();

            model.Amigos = new AmigoBusiness(this._configuration).ObterTodos();

            model.JogosDisponiveis = new JogoBusiness(this._configuration).ObterTodosDisponiveis();

            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(CadastroEmprestimoModel model)
        {
            Emprestimo entidade = new Emprestimo();

            entidade.Amigo = model.Amigo;
            entidade.Jogo = model.Jogo;
            entidade.DataEmprestimo = DateTime.Today;

            entidade = new EmprestimoBusiness(this._configuration).SalvarOuAtualizar(entidade);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}