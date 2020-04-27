using Code9.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Business
{
    public class ExceptionManager
    {
        private IUnitOfWork _unitOfWork;
        public ExceptionManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Log exception in DB
        /// </summary>
        /// <param name="executionPath">Method</param>
        /// <param name="param">Parameters used</param>
        /// <param name="exception">Exception details</param>
        /// <param name="userName">Logged in user</param>
        public void SaveLog(string executionPath, object param, Exception exception, string userName)
        {
            if (string.IsNullOrEmpty(userName))//hot fix till authorization work and pass right user name
                userName = "TestUser";
            _unitOfWork.ErrorLogRepository.SaveLog(executionPath, param, exception, userName);
            _unitOfWork.Save();
        }
    }
}

