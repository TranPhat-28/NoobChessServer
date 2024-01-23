using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoobChessServer.Auth;
using NoobChessServer.DTOs.LoginDtos;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public LoginController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Google")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            if (googleLoginDto.GoogleAccessToken == "")
            {
                // Bad request
                var response = new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "No Google token found"
                };

                return BadRequest(response);
            }
            else
            {
                // Call the service
                var response = await _authRepository.LoginWithGoogle(googleLoginDto);
                return Ok(response);
            }
        }

        [HttpPost("Facebook")]
        public async Task<ActionResult<string>> LoginWithFacebook(FacebookLoginDto facebookLoginDto)
        {
            if (facebookLoginDto.FacebookAccessToken == "")
            {
                // Bad request
                var response = new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "No Facebook token found"
                };

                return BadRequest(response);
            }
            else
            {
                // Call the service
                var response = await _authRepository.LoginWithFacebook(facebookLoginDto);
                return Ok(response);
            }
        }
    }
}