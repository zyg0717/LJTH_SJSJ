/*
 更新日志：
 1.0：初版 v_zhouqi
 1.1：放弃使用4.5部分类库 增强向下兼容性 v_zhouqi
 1.2：流预览接口添加可选参数withUser，用以标识当前获取的链接是否仅当前人可访问（默认为true）
 */

/*
 Description：文件平台相关通用帮助类集合
 Authors：周启（v_zhouqi）
 Version：1.2
 Date：2017年6月26日
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Wanda.IO.FileSystem.Helpers
{
    #region Customer Exception

    /// <summary>
    /// 无接口访问权限
    /// </summary>
    public class NoRightException : Exception
    {
        public NoRightException(string errorMessage) : base(errorMessage)
        {

        }
    }
    /// <summary>
    /// 参数输入不正确
    /// </summary>
    public class InputParameterException : Exception
    {
        public InputParameterException(string errorMessage) : base(errorMessage)
        {

        }
    }
    /// <summary>
    /// 服务端验证不通过
    /// </summary>
    public class ServerValidationException : Exception
    {
        public ServerValidationException(string errorMessage) : base(errorMessage)
        {
        }
    }
    /// <summary>
    /// 服务端运行异常
    /// </summary>
    public class ServerRuntimeException : Exception
    {
        public ServerRuntimeException(string errorMessage) : base(errorMessage)
        {

        }
    }
    /// <summary>
    /// 未知错误
    /// </summary>
    public class UnknownException : Exception
    {
        public UnknownException(string errorMessage) : base(errorMessage)
        {

        }
    }
    #endregion

    #region HelperCommon
    /// <summary>
    /// 用户来源枚举
    /// </summary>
    public enum UserFromSourceEnum
    {
        /// <summary>
        ///  内网用户
        /// </summary>
        Inside,
        /// <summary>
        /// 外网用户
        /// </summary>
        OutSide
    }
    public static class CommonHelper
    {
        /// <summary>
        /// url段落结尾
        /// </summary>
        private const string _Suffix = "/";
        #region Common Methods
        /// <summary>
        /// 把文件流转换成字节数组
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static byte[] ToByteArray(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 构建请求参数的字符串（形如：key=value，使用"&"进行连接）
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        public static string BuildRequestParameters(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }

        /// <summary>
        /// 构建请求地址
        /// </summary>
        /// <param name="baseUrl">基础地址</param>
        /// <param name="version">接口版本</param>
        /// <param name="resourceName">请求资源</param>
        /// <param name="resourcePropertyName">资源属性名称</param>
        /// <returns></returns>
        public static string BuildRequestUrl(string baseUrl, string version, string resourceName, string resourcePropertyName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(baseUrl);
            if (!baseUrl.EndsWith(_Suffix))
            {
                builder.Append(_Suffix);
            }
            builder.Append(version); builder.Append(_Suffix);
            builder.Append(resourceName); builder.Append(_Suffix);
            builder.Append(resourcePropertyName);// builder.Append(_Suffix);
            return builder.ToString();
        }


        /// <summary>
        /// 尝试读取响应状态 如果响应的状态码不正确会抛出自定义的异常
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public static void TryReadResponseStatus(HttpStatusCode statusCode, string message)
        {
            if (statusCode != HttpStatusCode.OK)
            {
                if (statusCode == HttpStatusCode.Forbidden)
                    throw new NoRightException(message);
                else if (statusCode == HttpStatusCode.BadRequest)
                    throw new InputParameterException(message);
                else if (statusCode == HttpStatusCode.InternalServerError)
                    throw new ServerRuntimeException(message);
                else
                    throw new UnknownException(string.Format("未知错误，错误代码：{0}，错误信息：{1}", statusCode.GetHashCode(), message));

            }
        }

        /// <summary>
        /// 提交文件到服务端
        /// </summary>
        /// <param name="url">服务端接口地址</param>
        /// <param name="content">待上传的文件对象</param>
        /// <param name="headers">请求头集合</param>
        /// <returns></returns>
        public static string PostFileToServer(string url, byte[] bytes, string fileName, Dictionary<string, string> headers, int timeOut = 30000)
        {
            try
            {
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                var encoding = System.Text.Encoding.UTF8;
                // The first boundary
                byte[] boundaryBytes = encoding.GetBytes("\r\n--" + boundary + "\r\n");
                // The last boundary
                byte[] trailer = encoding.GetBytes("\r\n--" + boundary + "--\r\n");
                // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
                //byte[] boundaryBytesF = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // Create the request and set parameters
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                request.Method = "POST";
                request.KeepAlive = true;
                request.Timeout = timeOut;
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                headers.ToList().ForEach(x =>
                {
                    request.Headers.Add(x.Key, x.Value);
                });

                // Get request stream
                Stream requestStream = request.GetRequestStream();


                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //dict.Add("id", "FILE_0");
                //dict.Add("name", fileName);
                //dict.Add("type", "application/octet-stream");
                //dict.Add("size", bytes.Length.ToString());
                //foreach (KeyValuePair<string, string> pair in dict)
                //{
                //    // Write item to stream
                //    byte[] formItemBytes = encoding.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", pair.Key, pair.Value));
                //    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                //    requestStream.Write(formItemBytes, 0, formItemBytes.Length);
                //}


                byte[] fileItemBytes = encoding.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", fileName, fileName));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(fileItemBytes, 0, fileItemBytes.Length);

                requestStream.Write(bytes, 0, bytes.Length);
                // Write trailer and close stream
                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();

                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        throw ex;
                    }
                    response = (HttpWebResponse)ex.Response;
                }
                var result = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                };
                TryReadResponseStatus(response.StatusCode, result);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
    #endregion

    /// <summary>
    /// 流预览通用帮助类
    /// </summary>
    public class PreviewByStreamHelper
    {
        #region Const
        private const string _Version = "v1";
        private const string _ResourceName = "File";
        private const string _ResourcePropertyName = "PreviewUrlByStream";
        #endregion

        #region Private Fields
        /// <summary>
        /// 当前连接是否仅当前登陆人可查看
        /// </summary>
        private bool _WithUser = true;
        /// <summary>
        /// 文件流
        /// </summary>
        private Stream _Stream = null;
        /// <summary>
        /// 文件接口
        /// </summary>
        private string _FileServiceUrl = "";
        /// <summary>
        /// 系统编码
        /// </summary>
        private string _SystemCode = "";
        /// <summary>
        /// 文件名称
        /// </summary>
        private string _FileName = "";
        /// <summary>
        /// 当前登录人标识
        /// </summary>
        private string _CurrentUserIdentity = "";
        /// <summary>
        /// 当前登陆人来源
        /// </summary>
        private UserFromSourceEnum _CurrentUserFromSource = UserFromSourceEnum.Inside;
        #endregion

        #region Init
        /// <summary>
        /// 通过文件流初始化
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="fileServiceUrl">文件接口地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="stream">文件流</param>
        /// <param name="userIdentity">当前登陆用户唯一标识（内网为useranme，即ctx账号；外网为MemberId）</param>
        /// <param name="userFromSource">当前登陆用户来源（内网或外网）</param>
        /// <param name="withUser">是否限制该链接仅当前人可查看</param>
        public PreviewByStreamHelper(string systemCode, string fileServiceUrl, string fileName, Stream stream, string userIdentity, UserFromSourceEnum userFromSource, bool withUser = true) : base()
        {
            if (string.IsNullOrEmpty(systemCode))
            {
                throw new InputParameterException("系统编码不可为空");
            }
            if (string.IsNullOrEmpty(fileServiceUrl))
            {
                throw new InputParameterException("文件接口地址不可为空");
            }
            if (stream == null)
            {
                throw new InputParameterException("待提交文件流不可为空");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InputParameterException("文件名称不可为空");
            }
            _SystemCode = systemCode;
            _WithUser = withUser;
            _FileServiceUrl = fileServiceUrl;
            _Stream = stream;
            _FileName = fileName;
            _CurrentUserIdentity = userIdentity;
            _CurrentUserFromSource = userFromSource;
        }

        /// <summary>
        /// 通过file初始化
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="fileServiceUrl">文件接口地址</param>
        /// <param name="postFile">从客户端提交的文件对象</param>
        /// <param name="userIdentity">当前登陆用户唯一标识（内网为useranme，即ctx账号；外网为MemberId）</param>
        /// <param name="userFromSource">当前登陆用户来源（内网或外网）</param>
        /// <param name="withUser">是否限制该链接仅当前人可查看</param>
        public PreviewByStreamHelper(string systemCode, string fileServiceUrl, HttpPostedFile postFile, string userIdentity, UserFromSourceEnum userFromSource, bool withUser = true) : base()
        {
            if (string.IsNullOrEmpty(systemCode))
            {
                throw new InputParameterException("系统编码不可为空");
            }
            if (string.IsNullOrEmpty(fileServiceUrl))
            {
                throw new InputParameterException("文件接口地址不可为空");
            }
            if (postFile == null || postFile.InputStream == null)
            {
                throw new InputParameterException("待提交文件流不可为空");
            }
            if (string.IsNullOrEmpty(postFile.FileName))
            {
                throw new InputParameterException("文件名称不可为空");
            }
            _SystemCode = systemCode;
            _WithUser = withUser;
            _FileServiceUrl = fileServiceUrl;
            _Stream = postFile.InputStream;
            _FileName = postFile.FileName;
            _CurrentUserIdentity = userIdentity;
            _CurrentUserFromSource = userFromSource;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 生成预览链接
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public string CreatePreviewUrl(Dictionary<string, string> properties = null)
        {

            //构建请求参数
            var parameters = new Dictionary<string, string>();
            parameters.Add("systemCode", _SystemCode);
            parameters.Add("withUser", _WithUser.ToString());

            //构建请求地址
            #region
            var url = CommonHelper.BuildRequestUrl(_FileServiceUrl, _Version, _ResourceName, _ResourcePropertyName);
            if (parameters.Any())
            {
                url += "?" + CommonHelper.BuildRequestParameters(parameters);
            }
            #endregion

            //构建请求头
            #region
            Dictionary<string, string> headers = new Dictionary<string, string>();
            var userName = _CurrentUserIdentity;
            var userSourceType = _CurrentUserFromSource;
            var userNameKey = "X-Wanda-CurrentLogin-UserName";
            var userSourceTypeKey = "X-Wanda-CurrentLogin-UserSourceType";
            headers.Add(userNameKey, userName);
            headers.Add(userSourceTypeKey, userSourceType == UserFromSourceEnum.Inside ? "Inside" : "Outside");
            properties = properties ?? new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in properties)
            {
                headers.Add(item.Key, item.Value);
            }
            #endregion

            //提交请求
            var previewUrl = CommonHelper.PostFileToServer(url, CommonHelper.ToByteArray(_Stream), _FileName, headers);

            return previewUrl;


        }
        #endregion
    }
}
