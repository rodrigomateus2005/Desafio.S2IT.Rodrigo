using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class AmigoRepository : RepositoryBase<Amigo>
    {
        public AmigoRepository(SqlConnectionOptions conOpt) : base(conOpt)
        {
        }

        public AmigoRepository(SqlTransaction trans) : base(trans)
        {
        }

        public override bool Deletar(Amigo entidade)
        {
            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            return ExecutarQueryBanco("DELETE FROM Amigo WHERE Id = @id", idParametro) > 0;
        }

        public override Amigo ObterPorIDs(object[] ids)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter idParametro = new SqlParameter("id", ids[0]);
            Amigo entidade = null;

            sql.AppendLine("SELECT TOP 1 Id, Nome");
            sql.AppendLine("FROM Amigo");
            sql.AppendLine("WHERE Id=@id");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    entidade = new Amigo();

                    DataRow linha = ds.Tables[0].Rows[0];

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();
                }
            }

            return entidade;
        }

        public override Amigo[] ObterTodos()
        {
            StringBuilder sql = new StringBuilder();
            List<Amigo> entidades = new List<Amigo>();

            sql.AppendLine("SELECT Id, Nome");
            sql.AppendLine("FROM Amigo");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Amigo entidade = new Amigo();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Nome = linha["Nome"].ToString();

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public override Amigo SalvarOuAtualizar(Amigo entidade)
        {
            StringBuilder sql = new StringBuilder();

            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            SqlParameter nomeParametro = new SqlParameter("nome", entidade.Nome);

            sql.AppendLine("IF NOT EXISTS(SELECT TOP 1 1 FROM Amigo WHERE Id = @id)");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    INSERT INTO Amigo (Nome)");
            sql.AppendLine("    VALUES(@nome)");

            sql.AppendLine("    SELECT Scope_Identity() AS Id");

            sql.AppendLine("END");
            sql.AppendLine("ELSE");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    UPDATE Amigo");
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
