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
    [Route("Citizen")]
    [ApiController]
    public class CitizenController : ControllerBase
    {
        private readonly CitizenManager _CitizenManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;
        private readonly Code9Context _Context;



        public CitizenController(IUnitOfWork unitOfWork, Code9Context Context)
        {
            _unitOfwork = unitOfWork;
            _Context = Context;
            _CitizenManager = new CitizenManager(unitOfWork, Context);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        // GetCategories
        [HttpPost]
        [Route("GetCategories")]
        public async Task<ActionResult<Result>> GetCategories(string id)
        {
            try
            {

                   var Result = await _CitizenManager.GetCategories(id);
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


    }
}