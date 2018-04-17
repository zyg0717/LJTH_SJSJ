using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;

namespace Framework.Web.Controls
{
    /// <summary>
    /// PageRenderModePageCache
    /// </summary>
    public class PageRenderModePageCache : Dictionary<string, object>
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, T defautValue)
        {
            T v = DictionaryHelper.GetValue<string, object, T>(this, key, defautValue);

            return v;
        }
    }
}
