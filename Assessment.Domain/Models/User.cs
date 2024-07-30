using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool IsVerified { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();
        public ICollection<TopUpTransaction> TopUpTransactions { get; set; }

    }
}
