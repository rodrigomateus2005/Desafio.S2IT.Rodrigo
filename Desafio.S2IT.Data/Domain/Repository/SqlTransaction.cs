using Desafio.S2IT.Data.Application.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class SqlTransaction : ITransaction
    {

        public SqlConnection Connection { get; set; }
        public System.Data.SqlClient.SqlTransaction Transaction { get; set; }

    }
}
