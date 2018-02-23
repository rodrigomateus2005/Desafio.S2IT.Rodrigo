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
    public class CadastroAmigoController : Controller
    {
        private readonly SqlConnectionOptions _configuration;

        public CadastroAmigoController(
            IOptions<SqlConnectionOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public IActionResult Index(string Id)
        {
            CadastroAmigoModel model = new CadastroAmigoModel();
            if (!String.IsNullOrWhiteSpace(Id))
            {
                Amigo entidade = new AmigoBusiness(this._configuration).ObterPorIDs(new object[] { int.Parse(Id) });

                if (entidade != null)
                {
                    model.Id = entidade.Id;
                    model.Nome = entidade.Nome;
                }
            }

            model.Entidades = new AmigoBusiness(this._configuration).ObterTodos();

            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(CadastroAmigoModel model)
        {
            Amigo entidade = null;
            if (model.Id > 0)
            {
                entidade = new AmigoBusiness(this._configuration).ObterPorIDs(new object[] { model.Id });
            }

            if (entidade == null)
            {
                entidade = new Amigo();
            }

            entidade.Nome = model.Nome;

            entidade = new AmigoBusiness(this._configuration).SalvarOuAtualizar(entidade);

            return RedirectToAction(nameof(CadastroAmigoController.Index), "CadastroAmigo");
        }

        [HttpPost]
        public IActionResult Deletar(string Id)
        {
            if (!String.IsNullOrWhiteSpace(Id))
            {
                Amigo entidade = new AmigoBusiness(this._configuration).ObterPorIDs(new object[] { int.Parse(Id) });
                new AmigoBusiness(this._configuration).Deletar(entidade);
            }

            return RedirectToAction(nameof(CadastroAmigoController.Index), "CadastroAmigo");
        }
    }
}