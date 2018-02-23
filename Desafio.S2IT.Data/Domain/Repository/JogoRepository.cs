using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class JogoRepository : RepositoryBase<Jogo>
    {
        public JogoRepository(SqlConnectionOptions conOpt) : base(conOpt)
        {
        }

        public JogoRepository(SqlTransaction trans) : base(trans)
        {
        }

        public override bool Deletar(Jogo entidade)
        {
            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            return ExecutarQueryBanco("DELETE FROM Jogo WHERE Id = @id", idParametro) > 0;
        }

        public override Jogo ObterPorIDs(object[] ids)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter idParametro = new SqlParameter("id", ids[0]);
            Jogo entidade = null;

            sql.AppendLine("SELECT TOP 1 Id, Nome");
            sql.AppendLine("FROM Jogo");
            sql.AppendLine("WHERE Id=@id");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    entidade = new Jogo();

                    DataRow linha = ds.Tables[0].Rows[0];

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();
                }
            }

            return entidade;
        }

        public override Jogo[] ObterTodos()
        {
            StringBuilder sql = new StringBuilder();
            List<Jogo> entidades = new List<Jogo>();

            sql.AppendLine("SELECT Id, Nome");
            sql.AppendLine("FROM Jogo");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Jogo entidade = new Jogo();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public Jogo[] ObterTodosDisponiveis()
        {
            StringBuilder sql = new StringBuilder();
            List<Jogo> entidades = new List<Jogo>();

            sql.AppendLine("SELECT J.Id, J.Nome");
            sql.AppendLine("FROM Jogo J");
            sql.AppendLine("WHERE J.Id NOT IN (SELECT E.Jogo FROM Emprestimo E WHERE E.DataDevolucao IS NULL)");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Jogo entidade = new Jogo();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public Jogo[] ObterTodosEmprestados()
        {
            StringBuilder sql = new StringBuilder();
            List<Jogo> entidades = new List<Jogo>();

            sql.AppendLine("SELECT J.Id, J.Nome, A.Nome AS EmprestadoPara, E.DataEmprestimo");
            sql.AppendLine("FROM Jogo J");
            sql.AppendLine("INNER JOIN Emprestimo E ON E.Jogo = J.Id AND E.DataDevolucao IS NULL");
            sql.AppendLine("INNER JOIN Amigo A ON A.Id = E.Amigo");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Jogo entidade = new Jogo();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();

                    entidade.EmprestadoPara = linha["EmprestadoPara"].ToString();
                    entidade.Dias = DateTime.Today.Subtract((DateTime)linha["DataEmprestimo"]).Days;

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public override Jogo SalvarOuAtualizar(Jogo entidade)
        {
            StringBuilder sql = new StringBuilder();

            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            SqlParameter nomeParametro = new SqlParameter("nome", entidade.Nome);

            sql.AppendLine("IF NOT EXISTS(SELECT TOP 1 1 FROM Jogo WHERE Id = @id)");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    INSERT INTO Jogo (Nome)");
            sql.AppendLine("    VALUES(@nome)");

            sql.AppendLine("    SELECT Scope_Identity() AS Id");

            sql.AppendLine("END");
            sql.AppendLine("ELSE");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    UPDATE Jogo");
            sql.AppendLine("    SET Nome = @nome");
            sql.AppendLine("    WHERE Id = @id");

            sql.AppendLine("    SELECT @id AS Id");

            sql.AppendLine("END");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro, nomeParametro))
            {
                entidade.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
            }

            return entidade;
        }
    }
}
