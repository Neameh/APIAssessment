using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Domain.Models
{
    public class TopUpTransaction
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Beneficiary Beneficiary { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Charge { get; set; }
    }

}
