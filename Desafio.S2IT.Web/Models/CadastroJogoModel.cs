using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Models
{
    public class CadastroJogoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Jogo[] Entidades { get; set; }
    }
}
