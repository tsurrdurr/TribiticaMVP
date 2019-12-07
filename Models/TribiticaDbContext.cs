using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TribiticaMVP.Models
{
    public class TribiticaDbContext : DbContext
    {
        public DbSet<TribiticaAccount> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Filename=TribiticaTest.db");
            }
        }
    }
}
