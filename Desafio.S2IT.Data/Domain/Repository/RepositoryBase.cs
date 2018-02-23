using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public abstract class RepositoryBase<Entity> : RepositoryBase where Entity : EntityBase
    {

        private SqlConnectionOptions conOpt;
        private SqlTransaction trans;

        private bool podeCommitar;

        public RepositoryBase(SqlConnectionOptions conOpt)
        {
            this.conOpt = conOpt;
            this.VerificarPodeCommitar();
        }

        public RepositoryBase(SqlTransaction trans)
        {
            this.trans = trans;
            this.VerificarPodeCommitar();
        }

        private void VerificarPodeCommitar()
        {
            this.podeCommitar = true;
            if (this.trans != null)
            {
                if (this.trans.Transaction != null)
                {
                    this.podeCommitar = false;
                }
            }
        }

        public SqlTransaction Abrir()
        {
            if (this.trans == null)
            {
                this.trans = new SqlTransaction();
            }

            if (this.trans.Connection == null)
            {
                this.trans.Connection = RetornarConexaoBanco();
                this.trans.Connection.Open();
            }

            return this.trans;
        }

        public void Fechar()
        {
            if (this.podeCommitar)
            {
                this.trans.Connection.Close();
                this.trans.Connection.Dispose();
                this.trans.Connection = null;
            }
        }

        public SqlTransaction BeginTransaction()
        {
            if (this.trans.Transaction == null)
            {
                this.trans.Transaction = this.trans.Connection.BeginTransaction();
            }

            return this.trans;
        }

        public SqlTransaction Commit()
        {
            if (this.podeCommitar)
            {
                this.trans.Transaction.Commit();
                this.trans.Transaction = null;
            }

            return this.trans;
        }

        public SqlTransaction Rollback()
        {
            if (this.trans.Transaction != null)
            {
                this.trans.Transaction.Rollback();
                this.trans.Transaction = null;
            }

            return this.trans;
        }

        public abstract Entity SalvarOuAtualizar(Entity entidade);

        public abstract bool Deletar(Entity entidade);

        public abstract Entity[] ObterTodos();

        public abstract Entity ObterPorIDs(object[] ids);

        protected int ExecutarQueryBanco(string query, params SqlParameter[] parametros)
        {
            int registrosAfetados;
            SqlConnection con = this.trans.Connection;
            using (SqlCommand cmd = new SqlCommand(query, con, this.trans.Transaction))
            {
                if (parametros != null)
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }
                }
                registrosAfetados = cmd.ExecuteNonQuery();
            }
            return registrosAfetados;
        }

        protected DataSet RetornarDataSet(string query, params SqlParameter[] parametros)
        {
            DataSet ds = new DataSet();
            SqlConnection con = this.trans.Connection;
            using (SqlCommand cmd = new SqlCommand(query, con, this.trans.Transaction))
            {
                if (parametros != null)
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }
                }
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            return ds;
        }

        private SqlConnection RetornarConexaoBanco()
        {
            SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();

            if (string.IsNullOrWhiteSpace(conOpt.InitialCatalog))
            {
                conOpt.InitialCatalog = "";
            }

            if (string.IsNullOrWhiteSpace(conOpt.DataSource))
            {
                conOpt.DataSource = "";
            }

            if (string.IsNullOrWhiteSpace(conOpt.UserID))
            {
                conOpt.UserID = "";
            }

            if (string.IsNullOrWhiteSpace(conOpt.Password))
            {
                conOpt.Password = "";
            }

            if (string.IsNullOrWhiteSpace(conOpt.DataSource) && string.IsNullOrWhiteSpace(conOpt.UserID))
            {
                return null;
            }

            conStr.InitialCatalog = conOpt.InitialCatalog;
            conStr.DataSource = conOpt.DataSource;
            conStr.UserID = conOpt.UserID;
            conStr.Password = conOpt.Password;

            SqlConnection con = new SqlConnection(conStr.ToString());

            return con;
        }



    }

    public abstract class RepositoryBase
    {
        public static bool IsValidOptions(SqlConnectionOptions conOpt)
        {
            try
            {
                SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();

                conStr.InitialCatalog = conOpt.InitialCatalog;
                conStr.DataSource = conOpt.DataSource;
                conStr.UserID = conOpt.UserID;
                conStr.Password = conOpt.Password;

                using (SqlConnection con = new SqlConnection(conStr.ToString()))
                {
                    con.Open();

                    con.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValidServer(SqlConnectionOptions conOpt)
        {
            try
            {
                SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();

                conStr.DataSource = conOpt.DataSource;
                conStr.UserID = conOpt.UserID;
                conStr.Password = conOpt.Password;

                using (SqlConnection con = new SqlConnection(conStr.ToString()))
                {
                    con.Open();

                    con.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CriarBanco(SqlConnectionOptions conOpt)
        {
            SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();

            conStr.DataSource = conOpt.DataSource;
            conStr.UserID = conOpt.UserID;
            conStr.Password = conOpt.Password;

            using (SqlConnection con = new SqlConnection(conStr.ToString()))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand("CREATE DATABASE[" + conOpt.InitialCatalog + "]", con))
                {
                    com.ExecuteNonQuery();
                }

                using (SqlCommand com = new SqlCommand(RetornarScriptCriacaoBanco(conOpt.InitialCatalog), con))
                {
                    com.ExecuteNonQuery();
                }

                con.Close();
            }

            return true;
        }

        private static string RetornarScriptCriacaoBanco(string nomeBanco)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("USE ["+ nomeBanco +"]");
            sql.AppendLine("");
            sql.AppendLine("CREATE TABLE Amigo (");
            sql.AppendLine("	Id INT PRIMARY KEY IDENTITY");
            sql.AppendLine("	, Nome VARCHAR(50) NOT NULL");
            sql.AppendLine(")");
            sql.AppendLine("");
            sql.AppendLine("CREATE TABLE Jogo (");
            sql.AppendLine("	Id INT PRIMARY KEY IDENTITY");
            sql.AppendLine("	, Nome VARCHAR(50) NOT NULL");
            sql.AppendLine(")");
            sql.AppendLine("");
            sql.AppendLine("CREATE TABLE Emprestimo (");
            sql.AppendLine("	Id INT PRIMARY KEY IDENTITY");
            sql.AppendLine("	, Amigo INT NOT NULL");
            sql.AppendLine("	, Jogo INT NOT NULL");
            sql.AppendLine("	, DataEmprestimo DATE NOT NULL DEFAULT GETDATE()");
            sql.AppendLine("	, DataDevolucao DATE");
            sql.AppendLine("	, FOREIGN KEY (Amigo) REFERENCES Amigo(Id)");
            sql.AppendLine("	, FOREIGN KEY (Jogo) REFERENCES Jogo(Id)");
            sql.AppendLine(")");
            sql.AppendLine("");
            sql.AppendLine("CREATE TABLE [Login] (");
            sql.AppendLine("	Id INT PRIMARY KEY IDENTITY");
            sql.AppendLine("	, Usuario VARCHAR(50) NOT NULL");
            sql.AppendLine("	, Senha VARCHAR(255) NOT NULL");
            sql.AppendLine(")");
            sql.AppendLine("");
            sql.AppendLine("INSERT INTO [Login] (Usuario, Senha)");
            sql.AppendLine("VALUES ('ADMIN', 'wdOHVNwOT6Rw77LL4GQ2Yg==')");

            return sql.ToString();
        }
    }
}
