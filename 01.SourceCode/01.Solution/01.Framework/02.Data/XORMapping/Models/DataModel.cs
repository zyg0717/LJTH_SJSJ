using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Data.XORMapping
{
    public class ModelBase
    {
        public string Name { get; set; }

        public string TypeName { get; set; }
    }

    public class DataModel : ModelBase
    {
        public Type Type
        {
            get;
            set;
        }     

        public string TableName { get; set; }

        private DataPropertyCollection primaryKey = null;
        public DataPropertyCollection PrimaryKey
        {
            get
            {
                if (this.primaryKey == null)
                {
                    this.primaryKey = new DataPropertyCollection();
                    this.Properties.ForEach(property =>
                        {
                            if (property.Primary)
                                this.primaryKey.Add(property);
                        });
                }
                return this.primaryKey;
            }
        }

        private DataPropertyCollection properties = null;
        public DataPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                    this.properties = new DataPropertyCollection();
                return this.properties;
            }
            set
            {
                this.properties = value;
            }
        }

        private ForeignModelCollection foreignModels = null;
        public ForeignModelCollection ForeignModels
        {
            get
            {
                if (this.foreignModels == null)
                    this.foreignModels = new ForeignModelCollection();
                return this.foreignModels;
            }
            set
            {
                this.foreignModels = value;
            }
        }
    }   

    public class DataModelCollection : DataObjectCollection<DataModel>
    { 
        
    }

    
}
