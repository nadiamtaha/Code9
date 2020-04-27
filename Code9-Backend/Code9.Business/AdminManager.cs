using Code9.Data;
using Code9.Data.UnitOfWork;
using Code9.Entities.Models;
using Code9.Shared;
using Code9.Shared.Helper;
using Code9.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.Business
{
    public class AdminManager
    {
        private IUnitOfWork _unitOfWork;
        private Code9Context _context;
        private readonly AppSettingsViewModel _appSettings;

        public AdminManager(Code9Context context, IOptions<AppSettingsViewModel> appSettings, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _appSettings = appSettings.Value;
        }





        public async Task<Result> EditCitizenStatus(EditStatusViewModel EditStatusVM)
        {
            if (EditStatusVM.UserStatusEnum != UserStatusEnum.Normal)
            {
                // change user status
                var UserId = _context.Users.Where(u => u.Id == EditStatusVM.UserId).Select(u => u.Id).FirstOrDefault();
                var oldStatus = _context.UserStatus.Where(u => u.UserId == EditStatusVM.UserId).OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault();

                if ( UserId != null)
                {
                    if (oldStatus != EditStatusVM.UserStatusEnum)
                    {
                        UserStatus userStatus = new UserStatus();
                        userStatus.UserId = UserId;
                        userStatus.UserStatusEnum = EditStatusVM.UserStatusEnum;
                        userStatus.Date = DateTime.Now;
                        userStatus.CreationDate = DateTime.Now;
                        userStatus.IsActive = true;
                        userStatus.CreatedBy = "test";
                        _context.UserStatus.Add(userStatus);
                        _context.SaveChanges();

                        //change all checkedin shops in last 14 day satus to be suspected
                        var checkedInPlacesLastTwoWeeks = _context.CheckInOut.Where(c => c.UserId == EditStatusVM.UserId && c.CheckInDate.Value.AddDays(14) > DateTime.Now).Select(c => c.ShopId).ToList();

                        var CheckInOutList = _context.CheckInOut.Where(c => c.UserId == EditStatusVM.UserId && c.CheckInDate.Value.AddDays(14) > DateTime.Now).ToList();

                        List<CheckInOutPeriod> CheckInOutPeriodList = new List<CheckInOutPeriod>();
                        foreach (var CheckInOut in CheckInOutList)
                        {
                            CheckInOutPeriod CheckInOutPeriod = new CheckInOutPeriod();
                            CheckInOutPeriod.CheckIn = CheckInOut.CheckInDate;
                            CheckInOutPeriod.CheckOut = CheckInOut.CheckOutDate;
                            CheckInOutPeriodList.Add(CheckInOutPeriod);
                        }

                        var CheckedInAtThisShops = _context.CheckInOut.Where(c => checkedInPlacesLastTwoWeeks.Contains(c.ShopId) && c.UserId != EditStatusVM.UserId).ToList();

                        List<string> userUdsForNotifications = new List<string>();

                        foreach (var CheckPeriod in CheckInOutPeriodList)
                        {
                            foreach (var usersCheckedIn in CheckedInAtThisShops)
                            {
                                if ((usersCheckedIn.CheckInDate < CheckPeriod.CheckOut && usersCheckedIn.CheckOutDate > CheckPeriod.CheckIn) || (usersCheckedIn.CheckInDate > CheckPeriod.CheckIn && CheckPeriod.CheckOut == null))
                                {

                                    UserStatus newUserStatus = new UserStatus();
                                    newUserStatus.UserId = usersCheckedIn.UserId;
                                    newUserStatus.UserStatusEnum = UserStatusEnum.Suspected;
                                    newUserStatus.Date = DateTime.Now;
                                    newUserStatus.CreationDate = DateTime.Now;
                                    newUserStatus.IsActive = true;
                                    newUserStatus.CreatedBy = "test";
                                    _context.UserStatus.Add(newUserStatus);
                                    _context.SaveChanges();
                                    userUdsForNotifications.Add(usersCheckedIn.UserId);

                                }
                            }
                        }

                        List<long> NormalShopIds = new List<long>();
                        List<long> SuspectedShopIds = new List<long>();
                        List<long> InfectedShopIds = new List<long>();


                        // change shop status

                        foreach (var checkedInShop in checkedInPlacesLastTwoWeeks)
                        {
                            var suspectedusersIds = _context.CheckInOut.Where(c => c.ShopId == checkedInShop).Select(c => c.UserId).Distinct().ToList();

                            List<UserStatusEnum> NoramlUsersStatesList = new List<UserStatusEnum>();
                            List<UserStatusEnum> SuspectedUsersStatesList = new List<UserStatusEnum>();
                            List<UserStatusEnum> InfectedUsersStatesList = new List<UserStatusEnum>();

                            foreach (var suspecteduserId in suspectedusersIds)
                            {
                                UserStatusEnum userLastStatus = _context.UserStatus.Where(u => u.UserId == suspecteduserId).OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault();

                                if (userLastStatus == UserStatusEnum.Normal)
                                {
                                    NoramlUsersStatesList.Add(userLastStatus);

                                }
                                else if (userLastStatus == UserStatusEnum.Suspected)
                                {
                                    SuspectedUsersStatesList.Add(userLastStatus);
                                }
                                else if (userLastStatus == UserStatusEnum.Infected)
                                {
                                    InfectedUsersStatesList.Add(userLastStatus);
                                }
                            }

                            if (NoramlUsersStatesList.Count() > (SuspectedUsersStatesList.Count() + InfectedUsersStatesList.Count()))
                            {
                                var shopStatus = _context.ShopStatus.Where(s => s.ShopId == checkedInShop).OrderByDescending(s => s.Id).Select(u => u.UserStatusEnum).FirstOrDefault();
                                if (shopStatus != UserStatusEnum.Normal)
                                {
                                    ShopStatus NewShopStatus = new ShopStatus();
                                    NewShopStatus.ShopId = checkedInShop;
                                    NewShopStatus.UserStatusEnum = UserStatusEnum.Normal;
                                    NewShopStatus.Date = DateTime.Now;
                                    NewShopStatus.CreationDate = DateTime.Now;
                                    NewShopStatus.IsActive = true;
                                    NewShopStatus.CreatedBy = "test";
                                    _context.ShopStatus.Add(NewShopStatus);
                                    _context.SaveChanges();
                                    NormalShopIds.Add(checkedInShop);
                                }


                            }

                            else if (SuspectedUsersStatesList.Count() > NoramlUsersStatesList.Count())
                            {
                                var shopStatus = _context.ShopStatus.Where(s => s.ShopId == checkedInShop).OrderByDescending(s => s.Id).Select(u => u.UserStatusEnum).FirstOrDefault();
                                if (shopStatus != UserStatusEnum.Suspected)
                                {
                                    ShopStatus NewShopStatus = new ShopStatus();
                                    NewShopStatus.ShopId = checkedInShop;
                                    NewShopStatus.UserStatusEnum = UserStatusEnum.Suspected;
                                    NewShopStatus.Date = DateTime.Now;
                                    NewShopStatus.CreationDate = DateTime.Now;
                                    NewShopStatus.IsActive = true;
                                    NewShopStatus.CreatedBy = "test";
                                    _context.ShopStatus.Add(NewShopStatus);
                                    _context.SaveChanges();
                                    SuspectedShopIds.Add(checkedInShop);
                                }
                            }

                            //else if (InfectedUsersStatesList.Count() >= NoramlUsersStatesList.Count() || InfectedUsersStatesList.Count() >= SuspectedUsersStatesList.Count() || (((SuspectedUsersStatesList.Count() + InfectedUsersStatesList.Count()) > 0) && NoramlUsersStatesList.Count() == 0))
                            //{
                            //    var shopStatus = _context.ShopStatus.Where(s => s.ShopId == checkedInShop).OrderByDescending(s => s.Id).Select(u => u.UserStatusEnum).FirstOrDefault();
                            //    if (shopStatus != UserStatusEnum.Infected)
                            //    {
                            //        ShopStatus NewShopStatus = new ShopStatus();
                            //        NewShopStatus.ShopId = checkedInShop;
                            //        NewShopStatus.UserStatusEnum = UserStatusEnum.Infected;
                            //        NewShopStatus.Date = DateTime.Now;
                            //        NewShopStatus.CreationDate = DateTime.Now;
                            //        NewShopStatus.IsActive = true;
                            //        NewShopStatus.CreatedBy = "test";
                            //        _context.ShopStatus.Add(NewShopStatus);
                            //        _context.SaveChanges();
                            //        InfectedShopIds.Add(checkedInShop);
                            //    }
                            //}

                        }


                        #region SendNotification For Infected User
                        //send notifications
                        var title = Resources.MedicalStatusAlertTitle;
                        var body = Resources.MedicalStatusIsChanged + " " + EditStatusVM.UserStatusEnum + " " + Resources.PleaseQuarantineYourSelf;

                        PayLoadViewModel payLoadData = new PayLoadViewModel
                        {
                            InfectedUserName = "Test",
                            Title = title,
                            Body = body
                        };

                        var TokensList = GetUserToken(EditStatusVM.UserId);

                        FirebaseHelper.SendNotification(TokensList, title, body, payLoadData);

                        #endregion


                        #region SendNotification For Suspected Users
                        //send notifications
                        var titleForSuspected = Resources.MedicalStatusAlertTitle;
                        var bodyForSuspected = Resources.MedicalStatusIsChanged + " " + Resources.ContactedWithInfectedPerson;

                        PayLoadViewModel payLoadDataForSuspected = new PayLoadViewModel
                        {
                            InfectedUserName = "Test",
                            Title = titleForSuspected,
                            Body = bodyForSuspected
                        };

                        var TokensListForSuspected = GetUsersTokens(userUdsForNotifications);

                        FirebaseHelper.SendNotification(TokensListForSuspected, titleForSuspected, bodyForSuspected, payLoadDataForSuspected);

                        #endregion


                        #region SendNotification For Normal Shops

                        if (NormalShopIds != null && NormalShopIds.Count() > 0)
                        {
                            var titleForShop = Resources.MedicalStatusAlertTitle;
                            var bodyForNormalShop = Resources.TruststatusIsNormal;

                            PayLoadViewModel payLoadDataForNormalShop = new PayLoadViewModel
                            {
                                InfectedUserName = "Test",
                                Title = titleForShop,
                                Body = bodyForNormalShop
                            };

                            var TokensListForNormalShops = GetShopsTokens(NormalShopIds);

                            FirebaseHelper.SendNotification(TokensListForNormalShops, titleForShop, bodyForNormalShop, payLoadDataForNormalShop);

                        }

                        #endregion

                        #region SendNotification For Suspected Shops

                        if (SuspectedShopIds != null && SuspectedShopIds.Count() > 0)
                        {
                            var titleForShop = Resources.MedicalStatusAlertTitle;
                            var bodyForSuspectedShops = Resources.InfectedPersonVisitStore;

                            PayLoadViewModel payLoadDataForSuspectedShops = new PayLoadViewModel
                            {
                                InfectedUserName = "Test",
                                Title = titleForShop,
                                Body = bodyForSuspectedShops
                            };

                            var TokensListForSuspectedShops = GetShopsTokens(SuspectedShopIds);

                            FirebaseHelper.SendNotification(TokensListForSuspectedShops, titleForShop, bodyForSuspectedShops, payLoadDataForSuspectedShops);

                        }

                        #endregion

                        //#region SendNotification For Infected Shops

                        //if (InfectedShopIds != null && InfectedShopIds.Count() > 0)
                        //{
                        //    var titleForShop = Resources.MedicalStatusAlertTitle;
                        //    var bodyForInfectedShops = Resources.InfectedPersonVisitStore;

                        //    PayLoadViewModel payLoadDataForInfectedShops = new PayLoadViewModel
                        //    {
                        //        InfectedUserName = "Test",
                        //        Title = titleForShop,
                        //        Body = bodyForInfectedShops
                        //    };

                        //    var TokensListForInfectedShops = GetShopsTokens(InfectedShopIds);

                        //    FirebaseHelper.SendNotification(TokensListForInfectedShops, titleForShop, bodyForInfectedShops, payLoadDataForInfectedShops);

                        //}

                        //#endregion


                        return new Result()
                        {
                            IsSuccess = true,
                            Errors = new List<string>()
                        };



                    }
                    else
                    {
                        return new Result()
                        {
                            IsSuccess = false,
                            Errors = new List<string> { Resources.UserIsAlreadyOnThisStatus }
                        };
                    }
                   
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
            else
            {
                // change user status
                var UserId = _context.Users.Where(u => u.Id == EditStatusVM.UserId).Select(u => u.Id).FirstOrDefault();
                var oldStatus = _context.UserStatus.Where(u => u.UserId == EditStatusVM.UserId).OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault();
                if (UserId != null)
                {
                    if(oldStatus != UserStatusEnum.Normal)
                    {
                        UserStatus userStatus = new UserStatus();
                        userStatus.UserId = UserId;
                        userStatus.UserStatusEnum = EditStatusVM.UserStatusEnum;
                        userStatus.Date = DateTime.Now;
                        userStatus.CreationDate = DateTime.Now;
                        userStatus.IsActive = true;
                        userStatus.CreatedBy = "test";
                        _context.UserStatus.Add(userStatus);
                        _context.SaveChanges();

                        #region SendNotification For Normal User
                        //send notifications
                        var title = Resources.MedicalStatusAlertTitle;
                        var body = Resources.MedicalStatusIsChanged + " " + EditStatusVM.UserStatusEnum;

                        PayLoadViewModel payLoadData = new PayLoadViewModel
                        {
                            InfectedUserName = "Test",
                            Title = title,
                            Body = body
                        };

                        var TokensList = GetUserToken(EditStatusVM.UserId);

                        FirebaseHelper.SendNotification(TokensList, title, body, payLoadData);

                        #endregion


                    }
                    else
                    {
                        return new Result()
                        {
                            IsSuccess = false,
                            Errors = new List<string> { Resources.UserIsAlreadyOnThisStatus }
                        };
                    }

                }
                else
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.UserNotExist }
                    };

                }

                return new Result()
                {
                    IsSuccess = true,
                    Errors = new List<string>()
                };
            }
 
        }


        public async Task<Result> EditShopStatus(EditStatusViewModel EditStatusVM)
        {

            var shopId = _context.Shop.Where(u => u.Id.ToString() == EditStatusVM.UserId).Select(u => u.Id).FirstOrDefault();
            var oldStatus = _context.ShopStatus.Where(u => u.ShopId.ToString() == EditStatusVM.UserId).OrderByDescending(u => u.Id).Select(u => u.UserStatusEnum).FirstOrDefault();

            if (shopId !=0 || shopId != null )
            {
                if(oldStatus != EditStatusVM.UserStatusEnum)
                {
                    ShopStatus shopStatus = new ShopStatus();
                    shopStatus.ShopId = shopId;
                    shopStatus.UserStatusEnum = EditStatusVM.UserStatusEnum;
                    shopStatus.Date = DateTime.Now;
                    shopStatus.CreationDate = DateTime.Now;
                    shopStatus.IsActive = true;
                    shopStatus.CreatedBy = "test";

                    _context.ShopStatus.Add(shopStatus);
                    _context.SaveChanges();

                    #region SendNotification For Shops

                        var titleForShop = Resources.MedicalStatusAlertTitle;
                        var bodyForShop = Resources.ShopStatusIsChanged + " " + EditStatusVM.UserStatusEnum;

                        PayLoadViewModel payLoadDataForShop = new PayLoadViewModel
                        {
                            InfectedUserName = "Test",
                            Title = titleForShop,
                            Body = bodyForShop
                        };

                        var TokensListForShop = GetShopToken(shopId);

                        FirebaseHelper.SendNotification(TokensListForShop, titleForShop, bodyForShop, payLoadDataForShop);


                    #endregion



                    #region SendNotification For CheckedIn Users In shop

                    if(EditStatusVM.UserStatusEnum == UserStatusEnum.Normal)
                    {
                        var userIds = _context.CheckInOut.Where(c => c.ShopId == shopId).Select(c => c.UserId).Distinct().ToList();
                        var shopName = _context.Shop.Where(c => c.Id == shopId).Select(c => c.Name).FirstOrDefault();

                        var titleForCheckedInUser = Resources.MedicalStatusAlertTitle;
                        var bodyForCheckedInUser = shopName + " " + Resources.ShopStatusIsChangedToNormal;

                        PayLoadViewModel payLoadDataForCheckedInUser = new PayLoadViewModel
                        {
                            InfectedUserName = "Test",
                            Title = titleForCheckedInUser,
                            Body = bodyForCheckedInUser
                        };

                        var TokensListForCheckedInUser = GetUsersTokens(userIds);

                        FirebaseHelper.SendNotification(TokensListForCheckedInUser, titleForCheckedInUser, bodyForCheckedInUser, payLoadDataForCheckedInUser);


                    }

                    #endregion


                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = new List<string>()
                    };

                }
                else
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.ShopIsAlreadyOnThisStatus }
                    };
                }
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ShopIsNotExist }
                };

            }
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


        public List<UserTokenViewModel> GetUsersTokens(List<string> UserIds)
        {

            var tokensList = _context.UserDevices.Where(u => UserIds.Contains(u.UserId)).Select(a => new UserTokenViewModel
            {
                Token = a.Token,
                Type = a.Type
            }).ToList();

            return tokensList;
        }

        public List<UserTokenViewModel> GetShopsTokens(List<long> shopIds)
        {

            var tokensList = _context.Shop.Where(u => shopIds.Contains(u.Id)).Select(a => new UserTokenViewModel
            {
                Token = a.FireBaseToken
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


        public Result RegisterDeviceToken(RegisterTokenViewModel RegisterTokenVM)
        {

            if(RegisterTokenVM.UserType == UserTypeEnum.Citizen)
            {
                var device = _context.UserDevices.FirstOrDefault(a => a.UserId == RegisterTokenVM.UserId);

                if (device != null)
                {
                    device.Token = RegisterTokenVM.NewToken;
                    device.Type = DeviceType.Android;
                    device.ModificationDate = DateTime.Now;
                    _context.Entry(device).State = EntityState.Modified;
                    _context.SaveChanges();
                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = new List<string>()
                    };
                }
                else
                {
                    var newDevice = new UserDevice
                    {
                        Token = RegisterTokenVM.NewToken,
                        UserId = RegisterTokenVM.UserId,
                        DeviceName = RegisterTokenVM.DeviceName,
                        Type = DeviceType.Android,
                        CreationDate = DateTime.Now,
                        ModificationDate = DateTime.Now
                    };
                    _context.UserDevices.Add(newDevice);
                    _context.SaveChanges();
                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = new List<string>()
                    };
                }
            }
            else if (RegisterTokenVM.UserType == UserTypeEnum.Shop)
            {
                var shop = _context.Shop.FirstOrDefault(a => a.Id.ToString() == RegisterTokenVM.UserId);

                if (shop != null)
                {
                    shop.FireBaseToken = RegisterTokenVM.NewToken;
                    _context.SaveChanges();
                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = new List<string>()
                    };
                }    
                
            }
            return new Result()
            {
                IsSuccess = false,
                Errors = new List<string>()
            };

        }



        public Result UnRegisterDeviceToken(RegisterTokenViewModel RegisterTokenVM)
        {

            if (RegisterTokenVM.UserType == UserTypeEnum.Citizen)
            {
                var devices = _context.UserDevices.Where(a => a.UserId == RegisterTokenVM.UserId).ToList();
                _context.UserDevices.RemoveRange(devices);
                _context.SaveChanges();
                return new Result()
                {
                    IsSuccess = true,
                    Errors = new List<string>()
                };
            }
            else if (RegisterTokenVM.UserType == UserTypeEnum.Shop)
            {
                var shop = _context.Shop.FirstOrDefault(a => a.Id.ToString() == RegisterTokenVM.UserId);

                if (shop != null)
                {
                    shop.FireBaseToken = null;
                    _context.SaveChanges();
                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = new List<string>()
                    };
                }

            }
            return new Result()
            {
                IsSuccess = false,
                Errors = new List<string>()
            };



          

        }



        //test notifications
        public Result TestNotifications()
        {
            var title = Resources.MedicalStatusAlertTitle;
            var body = Resources.NotificationBody;
            PayLoadViewModel payLoadData = new PayLoadViewModel
            {
                InfectedUserName = "Test",
                Title = "This Is Title",
                Body ="This is Body"
            };

            var allUsersIds = _context.Users.Select(a => a.Id).ToList();
            var TokensList = GetUsersTokens(allUsersIds);

            FirebaseHelper.SendNotification(TokensList, title, body, payLoadData);

            return new Result()
            {
                IsSuccess = true,
                Errors = new List<string>()
            };


        }




    }
}
