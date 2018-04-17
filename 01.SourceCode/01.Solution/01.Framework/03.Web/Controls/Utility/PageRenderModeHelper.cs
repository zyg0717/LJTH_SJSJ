using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Framework.Web.Controls
{
    internal static class PageRenderModeHelper
    {
        private static object _S_PageRenderModeCacheKey = new object();

        public static PageRenderModePageCache GetPageRenderModeCache(Page page)
        {
            PageRenderModePageCache cache = (PageRenderModePageCache)page.Items[_S_PageRenderModeCacheKey];

            if (cache == null)
            {
                cache = new PageRenderModePageCache();
                page.Items[_S_PageRenderModeCacheKey] = cache;
            }

            return cache;
        }

        public static string GetStringFromPageRenderModeCache(PageRenderModePageCache cache)
        {
            return Json.JsonHelper.Serialize(cache);
        }

        public static PageRenderModePageCache GetPageRenderModeCacheFromString(string str)
        {
            return String.IsNullOrEmpty(str) ? new PageRenderModePageCache() : Json.JsonHelper.Deserialize<PageRenderModePageCache>(str);
        }
    }
}
