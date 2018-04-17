using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core;

namespace Wanda.RCSJSJ.Common.Web
{
    //[Serializable]
    //public class SSoConfiguration
    //{
    //    /// <summary>
    //    /// 服务器地址
    //    /// </summary>
    //    public string Host { get; set; }
    //    /// <summary>
    //    /// 域名
    //    /// </summary>
    //    public string Domain { get; set; }
    //    /// <summary>
    //    /// 接口地址（绝对路径）
    //    /// </summary>
    //    public string ApiUrl { get; set;}

    //    /// <summary>
    //    /// 客户系统系统编码
    //    /// </summary>
    //    public string ClientSystemCode { get; set; }

    //    /// <summary>
    //    /// 返回值得类型
    //    /// </summary>
    //    public string ResultMIMEType { get; set; }

    //    /// <summary>
    //    /// 单点登录站点
    //    /// </summary>
    //    public string SsoLoginSite { get; set; }

    //}

    //public class SSoConfigurationManage
    //{
    //    public readonly static SSoConfigurationManage Instance = new SSoConfigurationManage();
    //    private static SSoConfiguration setting = new SSoConfiguration();
    //    private SSoConfigurationManage() { }
        
    //    /// <summary>获取配置文件的路径</summary>
    //    private string GetConfigFullPath()
    //    {

    //        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\SSoConfiguration.config");
    //    }
    //    /// <summary>获取实体对象</summary>
    //    public SSoConfiguration GetSSoConfiguration()
    //    {
    //        object obj = new object();
    //        try
    //        {
    //            lock (obj)
    //            {
    //                if (setting == null)
    //                {
    //                    setting = (SSoConfiguration)SerializationHelper.XmlDeserialize(typeof(SSoConfiguration), GetConfigFullPath());
    //                }
    //                return setting;
    //            }
    //        }
    //        catch (System.Exception ex)
    //        { throw ex; }
    //    }
    //}

    public class AuthEntity
    {
        public string ResultCode { get; set; }
        public string ResultMessageCN { get; set; }

        public string ResultMessageEN { get; set; }

        public DateTime AuthTime { get; set; }

        public DateTime ExpireTime { get; set; }

        public string AuthNum { get; set; }

        public string AuthToken { get; set; }

        public string AuthMAC { get; set; }
    }
   
}
