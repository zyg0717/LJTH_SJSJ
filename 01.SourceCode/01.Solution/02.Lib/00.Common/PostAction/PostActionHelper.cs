using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Common.PostAction
{
    public class PostActionHelper
    {

        public static WebProxy proxy = new WebProxy("10.199.75.12", 8080);


        public static string HttpPostData(string url, int timeOut, string fileKeyName, string fileName,
                                    Stream fileStream, NameValueCollection postData, Encoding encoding)
        {
            Framework.Core.Log.LogHelper.Instance.Info(string.Format("请求地址:{0}", url),
                   "PostActionHelper.HttpPostData"
                  );

            Framework.Core.Log.LogHelper.Instance.Info(string.Format("提交参数:{0}", string.Join("&", postData.AllKeys.Select(x => string.Format("{0}={1}", x, postData[x])).ToList())),
                   "PostActionHelper.HttpPostData"
                  );
            string responseContent;
            var memStream = new MemoryStream();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Proxy = proxy;
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, fileName);
            var headerbytes = encoding.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0  

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in postData.Keys
                                             select string.Format(stringKeyHeader, key, postData[key])
                                                 into formitem
                                             select encoding.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            encoding))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }
        public static byte[] DownLoadFile(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            WebClient client = new WebClient();
            //client.Proxy = proxy;
            return client.DownloadData(url);
        }

        //验证服务器证书
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        public static string GetPostData(string url, NameValueCollection postData, Encoding encoding, HttpMethod httpMethod)
        {
            Framework.Core.Log.LogHelper.Instance.Info(string.Format("请求地址:{0}", url),
                   "PostActionHelper.GetPostData"
                  );
            var postDataStr = string.Join("&", postData.AllKeys.Select(x => string.Format("{0}={1}", x, postData[x])).ToList());

            Framework.Core.Log.LogHelper.Instance.Info(string.Format("提交参数:{0}", postDataStr),
                   "PostActionHelper.GetPostData"
                  );
            byte[] data = encoding.GetBytes(postDataStr);
            //准备发送请求
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Proxy = proxy;
            if (httpMethod == HttpMethod.Post)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = data.Length;
                webRequest.Method = httpMethod.Method;
                Stream webStream = webRequest.GetRequestStream();
                //发送数据            
                webStream.Write(data, 0, data.Length);
                webStream.Close();
            }
            //获取返回数据
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), encoding);
            var retString = reader.ReadToEnd();
            return retString;
        }

    }
}
