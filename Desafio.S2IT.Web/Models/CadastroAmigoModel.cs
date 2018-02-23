using Desafio.S2IT.Data.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Models
{
    public class CadastroAmigoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Amigo[] Entidades { get; set; }
    }
}
