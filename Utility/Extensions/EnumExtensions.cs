using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Utility.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum[] Enumerate<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
        }

        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static string GetDisplayNameOrDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null)
                return "";

            DisplayNameAttribute dna = Attribute.GetCustomAttribute(field, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            if (dna != null)
                return dna.DisplayName;

            DisplayAttribute da = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
            if (da != null)
                return da.Name;

            DescriptionAttribute desca = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return desca == null ? value.ToString() : desca.Description;
        }
    }
}
