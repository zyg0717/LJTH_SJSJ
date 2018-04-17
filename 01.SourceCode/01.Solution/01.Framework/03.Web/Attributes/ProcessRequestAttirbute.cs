using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Web
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ProcessRequestAttribute : Attribute
    {
        private bool isDefault = true;

        /// <summary>
        /// 构造方法
        /// </summary>
        public ProcessRequestAttribute()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="defaultMethod"></param>
        public ProcessRequestAttribute(bool defaultMethod)
        {
            this.isDefault = defaultMethod;
        }

        /// <summary>
        /// 是否是缺省的方法
        /// </summary>
        public bool Default
        {
            get { return this.isDefault; }
            set { this.isDefault = value; }
        }
    }
}
