using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeployTesting : ControllerBase
    {
        [HttpGet]
        public ActionResult<ServiceResponse<string>> DeployTestingRoute()
        {
            return Ok("Connection OK");
        }
    }
}