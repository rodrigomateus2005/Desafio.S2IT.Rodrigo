using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Domain.Entity
{
    public class Emprestimo : EntityBase
    {
        public int Id { get; set; }
        public int Amigo { get; set; }
        public int Jogo { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
    }
}
