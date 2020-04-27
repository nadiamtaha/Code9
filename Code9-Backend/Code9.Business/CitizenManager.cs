using Code9.Data;
using Code9.Data.UnitOfWork;
using Code9.Shared.Helper;
using Code9.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.Business
{
    public class CitizenManager
    {
        private IUnitOfWork _unitOfWork;
        private Code9Context _context;

        public CitizenManager(IUnitOfWork unitOfWork, Code9Context context)
        {
            _unitOfWork = unitOfWork;
            _context = context;

        }

        public async Task<Result> GetDashboardData(UserLoginViewModel UserViewModel)
        {
            var LastStatus = _context.UserStatus.Where(us => us.UserId == UserViewModel.Id).OrderByDescending(us => us.Id).Select(us => us.UserStatusEnum).FirstOrDefault();
            var lastStatus = (int)LastStatus;
            var data = new
            {
                Status = lastStatus
            };
            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

        public async Task<Result> GetCategories(string userId)
        {
            var User = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            var UserLat = User.Latitude;
            var UserLong = User.Longitude;


            var sCoord = new GeoCoordinate(UserLat, UserLong);

            var categories = _context.Category.Include(c => c.Shops).Select(c => new CategoryViewModel
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                IconPath =c.IconPath,
                Shops = c.Shops.Select(shop => new ShopViewModel
                {
                    Id = shop.Id,
                    FullName = shop.Name,
                    LicenseNumber = shop.LicenseNumber,
                    Status = (int)shop.ShopStatus.OrderByDescending(s => s.Id).Select(s => s.UserStatusEnum).FirstOrDefault(),
                    ImagePath = shop.ImagePath,
                    Latitude = shop.Latitude,
                    Longitude = shop.Longitude,
                    IsActive = shop.IsActive
                }).ToList()
            }).ToList();
            foreach (var category in categories)
            {
                foreach (var Shop in category.Shops.ToList())
                {
                    Shop.Distance = sCoord.GetDistanceTo(new GeoCoordinate(Shop.Latitude, Shop.Longitude));
                    if (Shop.Distance > 2000)
                    {
                        category.Shops.Remove(Shop);
                    }
                }
            }
            return new Result()
            {
                Data = categories,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

    }
}
