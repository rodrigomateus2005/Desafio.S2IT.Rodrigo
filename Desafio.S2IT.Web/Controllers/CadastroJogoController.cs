using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Desafio.S2IT.Web.Models;
using Desafio.S2IT.Data.Application.Business;
using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.Extensions.Options;

namespace Desafio.S2IT.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class CadastroJogoController : Controller
    {
        private readonly SqlConnectionOptions _configuration;

        public CadastroJogoController(
            IOptions<SqlConnectionOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public IActionResult Index(string Id)
        {
            CadastroJogoModel model = new CadastroJogoModel();
            if (!String.IsNullOrWhiteSpace(Id))
            {
                Jogo entidade = new JogoBusiness(this._configuration).ObterPorIDs(new object[] { int.Parse(Id) });

                if (entidade != null)
                {
                    model.Id = entidade.Id;
                    model.Nome = entidade.Nome;
                }
            }

            model.Entidades = new JogoBusiness(this._configuration).ObterTodos();

            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(CadastroJogoModel model)
        {
            Jogo entidade = null;
            if (model.Id > 0)
            {
                entidade = new JogoBusiness(this._configuration).ObterPorIDs(new object[] { model.Id });
            }

            if (entidade == null)
            {
                entidade = new Jogo();
            }

            entidade.Nome = model.Nome;

            entidade = new JogoBusiness(this._configuration).SalvarOuAtualizar(entidade);

            return RedirectToAction(nameof(CadastroJogoController.Index), "CadastroJogo");
        }

        [HttpPost]
        public IActionResult Deletar(string Id)
        {
            if (!String.IsNullOrWhiteSpace(Id))
            {
                Jogo entidade = new JogoBusiness(this._configuration).ObterPorIDs(new object[] { int.Parse(Id) });
                new JogoBusiness(this._configuration).Deletar(entidade);
            }

            return RedirectToAction(nameof(CadastroJogoController.Index), "CadastroJogo");
        }


    }
}