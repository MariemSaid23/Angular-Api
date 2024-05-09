using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities.Identity;
using Talabat.core.Services.Contract;
using Talabate.Dtos;
using Talabate.Errors;

namespace Talabate.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,

           IAuthService authService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }


        [HttpPost("login")]

        public async Task<ActionResult<UserDto>>
            Login(LoginDto model)
        {
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401, "InValid"));
            var result=await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "InValid"));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)

            });


        }


        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegistereDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.Phone
            };
            var result =await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse() { Errors=result.Errors.Select(E=>E.Description)});

            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token=await _authService.CreateTokenAsync(user,_userManager)
            });
        }
    }
}
