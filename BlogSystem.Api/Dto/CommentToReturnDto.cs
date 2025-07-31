using BlogSystem.Core.Entities;

namespace BlogSystem.Api.Dto
{
    public class CommentToReturnDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public string CommentWriterId { get; set; }
        public string CommentWriterName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
