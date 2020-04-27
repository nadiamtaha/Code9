using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Code9.Shared
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserStatusEnum
    {
        [Display(Name = "Normal", ResourceType = typeof(Resources))]
        Normal = 1,
        [Display(Name = "Suspected", ResourceType = typeof(Resources))]
        Suspected = 2,
        [Display(Name = "Infected", ResourceType = typeof(Resources))]
        Infected = 3
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserTypeEnum
    {
        [Display(Name = "Citizen", ResourceType = typeof(Resources))]
        Citizen = 1,
        [Display(Name = "Shop", ResourceType = typeof(Resources))]
        Shop = 2,

    }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        [Display(Name = "Male", ResourceType = typeof(Resources))]
        Male = 1,
        [Display(Name = "Female", ResourceType = typeof(Resources))]
        Female = 2,

    }
    public enum DeviceType
    {
        Ios=1,
        Android=2
    }
}
