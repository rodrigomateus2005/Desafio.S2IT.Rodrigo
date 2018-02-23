using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Entity
{
    public class Jogo : EntityBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string EmprestadoPara { get; set; }
        public int Dias { get; set; }
    }
}
