using Code9.Entities.Abstractions;
using Code9.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Entities.Models
{
    public class ShopStatus :BaseModel
    {
        public UserStatusEnum UserStatusEnum { get; set; }
        public DateTime? Date { get; set; }

        public long ShopId { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
