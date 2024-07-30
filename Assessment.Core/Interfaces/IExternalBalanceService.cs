using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Interfaces
{

    public interface IExternalBalanceService
    {
        Task<decimal> GetBalanceAsync(int userId);
        Task<bool> DebitBalanceAsync(int userId, decimal amount);
    }
}

