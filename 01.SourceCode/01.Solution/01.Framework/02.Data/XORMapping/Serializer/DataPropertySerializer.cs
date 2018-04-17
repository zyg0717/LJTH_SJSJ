using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Data.XORMapping
{
    public class DataPropertySerializer : IDataModelSerilizer
    {

        #region IXORMappingSerializer Members

        public XElement Serialize(ModelBase model)
        {
            throw new NotImplementedException();
        }

        public ModelBase Deserialize(XElement element)
        {
            DataProperty property = new DataProperty()
            {
                Name = XmlLinqHelper.GetAttributeValue(element, "name", string.Empty),
                FieldName = XmlLinqHelper.GetAttributeValue(element, "fieldname", string.Empty),
                InnerSort = XmlLinqHelper.GetAttributeValue(element, "innersort", 0),
                TypeName = XmlLinqHelper.GetAttributeValue(element, "type", string.Empty),
                Primary = XmlLinqHelper.GetAttributeValue(element, "primary", false),
                InputMapping = XmlLinqHelper.GetAttributeValue(element, "inputmapping", false),
                OutputMapping = XmlLinqHelper.GetAttributeValue(element, "outputmapping", false),
                DefaultValue = XmlLinqHelper.GetAttributeValue(element, "defaultvalue", string.Empty)
            };
            return property;
        }

        #endregion
    }
}
