using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Application.Business
{
    public partial class JogoBusiness : BusinessBase<Jogo, JogoRepository>
    {
        public JogoBusiness(IConnectionOptions conOpt) : base(conOpt)
        {
        }

        public Jogo[] ObterTodosDisponiveis()
        {
            this.Abrir();
            try
            {
                return this.repo.ObterTodosDisponiveis();
            }
            finally
            {
                this.Fechar();
            }
        }

        public Jogo[] ObterTodosEmprestados()
        {
            this.Abrir();
            try
            {
                return this.repo.ObterTodosEmprestados();
            }
            finally
            {
                this.Fechar();
            }
        }
    }
}
