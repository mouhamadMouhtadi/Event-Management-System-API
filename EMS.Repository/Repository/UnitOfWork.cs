using EMS.Core.Entities;
using EMS.Core.Repository.Interfaces;
using EMS.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()=> await _context.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.ContainsKey(typeName))
                return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            else
            {
                var repositoryInstance = new GenericRepository<TEntity, TKey>(_context);
                _repositories[typeName]= repositoryInstance;
                return repositoryInstance;
            }
        }
    }
}
