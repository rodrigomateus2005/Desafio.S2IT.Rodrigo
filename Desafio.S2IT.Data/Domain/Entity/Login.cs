using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Entity
{
    public class Login : EntityBase
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
