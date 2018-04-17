using System;
using System.Web;
using System.Xml;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Wanda.RCSJSJ.Common.Web
{
    public class SSoAuthUserClientUtility
    {
        public static readonly SSoAuthUserClientUtility Instance = new SSoAuthUserClientUtility();
        private SSoAuthUserClientUtility()
        {

        }
        private SSOConfigSettings GetSSOConfigSettings
        {
            get
            {
                return SSOConfigSettings.GetConfig();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(string url, string method, ContentType type)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            request.Method = method;
            switch (type)
            {
                case ContentType.AtomXmlFeed:
                    request.ContentType = "application/atom+xml;type=feed";
                    break;
                case ContentType.AtomXmlEntry:
                    request.ContentType = "application/atom+xml;type=entry";
                    break;
                case ContentType.Urlencoded:
                    request.ContentType = "application/x-www-form-urlencoded";
                    break;
            }
            request.Host = GetSSOConfigSettings.Informations["Host"].Value;

            return request;
        }
        private HttpWebRequest SsoHttpWebRequest(HttpRequestOperation operation, string url)
        {
            HttpWebRequest request = null;
            switch (operation)
            {
                case HttpRequestOperation.Token:
                    request = CreateRequest(url, "POST", ContentType.Urlencoded);
                    break;
                case HttpRequestOperation.Add:
                    request = CreateRequest(url, "POST", ContentType.AtomXmlEntry);
                    break;
                case HttpRequestOperation.Delete:
                    request = CreateRequest(url, "DELETE", ContentType.AtomXmlEntry);
                    break;
                case HttpRequestOperation.Update:
                    request = CreateRequest(url, "PUT", ContentType.AtomXmlEntry);
                    break;
            }
            return request;
        }
        public string Request(HttpRequestOperation operation, string postData, string postUrl)
        {
            string result = string.Empty;
            string characterSet = "utf-8";
            HttpWebRequest request = SsoHttpWebRequest(operation, postUrl);
            if (!string.IsNullOrEmpty(postData))
            {
                byte[] b = Encoding.ASCII.GetBytes(postData);//这个地方对postData的数据进行
                request.ContentLength = b.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(b, 0, b.Length);
                }
            }
            try
            {
                #region 获取服务器返回的资源
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (!string.IsNullOrEmpty(response.CharacterSet))
                    {
                        characterSet = response.CharacterSet;
                    }
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(characterSet)))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                #endregion
            }
            catch (WebException wex)
            {
                #region
                WebResponse wr = wex.Response;
                using (Stream st = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(st, Encoding.GetEncoding(characterSet)))
                    {
                        result = HttpUtility.HtmlDecode(sr.ReadToEnd());
                    }
                }
                throw new Exception("SSO接口调用异常！");
                #endregion
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                request = null;
            }
            return result;
        }

        public AuthEntity GetUserToken(string loginName, string password)
        {
            AuthEntity authContent = new AuthEntity();
            string reqcontent = "ssoLoginSite=" + GetSSOConfigSettings.Informations["LoginSite"].Value
                                       + "&SystemCode=" + GetSSOConfigSettings.Informations["SystemCode"].Value 
                                       + "&ResultMIMEType=" + GetSSOConfigSettings.Informations["ResultMIMEType"].Value  
                                       + "&UserID=" + loginName
                                       + "&UserPassword=" + password;
           
            try
            {
                string xmlStr = Request(HttpRequestOperation.Token, reqcontent, GetSSOConfigSettings.Informations["ApiUrl"].Value);
              
                ConvertToSSOMessage(xmlStr, authContent);
                
            }
            catch (Exception e)
            {
                authContent.ResultCode = "-1";
                authContent.ResultMessageCN = e.Message;
                authContent.ResultMessageEN = e.Message;
            }
            return authContent;

        }
        private void ConvertToSSOMessage(string xmlStr,AuthEntity authContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            #region
            XmlNode node = doc.SelectSingleNode("/SSOMessage/ResultCode");
            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                authContent.ResultCode = node.InnerText;
            }
            node = doc.SelectSingleNode("/SSOMessage/ResultMessageCN");
            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                authContent.ResultMessageCN = node.InnerText;
            }
            node = doc.SelectSingleNode("/SSOMessage/ResultMessageEN");
            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                authContent.ResultMessageEN = node.InnerText;
            }
            node = doc.SelectSingleNode("/SSOMessage/AuthTime");
            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                authContent.AuthTime = Convert.ToDateTime(node.InnerText);
            }
            node = doc.SelectSingleNode("/SSOMessage/ExpireTime");
            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                authContent.ExpireTime = Convert.ToDateTime(node.InnerText);
            }
            #endregion
            XmlNodeList nodes = doc.SelectNodes("/SSOMessage/Cookies/Cookie");
            if (node != null)
            {
                foreach (XmlNode item in nodes)
                {
                    switch (item.Attributes["name"].Value)
                    { 
                        case "AuthNum":
                            authContent.AuthNum = item.InnerText;
                            break;
                        case "AuthToken":
                            authContent.AuthToken = item.InnerText;
                            break;
                        case "AuthMAC":
                            authContent.AuthMAC = item.InnerText;
                            break;
                    }

                }
            }
        }
        public LoginResultObject GetUserTokenII(string loginName, string password)
        {
            LoginResultObject result = null;
            HttpCookieHelper.ClearCurrentAuthCooike();
            var identity = new LoginIdentity(loginName, password, GetSSOConfigSettings.Informations["SystemCode"].Value);
            if (ValidateIdentity(identity))
            {
                using (var channel = ChannelFactory.Create<ILogin>(ServiceAddress))
                {
                    result = channel.Channel.Login(RemotingToken.Token, identity);                   
                }
            }
            return result;
        }
        private bool ValidateIdentity(LoginIdentity identity)
        {
            if (string.IsNullOrEmpty(identity.Account))
            {
                //WriteErrorTip(this.account.ClientID, "请输入用户名");
                return false;
            }
            if (identity.Account.Length > 50)
            {
                //WriteErrorTip(this.account.ClientID, "用户名超出最大长度，请重新输入");
                return false;
            }
            if (string.IsNullOrEmpty(identity.Password))
            {
                //WriteErrorTip(this.password.ClientID, "请重新输入密码");
                return false;
            }
            //if ((!IsTrustAccessor) && (string.IsNullOrWhiteSpace(txtVerificationCodeStr) || ImageVerifier1.Text != txtVerificationCodeStr))
            //{
            //    this.hiddenPwd.Value = identity.Password;
            //    this.Verfiy.Text = "";
            //    WriteErrorTip(this.Verfiy.ClientID, "验证码错误，请重新输入");
            //    return false;
            //}
            return true;
        }
        private string ServiceAddress
        {
            get
            {
                var address = string.Empty;
                if (OSSOSection.Instance.OSSOServer.Host.Contains("http"))
                {
                    address = OSSOSection.Instance.OSSOServer.Host;
                    if (address.Last() != '/')
                        address += "/";
                }
                address += OSSOSection.Instance.OSSOServer.NetElements["loginService"].Location;
                return address;
            }
        }
    }
    public enum ContentType
    {
        Urlencoded = 0,
        AtomXmlFeed = 1,
        AtomXmlEntry = 2
    }
    public enum HttpRequestOperation
    {
        Token = 0,
        Add = 1,
        Delete = 2,
        Update = 3,
        GetSingleUser = 4,
        GetUserList = 5
    }
    internal static class RemotingToken
    {
        internal const string Token = "2F1BA7D7-2C1C-44F3-BB3C-BD11D3EB562E";
    }
}
