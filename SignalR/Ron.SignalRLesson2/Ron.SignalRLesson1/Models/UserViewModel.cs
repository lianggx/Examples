using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ron.SignalRLesson1.Models
{
    public class UserInfoViewModel
    {
        [Required(ErrorMessage = "The field UserName required.")] public string UserName { get; set; }
        public string Content { get; set; }
    }

    public class UserViewModel
    {
        [Required(ErrorMessage = "The field UserName required.")] public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GroupViewModel
    {
        [Required(ErrorMessage = "The field UserName required.")] public string Name { get; set; }
    }
}
