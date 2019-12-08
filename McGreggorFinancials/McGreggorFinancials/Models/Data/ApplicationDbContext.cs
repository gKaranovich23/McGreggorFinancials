using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Targets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<IncomeCategory> IncomeCategories { get; set; }

        public DbSet<IncomeEntry> IncomeEntries { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountType> AccountTypes { get; set; }

        public DbSet<TargetType> TargetTypes { get; set; }

        public DbSet<TargetAmount> TargetAmounts { get; set; }
    }
}
