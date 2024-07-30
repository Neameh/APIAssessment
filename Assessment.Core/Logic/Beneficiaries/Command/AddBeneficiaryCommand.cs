using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Logic.Beneficiaries.Command
{

    public class AddBeneficiaryCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public string? Nickname { get; set; }
    }

}
