using Code9.Entities.Abstractions;
using Code9.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Entities.Models
{
    public class UserDevice : BaseModel
    {
        /// <summary>
        ///  User
        /// Navigation Property
        /// </summary>
        public User User { get; set; }



        /// <summary>
        /// User Id
        /// Foreign Key
        /// </summary>
        public string UserId { get; set; }




        /// <summary>
        /// User Device Token
        /// </summary>
        public string Token { get; set; }



        /// <summary>
        /// Device Name
        /// </summary>
        public string DeviceName { get; set; }



        /// <summary>
        /// Device Type
        /// </summary>

        public virtual DeviceType Type { get; set; }
    }
}
