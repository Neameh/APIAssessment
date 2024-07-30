using Assessment.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assessment.Infrastructure.Data
{


    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<TopUpTransaction> TopUpTransactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopUpTransaction>().HasOne(x=>x.User).WithMany(e=>e.TopUpTransactions).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
            
        }
     
    }

}
