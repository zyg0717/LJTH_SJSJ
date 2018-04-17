using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Common
{
    /// <summary>
    /// 对应数据库中的主键
    /// </summary>
    public class PrimaryKey : Attribute
    {
    }

    /// <summary>
    /// 自增长ID
    /// </summary>
    public class Identity : Attribute
    {
    }

    /// <summary>
    /// 对应数据库中的外键
    /// </summary>
    public class ForeignKey : Attribute
    {
    }

    public class DisableFilter : Attribute
    { }

    public class UniqueCheck : Attribute
    { }

    public class ParentFiled : Attribute
    { }

    public class Ignore : Attribute
    { }

    /// <summary>
    /// 对应数据库中表的名称
    /// </summary>
    public class TableName : Attribute
    {
        string name = string.Empty;

        public TableName(string tableName)
        {
            name = tableName;
        }

        public string Name
        {
            get { return name; }
        }

        protected void ResetName(string name)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// 外键依赖表
    /// </summary>
    public class ReferenceTableName : Attribute
    {
        private string _name = string.Empty;

        public ReferenceTableName(string tableName)
        {
            this._name = tableName;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }

    public class Length : Attribute
    {
        int fieldLenth = 0;
        public Length(int length)
        {
            fieldLenth = length;
        }

        public int FieldLength
        {
            get { return fieldLenth; }
        }
    }
    /// <summary>
    /// 对应计划大类
    /// </summary>
    public class PrimaryType : Attribute
    {
        string _typeNames = string.Empty;
        public PrimaryType(string Types)
        {
            _typeNames = Types;
        }

        public string TypeNums
        {
            get { return _typeNames; }
        }
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public class ItemTypeAttribute : Attribute
    {
        public Int32 ItemType { get; private set; }

        public ItemTypeAttribute(Int32 itemType)
        {
            /*
             * 为保证兼容旧逻辑（按位运算过滤节点）
             * 节点类型只支持以下几种
             */
            switch (itemType)
            {
                case 1:
                case 2:
                case 4:
                case 8:
                    this.ItemType = itemType;
                    break;
                default:
                    throw new ArgumentException("节点类型不正确", "itemType");
            }
        }
    }

    public class BizStageAttrib : Attribute
    {
        string _stageNum = string.Empty;
        public BizStageAttrib(string stage)
        {
            _stageNum = stage;
        }

        public string BizStageNum
        {
            get { return _stageNum; }
        }
    }
    /// <summary>
    /// HR职位级别名称
    /// </summary>
    public class HRJobNameAttrib : Attribute
    {
        public string HRJobName { get; private set; }
        public HRJobNameAttrib(string hrJobName) { this.HRJobName = hrJobName; }
    }
}
