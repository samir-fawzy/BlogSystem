using AutoMapper;
using BlogSystem.Api.Dto;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Interfaces;
using BlogSystem.Repository.Specification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace BlogSystem.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CommentsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpPost("add_comment")]
        public async Task<ActionResult<CommentToReturnDto>> AddComment(CommentToReturnFromUserDto model)
        {
            // Comment Writer Id
            var commentWriterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Comment Writer Name
            var commentWriterName = User.FindFirstValue(ClaimTypes.GivenName);
            var comment = new Comment()
            {
                Content = model.Content,
                PostId = model.PostId,
                CommentWriterId = commentWriterId,
                CommentWriterName = commentWriterName
            };
            
            await unitOfWork.Repository<Comment>().Add(comment);
            await unitOfWork.Complete();
            // mapping
            var commentToReturnDto = mapper.Map<Comment, CommentToReturnDto>(comment);

            return Ok(commentToReturnDto);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> DeleteComment(int id)
        {
            var comment = await unitOfWork.Repository<Comment>().GetById(id);
            unitOfWork.Repository<Comment>().Delete(comment);
            await unitOfWork.Complete();
            return true;
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<IReadOnlyList<CommentToReturnDto>>> GetAllComments(int postId)
        {
           var comments = await unitOfWork.Repository<Comment>().GetAllWithSpec(new CommentSpecification(postId));
            var commentsToReturnDto = mapper.Map<IReadOnlyList<Comment>, IReadOnlyList<CommentToReturnDto>>(comments);
            return Ok(commentsToReturnDto);
        }
    }
}
