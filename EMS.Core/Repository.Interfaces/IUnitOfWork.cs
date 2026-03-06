using EMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        //IGenericRepository<User, Guid> Users { get; }
        //IGenericRepository<Category, int> Categories { get; }
        //IGenericRepository<Event, int> Events { get; }
        /// <summary>
        /// /////////////////////instead of these we can use generic repository pattern
        /// </summary>
        /// 

        IGenericRepository<TEntity,TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<int> CompleteAsync();
    }
}
