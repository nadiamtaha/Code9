using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Code9.Data.Repositories.GenericRepository;
using Code9.Entities.Common;
using System.Linq;

namespace Code9.Data.Repositories.Common
{
    /// <summary>
    /// Error Log Repository
    /// </summary>
    public class ErrorLogRepository : GenericRepository<ExceptionLog>
    {
        private readonly Code9Context _context;

        /// <summary>
        /// Constructor for ErrorLogRepository
        /// </summary>
        /// <param name="context"></param>
        public ErrorLogRepository(Code9Context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Saving exception log
        /// </summary>
        /// <param name="executionPath">Method</param>
        /// <param name="param">Parameters used</param>
        /// <param name="exception">Exception details</param>
        /// <param name="userName">Logged in user</param>
        public void SaveLog(string executionPath, object param, Exception exception, string userName)
        {
            try
            {
                ClearTrackedEntities();
                var log = new ExceptionLog
                {
                    Method = executionPath,
                    Exception = SerializeException(exception),

                    Data = SerializeObject(param),

                    UserName = userName,

                    CreatedBy = userName,
                    UpdatedBy = userName,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,

                };
                _context.Add(log);

            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Serialize exception object
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string SerializeException(Exception e)
        {
            try
            {
                return JsonConvert.SerializeObject(e,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            catch (Exception)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Serialze object to string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string SerializeObject(object data)
        {
            try
            {
                return JsonConvert.SerializeObject(data,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            catch (Exception)
            {
                return data.ToString();
            }
        }

        /// <summary>
        /// Clear entities 
        /// </summary>
        private void ClearTrackedEntities()
        {
            //Clear Tracked Entities to avoid re-throwing db related exceptions
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
