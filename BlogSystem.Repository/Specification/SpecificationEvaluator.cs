using BlogSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> table,ISpecification<T> spec)
        {
            var query = table;

            if (spec.Criteria != null) query = query.Where(spec.Criteria);

            // Tail of Query
            if (spec.IsBagination == true)
            {
                query = query.Skip(spec.Skip).Take(spec.Take); 
            }
            return query;
        }
    }
}
