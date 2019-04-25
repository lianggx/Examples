using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ron.SignalRLesson3.Models
{
    public class ChatMessage
    {
        public int Type { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
