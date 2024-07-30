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
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly ApplicationDbContext _context;

        public BeneficiaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Beneficiary> GetByIdAsync(int id)
        {
            return await _context.Beneficiaries.FindAsync(id);
        }

        public async Task AddAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
        }

        public async Task<List<Beneficiary>> GetByUserIdAsync(int userId)
        {
            return await _context.Beneficiaries.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
