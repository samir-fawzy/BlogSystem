using BlogSystem.Repository;
using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class PostSpecification : Specification<Post>
    {
        public PostSpecification(PostSpecParams parameters)    
            :base(P => 
            (string.IsNullOrEmpty(parameters.AuthorName)|| P.PostAuthor.Name == parameters.AuthorName) &&
            (parameters.DateSearch == null || (P.DateOfCreated.Date.Year == parameters.DateSearch.Value.Year &&
                                             P.DateOfCreated.Date.Month == parameters.DateSearch.Value.Month &&
                                             P.DateOfCreated.Date.Day == parameters.DateSearch.Value.Day))
            )
        {
            UseBagination((parameters.PageIndex - 1)* parameters.PageSize, parameters.PageSize);   
        }
    }
}
