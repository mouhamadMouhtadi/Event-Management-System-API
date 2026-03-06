using EMS.Core.Entities;
using EMS.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Repository.Interfaces
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task <IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec);
        Task<TEntity> GetByIdWithSpecAsync(ISpecification<TEntity, TKey> spec);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entity);
        Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec);

        void Update(TEntity entity);
        void Delete(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
