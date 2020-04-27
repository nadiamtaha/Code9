using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class EditStatusViewModel
    {
        public string UserId { get; set; }
        public UserStatusEnum UserStatusEnum { get; set; }
        
        public UserTypeEnum UserType { get; set; }

    }
}
