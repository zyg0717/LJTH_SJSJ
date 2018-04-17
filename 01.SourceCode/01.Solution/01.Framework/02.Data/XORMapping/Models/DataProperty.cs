using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Framework.Data.XORMapping
{
    public class DataProperty : ModelBase
    {
        /// <summary>
        /// 数据库中的列明
        /// </summary>
        public string FieldName { get; set; }

        public int InnerSort { get; set; }

        public bool Primary { get; set; }

        public bool InputMapping { get; set; }

        public bool OutputMapping { get; set; }

        public string DefaultValue { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }

        public MemberInfo MemberInfo
        {
            get;
            set;
        }

        public DataModel Model { get; set; }
    }

    public class DataPropertyCollection : KeyedCollection<string, DataProperty>
    {
        protected override string GetKeyForItem(DataProperty item)
        {
            return item.FieldName;
        }

        public void ForEach(Action< DataProperty> act)
        {
            foreach (KeyValuePair<string, DataProperty> pair in Dictionary)
            {
                act(pair.Value);
            }
        }
    }
}
