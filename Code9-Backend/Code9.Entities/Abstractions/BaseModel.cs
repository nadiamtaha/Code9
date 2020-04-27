using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Code9.Entities.Abstractions
{
    /// <summary>
    /// Base Entity Structure for all system entities
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Record's Primary Key
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Record's Created-By User
        /// 
        /// </summary>
        public string CreatedBy { get; set; }



        /// <summary>
        /// Record's Last Update-By User if Any
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Record's Creation Date
        /// For Audit Purpose
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Record's Last Modification Date
        /// For Audit Purpose
        /// </summary>
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Does this record marked as deleted or not
        /// </summary>
        public bool IsActive { get; set; }
    }
}
