using Code9.Entities.Abstractions;
using Code9.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Code9.Entities.Models
{
    public class UserStatus : BaseModel
    {
        public UserStatusEnum UserStatusEnum { get; set; }
        public DateTime? Date { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

    }
}
