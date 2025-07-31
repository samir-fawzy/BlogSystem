using BlogSystem.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class Comment
    {
        public int Id { get;set; }  
        public string Content { get;set; }
        public int PostId { get;set; }
        public Post post { get;set; }
        public string CommentWriterId { get;set; }
        public string CommentWriterName { get; set; } 
        public DateTime CreatedAt { get;set; } = DateTime.Now;
    }
}
