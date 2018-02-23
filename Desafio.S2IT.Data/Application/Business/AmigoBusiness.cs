using Desafio.S2IT.Data.Domain.Entity;
using Desafio.S2IT.Data.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.S2IT.Data.Application.Business
{
    public class AmigoBusiness : BusinessBase<Amigo, AmigoRepository>
    {
        public AmigoBusiness(IConnectionOptions conOpt) : base(conOpt)
        {
        }
    }
}
