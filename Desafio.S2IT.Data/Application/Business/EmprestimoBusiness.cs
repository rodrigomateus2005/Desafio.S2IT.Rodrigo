using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Application.Business
{
    public class EmprestimoBusiness : BusinessBase<Emprestimo, EmprestimoRepository>
    {
        public EmprestimoBusiness(IConnectionOptions conOpt) : base(conOpt)
        {
        }

        public void DevolverJogo(int idJogo)
        {
            this.Abrir();
            try
            {
                this.repo.DevolverJogo(idJogo);
            }
            finally
            {
                this.Fechar();
            }
        }
    }
}
