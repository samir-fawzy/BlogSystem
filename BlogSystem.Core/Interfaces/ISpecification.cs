using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Interfaces
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T,bool>> Criteria { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        bool IsBagination { get; set; }
    }
}
