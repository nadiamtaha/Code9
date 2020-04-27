using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class RegisterTokenViewModel
    {
        public string NewToken { get; set; }
        public int OSType { get; set; }
        public string DeviceName { get; set; }
        public string UserId { get; set; }
        public UserTypeEnum UserType { get; set; }
    }
}
