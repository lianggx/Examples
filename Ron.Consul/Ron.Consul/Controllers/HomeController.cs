using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ron.Consul.Controllers
{
    [Route("home"), ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("index")]
        public ActionResult<string> Index()
        {
            return "Index";
        }

        [HttpGet("add/{x:int}/{y:int}")]
        public ActionResult<int> Add(int x, int y)
        {
            return x + y;
        }
    }
}
