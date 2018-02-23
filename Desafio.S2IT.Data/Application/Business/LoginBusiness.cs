using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Application.Business
{
    public class LoginBusiness : BusinessBase<Login, LoginRepository>
    {
        public LoginBusiness(IConnectionOptions conOpt) : base(conOpt)
        {
        }

        public Login ObterPorNome(string nome)
        {
            if (String.IsNullOrWhiteSpace(nome))
            {
                return null;
            }

            this.Abrir();
            try
            {
                return this.repo.ObterPorNome(nome);
            }
            finally
            {
                this.Fechar();
            }
        }
    }
}
