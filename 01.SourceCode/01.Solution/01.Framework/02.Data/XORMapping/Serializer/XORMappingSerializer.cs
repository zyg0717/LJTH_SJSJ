using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Framework.Core;

namespace Framework.Data.XORMapping
{
    public static class XORMappingSerializer
    {
        public static string Serialize(DataModel model)
        {
            throw new NotImplementedException();
        }

        public static DataModel Deserialize(string fileKey, string modelName)
        {
            //添加缓存
            DataModel result = new DataModel();
            XElement root = MappingFileManager.LoadMappingFile(fileKey);

            IEnumerable<XElement> elements = from item in root.Descendants("datamodel")
                                             where item.Attribute("name").Value == modelName
                                             select item;
            ExceptionHelper.TrueThrow(elements.Count() > 1, string.Format("Model:{0} 存在多个匹配节点", modelName));

            IDataModelSerilizer serializer = SerializerFactory.CreateSerializer("datamodel");
            result = ((DataModel)serializer.Deserialize(elements.First()));

            return result;
        }

        public static DataModel Deserialize(Type type)
        {
            return Deserialize(type.Namespace, type.Name);
        }


        /// <summary>
        /// 针对ForeignModel 反序列化，根据其FullName解析出其文件名和和ModelName
        /// </summary>
        /// <param name="fullName">Type.FullName</param>
        /// <returns></returns>
        public static DataModel Deserialize(string fullName)
        {
            return Deserialize(fullName.Substring(0, fullName.LastIndexOf('.')),
                fullName.Substring(fullName.LastIndexOf('.') + 1));
        }

        private static ModelBase Deserialize(XElement e)
        {
            IDataModelSerilizer serializer = SerializerFactory.CreateSerializer(e.Name.LocalName);

            return serializer.Deserialize(e);
        }
    }
}
