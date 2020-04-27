using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
        public class UserViewModel
        {
            public string Id { get; set; }
            public string IDNumber { get; set; }
            public string FullName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string ImagePath { get; set; }
            public int Gender { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public string Token { get; set; }
            public List<RoleViewModel> RoleViewModel { get; set; }
            public int Status { get; set; }
        

    }
    
}
