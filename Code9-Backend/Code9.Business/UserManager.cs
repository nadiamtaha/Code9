using Code9.Data;
using Code9.Data.UnitOfWork;
using Code9.Entities.Models;
using Code9.Shared.ViewModels;
using Code9.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Code9.Business
{
    public interface IUserManager
    {
        public Result Login(UserLoginViewModel model);

        public User GetByUserId(string userId);

        public Task<Result> CreateUser(RegisterViewModel UserViewModel);

    }
    public class UserManager : IUserManager
    {
        private Code9Context _context;
        private readonly AppSettingsViewModel _appSettings;
        private IUnitOfWork _unitOfWork;

        public UserManager(Code9Context context, IOptions<AppSettingsViewModel> appSettings, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get user by id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetByUserId(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }


        public Result Login(UserLoginViewModel model)
        {
            if(model.UserType == UserTypeEnum.Citizen)
            {
                #region Citizen
                if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Password))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.IdAndPasswordRequired }
                    };

                var user = _context.Users.SingleOrDefault(x => x.IDNumber == model.Id && x.IsActive == true);

                // check if username exists
                if (user == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.UserNotExist }
                    };

                // check if password is correct

                if (!VerifyPasswordHash(model.Password, user.GWTPasswordHash, user.GWTPasswordSalt))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.PasswordIsNotTrue }

                    };


                #region Token
                #region Get User Roles

                List<RoleViewModel> RoleVmList = new List<RoleViewModel>();
                var RolesIds = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.IsActive).Select(u => u.RoleId).ToList();

                foreach (var RoleId in RolesIds)
                {
                    var roleObj = _context.Roles.Where(r => r.Id == RoleId).FirstOrDefault();
                    RoleViewModel RoleVM = new RoleViewModel()
                    {
                        RoleId = roleObj.Id,
                        RoleEnglishName = roleObj.Name,
                        RoleArabicName = roleObj.NormalizedName,
                        IsActive = roleObj.IsActive
                    };
                    RoleVmList.Add(RoleVM);
                }

                #endregion

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IDNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Id)
            };
                claims.AddRange(RoleVmList.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleEnglishName)));


                UserViewModel userLoggedInViewModel = new UserViewModel();
                userLoggedInViewModel.Email = user.Email;
                userLoggedInViewModel.FullName = user.FullName;
                userLoggedInViewModel.Id = user.Id;
                userLoggedInViewModel.IDNumber = user.IDNumber;
                userLoggedInViewModel.RoleViewModel = RoleVmList;
                userLoggedInViewModel.DateOfBirth = user.DateOfBirth;
                userLoggedInViewModel.ImagePath = user.ImagePath;
                userLoggedInViewModel.Gender = (int)user.Gender;



                var tokenHandler = new JwtSecurityTokenHandler();

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                                                    null,
                                                    null,
                                                    claims,
                                                    expires: DateTime.UtcNow.AddDays(_appSettings.TokenExpirationDays),
                                                    signingCredentials: creds
                                                );


                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                userLoggedInViewModel.Token = tokenString;
                #endregion

                // authentication successful
                return new Result()
                {
                    Data = userLoggedInViewModel,
                    IsSuccess = true

                };

                #endregion
            }

            else if (model.UserType == UserTypeEnum.Shop)
            {
                #region Shop

                if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Password))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.IdAndPasswordRequired }
                    };

                var shop = _context.Shop.SingleOrDefault(x => x.LicenseNumber == model.Id && x.IsActive == true);

                // check if username exists
                if (shop == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.ShopIsNotExist }
                    };

                // check if password is correct

                if (!VerifyPasswordHash(model.Password, shop.GWTPasswordHash, shop.GWTPasswordSalt))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.ShopIsNotExist }

                    };


                #region Token
         
                ShopViewModel shopViewModel = new ShopViewModel();
                shopViewModel.Id = shop.Id;
                shopViewModel.FullName = shop.Name;
                shopViewModel.LicenseNumber = shop.LicenseNumber;
                shopViewModel.ImagePath = shop.ImagePath;
                shopViewModel.Latitude = shop.Latitude;
                shopViewModel.Longitude = shop.Longitude;



                var tokenHandler = new JwtSecurityTokenHandler();

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                                                    null,
                                                    null,
                                                    expires: DateTime.UtcNow.AddDays(_appSettings.TokenExpirationDays),
                                                    signingCredentials: creds
                                                );


                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                shopViewModel.Token = tokenString;
                #endregion

                // authentication successful
                return new Result()
                {
                    Data = shopViewModel,
                    IsSuccess = true

                };


                #endregion

            }

            else
            {
                #region Admin

                if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Password))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.IdAndPasswordRequired }
                    };

                var user = _context.Users.SingleOrDefault(x => x.Email == model.Id && x.IsActive == true);

                // check if username exists
                if (user == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.UserNotExist }
                    };

                // check if password is correct

                if (!VerifyPasswordHash(model.Password, user.GWTPasswordHash, user.GWTPasswordSalt))
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = new List<string> { Resources.UserNotExist }

                    };


                #region Token
                #region Get User Roles

                List<RoleViewModel> RoleVmList = new List<RoleViewModel>();
                var RolesIds = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.IsActive).Select(u => u.RoleId).ToList();

                foreach (var RoleId in RolesIds)
                {
                    var roleObj = _context.Roles.Where(r => r.Id == RoleId).FirstOrDefault();
                    RoleViewModel RoleVM = new RoleViewModel()
                    {
                        RoleId = roleObj.Id,
                        RoleEnglishName = roleObj.Name,
                        RoleArabicName = roleObj.NormalizedName,
                        IsActive = roleObj.IsActive
                    };
                    RoleVmList.Add(RoleVM);
                }

                #endregion

                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Id)
                };
                claims.AddRange(RoleVmList.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleEnglishName)));

                UserViewModel userLoggedInViewModel = new UserViewModel();

                foreach (var roleVm in RoleVmList)
                {
                    if(roleVm.RoleEnglishName == "Admin")
                    {
                        userLoggedInViewModel.Email = user.Email;
                        userLoggedInViewModel.FullName = user.FullName;
                        userLoggedInViewModel.Id = user.Id;
                        userLoggedInViewModel.RoleViewModel = RoleVmList;
                        userLoggedInViewModel.DateOfBirth = user.DateOfBirth;
                        userLoggedInViewModel.ImagePath = user.ImagePath;
                        userLoggedInViewModel.Gender = (int)user.Gender;

                        var tokenHandler = new JwtSecurityTokenHandler();

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                        var token = new JwtSecurityToken(
                                                            null,
                                                            null,
                                                            claims,
                                                            expires: DateTime.UtcNow.AddDays(_appSettings.TokenExpirationDays),
                                                            signingCredentials: creds
                                                        );


                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        userLoggedInViewModel.Token = tokenString;
                        return new Result()
                        {
                            Data = userLoggedInViewModel,
                            IsSuccess = true

                        };
                    }
                    else
                    {
                        return new Result()
                        {
                            IsSuccess = false,
                            Errors = new List<string> { Resources.UserIsNotHavePermission }

                        };
                    }
                }
         



                #endregion


         

                #endregion
            }


            return new Result()
            {
                IsSuccess = true

            };
        }


        /// <summary>
        /// Verify is hashed password in DB mathch password passed by user or not
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {


            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }


            }

            return true;
        }

        public async Task<Result> CreateUser(RegisterViewModel UserViewModel)
        {
            // validation
            if (string.IsNullOrWhiteSpace(UserViewModel.Password) || string.IsNullOrWhiteSpace(UserViewModel.Id))
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.IdAndPasswordRequired }
                };



            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(UserViewModel.Password, out passwordHash, out passwordSalt);

            if(UserViewModel.UserType == UserTypeEnum.Citizen)
            {
                User user = new User();
                user.IDNumber = UserViewModel.Id;
                user.NormalizedUserName = UserViewModel.FullName;
                user.FullName = UserViewModel.FullName;
                user.GWTPasswordHash = passwordHash;
                user.GWTPasswordSalt = passwordSalt;
                user.IsActive = true;
                user.CreatedBy = "test";
                user.CreationDate = DateTime.Now;
                _context.Users.Add(user);
                _context.SaveChanges();

                var UserId = _context.Users.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();
                UserStatus userStatus = new UserStatus();
                userStatus.UserId = UserId;
                userStatus.UserStatusEnum = UserStatusEnum.Normal;
                userStatus.CreationDate = DateTime.Now;
                userStatus.IsActive = true;
                userStatus.CreatedBy = "test";
                _context.UserStatus.Add(userStatus);
                _context.SaveChanges();
            }
            else if (UserViewModel.UserType == UserTypeEnum.Shop)
            {
                Shop shop = new Shop();
                shop.LicenseNumber = UserViewModel.Id;
                shop.Name = UserViewModel.FullName;
                shop.GWTPasswordHash = passwordHash;
                shop.GWTPasswordSalt = passwordSalt;
                shop.IsActive = true;
                shop.CreatedBy = "test";
                shop.CreationDate = DateTime.Now;
                shop.CategoryId = UserViewModel.CategoryId;
                _context.Shop.Add(shop);
                _context.SaveChanges();
                var ShopId = _context.Shop.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();
                ShopStatus shopStatus = new ShopStatus();
                shopStatus.ShopId = ShopId;
                shopStatus.UserStatusEnum = UserStatusEnum.Normal;
                shopStatus.CreationDate = DateTime.Now;
                shopStatus.IsActive = true;
                shopStatus.CreatedBy = "test";
                _context.ShopStatus.Add(shopStatus);
                _context.SaveChanges();
            }
            else
            {
                User user = new User();
                user.Email = UserViewModel.Id;
                user.NormalizedUserName = UserViewModel.FullName;
                user.FullName = UserViewModel.FullName;
                user.GWTPasswordHash = passwordHash;
                user.GWTPasswordSalt = passwordSalt;
                user.IsActive = true;
                user.CreatedBy = "test";
                user.CreationDate = DateTime.Now;
                _context.Users.Add(user);
                _context.SaveChanges();

            }


            return new Result()
            {
                Data = true,
                IsSuccess = true,
                Errors = new List<string> { }
            };

        }

        /// <summary>
        /// create hashed password 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
