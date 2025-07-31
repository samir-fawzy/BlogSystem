using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class PostSpecificationWithFilterOnly : Specification<Post>
    {
        public PostSpecificationWithFilterOnly(PostSpecParams parameters)
            : base(P =>
            (string.IsNullOrEmpty(parameters.AuthorName) || (P.PostAuthor.Name == parameters.AuthorName) &&
            (parameters.DateSearch == null || P.DateOfCreated.Date.Year == parameters.DateSearch.Value.Year &&
                                             P.DateOfCreated.Date.Month == parameters.DateSearch.Value.Month &&
                                             P.DateOfCreated.Date.Day == parameters.DateSearch.Value.Day))
            )
        {

        }
    }
}
