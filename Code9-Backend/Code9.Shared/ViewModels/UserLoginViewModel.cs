using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class UserLoginViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }
    }
}
