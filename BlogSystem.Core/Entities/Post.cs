using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class Post 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime DateOfCreated { get; set; } = DateTime.Now;
        public PostAuthor PostAuthor { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
