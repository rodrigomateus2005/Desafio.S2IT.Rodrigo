using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Middlewares
{
    public class VerifyDataBaseConfiguredMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IOptions<SqlConnectionOptions> _configurations;

        public VerifyDataBaseConfiguredMiddleware(RequestDelegate next, IOptions<SqlConnectionOptions> configurations)
        {
            _next = next;
            _configurations = configurations;
        }

        public async Task Invoke(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(this._configurations.Value.DataSource) && context.Request.Path != "/Migrate")
            {
                context.Request.Path = "/Migrate";
            }

            await _next(context);

        }

    }
}

