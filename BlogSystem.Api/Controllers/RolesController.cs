using BlogSystem.Api.Dto;
using BlogSystem.Api.Error;
using BlogSystem.Core.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class RolesController : ApiBaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("create_role")]
        public async Task<ActionResult<IdentityRole>> AddRole(RoleDto model)
        {
            var role = new IdentityRole()
            {
                Name = model.RoleName.ToLower()
            };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(role);
        }
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("{roleName}")]
        public async Task<ActionResult<IdentityRole>> DeleteRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null) return BadRequest(new ApiErrorResponse(400));
            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(role);
        }
        [HttpPost("{id}/{roleName}")]
        public async Task<ActionResult> AddRoleToUser(string id,string roleName)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return BadRequest(new ApiErrorResponse(400));
            var result = await userManager.AddToRoleAsync(user, roleName);
            if(!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(result);
        }

    }
}
