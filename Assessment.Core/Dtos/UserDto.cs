using Assessment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
            Name = "";
            Beneficiaries = new List<BeneficiaryDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVerified { get; set; }
        public decimal Balance { get; set; }
        public ICollection<BeneficiaryDto> Beneficiaries { get; set; }
    }
    
}
