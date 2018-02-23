using Desafio.S2IT.Data.Application.Business;
using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Stores
{
    public class LoginStore : IUserStore<Login>, IUserPasswordStore<Login>
    {
        private readonly SqlConnectionOptions _connectionOpt;

        public LoginStore(IOptions<SqlConnectionOptions> configuration)
        {
            _connectionOpt = configuration.Value;
        }

        public Task<IdentityResult> CreateAsync(Login user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

            loginBll.SalvarOuAtualizar(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Login user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

            loginBll.Deletar(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<Login> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (String.IsNullOrWhiteSpace(this._connectionOpt.DataSource))
            {
                LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

                return Task.FromResult<Login>(null);
            }
            else
            {
                LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

                return Task.FromResult(loginBll.ObterPorIDs(new object[] { int.Parse(userId) }));
            }
            
        }

        public Task<Login> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

            return Task.FromResult(loginBll.ObterPorNome(normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(Login user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Usuario);
        }

        public Task<string> GetUserIdAsync(Login user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(Login user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Usuario);
        }

        public Task SetNormalizedUserNameAsync(Login user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Usuario = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(Login user, string userName, CancellationToken cancellationToken)
        {
            user.Usuario = userName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(Login user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            LoginBusiness loginBll = new LoginBusiness(this._connectionOpt);

            loginBll.SalvarOuAtualizar(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task SetPasswordHashAsync(Login user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Senha = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(Login user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Senha);
        }

        public Task<bool> HasPasswordAsync(Login user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Senha != null);
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
