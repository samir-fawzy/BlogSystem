using BlogSystem.Core.Entities;
using BlogSystem.Core.Interfaces;
using BlogSystem.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repository;
        private readonly BlogDbContext context;

        public UnitOfWork(BlogDbContext context)
        {
            _repository = new Hashtable();
            this.context = context;
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if(!_repository.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(context);
                _repository.Add(type, repository);
            }
            return _repository[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }
    }
}
