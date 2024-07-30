using Assessment.Core;
using Assessment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Interfaces
{
    public interface IBeneficiaryRepository
    {
        Task<Beneficiary> GetByIdAsync(int id);
        Task AddAsync(Beneficiary beneficiary);
        Task<List<Beneficiary>> GetByUserIdAsync(int userId);
        Task SaveChangesAsync();
    }
}
