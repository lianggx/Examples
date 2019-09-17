using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ron.RedPacketTest.Service;
using Ron.RedPacketTest.ViewModel;

namespace Ron.RedPacketTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IEnumerable<IRedPacket> redpackets;
        public HomeController(IEnumerable<IRedPacket> redpackets)
        {
            this.redpackets = redpackets;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] RedPacketViewModel model)
        {
            var rp = this.redpackets.Where(f => f.Name == model.Type).FirstOrDefault();
            if (rp == null)
            {
                var msg = $"红包业务类型：{model.Type}不存在";
                Console.WriteLine(msg);
                return msg;
            }

            var result = rp.Put(model.Org_Id, model.Money, model.Count, model.Reason);

            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            // 生产环境下，该红包消息应该是从数据库中读取
            var model = GetRedPacket(id);
            var rp = this.redpackets.Where(f => f.Name == model.Type).FirstOrDefault();
            var result = rp.Get(id);

            return result;
        }

        private RedPacketViewModel GetRedPacket(int id)
        {
            int type = --id;
            string[] redPackets = { "ChatOne", "ChatGroup", "Live" };

            var model = new RedPacketViewModel
            {
                Count = 3,
                Money = 8,
                Org_Id = 115,
                Reason = "恭喜发财，大吉大利！",
                Type = redPackets[type]
            };
            return model;
        }
    }
}
