using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Code9.Data;
using Code9.Entities.Models;
using Code9.Business;
using Code9.Shared.ViewModels;
using Code9.Data.UnitOfWork;
using Microsoft.Extensions.Options;
using Code9.Shared;

namespace Code9.API.Controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserManager _userManager;
        private readonly AppSettingsViewModel _appSettings;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;
        public AccountController(IUnitOfWork unitOfWork,
           IUserManager userService,
           IOptions<AppSettingsViewModel> appSettings)
        {
            _userManager = userService;
            _appSettings = appSettings.Value;
            _unitOfwork = unitOfWork;

            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        /// <summary>
        /// Login using email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public ActionResult<Result> Login(UserLoginViewModel model)
        {
            try
            {
                var result = _userManager.Login(model);

                return result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, model, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }

        // POST : CreateUser
        [Route("CreateUser")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(RegisterViewModel UserViewModel)
        {
            try
            {
                var Result = _userManager.CreateUser(UserViewModel);
                return await Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, UserViewModel, ex,null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }



    }
}
