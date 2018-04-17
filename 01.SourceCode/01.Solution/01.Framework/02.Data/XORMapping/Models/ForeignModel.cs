using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Data.XORMapping
{
    public class ForeignModel : ModelBase
    {
        public int InnerSort { get; set; }

        /// <summary>
        /// 当前ForeignModel的主键
        /// </summary>
        public string PropertyPrimary { get; set; }

        /// <summary>
        /// 当前ForeignModel的外键
        /// 父对象的主键
        /// </summary>
        public string ForeignKey { get; set; }


        /// <summary>
        /// 父表结构对应的Model
        /// </summary>
        public DataModel ParentModel { get; set; }

        private DataModel originalModel = null;

        /// <summary>
        /// 当前ForeignModel对应的真实DataModel
        /// </summary>
        public DataModel OriginalModel
        {
            get
            {
                if (this.originalModel == null)
                    this.originalModel = XORMappingSerializer.Deserialize(this.Name);
                return this.originalModel;
            }
        }
    }

    public class ForeignModelCollection : DataObjectCollection<ForeignModel>
    {

    }
}
