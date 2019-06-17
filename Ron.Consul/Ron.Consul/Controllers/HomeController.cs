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
            return "Hello wrold";
        }

        [HttpGet("add/{x:int}/{y:int}")]
        public ActionResult<int> Add(int x, int y)
        {
            var result = x + y;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("x+y={0} {1}", result, DateTime.Now.ToString("HH:mm:ss"));
            Console.ForegroundColor = ConsoleColor.Gray;
            return result;
        }
    }
}
