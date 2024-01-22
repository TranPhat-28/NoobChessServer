using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoobChessServer.DTOs.LoginDtos;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost("Google")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            if (googleLoginDto.GoogleAccessToken == "")
            {
                // Bad request
                var response = new ServiceResponse<string>();
                response.IsSuccess = false;
                response.Message = "No Google token found";

                return BadRequest(response);
            }
            else
            {
                // Call the service
                var response = new ServiceResponse<string>();
                response.Message = "Login with Google";
                return Ok(response);
            }
        }

        [HttpPost("Facebook")]
        public async Task<ActionResult<string>> LoginWithFacebook()
        {
            return Ok("Login with Facebook");
        }
    }
}