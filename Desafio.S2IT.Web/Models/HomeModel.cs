using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Models
{
    public class HomeModel
    {
        public Jogo[] JogosDisponiveis { get; set; }
        public Jogo[] JogosEmprestados { get; set; }
    }
}
