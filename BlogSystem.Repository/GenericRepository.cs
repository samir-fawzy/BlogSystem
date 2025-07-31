using BlogSystem.Core.Interfaces;
using BlogSystem.Repository.Data;
using BlogSystem.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BlogDbContext context;
        public GenericRepository(BlogDbContext context)
        {
            this.context = context;
        }
        public async Task Add(T entity)
            => await context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => context.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            var results = await ApplySpecification(context.Set<T>(), spec).ToListAsync();
            return results;
        }

        public async Task<T> GetById(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        => context.Set<T>().Update(entity);

        private IQueryable<T> ApplySpecification(IQueryable<T> table,ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(table, spec);
        }
    }
}
