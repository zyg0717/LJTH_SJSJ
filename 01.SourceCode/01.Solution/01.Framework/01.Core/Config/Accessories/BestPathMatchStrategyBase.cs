
using System;
using System.Collections.Generic;
using System.IO;

using Framework.Core;

namespace Framework.Core.Config
{
    /// <summary>
    /// ·��������ƥ���㷨ʵ����
    /// </summary>
    internal abstract class BestPathMatchStrategyBase : IStrategy<IList<KeyValuePair<string, string>>, string>
    {
        #region Protected field
        protected string path;
        protected IList<KeyValuePair<string, string>> candidates;
        #endregion

        /// <summary>
        /// IStrategy IList �ĳ���ʵ��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract string Calculate(IList<KeyValuePair<string, string>> data);


        /// <summary>
        /// ��ɸѡ���ļ�����
        /// </summary>
        public virtual IList<KeyValuePair<string, string>> Candidates
        {
            get { return candidates; }
        }


        /// <summary>
        /// �ж�ĳ��·���Ƿ�ΪĿ¼
        /// </summary>
		/// <param name="folderPath">·��</param>
        /// <returns></returns>
        protected static bool IsDirectory(string folderPath)
        {
			return string.IsNullOrEmpty(Path.GetFileName(folderPath));
        }

        /// <summary>
        /// ����·�����ͣ��ļ�/Ŀ¼���͸�ʽɸѡ·��
        /// </summary>
        /// <param name="instances">MetaConfigurationSourceInstanceElementCollection</param>
        /// <param name="isDirectory">�Ƿ�Ŀ¼</param>
        /// <returns>KeyValuePair �� Value Ϊ meta.config �ļ�, key Ϊapplication ·��</returns>
        protected IList<KeyValuePair<string, string>> FileterPath(MetaConfigurationSourceInstanceElementCollection instances, bool isDirectory)
        {
            if ((instances == null) || (instances.Count <= 0)) return null;

            IList<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            InstanceMode mode = EnvironmentHelper.Mode;
            
			Uri pathAbsolute = null;

            foreach (MetaConfigurationSourceInstanceElement instance in instances)
            {
                if ((instance == null) || (instance.Mappings == null) 
                    || (instance.Mappings.Count <= 0) || (instance.GetMode() != mode)) 
                    continue;

                string metaConfig = FormatPath(instance.Path);
 
                foreach (MetaConfigurationSourceMappingElement mapping in instance.Mappings)
                {
                    string applicationPath = mapping.Application;
                    if(false == (isDirectory ^ IsDirectory(applicationPath)))
                    {
                        if (mode == InstanceMode.Web)
                        {
                            //����������ʱ����·����ת��Ϊͨ�����·������ƥ�� 
							if (pathAbsolute == null)
								pathAbsolute = new Uri(path, UriKind.Absolute);

                            Uri appAbsolute = new Uri(applicationPath, UriKind.RelativeOrAbsolute);

                            if (appAbsolute.IsAbsoluteUri)
                            {
                                if (pathAbsolute.Scheme == appAbsolute.Scheme &&
                                    pathAbsolute.Port == appAbsolute.Port &&
                                    pathAbsolute.UserInfo == appAbsolute.UserInfo &&
                                    pathAbsolute.Host == appAbsolute.Host &&
                                    pathAbsolute.HostNameType == appAbsolute.HostNameType)
                                {
                                    applicationPath = appAbsolute.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                                }
                            }
                           
                           applicationPath = FormatPath(applicationPath);
   
                        }
                        else
                        {
                            applicationPath = FormatPath(Path.GetFullPath(applicationPath));
                        }

                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(applicationPath, metaConfig);
                        result.Add(item);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// ��·��ȥ���ո�ת��ΪСд
        /// </summary>
		/// <param name="filePath"></param>
        /// <returns></returns>
        protected static string FormatPath(string filePath)
        {
			return filePath.ToLower().Trim();
        }
    }
}
