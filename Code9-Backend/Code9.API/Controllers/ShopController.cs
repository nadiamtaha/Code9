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
    [Route("Shop")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopManager _ShopManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;
        private readonly Code9Context _Context;



        public ShopController(IUnitOfWork unitOfWork, Code9Context Context)
        {
            _unitOfwork = unitOfWork;
            _Context = Context;
            _ShopManager = new ShopManager(unitOfWork, Context);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        // GetUserInfo
        [HttpPost]
        [Route("GetUserInfo")]
        public async Task<ActionResult<Result>> GetUserInfo(string id)
        {
            try
            {

                var Result = await _ShopManager.GetUserInfo(id);

                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, id, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }

        // checkin 
        [HttpPost]
        [Route("CheckIn")]
        public async Task<ActionResult<Result>> CheckIn(CheckInOutViewModel CheckInOutVM)
        {
            try
            {

                var Result = await _ShopManager.CheckIn(CheckInOutVM);
                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, CheckInOutVM, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }

        // Checkout
        [HttpPost]
        [Route("Checkout")]
        public async Task<ActionResult<Result>> Checkout(CheckInOutViewModel CheckInOutVM)
        {
            try
            {

                var Result = await _ShopManager.Checkout(CheckInOutVM);
                return Result;


            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, CheckInOutVM, ex, null);
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }







    }
}