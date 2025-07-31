using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Specification
{
    public class LikeSpecification : Specification<Like>
    {
        public LikeSpecification(int postId)
            :base(L => L.PostId == postId)
        {
            
        }
    }
}
