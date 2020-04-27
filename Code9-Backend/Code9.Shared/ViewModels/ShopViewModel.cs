using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class ShopViewModel
    {
        public long Id { get; set; }
        public string LicenseNumber { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public double Distance { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public int Status { get; set; }
        
    }
}
