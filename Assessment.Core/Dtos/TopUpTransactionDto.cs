using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core
{
    public class TopUpTransactionDto
    {
        public int Id { get; set; }
   
        public int UserId { get; set; }

        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Charge { get; set; }
    }

}
