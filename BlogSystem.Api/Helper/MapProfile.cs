using AutoMapper;
using BlogSystem.Api.Dto;
using BlogSystem.Core.Entities;

namespace BlogSystem.Api.Helper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Post, PostToReturnDto>()
                .ForMember(D => D.PostId, M => M.MapFrom(P => P.Id))
                .ForMember(D => D.AuthorId,M => M.MapFrom(P => P.PostAuthor.Id))
                .ForMember(D=>D.AuthorName,M =>M.MapFrom(P => P.PostAuthor.Name))
                .ForMember(D => D.PictureUrl,M => M.MapFrom<PostPicturUrlResolver>())
                .ReverseMap();

            CreateMap<Comment, CommentToReturnDto>();
            CreateMap<Like, LikeToReturnDto>();

        }
    }
}
