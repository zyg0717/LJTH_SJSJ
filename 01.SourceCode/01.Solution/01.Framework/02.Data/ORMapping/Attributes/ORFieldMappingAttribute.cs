using System;

namespace Framework.Data
{
    /// <summary>
    /// ORM映射属性
    /// </summary>
    /// <remarks>
    /// ORM映射的属性类
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ORFieldMappingAttribute : System.Attribute
    {
        private string dataFieldName = string.Empty;
        private bool isIdentity = false;
        private bool primaryKey = false;
        private bool isUpdateWhere = false;
        private int length = 0;
        private bool isNullable = true;

        /// <summary>
        /// 构造方法
        /// </summary>
        protected ORFieldMappingAttribute()
        {
        }

        /// <summary>
        /// 取字段对应的值
        /// </summary>
        /// <param name="fieldName">字段</param>
        /// <remarks>
        /// 
        /// </remarks>
        public ORFieldMappingAttribute(string fieldName)
        {
            this.dataFieldName = fieldName;
        }

        /// <summary>
        /// 取字段对应的值
        /// </summary>
        /// <param name="fieldName">字段</param>
        /// <param name="nullable">是否为空</param>
        /// <remarks>
        /// 
        /// </remarks>
        public ORFieldMappingAttribute(string fieldName, bool nullable)
            : this(fieldName)
        {
            this.isNullable = nullable;
        }

        /// <summary>
        /// 字段是否可为空
        /// </summary>
        public bool IsNullable
        {
            get { return this.isNullable; }
            set { this.isNullable = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return this.length; }
            set { this.length = value; }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string DataFieldName
        {
            get { return this.dataFieldName; }
            set { this.dataFieldName = value; }
        }

        /// <summary>
        /// 是否标识列
        /// </summary>
        /// <remarks>
        /// 是否标识列，是返回TRUE，否返回FALSE
        /// </remarks>
        public bool IsIdentity
        {
            get { return this.isIdentity; }
            set { this.isIdentity = value; }
        }

        /// <summary>
        /// 是否主键
        /// </summary>
        /// <remarks>
        /// 是否主键，是返回TRUE，否返回FALSE
        /// </remarks>
        public bool PrimaryKey
        {
            get { return this.primaryKey; }
            set { this.primaryKey = value; }
        }

        /// <summary>
        /// 设置此属性则不根据主键更新数据 
        /// </summary>
        /// <remarks>
        /// 是否为更新数据依据条件，是返回TRUE，否返回FALSE
        /// </remarks>
        public bool IsUpdateWhere
        {
            get { return this.isUpdateWhere; }
            set { this.isUpdateWhere = value; }
        }
    }
}
