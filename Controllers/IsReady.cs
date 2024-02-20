using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IsReady : ControllerBase
    {
        [HttpGet]
        public ActionResult<ServiceResponse<string>> ConnectionIsReady()
        {
            return Ok("OK");
        }
    }
}