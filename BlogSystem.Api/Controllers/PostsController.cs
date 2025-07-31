using AutoMapper;
using BlogSystem.Api.Dto;
using BlogSystem.Api.Error;
using BlogSystem.Api.Helper;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Interfaces;
using BlogSystem.Repository;
using BlogSystem.Repository.Specification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogSystem.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "author")]
    public class PostsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public PostsController(IUnitOfWork unitOfWork,RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost("create_post")]
        public async Task<ActionResult<PostToReturnDto>> CreatePost(PostDto model)
        {
            
            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                PictureUrl = model.PictureUrl,
                
            };
            post.PostAuthor = new PostAuthor()
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = User.FindFirstValue(ClaimTypes.GivenName)
            };

            await unitOfWork.Repository<Post>().Add(post);
            await unitOfWork.Complete();

            var postToReturnDto = mapper.Map<Post,PostToReturnDto>(post);

            return (postToReturnDto);
        }

        [HttpPost("edit_post")]
        public async Task<ActionResult<PostToReturnDto>> EditPost(PostEditedDto model)
        {
            var post = await unitOfWork.Repository<Post>().GetById(model.Id);
 
            if (post == null) return BadRequest(new ApiErrorResponse(400));
            // check if author who create post
            if (!CheckAuthorWhoCreatePost(post)) return Unauthorized(new ApiErrorResponse(401));
            // **************************** //
            post.Title = model.Title;
            post.Content = model.Content;
            post.PictureUrl = model.PictureUrl;
            unitOfWork.Repository<Post>().Update(post);
            await unitOfWork.Complete();

            var postToReturnDto = mapper.Map<Post, PostToReturnDto>(post);

            return (postToReturnDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "author,admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePost(int id)
        {
            var post = await unitOfWork.Repository<Post>().GetById(id);
            if (post == null) return BadRequest(new ApiErrorResponse(400));

            if(!await CheckIfUserIsAdmin())
                if (!CheckAuthorWhoCreatePost(post)) return Unauthorized(new ApiErrorResponse(401));

            unitOfWork.Repository<Post>().Delete(post);
            await unitOfWork.Complete();
            return true;
        }

        [HttpGet("get_posts")]
        public async Task<ActionResult<Pagination<PostToReturnDto>>> GetAllPosts([FromQuery]PostSpecParams parametars)
        {
            var posts = await unitOfWork.Repository<Post>().GetAllWithSpec(new PostSpecification(parametars));
            var postsDto = mapper.Map<IReadOnlyList<Post>, IReadOnlyList<PostToReturnDto>>(posts);
            var allPostsAfterFilter = await unitOfWork.Repository<Post>().GetAllWithSpec(new PostSpecificationWithFilterOnly(parametars));
            var count = allPostsAfterFilter.Count;
                return Ok(new Pagination<PostToReturnDto>(parametars.PageSize, parametars.PageIndex, count, postsDto));
        }

        [HttpPost("{postId}")]
        public async Task<ActionResult<LikeToReturnDto>> AddLike(int postId)
        {
            // Get User Id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // create object from Like
            var like = new Like()
            {
                PostId = postId,
                UserId = userId
            };
            await unitOfWork.Repository<Like>().Add(like);
            await unitOfWork.Complete();

            var likeToReturnDto = mapper.Map<Like, LikeToReturnDto>(like);

            return Ok(likeToReturnDto);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<int>> GetCountOfLikes(int postId)
        {
            var likes = await unitOfWork.Repository<Like>().GetAllWithSpec(new LikeSpecification(postId));
            var likesCount = likes.Count;
            return likesCount;
        }

        private bool CheckAuthorWhoCreatePost(Post post)
        {
            var authorPost = post.PostAuthor.Id;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return authorPost == userId;
        }

        private async Task<bool> CheckIfUserIsAdmin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByNameAsync("admin");
            return role is not null;

        }
    }
}
