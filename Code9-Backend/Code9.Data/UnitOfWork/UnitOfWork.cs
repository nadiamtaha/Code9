
using Code9.Data.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Code9.Data;
using Code9.Data.Repositories;
using Code9.Data.Repositories.Common;
using Code9.Data.UnitOfWork;
using System;

namespace Code9.Data.UnitOfWork
{
    /// <summary>
    /// Unit of work concrete class
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Private members
        private Code9Context _context = null;

        private ErrorLogRepository _errorLogRepository = null;


        #endregion

        /// <summary>
        /// Constructor for UnitOfWork
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(Code9Context context)
        {
            _context = context;
        }


        #region Repositories

        /// <summary>
        /// Error Log Repository
        /// </summary>
        public ErrorLogRepository ErrorLogRepository
        {
            get
            {
                if (_errorLogRepository == null)
                {
                    _errorLogRepository = new ErrorLogRepository(_context);
                }

                return _errorLogRepository;
            }
        }
        #endregion


        /// <summary>
        /// Disposing 
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Commit saving entities 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            int saved = _context.SaveChanges();
            if (saved > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
