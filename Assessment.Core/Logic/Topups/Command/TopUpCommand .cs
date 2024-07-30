using Assessment.Core.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Logic.Topups.Command
{
    public class TopUpCommand : IRequest<TopUpResult>
    {
        public int UserId { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
    }
}
