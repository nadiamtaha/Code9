using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Code9.Entities.Models
{
    public class UserRole : IdentityUserRole<string>
    {


        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsActive { get; set; }


    }
}
