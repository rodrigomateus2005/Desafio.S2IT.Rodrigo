using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Desafio.S2IT.Data.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Desafio.S2IT.Web.Stores;
using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Desafio.S2IT.Web.Middlewares;

namespace Desafio.S2IT.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SqlConnectionOptions>(Configuration.GetSection("DesafioConexao"));

            services.AddSingleton<IUserStore<Login>, LoginStore>();
            services.AddSingleton<IRoleStore<Perfil>, PerfilStore>();

            services.AddIdentity<Login, Perfil>().AddDefaultTokenProviders().AddUserManager<DesafioUserMananger>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Login/Index");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMiddleware<VerifyDataBaseConfiguredMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
