﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.core.Entities.Identity;
using Talabat.core.Services.Contract;
using Talabate.Dtos;
using Talabate.Errors;
using Talabate.Extensions;

namespace Talabate.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,

           IAuthService authService,
           IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
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
                message = "success",
                DisplayName =user.DisplayName,
                Email=user.Email,
                Token=await _authService.CreateTokenAsync(user,_userManager)
            });
        }


        [Authorize]
        [HttpGet]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email) ??string.Empty;

            var user=await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                    DisplayName=user?.DisplayName??string.Empty,
                    Email=user?.Email??string.Empty,
                    Token=await _authService.CreateTokenAsync(user,_userManager)


            });

        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
             
            var user = await _userManager.FindUserWithAddressAsync(User);
            return Ok(_mapper.Map<AddressDto>(user.Address));

        }


        [Authorize]
        [HttpPut("address")]

        public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto address)
        {

            var updateAddress=_mapper.Map<Address>(address);
            
            var user = await _userManager.FindUserWithAddressAsync(User);
            updateAddress.Id = user.Address.Id;
            user.Address=updateAddress;

             
         var result=await _userManager.UpdateAsync(user);

            if(!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors= result.Errors.Select(E=>E.Description)

                });


           return Ok(address);






        }




    }
}
