using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Framework.Core;
using System.Web;
using System.Reflection;
using System.Web.UI.HtmlControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public static class ScriptControlHelper
    {
        private static string _S_PageLevelKey = "PageLevelKey";
        private static string _S_PageUniqueIDKey = "PageUniqueID";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="page"></param>
        public static void EnsureScriptManager(ref ScriptManager sm, Page page)
        {
            if (sm == null)
            {
                sm = ScriptManager.GetCurrent(page);
                if (sm == null)
                {
                    ExceptionHelper.TrueThrow(page.Form.Controls.IsReadOnly, Resources.DeluxeWebResource.E_NoScriptManager);

                    sm = new ScriptManager();

                    //根据应用的Debug状态来决定ScriptManager的状态 2008-9-18
                    //sm.ScriptMode = ScriptMode.Release;
                    bool debug = WebUtility.GetWebApplicationCompilationDebug();
                    sm.ScriptMode = debug ? ScriptMode.Debug : ScriptMode.Release;

                    sm.EnableScriptGlobalization = true;
                    page.Form.Controls.Add(sm);
                    //throw new HttpException(Resources.DeluxeWebResource.E_NoScriptManager);
                }
            }
            else
            {
                ExceptionHelper.FalseThrow(sm.EnableScriptGlobalization, "页面中ScriptManger对象中属性EnableScriptGlobalization值应该设置为True！");
            }
        }

        /// <summary>
        /// 得到页面Page类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetBasePageTypeInfo(Type type)
        {
            Type baseType = null;

            while (type != null)
            {
                if (type == typeof(Page))
                {
                    baseType = type;
                    break;
                }

                type = type.BaseType;
            }

            return baseType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static int GetPageLevel(PageRenderModePageCache cache)
        {
            int level = cache.GetValue<int>(_S_PageLevelKey, 0);
            return level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="level"></param>
        public static void SetPageLevel(PageRenderModePageCache cache, int level)
        {
            cache[_S_PageLevelKey] = level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static string GetPageUniqueID(PageRenderModePageCache cache)
        {
            string id = cache.GetValue<string>(_S_PageUniqueIDKey, string.Empty);
            return id == null ? string.Empty : (string)id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="id"></param>
        public static void SetPageUniqueID(PageRenderModePageCache cache, string id)
        {
            cache[_S_PageUniqueIDKey] = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctr"></param>
        /// <param name="renderMode"></param>
        public static void CheckOnlyRenderSelf(Control ctr, ControlRenderMode renderMode)
        {
            if (renderMode.OnlyRenderSelf && renderMode.UseNewPage && ctr.Page.Items[WebUtility.PageRenderControlItemKey] != ctr)
            {
                Page currentPage = ctr.Page;
                ctr.Parent.Controls.GetType().GetMethod("SetCollectionReadOnly", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ctr.Parent.Controls, new object[1] { null });
                Page page = new Page();
                PageRenderModePageCache currentPageCache = PageRenderModeHelper.GetPageRenderModeCache(currentPage);
                PageRenderModePageCache pageCache = PageRenderModeHelper.GetPageRenderModeCache(page);
                SetPageLevel(pageCache, GetPageLevel(currentPageCache) + 1);
                string currentPageUniqueID = GetPageUniqueID(currentPageCache);
                if (currentPageUniqueID != string.Empty) currentPageUniqueID += ",";
                SetPageUniqueID(pageCache, string.Format("{0}{1}", GetPageUniqueID(currentPageCache), ctr.UniqueID));
                page.AppRelativeVirtualPath = ctr.Page.AppRelativeVirtualPath;
                page.EnableEventValidation = false;

                InitNewPageContent(page, ctr);

                WebUtility.AttachPageModules(page);

                page.ProcessRequest(HttpContext.Current);

                HttpContext.Current.Response.End();
            }
        }

        private static void InitNewPageContent(Page page, Control ctr)
        {
            HtmlGenericControl html = new HtmlGenericControl("html");
            WebUtility.SetCurrentPage(page);
            page.Items[WebUtility.PageRenderControlItemKey] = ctr;
            page.Controls.Add(html);

            HtmlHead head = new HtmlHead();
            html.Controls.Add(head);
            HtmlGenericControl body = new HtmlGenericControl("body");
            html.Controls.Add(body);
            HtmlForm form = new HtmlForm();
            form.ID = "pageForm";
            form.Controls.Add(ctr);
            body.Controls.Add(form);
            if (!page.IsCallback)
            {
                DeluxeClientScriptManager.RegisterStartupScript(page, "document.getElementById('__VIEWSTATE').value = '';");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        public static ControlRenderMode GetControlRenderMode(Control ctr)
        {
            PageRenderMode pageRenderMode = WebUtility.GetRequestPageRenderMode();
            ControlRenderMode renderMode = new ControlRenderMode(pageRenderMode);

            PageRenderModePageCache currentPageCache = PageRenderModeHelper.GetPageRenderModeCache(ctr.Page);
            PageRenderModePageCache requestPageCache = renderMode.PageCache;

            int currentPageLevel = GetPageLevel(currentPageCache);
            string currentPageUniqueID = GetPageUniqueID(currentPageCache);
            int requestPageLevel = GetPageLevel(requestPageCache);
            string requestPageUniqueID = GetPageUniqueID(requestPageCache);

            if (requestPageLevel == currentPageLevel)
            {
                if (ctr.UniqueID == pageRenderMode.RenderControlUniqueID) renderMode.OnlyRenderSelf = true;
            }
            else if (requestPageLevel > currentPageLevel)
            {
                string id = requestPageUniqueID.Split(',')[currentPageLevel];

                if (ctr.UniqueID == id) renderMode.OnlyRenderSelf = true;
            }

            return renderMode;
        }
    }
}
