using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class EmprestimoRepository : RepositoryBase<Emprestimo>
    {
        public EmprestimoRepository(SqlConnectionOptions conOpt) : base(conOpt)
        {
        }

        public EmprestimoRepository(SqlTransaction trans) : base(trans)
        {
        }

        public override bool Deletar(Emprestimo entidade)
        {
            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            return ExecutarQueryBanco("DELETE FROM Emprestimo WHERE Id = @id", idParametro) > 0;
        }

        public override Emprestimo ObterPorIDs(object[] ids)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter idParametro = new SqlParameter("id", ids[0]);
            Emprestimo entidade = null;

            sql.AppendLine("SELECT TOP 1 Id, Amigo, Jogo, DataEmprestimo, DataDevolucao");
            sql.AppendLine("FROM Emprestimo");
            sql.AppendLine("WHERE Id=@id");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    entidade = new Emprestimo();

                    DataRow linha = ds.Tables[0].Rows[0];

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Amigo = (int)linha["Amigo"];
                    entidade.Jogo = (int)linha["Jogo"];
                    entidade.DataEmprestimo = (DateTime)linha["DataEmprestimo"];
                    entidade.DataDevolucao = (DateTime?)(linha["DataDevolucao"] == DBNull.Value ? null : linha["DataDevolucao"]);
                }
            }

            return entidade;
        }

        public override Emprestimo[] ObterTodos()
        {
            StringBuilder sql = new StringBuilder();
            List<Emprestimo> entidades = new List<Emprestimo>();

            sql.AppendLine("SELECT Id, Amigo, Jogo, DataEmprestimo, DataDevolucao");
            sql.AppendLine("FROM Emprestimo");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Emprestimo entidade = new Emprestimo();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Amigo = (int)linha["Amigo"];
                    entidade.Jogo = (int)linha["Jogo"];
                    entidade.DataEmprestimo = (DateTime)linha["DataEmprestimo"];
                    entidade.DataDevolucao = (DateTime?) (linha["DataDevolucao"] == DBNull.Value ? null : linha["DataDevolucao"]);
                    

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public override Emprestimo SalvarOuAtualizar(Emprestimo entidade)
        {
            StringBuilder sql = new StringBuilder();

            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            SqlParameter amigoParametro = new SqlParameter("amigo", entidade.Amigo);
            SqlParameter jogoParametro = new SqlParameter("jogo", entidade.Jogo);
            SqlParameter dataEmprestimoParametro = new SqlParameter("dataEmprestimo", entidade.DataEmprestimo);
            SqlParameter dataDevolucaoParametro = new SqlParameter("dataDevolucao", (object)entidade.DataDevolucao ?? DBNull.Value);

            sql.AppendLine("IF NOT EXISTS(SELECT TOP 1 1 FROM Emprestimo WHERE Id = @id)");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    INSERT INTO Emprestimo (Amigo, Jogo, DataEmprestimo, DataDevolucao)");
            sql.AppendLine("    VALUES(@amigo, @jogo, @dataEmprestimo, @dataDevolucao)");

            sql.AppendLine("    SELECT Scope_Identity() AS Id");

            sql.AppendLine("END");
            sql.AppendLine("ELSE");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    UPDATE Emprestimo");
            sql.AppendLine("    SET Amigo = @amigo");
            sql.AppendLine("    , Jogo = @jogo");
            sql.AppendLine("    , DataEmprestimo = @dataEmprestimo");
            sql.AppendLine("    , DataDevolucao = @dataDevolucao");
            sql.AppendLine("    WHERE Id = @id");

            sql.AppendLine("    SELECT @id AS Id");

            sql.AppendLine("END");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro, amigoParametro, jogoParametro, dataEmprestimoParametro, dataDevolucaoParametro))
            {
                entidade.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
            }

            return entidade;
        }

        public void DevolverJogo(int idJogo)
        {
            StringBuilder sql = new StringBuilder();

            SqlParameter idJogoParametro = new SqlParameter("idJogo", idJogo);

            sql.AppendLine("UPDATE Emprestimo SET DataDevolucao = GETDATE() WHERE Jogo = @idJogo");

            ExecutarQueryBanco(sql.ToString(), idJogoParametro);
        }
    }
}
