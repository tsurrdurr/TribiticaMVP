using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TribiticaMVP.Models;
using TribiticaMVP.Models.Abstractions;

namespace TribiticaMVP.Services
{
    public class GoalService<T> : IGoalService<T> where T : class, IGoal
    {
        private readonly DbContextOptions<TribiticaDbContext> _options;

        public GoalService(DbContextOptions<TribiticaDbContext> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<T>> GetAll(Guid ownerId)
        {
            using (var disposable = GetTableBasedOnType<T>())
            {
                return await disposable.Table.Where(x => x.OwnerId == ownerId).ToArrayAsync();
            };
        }

        public async Task<T> Add(T goal)
        {
            goal.CreatedTimeStamp = DateTimeOffset.Now;
            using (var disposable = GetTableBasedOnType(goal))
            {
                var entry = await disposable.Table.AddAsync(goal);
                return entry.Entity;
            };
        }

        public async Task<bool> Delete(Guid id)
        {
            using (var disposable = GetTableBasedOnType<T>())
            {
                var entry = await disposable.Table.FindAsync(id);
                if (entry != null)
                {
                    disposable.Table.Remove(entry);
                    return true;
                }
            };
            return false;
        }

        public async Task<T> Get(Guid id)
        {
            using (var disposable = GetTableBasedOnType<T>())
            {
                var entry = await disposable.Table.FindAsync(id);
                return entry;
            };
        }


        public async Task<T> Update(T goal)
        {
            using (var disposable = GetTableBasedOnType<T>())
            {
                var entry = await disposable.Table.FindAsync(goal.Id);
                Mapping.ProjectNonNullGoalProperties(goal, entry);
                disposable.Table.Update(entry);
                return entry;
            };
        }

        private DisposableFacade<T> GetTableBasedOnType<T>(T goal) where T : class, IGoal
        {
            object stuff;
            var dbContext = new TribiticaDbContext(_options);
            switch (goal)
            {
                case GoalDay _:
                    stuff = dbContext.GoalsDay;
                    break;
                case GoalWeek _:
                    stuff = dbContext.GoalsWeek;
                    break;
                case GoalYear _:
                    stuff = dbContext.GoalsYear;
                    break;
                default:
                    throw new ArgumentException($"Type {typeof(T).ToString()} was not expected.");
            }
            return new DisposableFacade<T>(dbContext, (DbSet<T>)stuff);
        }

        private DisposableFacade<T> GetTableBasedOnType<T>() where T : class, IGoal
        {
            object stuff;
            var dbContext = new TribiticaDbContext(_options);
            if (typeof(T) == typeof(GoalDay))
            {
                stuff = dbContext.GoalsDay;
            }
            else if (typeof(T) == typeof(GoalWeek))
            {
                stuff = dbContext.GoalsYear;
            }
            else if (typeof(T) == typeof(GoalYear))
            {
                stuff = dbContext.GoalsYear;
            }
            else
            {
                throw new ArgumentException($"Type {typeof(T).ToString()} was not expected.");
            }
            return new DisposableFacade<T>(dbContext, (DbSet<T>)stuff);
        }

        private class DisposableFacade<T1> : IDisposable where T1: class, IGoal
        {
            public DbSet<T1> Table;
            private TribiticaDbContext _dbContext;

            public DisposableFacade(TribiticaDbContext dbContext, DbSet<T1> dbSet)
            {
                _dbContext = dbContext;
                Table = dbSet;
            }

            public async void Dispose()
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.Dispose();
            }
        }
    }
}
