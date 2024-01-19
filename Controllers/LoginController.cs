using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet("Google")]
        public async Task<ActionResult<string>> LoginWithGoogle()
        {
            return Ok("Login with Google");
        }

        [HttpGet("Facebook")]
        public async Task<ActionResult<string>> LoginWithFacebook()
        {
            return Ok("Login with Facebook");
        }
    }
}