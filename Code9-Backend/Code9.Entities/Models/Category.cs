using Code9.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Entities.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string IconPath { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
    }
}
