using Assessment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Interfaces
{
    public interface ITopUpTransactionRepository
    {
        Task AddAsync(TopUpTransaction transaction);
        Task<decimal> GetTotalTopUpAmountForBeneficiaryAsync(int beneficiaryId, int month, int year);
        Task<decimal> GetTotalTopUpAmountForUserAsync(int userId, int month, int year);
        Task SaveChangesAsync();
    }
}
