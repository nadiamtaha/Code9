using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Entities.Models
{
    public class Role : IdentityRole
    {
        public bool IsActive { get; set; }
    }
}
