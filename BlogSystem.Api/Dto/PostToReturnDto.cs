using BlogSystem.Core.Entities;

namespace BlogSystem.Api.Dto
{
    public class PostToReturnDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

    }
}
