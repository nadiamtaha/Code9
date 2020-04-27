
using Code9.Data.Repositories;
using Code9.Data.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Data.UnitOfWork
{
    /// <summary>
    /// Unit of work interface 
    /// </summary>
    public interface IUnitOfWork 
    {
       
        /// <summary>
        /// Repository that can be use for logging
        /// </summary>
        public ErrorLogRepository ErrorLogRepository { get; }


        /// <summary>
        /// Commit saving entities 
        /// </summary>
        /// <returns></returns>
        bool Save();
    }
}
