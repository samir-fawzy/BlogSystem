using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetById(int id);
    }
}
