using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Data.XORMapping
{
    public interface IDataModelSerilizer
    {
        XElement Serialize(ModelBase model);

        ModelBase Deserialize(XElement element);
    }    

    public class SerializerFactory
    {
        public static IDataModelSerilizer CreateSerializer(string name)
        {
            switch (name)
            {
                case "datamodel":
                    return new DataModelSerializer();
                case "property":
                    return new DataPropertySerializer();
                case "foreignmodel":
                    return new ForeignModelSerializer();
                default:
                    throw new Exception("没有找到对应节点序列化器");
            }
        }
    }
}
