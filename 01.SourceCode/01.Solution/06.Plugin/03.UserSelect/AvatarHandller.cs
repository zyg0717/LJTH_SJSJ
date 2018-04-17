using Plugin.Storage;
using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace Plugin.UserSelect
{
    public class AvatarHandller : IHttpHandler
    {
        /// <summary>
        /// 您将需要在网站的 Web.config 文件中配置此处理程序 
        /// 并向 IIS 注册它，然后才能使用它。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //在此处写入您的处理程序实现。

            var request = context.Request;
            var response = context.Response;

            var userId = Convert.ToInt32(request["uid"]);
            VEmployeeEntity model = VEmployeeOperator.Instance.GetEmployeeByID(userId);
            try
            {
                using (NASStorageProvider provider = new NASStorageProvider(address, userName, userPwd))
                {
                    var bytes = provider.LoadFile(address + model.Thumb.Replace("/", "\\"));
                    MemoryStream stream = new MemoryStream(bytes);
                    stream.Seek(0, SeekOrigin.Begin);

                    response.ClearContent();
                    response.ContentType = "image/jpeg";
                    response.BinaryWrite(bytes);
                    response.Flush();
                    response.End();
                }
            }
            catch (Exception ex)
            {
                response.ClearContent();
                response.Write("图片加载失败");
                response.Flush();
                response.End();
                //throw new Exception(string.Format("文件上传失败：{0}，{1}", ex.Message, ex.StackTrace));
            }
            var filePath = model.Thumb;

        }

        private static readonly string address = ConfigurationManager.AppSettings["UserProfile.Avatar.Address"];
        private static readonly string userName = ConfigurationManager.AppSettings["UserProfile.Avatar.UserName"];
        private static readonly string userPwd = ConfigurationManager.AppSettings["UserProfile.Avatar.UserPwd"];

        #endregion
    }
}
