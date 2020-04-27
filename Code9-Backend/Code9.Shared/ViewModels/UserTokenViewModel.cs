using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class UserTokenViewModel
    {
        public string Token { get; set; }

        public int UnReadMessagesCount { get; set; }

        public DeviceType Type { get; set; }

        public bool IsMuted { get; set; }
    }
}
