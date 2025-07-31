using AutoMapper;
using BlogSystem.Api.Dto;
using BlogSystem.Core.Entities;

namespace BlogSystem.Api.Helper
{
    public class PostPicturUrlResolver : IValueResolver<Post, PostToReturnDto, string>
    {
        private readonly IConfiguration configuration;

        public PostPicturUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Post source, PostToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["BaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
