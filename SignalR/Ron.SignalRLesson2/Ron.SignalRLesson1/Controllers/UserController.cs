using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ron.SignalRLesson1.Models;
using Ron.SignalRLesson1.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ron.SignalRLesson1.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private WeChatHub chatHub;
        public UserController(WeChatHub chatHub)
        {
            this.chatHub = chatHub;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserViewModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Chat", "Home");
        }

        [Authorize(Roles = "User")]
        [HttpPost("SendToUser")]
        public async Task<IActionResult> SendToUser([FromBody] UserInfoViewModel model)
        {
            ChatMessage message = new ChatMessage()
            {
                Type = 1,
                Content = model.Content,
                UserName = model.UserName
            };

            if (this.chatHub.UserList.ContainsKey(model.UserName))
            {
                var connections = this.chatHub.UserList[model.UserName].First();
                await this.chatHub.Clients.Client(connections).SendAsync("Recv", new object[] { message });
            }

            return Json(new { Code = 0 });
        }

        [Authorize(Roles = "User")]
        [HttpPost("Group-Join")]
        public async Task<IActionResult> Join([FromBody] GroupViewModel model)
        {
            await this.chatHub.AddToGroupAsync(model.Name);

            return Json(new { Code = 0 });
        }

        [Authorize(Roles = "User")]
        [HttpPost("Group-Leave")]
        public async Task<IActionResult> Leave([FromBody] GroupViewModel model)
        {
            await this.chatHub.RemoveFromGroupAsync(model.Name);

            return Json(new { Code = 0 });
        }

        [Authorize(Roles = "User")]
        [HttpPost("SendToGroup")]
        public async Task<IActionResult> SendToGroup([FromBody] GroupChatMessage model)
        {
            ChatMessage message = new ChatMessage()
            {
                Type = 1,
                Content = model.Content,
                UserName = User.Identity.Name
            };
            await this.chatHub.SendToGroupAsync(model.GroupName, message);

            return Json(new { Code = 0 });
        }
    }
}
