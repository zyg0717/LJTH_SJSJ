using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class ExtensionMethod
    {
        public static Guid ToGuid(this string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);
            return result;
        }


        /// <summary>
        /// 扩展foreach方法
        /// </summary>
        /// <typeparam name="T">集合中元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="action">执行的方法</param>
        /// <returns>执行后的集合</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
            return list;
        }

        #region String Format
        public static string StringFormat(this string s, params object[] paramteres)
        {
            return string.Format(s, paramteres);
        }

        public static string StringFormat(this string s, object arg0)
        {
            return string.Format(s, arg0);
        }

        public static string StringFormat(this string s, object arg0, object arg1)
        {
            return string.Format(s,arg0,arg1);
        }

        public static string StringFormat(this string s, object arg0, object arg1, object arg2)
        {
            return string.Format(s, arg0, arg1, arg2);
        }
        #endregion String Format

    }
}
