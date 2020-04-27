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

namespace Code9.API.Controllers
{
    [Route("Dashbord")]
    [ApiController]
    public class DashbordController : ControllerBase
    {

        private readonly CitizenManager _CitizenManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;
        private readonly Code9Context _Context;
        private readonly ShopManager _ShopManager;



        public DashbordController(IUnitOfWork unitOfWork,Code9Context Context)
        {
            _unitOfwork = unitOfWork;
            _Context = Context;
            _ShopManager = new ShopManager(unitOfWork, Context);
            _CitizenManager = new CitizenManager(unitOfWork, Context);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        // GetDashboardData
        [HttpPost]
        [Route("GetDashboardData")]
        public async Task<ActionResult<Result>> GetDashboardData(UserLoginViewModel userViewModel)
        {
            try
            {
                var Result = new Result(); 
                if (userViewModel.UserType == UserTypeEnum.Citizen)
                {
                     Result = await _CitizenManager.GetDashboardData(userViewModel);
                    return Result;
                }
                else if (userViewModel.UserType == UserTypeEnum.Shop)
                {
                     Result = await _ShopManager.GetDashboardData(userViewModel);
                    return Result;

                }
                return Result;

            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, userViewModel, ex,null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }
    }
}