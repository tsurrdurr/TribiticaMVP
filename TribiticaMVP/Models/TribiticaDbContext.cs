using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Models.Abstractions;

namespace TribiticaMVP.Models
{
    public class TribiticaDbContext : DbContext
    {
        public static string DbFileName = "TribiticaDefault.db";

        public DbSet<TribiticaAccount> Accounts { get; set; }

        public DbSet<GoalYear> GoalsYear { get; set; }

        public DbSet<GoalWeek> GoalsWeek { get; set; }

        public DbSet<GoalDay> GoalsDay { get; set; }


        public TribiticaDbContext(DbContextOptions<TribiticaDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Filename={DbFileName}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoalYear>(
                year => year
                    .HasOne(goal => goal.Owner)
                    .WithMany(acc => acc.GoalsYear)
                    .HasForeignKey(goal => goal.OwnerId));

            modelBuilder.Entity<GoalYear>(
               year => year
                   .HasOne(goal => goal.Owner)
                   .WithMany(acc => acc.GoalsYear)
                   .HasForeignKey(goal => goal.OwnerId));

            modelBuilder.Entity<GoalWeek>(
                week => week
                    .HasOne(goal => goal.Owner)
                    .WithMany(acc => acc.GoalsWeek)
                    .HasForeignKey(goal => goal.OwnerId));

            modelBuilder.Entity<GoalDay>(
                day => day
                    .HasOne(goal => goal.Owner)
                    .WithMany(acc => acc.GoalsDay)
                    .HasForeignKey(goal => goal.OwnerId));
        }
    }
}
