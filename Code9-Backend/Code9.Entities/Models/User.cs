using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Code9.Shared;
namespace Code9.Entities.Models
{
    public class User : IdentityUser
    {
        public string IDNumber { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImagePath { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public bool IsActive { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public byte[] GWTPasswordHash { get; set; }
        public byte[] GWTPasswordSalt { get; set; }
        public virtual ICollection<UserStatus> UserStatus { get; set; }
        
        public virtual ICollection<CheckInOut> CheckInOut { get; set; }
        public virtual ICollection<UserDevice> UserDevice { get; set; }



    }
}
