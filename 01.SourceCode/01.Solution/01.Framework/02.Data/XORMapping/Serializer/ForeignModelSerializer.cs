using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Data.XORMapping
{
    public class ForeignModelSerializer : IDataModelSerilizer
    {

        #region IXORMappingSerializer Members

        public XElement Serialize(ModelBase model)
        {
            throw new NotImplementedException();
        }

        public ModelBase Deserialize(XElement element)
        {
            ForeignModel model = new ForeignModel()
            {
                Name = XmlLinqHelper.GetAttributeValue(element, "name", string.Empty),
                PropertyPrimary = XmlLinqHelper.GetAttributeValue(element, "propertyprimary", string.Empty),
                ForeignKey = XmlLinqHelper.GetAttributeValue(element, "foreignkey", string.Empty),
                InnerSort = XmlLinqHelper.GetAttributeValue(element, "innersort", 0)
            };

            return model;
        }

        #endregion
    }
}
