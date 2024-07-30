using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Domain.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string NickName { get; set; }
        public decimal MonthlyTopUpAmount { get; set; }
        public User User { get; set; } 
        public int UserId { get; set; }
    }
}
