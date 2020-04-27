using Code9.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Code9.Entities.Common
{
    public class ExceptionLog : BaseModel
    {
        /// <summary>
        /// The method which had exception
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Method { get; set; }

        /// <summary>
        /// Parameters used if any
        /// </summary>
        [Required]
        public string Data { get; set; }

        /// <summary>
        /// Exception Details
        /// </summary>
        [Required]
        public string Exception { get; set; }


        /// <summary>
        /// Logged User name - while exception happen - if any
        /// </summary>
        [MaxLength(128)]
        public string UserName { get; set; }


    }
}