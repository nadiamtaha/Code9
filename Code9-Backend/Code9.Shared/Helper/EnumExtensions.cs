using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Text;

namespace Code9.Shared.Helper
{
    public static class EnumExtensions
    {

        public static string GetDescription(this Enum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());



            var attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);



            if (attributes.Length > 0)
                return new ResourceManager(attributes[0].ResourceType).GetString(attributes[0].Name);
            return enumValue.ToString();
        }
    }
}
