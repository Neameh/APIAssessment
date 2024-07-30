using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Infrastructure.Models
{
    public class BalanceResponse
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

