using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Framework.Core;

namespace Framework.Data.XORMapping
{
    public class MappingFileManager
    {
        public static List<XElement> LoadMappingFile()
        {
            List<XElement> result = new List<XElement>();
            string baseDir = GetBaseDir();

            MappingFileElementCollection collection = MappingFileSection.GetConfig().FileLocations;
            foreach (MappingFileElement element in collection)
            {
                result.Add(LoadMappingFile(element));
            }
            return result;
        }

        public static XElement LoadMappingFile(string fileKey)
        {
            MappingFileElement element = MappingFileSection.GetConfig().FileLocations[fileKey];
            ExceptionHelper.TrueThrow(element == null, string.Format("没有与fileKey :{0}匹配的配置信息", fileKey));
            return LoadMappingFile(element);
        }

        public static XElement LoadMappingFile(MappingFileElement element)
        {
            using (XmlReader reader = XmlReader.Create(GetFilePath(element)))
            {
                while (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                }
                return XElement.Load(reader);
            }
        }

        private static string GetBaseDir()
        {
            string baseDir = MappingFileSection.GetConfig().BaseDir;
            ExceptionHelper.TrueThrow(string.IsNullOrWhiteSpace(baseDir), string.Format("配置信息baseDir不能为空"));
            ExceptionHelper.FalseThrow(Directory.Exists(baseDir), string.Format("未找到baseDir：{0}对应的目录", baseDir));
            return baseDir;
        }

        private static string GetFilePath(string modelName)
        {
            return GetFilePath(MappingFileSection.GetConfig().FileLocations[modelName]);
        }

        private static string GetFilePath(MappingFileElement element)
        {            
            ExceptionHelper.TrueThrow(string.IsNullOrWhiteSpace(element.FileName), string.Format("配置信息fileName不能为空"));
            string filePath = Path.Combine(GetBaseDir(), element.FileName);
            ExceptionHelper.FalseThrow(File.Exists(filePath), string.Format("未找到filePath：{0}对应的文件", filePath));
            return filePath;
        }
    }
}
