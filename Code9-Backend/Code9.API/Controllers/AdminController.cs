using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code9.Business;
using Code9.Data;
using Code9.Data.UnitOfWork;
using Code9.Shared;
using Code9.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Code9.API.Controllers
{
    [Route("Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminManager _AdminManager;
        private readonly ShopManager _ShopManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;
        private readonly Code9Context _Context;



        public AdminController(IUnitOfWork unitOfWork, IOptions<AppSettingsViewModel> appSettings, Code9Context Context)
        {
            _unitOfwork = unitOfWork;
            _Context = Context;
            _AdminManager = new AdminManager(Context, appSettings, unitOfWork);
            _ShopManager = new ShopManager(unitOfWork, Context);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        // GetUserInfo
        [HttpPost]
        [Route("Search")]
        public async Task<ActionResult<Result>> Search(UserLoginViewModel userVm)
        {
            try
            {
                var Result = new Result() ;
                if (userVm.UserType == UserTypeEnum.Citizen)
                {
                     Result = await _ShopManager.GetUserInfoForSerach(userVm.Id);

                }
                else if (userVm.UserType == UserTypeEnum.Shop)
                {
                     Result = await _ShopManager.GetShopInfo(userVm.Id);

                }
                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, userVm, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }



        // EditStatus
        [HttpPost]
        [Route("EditStatus")]
        public async Task<ActionResult<Result>> EditStatus(EditStatusViewModel EditStatusVM)
        {
            try
            {
      
                var Result = new Result();
                if (EditStatusVM.UserType == UserTypeEnum.Citizen)
                {
                     Result = await _AdminManager.EditCitizenStatus(EditStatusVM);

                }
                else if (EditStatusVM.UserType == UserTypeEnum.Shop)
                {
                     Result = await _AdminManager.EditShopStatus(EditStatusVM);

                }
                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, EditStatusVM, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }



        // RegisterDeviceToken
        [HttpPost]
        [Route("RegisterDeviceToken")]
        public async Task<ActionResult<Result>> RegisterDeviceToken(RegisterTokenViewModel RegisterTokenVM)
        {
            try
            {
                var Result =  _AdminManager.RegisterDeviceToken(RegisterTokenVM);

                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, RegisterTokenVM, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }
        
        [HttpPost]
        [Route("UnRegisterDeviceToken")]
        public async Task<ActionResult<Result>> UnRegisterDeviceToken(RegisterTokenViewModel RegisterTokenVM)
        {
            try
            {
                var Result = _AdminManager.UnRegisterDeviceToken(RegisterTokenVM);

                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, RegisterTokenVM, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }


        


        [HttpGet]
        [Route("TestNotifications")]
        public async Task<ActionResult<Result>> TestNotifications()
        {
            try
            {
                var Result = _AdminManager.TestNotifications();

                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, null, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }


    }
}