using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.CDLX.BLL;
using RabbitMQ.CDLX.Models;
using RabbitMQ.CDLX.Services;

namespace RabbitMQ.CDLX.Controllers
{
    public class HomeController : Controller
    {
        private CdlxMasterService masterService;
        private IConfiguration configuration;
        private ILogger<HomeController> logger;
        public HomeController(CdlxMasterService masterService,
                              IConfiguration configuration,
                              ILogger<HomeController> logger)
        {
            this.masterService = masterService;
            this.configuration = configuration;
            this.logger = logger;
        }



        public IActionResult Index()
        {
            var list = OnRefresh();
            return View(list);
        }

        /// <summary>
        ///  轮询死信队列
        /// </summary>
        public List<CdlxMessage> OnRefresh()
        {
            var dqs = new CdlxTimerService(configuration, logger);
            var list = dqs.CheckMessage();
            return list;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Publish()
        {
            int total = 10;
            Contract contract = new Contract(this.masterService);
            for (int i = 0; i < total; i++)
            {
                contract.Publish(MessageType.RedPackage, "红包信息，超时时间1024s", 24);
                contract.Publish(MessageType.Order, "订单信息，超时时间2048s", 48);
                contract.Publish(MessageType.Vote, "投票信息，超时时间4096s", 96);
            }
            ViewBag.total = total * 3;

            return View();
        }
    }
}
