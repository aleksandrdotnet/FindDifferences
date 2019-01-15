using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Search2.Static
{
    public static class EnumExtension
    {
        /// <summary>
        /// Get Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">Enum</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            object[] attribs = field.GetCustomAttributes(typeof(T), false);
            T result = default(T);

            if (attribs.Any())
            {
                result = attribs.First() as T;
            }

            return result;
        }

        /// <summary>
        /// Get Description in Enum
        /// </summary>
        /// <param name="currentEnum">Enum</param>
        /// <returns></returns>
        public static string GetDescription(this Enum currentEnum)
        {
            var fi = currentEnum.GetType().GetField(currentEnum.ToString());

            if (fi != null)
            {
                var da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                return da != null ? da.Description : currentEnum.ToString();
            }
            return "";
        }

        /// <summary>
        /// Next Value in Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static T Next<T>(this T src, T[] skip) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");

            var arr = (T[])Enum.GetValues(typeof(T));

            if (skip.Any())
            {
                var col = arr.Except(skip).ToArray();

                if (col.Any())
                {
                    int i = Array.IndexOf(col, src) + 1;
                    return (col.Length == i) ? col[0] : col[i];
                }

                return src;
            }

            int j = Array.IndexOf(arr, src) + 1;
            return (arr.Length == j) ? arr[0] : arr[j];
        }
        
        /// <summary>
        /// Get Value Enum from description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(@"Not found.", nameof(description));
            //return default(T);
        }
    }
}