using BlogSystem.Core.Entities;

namespace BlogSystem.Api.Dto
{
    public class CommentToReturnFromUserDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
