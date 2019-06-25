using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ron.DI.Services;

namespace Ron.DI.Controllers
{
    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [PropertyFromService] public IConfiguration Configuration { get; set; }
        [PropertyFromService] public IHostingEnvironment Environment { get; set; }
        [PropertyFromService] public CarService CarService { get; set; }
        [PropertyFromService] public PostServices PostServices { get; set; }
        [PropertyFromService] public TokenService TokenService { get; set; }
        [PropertyFromService] public TopicService TopicService { get; set; }
        [PropertyFromService] public UserService UserService { get; set; }

        public HomeController()
        {

        }

        [HttpGet("index")]
        public ActionResult<string> Index()
        {
            return "Hello world!";
        }
    }
}
