using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core
{
    public class BeneficiaryDto
    {
        public BeneficiaryDto()
        {
            Nickname = "";
        }
        public int Id { get; set; }
        public string Nickname { get; set; }
    }
}
