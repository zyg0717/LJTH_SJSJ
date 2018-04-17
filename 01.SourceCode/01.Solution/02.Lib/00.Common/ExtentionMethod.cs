using Framework.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace Lib.Common
{
    public static class ExtensionMethod
    {
        public static DateTime ValidateSqlMinDate(this DateTime date)
        {
            var minDateTime = Convert.ToDateTime(SqlDateTime.MinValue.ToString());
            if (date < minDateTime)
            {
                date = minDateTime;
            }
            return date;
        }
        public static string ToFilterString(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
        public static Guid ToGuid(this string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);
            return result;
        }

        public static Stream ToStream(this byte[] bytes)
        {
            MemoryStream stream = new System.IO.MemoryStream(bytes);
            return stream;
        }

        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
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
        /// <summary>
        /// 从资源中加载字符串
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadStringFromResource(this Assembly assembly, string path)
        {
            using (Stream stm = GetResourceStream(assembly, path))
            {
                using (StreamReader sr = new StreamReader(stm))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 从资源中得到流
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream GetResourceStream(Assembly assembly, string path)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(assembly != null, "assembly");
            ExceptionHelper.CheckStringIsNullOrEmpty(path, "path");

            Stream stm = assembly.GetManifestResourceStream(path);

            ExceptionHelper.FalseThrow(stm != null, "不能在Assembly:{0}中找到资源{1}", assembly.FullName, path);

            return stm;
        }
    }
}
