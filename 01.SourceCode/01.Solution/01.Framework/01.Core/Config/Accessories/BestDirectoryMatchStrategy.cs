using System;
using System.Collections.Generic;
using System.Web;
using Framework.Core;
using Framework.Core.Properties;

namespace Framework.Core.Config
{
    /// <summary>
    /// Ѱ��Ŀ¼�������ƥ��һ����㷨
    /// </summary>
    internal sealed class BestDirectoryMatchStrategy : BestPathMatchStrategyBase
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="instances">MetaConfigurationSourceInstanceElementCollection���󣨼��ϣ�</param>
        public BestDirectoryMatchStrategy(MetaConfigurationSourceInstanceElementCollection instances)
        {
            if (instances == null) throw new NullReferenceException("instances");

            if (EnvironmentHelper.Mode == InstanceMode.Web)
                this.path = HttpContext.Current.Request.Url.AbsoluteUri;
            else
                this.path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            this.path = FormatPath(path);
            this.candidates = FileterPath(instances, true);
        }


        /// <summary>
        /// ��·���б���ѡ����Ŀ��·����������һ�
        /// </summary>
        /// <param name="items">·���б�</param>
        /// <returns>��������Ŀ</returns>
        public override string Calculate(IList<KeyValuePair<string, string>> items)
        {
            if ((items == null) || (items.Count <= 0) || string.IsNullOrEmpty(path)) 
                return string.Empty;

            int maxMatchLength = -1;
            string metaConfig = string.Empty;

            if (EnvironmentHelper.Mode == InstanceMode.Web)
            {
                Uri pathAbsolute = new Uri(this.path, UriKind.Absolute);
                this.path = FormatPath(pathAbsolute.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));
            }

            foreach (KeyValuePair<string, string> item in items)
            {
                // ����·��ƥ��
                if (path.StartsWith(item.Key))
                {
                    if (item.Key.Length > maxMatchLength)
                    {
                        maxMatchLength = item.Key.Length;
                        metaConfig = item.Value;
                    }
                    else
                    {
                        if ((item.Key.Length == maxMatchLength) && !string.Equals(metaConfig, item.Value))
                        {
                            string message = string.Format(Resource.ExceptionConflictPathDefinition,
                                item.Key, metaConfig, item.Value);
                            throw new Exception(message);
                        }
                    }
                }
            }

            return metaConfig;
        }
    }
}
