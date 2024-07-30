using Assessment.Core.Interfaces;
using Assessment.Domain.Models;
using Assessment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assessment.Infrastructure.Repositories
{
    public class TopUpTransactionRepository : ITopUpTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TopUpTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TopUpTransaction transaction)
        {
            _context.TopUpTransactions.Add(transaction);
        }

        public async Task<decimal> GetTotalTopUpAmountForBeneficiaryAsync(int beneficiaryId, int month, int year)
        {
            return await _context.TopUpTransactions
                .Where(t => t.BeneficiaryId == beneficiaryId && t.Date.Month == month && t.Date.Year == year)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalTopUpAmountForUserAsync(int userId, int month, int year)
        {
            return await _context.TopUpTransactions
                .Where(t => t.UserId == userId && t.Date.Month == month && t.Date.Year == year)
                .SumAsync(t => t.Amount);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}


