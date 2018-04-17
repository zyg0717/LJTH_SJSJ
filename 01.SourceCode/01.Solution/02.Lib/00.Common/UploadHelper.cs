using Plugin.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Web;

namespace Lib.Common
{
    /// <summary>
    /// 上传文件帮助类
    /// </summary>
    public class FileUploadHelper
    {


        public static string GetContentLength(long length)
        {
            var fileSize = "";
            if (length > 1024 * 1024)
            {
                fileSize = (length / (1024 * 1024)) + " M";
            }
            else
            {
                fileSize = (length / (1024)) + " KB";
            }
            return fileSize;
        }


        /// <summary>
        /// 构建路径信息
        /// </summary>
        /// <param name="fileCode"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string BuildTemplatePath(string fileCode, string fileName = null)
        {
            string ext = "";
            if (!string.IsNullOrEmpty(fileName))
            {
                var dotIndex = fileName.LastIndexOf(".");
                if (dotIndex >= 0)
                {
                    ext = fileName.Substring(fileName.LastIndexOf("."));
                }
            }
            return BuildTemplatePathExt(fileCode, ext);
        }

        public static string BuildTemplatePathExt(string fileCode, string fileExt)
        {
            return fileCode + "|" + fileExt;
        }

        public static string UploadFile(HttpPostedFile fileInfo, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = fileInfo.FileName;
            }
            return UploadFile(fileInfo.InputStream, fileName);

            //string flieCode = "";
            //if (IsUseV1)
            //    flieCode = Wanda.FileService.Client.Upload(fileInfo, fileName);
            //else
            //    flieCode = Wanda.IO.FileSystem.SDK.Refrence.Client.Upload(fileInfo, fileName);

            //return flieCode;
            //todo files
        }
        private static readonly string address = ConfigurationManager.AppSettings["FileStorage.Address"];
        private static readonly string userName = ConfigurationManager.AppSettings["FileStorage.UserName"];
        private static readonly string userPwd = ConfigurationManager.AppSettings["FileStorage.UserPwd"];
        public static string UploadFile(Stream fileStream, string fileName)
        {
            var guid = Guid.NewGuid().ToString();
            var filePhysicalName = guid;
            try
            {
                using (NASStorageProvider provider = new NASStorageProvider(address, userName, userPwd))
                {
                    provider.SaveFile(address, filePhysicalName, fileStream.ToBytes());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文件上传失败：{0}，{1}", ex.Message, ex.StackTrace));
            }

            return Path.Combine(address, filePhysicalName);
            //int ContentLength = (int)fileStream.Length;
            //string flieCode = "";
            //if (IsUseV1)
            //    flieCode = Wanda.FileService.Client.Upload(fileStream, ContentLength, fileName);
            //else
            //{
            //    try
            //    {
            //        flieCode = Wanda.IO.FileSystem.SDK.Refrence.Client.Upload(fileStream, fileName);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(string.Format("文件上传失败：{0}，{1}", ex.Message, ex.StackTrace));
            //    }
            //}

            //return flieCode;

            //todo files
        }

        public static byte[] UploadFileReturnStream(HttpPostedFile fileInfo, string fileName = null)
        {
            var ticket = UploadFile(fileInfo, fileName);
            return DownLoadFileStream(ticket, false);

            //if (string.IsNullOrEmpty(fileName))
            //{
            //    fileName = fileInfo.FileName;
            //}
            //string flieCode = "";
            //if (IsUseV1)
            //    flieCode = Wanda.FileService.Client.Upload(fileInfo, fileName);
            //else
            //    flieCode = Wanda.IO.FileSystem.SDK.Refrence.Client.Upload(fileInfo, fileName);
            //return DownLoadFileStream(flieCode, IsUseV1);


            //todo files
        }

        public static byte[] DownLoadFileStream(string fileCode, bool isUseV1)
        {
            var splitIndex = fileCode.LastIndexOf('|');
            if (splitIndex >= 0)
            {
                fileCode = fileCode.Substring(0, fileCode.LastIndexOf('|'));
            }
            byte[] bytes = null;
            try
            {
                using (NASStorageProvider provider = new NASStorageProvider(address, userName, userPwd))
                {
                    bytes = provider.LoadFile(fileCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文件上传失败：{0}，{1}", ex.Message, ex.StackTrace));
            }
            return bytes;

            //var splitIndex = fileCode.LastIndexOf('|');
            //if (splitIndex >= 0)
            //{
            //    fileCode = fileCode.Substring(0, fileCode.LastIndexOf('|'));
            //}

            //byte[] byteArray = null;
            //if (isUseV1)
            //    byteArray = Wanda.FileService.Client.DownStream(fileCode);
            //else
            //    byteArray = Wanda.IO.FileSystem.SDK.Refrence.Client.DownStream(fileCode);
            //return byteArray;
        }


        //public static string GetDownLoadUrl(string fileCode, string fileName, bool isUseV1)
        //{
            //if (!string.IsNullOrEmpty(fileCode))
            //{
            //    if (isUseV1)
            //        return Wanda.FileService.Client.GetDownloadURL(fileCode, fileName);
            //    else
            //        return Wanda.IO.FileSystem.SDK.Refrence.Extend.ClientExtend.GetPreviewURL(fileCode, fileName, true);
            //}
            //return "";
            //return null;
        //}

        public static string UploadFileStream(Stream stream, int contentLength, string fileName)
        {

            return UploadFile(stream, fileName);
        }
    }
}