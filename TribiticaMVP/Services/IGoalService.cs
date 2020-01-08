using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TribiticaMVP.Models;

namespace TribiticaMVP.Services
{
    public interface IGoalService<T> where T : class, IGoal
    {
        Task<IEnumerable<T>> GetAll(Guid ownerId);
        
        Task<T> Get(Guid id);

        Task<T> Add(T goal);

        Task<T> Update(T goal);

        Task<bool> Delete(Guid id);
    }
}
