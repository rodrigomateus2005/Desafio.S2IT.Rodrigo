using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Models
{
    public class CadastroEmprestimoModel
    {
        public int Id { get; set; }
        public int Amigo { get; set; }
        public int Jogo { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }

        public Amigo[] Amigos { get; set; }
        public Jogo[] JogosDisponiveis { get; set; }
    }
}
