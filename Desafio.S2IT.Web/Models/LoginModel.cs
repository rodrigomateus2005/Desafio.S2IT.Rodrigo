﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.S2IT.Web.Models
{
    public class LoginModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool LoginIncorreto { get; set; }
    }
}
