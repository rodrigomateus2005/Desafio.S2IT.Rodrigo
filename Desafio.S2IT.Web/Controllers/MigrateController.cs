using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.S2IT.Data.Domain.Repository;
using Desafio.S2IT.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Desafio.S2IT.Web.Controllers
{
    public class MigrateController : Controller
    {

        private readonly IOptions<SqlConnectionOptions> _configuration;

        public MigrateController(
            IOptions<SqlConnectionOptions> configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            MigrateModel model = new MigrateModel();

            model.DataSource = ".";
            model.InitialCatalog = "DESAFIO";
            model.UserID = "sa";

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(MigrateModel model)
        {
            SqlConnectionOptions con = new SqlConnectionOptions();

            con.DataSource = model.DataSource;
            con.InitialCatalog = model.InitialCatalog;
            con.UserID = model.UserID;
            con.Password = model.Password;

            if (String.IsNullOrWhiteSpace(con.InitialCatalog))
            {
                con.InitialCatalog = "DESAFIO";
            }

            try
            {

                if (RepositoryBase.IsValidServer(con))
                {
                    if (!RepositoryBase.IsValidOptions(con))
                    {
                        RepositoryBase.CriarBanco(con);

                    }

                    this._configuration.Value.InitialCatalog = con.InitialCatalog;
                    this._configuration.Value.DataSource = con.DataSource;
                    this._configuration.Value.UserID = con.UserID;
                    this._configuration.Value.Password = con.Password;

                    return RedirectToAction(nameof(LoginController.Index), "Login");
                }
                else
                {
                    model.Mensagem = "Configurações do banco incorretas";
                }

            }
            catch (Exception ex)
            {
                model.Mensagem = "Erro: " + ex.Message;
            }

            return View(model);
        }
    }
}