using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Application.Business
{
    public abstract class BusinessBase<Entity, Repository>
        where Entity : EntityBase
        where Repository : RepositoryBase<Entity>
    {

        protected Repository repo;
        private ITransaction trans;

        public BusinessBase(IConnectionOptions conOpt)
        {
            this.repo = (Repository)Activator.CreateInstance(typeof(Repository), conOpt);
        }

        internal BusinessBase(ITransaction trans)
        {
            this.trans = trans;
            this.repo = (Repository)Activator.CreateInstance(typeof(Repository), trans);
        }

        public virtual bool ValidarSalvarOuAlterar(Entity entidade)
        {
            return true;
        }

        public virtual bool ValidarDeletar(Entity entidade)
        {
            return true;
        }

        public virtual void OnAntesSalvarOuAtualizar(Entity entidade)
        {
        }

        public virtual void OnDepoisSalvarOuAtualizar(Entity entidade)
        {
        }

        public virtual void OnAntesDeletar(Entity entidade)
        {
        }

        public virtual void OnDepoisDeletar(Entity entidade)
        {
        }

        protected void Abrir()
        {
            this.trans = this.repo.Abrir();
        }

        protected void Fechar()
        {
            this.repo.Fechar();
            this.trans = null;
        }

        protected void BeginTransaction()
        {
            this.repo.BeginTransaction();
        }

        protected void Commit()
        {
            this.repo.Commit();
        }

        protected void Rollback()
        {
            this.repo.Rollback();
        }

        public Entity SalvarOuAtualizar(Entity entidade)
        {
            if (entidade == null)
            {
                throw new Exception("Informe uma entidade para ser persistida");
            }

            if (!this.ValidarSalvarOuAlterar(entidade))
            {
                throw new Exception("Revise as informações para serem persistidas");
            }

            this.Abrir();
            try
            {
                this.BeginTransaction();
                try
                {
                    this.OnAntesSalvarOuAtualizar(entidade);

                    entidade = repo.SalvarOuAtualizar(entidade);

                    this.OnDepoisSalvarOuAtualizar(entidade);

                    this.Commit();
                }
                catch
                {
                    this.Rollback();
                    throw;
                }
                return entidade;
            }
            finally
            {
                this.Fechar();
            }
        }

        public bool Deletar(Entity entidade)
        {
            bool retorno = false;
            if (entidade == null)
            {
                throw new Exception("Informe uma entidade para ser excluida");
            }

            if (!this.ValidarDeletar(entidade))
            {
                throw new Exception("Revise as informações para serem deletadas");
            }

            this.Abrir();
            try
            {
                this.BeginTransaction();
                try
                {
                    this.OnAntesDeletar(entidade);

                    retorno = repo.Deletar(entidade);

                    this.OnDepoisDeletar(entidade);

                    this.Commit();
                }
                catch
                {
                    this.Rollback();
                    throw;
                }
                return retorno;
            }
            finally
            {
                this.Fechar();
            }
        }

        public Entity[] ObterTodos()
        {
            this.Abrir();
            try
            {
                return this.repo.ObterTodos();
            }
            finally
            {
                this.Fechar();
            }
            
        }

        public Entity ObterPorIDs(object[] ids)
        {
            this.Abrir();
            try
            {
                return this.repo.ObterPorIDs(ids);
            }
            finally
            {
                this.Fechar();
            }
        }

    }
}
