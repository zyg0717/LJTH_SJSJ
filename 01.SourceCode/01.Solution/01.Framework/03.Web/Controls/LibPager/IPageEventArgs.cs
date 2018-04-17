﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 翻页事件接口
    /// </summary>
    /// <remarks>
    ///  翻页事件接口
    /// </remarks>
    public interface IPageEventArgs
    {
        /// <summary>
        /// 获取绑定分页控件对应控件的翻页事件Args
        /// </summary>
        /// <param name="controlMode">绑定的控件模型</param>
        /// <param name="eventName">事件名</param>
        /// <param name="commandSource">数据源对象</param>
        /// <param name="newPageIndex">当前的页码</param>
        /// <returns></returns>
        /// <remarks>
        ///  获取绑定分页控件对应控件的翻页事件Args
        /// </remarks>
        object GetPageEventArgs(DataListControlType controlMode, string eventName, object commandSource, int newPageIndex);

        /// <summary>
        /// 设置绑定对应控件的分页属性
        /// </summary>
        /// <param name="objControl">控件对象</param>
        /// <param name="controlMode">对象模型</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        /// <remarks>
        ///  设置绑定对应控件的分页属性
        /// </remarks>
        bool SetBoundControlPagerSetting(object objControl, DataListControlType controlMode, int pageSize);
    }


    /// <summary>
    /// 对象接口
    /// </summary>
    /// <remarks>
    /// 对象接口
    /// </remarks>
    public class PageEventArgs : IPageEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 构造函数
        /// </remarks>
        public PageEventArgs()
        {
            // 
            // TODO: 
            //
        }

        /// <summary>
        /// 获取绑定分页控件对应控件的翻页事件Args
        /// </summary>
        /// <param name="controlMode"></param>
        /// <param name="eventName"></param>
        /// <param name="commandSource"></param>
        /// <param name="newPageIndex"></param>
        /// <returns></returns>
        /// <remarks>
        /// 获取绑定分页控件对应控件的翻页事件Args
        /// </remarks>
        public object GetPageEventArgs(DataListControlType controlMode, string eventName, object commandSource, int newPageIndex)
        {
            object obj = null;
            switch (controlMode)
            {
                case DataListControlType.GridView:
                    switch (eventName)
                    {
                        case "EventPageIndexChanged":
                            EventArgs gvEventArgs = new EventArgs();
                            obj = (object)gvEventArgs;
                            break;
                        case "EventPageIndexChanging":
                            GridViewPageEventArgs gridViewEventArgs = new GridViewPageEventArgs(newPageIndex);
                            obj = (object)gridViewEventArgs;
                            break;
                    }

                    break;
                case DataListControlType.DeluxeGrid:
                    switch (eventName)
                    {
                        case "EventPageIndexChanged":
                            EventArgs gvEventArgs = new EventArgs();
                            obj = (object)gvEventArgs;
                            break;
                        case "EventPageIndexChanging":
                            GridViewPageEventArgs gridViewEventArgs = new GridViewPageEventArgs(newPageIndex);
                            obj = (object)gridViewEventArgs;
                            break;
                    }

                    break;

                case DataListControlType.DetailsView:
                    //DetailsViewPageEventArgs detailsViewEventArgs = new DetailsViewPageEventArgs(newPageIndex);
                    //obj = (object)detailsViewEventArgs;
                    switch (eventName)
                    {
                        case "EventPageIndexChanged":
                            EventArgs detailsViewEventArgs = new EventArgs();
                            obj = (object)detailsViewEventArgs;
                            break;
                        case "EventPageIndexChanging":
                            DetailsViewPageEventArgs dvEventArgs = new DetailsViewPageEventArgs(newPageIndex);
                            obj = (object)dvEventArgs;
                            break;
                    }

                    break;

                case DataListControlType.FormView:
                    //DetailsViewPageEventArgs detailsViewEventArgs = new DetailsViewPageEventArgs(newPageIndex);
                    //obj = (object)detailsViewEventArgs;
                    switch (eventName)
                    {
                        case "EventPageIndexChanged":
                            EventArgs formViewEventArgs = new EventArgs();
                            obj = (object)formViewEventArgs;
                            break;
                        case "EventPageIndexChanging":
                            FormViewPageEventArgs fvEventArgs = new FormViewPageEventArgs(newPageIndex);
                            obj = (object)fvEventArgs;
                            break;
                    }

                    break;

                case DataListControlType.DataGrid:
                    DataGridPageChangedEventArgs dataGridEventArgs = new DataGridPageChangedEventArgs(commandSource, newPageIndex);
                    obj = (object)dataGridEventArgs;

                    break;
            }
            return obj;
        }

        /// <summary>
        /// 设置绑定对应控件的分页属性
        /// </summary>
        /// <param name="objControl"></param>
        /// <param name="controlMode"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <remarks>
        /// 设置绑定对应控件的分页属性
        /// </remarks>
        public bool SetBoundControlPagerSetting(object objControl, DataListControlType controlMode, int pageSize)
        {
            bool bl = true;
            if (objControl == null || pageSize <= 0)
                return false;
            switch (controlMode)
            {
                case DataListControlType.DeluxeGrid:
                    DeluxeGrid dg = (DeluxeGrid)objControl;
                    dg.PageSize = pageSize;
                    break;

                case DataListControlType.GridView:
                    GridView gv = (GridView)objControl;
                    gv.PageSize = pageSize;
                    break;

                case DataListControlType.DataGrid:
                    DataGrid dgOld = (DataGrid)objControl;
                    dgOld.PageSize = pageSize;
                    break;

                default:
                    bl = false;
                    break;
            }
            return bl;
        }
    }
}
