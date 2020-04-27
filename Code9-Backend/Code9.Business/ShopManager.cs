using Code9.Data;
using Code9.Data.UnitOfWork;
using Code9.Entities.Models;
using Code9.Shared;
using Code9.Shared.Helper;
using Code9.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.Business
{
    public class ShopManager
    {
        private IUnitOfWork _unitOfWork;
        private Code9Context _context;

        public ShopManager(IUnitOfWork unitOfWork, Code9Context context)
        {
            _unitOfWork = unitOfWork;
            _context = context;

        }

        public async Task<Result> GetDashboardData(UserLoginViewModel UserViewModel)
        {
            var LastStatus = _context.ShopStatus.Where(us => us.ShopId.ToString() == UserViewModel.Id).OrderByDescending(us => us.Id).Select(us => us.UserStatusEnum).FirstOrDefault();
            var CheckInUsers = _context.CheckInOut.Where(c => c.ShopId.ToString() == UserViewModel.Id && c.CheckOutDate == null).ToList();
            var data = new
            {
                Status = (int)LastStatus,
                NumberOfUsersIn = CheckInUsers.Count()
            };

            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }


        public async Task<Result> GetUserInfo(string userId)
        {
            var data = _context.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                IDNumber=s.IDNumber,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Gender = (int)s.Gender,
                Status = (int)s.UserStatus.OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault(),
                ImagePath = s.ImagePath
            }).Where(u => u.Id == userId).FirstOrDefault();

            if (data != null )
            {
                return new Result()
                {
                    Data = data,
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.UserNotExist }

                };
            }
          
        }


        
        public async Task<Result> GetUserInfoForSerach(string userId)
        {
            var data = _context.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                IDNumber = s.IDNumber,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Gender = (int)s.Gender,
                Status = (int)s.UserStatus.OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault(),
                ImagePath = s.ImagePath
            }).Where(u => u.IDNumber == userId);

            if (data != null && data.Count() > 0)
            {
                return new Result()
                {
                    Data = data,
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.UserNotExist }

                };
            }

        }


        public async Task<Result> GetShopInfo(string userId)
        {
            var data = _context.Shop.Select(s => new ShopViewModel
            {
                Id = s.Id,
                LicenseNumber = s.LicenseNumber,
                FullName = s.Name,
                Latitude= s.Latitude,
                Longitude = s.Longitude,
                Status = (int)s.ShopStatus.OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault(),
                ImagePath = s.ImagePath
            }).Where(u => u.LicenseNumber == userId);

            if (data != null && data.Count() > 0)
            {
                return new Result()
                {
                    Data = data,
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.UserNotExist }

                };
            }

        }




        public async Task<Result> CheckIn(CheckInOutViewModel CheckInOutVM)
        {
            var LatestCheckOut = _context.CheckInOut.Where(c => c.ShopId == CheckInOutVM.ShopId && c.UserId == CheckInOutVM.UserId).OrderByDescending(c => c.Id).Select(c => c.CheckOutDate).FirstOrDefault();
            var LatestCheckIn = _context.CheckInOut.Where(c => c.ShopId == CheckInOutVM.ShopId && c.UserId == CheckInOutVM.UserId).OrderByDescending(c => c.Id).Select(c => c.CheckInDate).FirstOrDefault();
            var userName = _context.Users.Where(u => u.Id == CheckInOutVM.UserId).Select(u => u.FullName).FirstOrDefault();
            var shopName = _context.Shop.Where(u => u.Id == CheckInOutVM.ShopId).Select(u => u.Name).FirstOrDefault();

            if (LatestCheckIn != null && LatestCheckOut == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.PleaseCheckOutOfShopToCheckInAgain }

                };
            }
            else
            {

                CheckInOut CheckInOut = new CheckInOut();
                CheckInOut.UserId = CheckInOutVM.UserId;
                CheckInOut.ShopId = CheckInOutVM.ShopId;
                CheckInOut.CheckInDate = DateTime.Now;
                CheckInOut.CreationDate = DateTime.Now;
                CheckInOut.CreatedBy = "Test";
                CheckInOut.IsActive = true;
                _context.CheckInOut.Add(CheckInOut);
                _context.SaveChanges();


                #region SendNotification For checkedIn User
                //send notifications
                var title = Resources.CheckInTitle;
                var body = Resources.CheckedIn + " " + shopName + " " + Resources.Successfully;

                PayLoadViewModel payLoadData = new PayLoadViewModel
                {
                    InfectedUserName = "Test",
                    Title = title,
                    Body = body
                };

                var TokensList = GetUserToken(CheckInOut.UserId);

                FirebaseHelper.SendNotification(TokensList, title, body, payLoadData);

                #endregion


                #region SendNotification For Shop
                //send notifications
                var titleShop = Resources.CheckInTitle;
                var bodyShop = userName + " " + Resources.CheckInStore;

                PayLoadViewModel payLoadDataShop = new PayLoadViewModel
                {
                    InfectedUserName = "Test",
                    Title = titleShop,
                    Body = bodyShop
                };

                var TokensListShop = GetShopToken(CheckInOut.ShopId);

                FirebaseHelper.SendNotification(TokensListShop, titleShop, bodyShop, payLoadDataShop);

                #endregion




                return new Result()
                {
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }





        }

        public async Task<Result> Checkout(CheckInOutViewModel CheckInOutVM)
        {
            var CheckInOut = _context.CheckInOut.Where(c => c.UserId == CheckInOutVM.UserId && c.ShopId == CheckInOutVM.ShopId && c.CheckOutDate == null).FirstOrDefault();
            var userName = _context.Users.Where(u => u.Id == CheckInOutVM.UserId).Select(u => u.FullName).FirstOrDefault();
            var shopName = _context.Shop.Where(u => u.Id == CheckInOutVM.ShopId).Select(u => u.Name).FirstOrDefault();

            CheckInOut.CheckOutDate = DateTime.Now;
            CheckInOut.ModificationDate = DateTime.Now;
            CheckInOut.UpdatedBy = "Test";

            _context.SaveChanges();


            #region SendNotification For checkedIn User
            //send notifications
            var title = Resources.CheckOutTitle;
            var body = Resources.Checkedout + " " + shopName + " " + Resources.Successfully;

            PayLoadViewModel payLoadData = new PayLoadViewModel
            {
                InfectedUserName = "Test",
                Title = title,
                Body = body
            };

            var TokensList = GetUserToken(CheckInOut.UserId);

            FirebaseHelper.SendNotification(TokensList, title, body, payLoadData);

            #endregion


            #region SendNotification For Shop
            //send notifications
            var titleShop = Resources.CheckOutTitle;
            var bodyShop = userName + " " + Resources.CheckOutStore;

            PayLoadViewModel payLoadDataShop = new PayLoadViewModel
            {
                InfectedUserName = "Test",
                Title = titleShop,
                Body = bodyShop
            };

            var TokensListShop = GetShopToken(CheckInOut.ShopId);

            FirebaseHelper.SendNotification(TokensListShop, titleShop, bodyShop, payLoadDataShop);

            #endregion

            return new Result()
            {
                IsSuccess = true,
                Errors = new List<string>()

            };
        }


        public List<UserTokenViewModel> GetUserToken(string userId)
        {
            var tokensList = _context.UserDevices.Where(u => u.UserId == userId).Select(a => new UserTokenViewModel
            {
                Token = a.Token,
                Type = a.Type
            }).ToList();

            return tokensList;
        }
        public List<UserTokenViewModel> GetShopToken(long shopId)
        {

            var tokensList = _context.Shop.Where(u => u.Id == shopId).Select(a => new UserTokenViewModel
            {
                Token = a.FireBaseToken
            }).ToList();

            return tokensList;
        }

    }
}
