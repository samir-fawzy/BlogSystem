using BlogSystem.Api.Dto;
using BlogSystem.Api.Error;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{

    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(UserManager<AppUser> userManager,ITokenService tokenService,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var existUser = await userManager.FindByEmailAsync(model.Email);
            if (existUser != null) return BadRequest(new ApiValidationError() { Errors = new[] {"This E-Mail is already exist"} }); 
            var user = new AppUser()
            {
                DisplayName = $"{model.FName.ToLower()} {model.LName.ToLower()}",
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],
            };
            
            bool nonExistUsers = false;
            if (!userManager.Users.Any()) nonExistUsers = true;
            // create user
            var result = await userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            // Add admin role to the first user
            if (nonExistUsers)
            {
                await userManager.AddToRoleAsync(user, "admin");
                await userManager.AddToRoleAsync(user, "author");
            }
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if(!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });
        }


    }
}
