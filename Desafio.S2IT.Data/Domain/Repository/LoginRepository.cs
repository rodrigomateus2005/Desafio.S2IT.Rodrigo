using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class LoginRepository : RepositoryBase<Login>
    {
        public LoginRepository(SqlConnectionOptions conOpt) : base(conOpt)
        {
        }

        public LoginRepository(SqlTransaction trans) : base(trans)
        {
        }

        public override bool Deletar(Login entidade)
        {
            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            return ExecutarQueryBanco("DELETE FROM Login WHERE Id = @id", idParametro) > 0;
        }

        public override Login ObterPorIDs(object[] ids)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter idParametro = new SqlParameter("id", ids[0]);
            Login entidade = null;

            sql.AppendLine("SELECT TOP 1 Id, Usuario, Senha");
            sql.AppendLine("FROM Login");
            sql.AppendLine("WHERE Id=@id");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    entidade = new Login();

                    DataRow linha = ds.Tables[0].Rows[0];

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Usuario = linha["Usuario"].ToString();
                    entidade.Senha = linha["Senha"].ToString();
                }
            }

            return entidade;
        }

        public override Login[] ObterTodos()
        {
            StringBuilder sql = new StringBuilder();
            List<Login> entidades = new List<Login>();

            sql.AppendLine("SELECT Id, Usuario, Senha");
            sql.AppendLine("FROM Login");

            using (DataSet ds = RetornarDataSet(sql.ToString()))
            {
                foreach (DataRow linha in ds.Tables[0].Rows)
                {
                    Login entidade = new Login();

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Usuario = linha["Usuario"].ToString();
                    entidade.Senha = linha["Senha"].ToString();

                    entidades.Add(entidade);
                }
            }

            return entidades.ToArray();
        }

        public override Login SalvarOuAtualizar(Login entidade)
        {
            StringBuilder sql = new StringBuilder();

            SqlParameter idParametro = new SqlParameter("id", entidade.Id);
            SqlParameter usuarioParametro = new SqlParameter("usuario", entidade.Usuario);
            SqlParameter senhaParametro = new SqlParameter("senha", entidade.Senha);

            sql.AppendLine("IF NOT EXISTS(SELECT TOP 1 1 FROM Login WHERE Id = @id)");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    INSERT INTO Login (Usuario, Senha)");
            sql.AppendLine("    VALUES(@usuario, @senha)");

            sql.AppendLine("    SELECT Scope_Identity() AS Id");

            sql.AppendLine("END");
            sql.AppendLine("ELSE");
            sql.AppendLine("BEGIN");

            sql.AppendLine("    UPDATE Login");
            sql.AppendLine("    SET Usuario = @usuario");
            sql.AppendLine("    , Senha = @senha");
            sql.AppendLine("    WHERE Id = @id");

            sql.AppendLine("    SELECT @id AS Id");

            sql.AppendLine("END");

            using (DataSet ds = RetornarDataSet(sql.ToString(), idParametro, usuarioParametro, senhaParametro))
            {
                entidade.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
            }

            return entidade;
        }

        public Login ObterPorNome(string nome)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter nomeParametro = new SqlParameter("nome", nome);
            Login entidade = null;

            sql.AppendLine("SELECT TOP 1 Id, Usuario, Senha");
            sql.AppendLine("FROM Login");
            sql.AppendLine("WHERE Usuario=@nome");

            using (DataSet ds = RetornarDataSet(sql.ToString(), nomeParametro))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    entidade = new Login();

                    DataRow linha = ds.Tables[0].Rows[0];

                    entidade.Id = int.Parse(linha["Id"].ToString());
                    entidade.Usuario = linha["Usuario"].ToString();
                    entidade.Senha = linha["Senha"].ToString();
                }
            }

            return entidade;
        }
    }
}
