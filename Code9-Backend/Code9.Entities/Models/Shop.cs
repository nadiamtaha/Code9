using Code9.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Entities.Models
{
    public class Shop : BaseModel
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string FireBaseToken { get; set; }
        public string ImagePath { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] GWTPasswordHash { get; set; }
        public byte[] GWTPasswordSalt { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ShopStatus> ShopStatus { get; set; }
        public virtual ICollection<CheckInOut> CheckInOut { get; set; }


    }
}
