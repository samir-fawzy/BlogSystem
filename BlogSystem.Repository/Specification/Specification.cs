using BlogSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class Specification<T> : ISpecification<T> where T : class
    {
        public int Skip { get ; set ; }
        public int Take { get; set; }
        public bool IsBagination { get; set; }
        public Expression<Func<T, bool>> Criteria { get; set; }

        public Specification()
        {
            
        }

        public Specification(Expression<Func<T, bool>> Criteria)
        {
            this.Criteria = Criteria;
        }
        protected void UseBagination(int skip,int take)
        {
            Skip = skip;
            Take = take;
            IsBagination = true;
        }

    }
}
