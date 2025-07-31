namespace BlogSystem.Api.Dto
{
    public class PostDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? PictureUrl { get; set; }
    }
}
