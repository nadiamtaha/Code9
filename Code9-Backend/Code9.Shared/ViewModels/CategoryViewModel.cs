using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class CategoryViewModel
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string IconPath { get; set; }

        public List<ShopViewModel> Shops { get; set; }
    }
}
