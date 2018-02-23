using Desafio.S2IT.Data.Application.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Repository
{
    public class SqlConnectionOptions : IConnectionOptions
    {

        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

    }
}
