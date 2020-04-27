using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class RegisterViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }    
        public string Email { get; set; }
        public UserTypeEnum UserType { get; set; }
        public string Password { get; set; }
        public long CategoryId { get; set; }

    }
}
