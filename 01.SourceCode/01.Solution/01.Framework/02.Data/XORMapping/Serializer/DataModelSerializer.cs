using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Data.XORMapping
{
    public class DataModelSerializer : IDataModelSerilizer
    {
        #region IXORMappingSerializer Members

        public XElement Serialize(ModelBase model)
        {
            throw new NotImplementedException();
        }

        public ModelBase Deserialize(XElement element)
        {
            DataModel model = new DataModel()
            {
                Name = XmlLinqHelper.GetAttributeValue(element, "name", string.Empty),
                TableName = XmlLinqHelper.GetAttributeValue(element, "tablename", string.Empty)
            };

            foreach (XElement pe in element.Elements("property"))
            {
                IDataModelSerilizer serializer = new DataPropertySerializer();
                DataProperty property = serializer.Deserialize(pe) as DataProperty;
                if (property != null)
                    property.Model = model;
                model.Properties.Add(property);
            }

            foreach (XElement fe in element.Descendants("foreignmodel"))
            {
                IDataModelSerilizer serializer = new ForeignModelSerializer();
                ForeignModel fm = serializer.Deserialize(fe) as ForeignModel;
                if (fm != null)
                    fm.ParentModel = model;

                model.ForeignModels.Add(fm);
            }

            return model;
        }

        #endregion
    }

}
