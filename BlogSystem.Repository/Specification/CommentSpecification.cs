using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class CommentSpecification : Specification<Comment>
    {
        public CommentSpecification(int postId)
            :base(C => C.PostId == postId)
        {
            
        }
    }
}
