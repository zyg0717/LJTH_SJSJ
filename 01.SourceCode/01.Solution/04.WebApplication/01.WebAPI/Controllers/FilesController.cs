using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Helper.Log;
using WebApplication.WebAPI.Models.Helper;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Lib.BLL;
using Lib.Common;
using Lib.Model;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// WOPI相关接口
    /// </summary>
    [UserAuthorize(true)]
    [System.Web.Http.RoutePrefix("api/wopi")]
    public class FilesController : BaseController
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("files/download/{Id}")]
        public IHttpActionResult DownLoadFile(string Id)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(Id) || !Guid.TryParse(Id, out ret))
            {
                throw new BizException("参数错误");
            }
            var finder = PreviewRelationOperator.Instance.GetModel(Id);
            if (finder == null)
            {
                throw new BizException("附件查找失败");
            }
            var attachment = AttachmentOperator.Instance.GetModel(finder.BusinessID);
            var downloadUrl = FileUploadHelper.GetDownLoadUrl(finder.DocumentFileCode, finder.DocumentFileName, attachment.IsUseV1);
            return Redirect(downloadUrl);
        }
        public string CommonGetPreviewLinkToken(string attachmentId)
        {
            return null;
            var attachment = AttachmentOperator.Instance.GetModel(attachmentId);
            //如果当前文件是v1版并且当前使用的文件平台为v2版 则提交预览流
            var stream = FileUploadHelper.DownLoadFileStream(attachment.AttachmentPath, attachment.IsUseV1).ToStream();

            var accessToken = "";
            var previewRelation = new PreviewRelation();
            previewRelation.BusinessID = attachmentId;
            previewRelation.CreateDate = DateTime.Now;
            var currentUser = WebHelper.GetCurrentUser(); ;
            previewRelation.CreatorLoginName = currentUser.LoginName;
            previewRelation.CreatorName = currentUser.CNName;
            previewRelation.ID = Guid.NewGuid().ToString();
            previewRelation.DocumentByteLength = stream.Length;
            previewRelation.DocumentFileCode = attachment.AttachmentPath;
            previewRelation.DocumentFileName = attachment.Name;
            var link = OWAHelper.Instance.GetDocumentLink(attachment.Name, previewRelation.ID, out accessToken);
            previewRelation.AccessToken = accessToken;
            previewRelation.DocumentLink = link;
            PreviewRelationOperator.Instance.AddModel(previewRelation);
            return previewRelation.ID;

        }
        /// <summary>
        /// 获取在线预览地址凭据
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [System.Web.Http.Route("files/getlink/{attachmentId}")]
        [HttpPost]
        public IHttpActionResult GetPreviewLinkToken(string attachmentId)
        {
            return BizResult(CommonGetPreviewLinkToken(attachmentId));
        }
        
        /// <summary>
        /// 获取预览相关信息
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [System.Web.Http.Route("files/preview/{businessId}")]
        [HttpGet]
        public IHttpActionResult GetPreviewLinkInfo(string businessId)
        {
            var finder = PreviewRelationOperator.Instance.GetModel(businessId);
            finder.DocumentLength = FileUploadHelper.GetContentLength(finder.DocumentByteLength);
            return BizResult(finder);
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="businessId"></param>
        /// <param name="access_token"></param>
        /// <param name="wdEnableRoaming"></param>
        /// <returns></returns>
        //[System.Web.Http.Route("files/{businessId}")]
        //public CheckFileInfo GetFileInfo(string businessId, string access_token)
        //{
        //    NLogTraceHelper.Instace.Info(string.Format("获取文件：请求参数,businessId:{0},access_token:{1}", businessId, access_token));

        //    try
        //    {
        //        IFileHelper _fileHelper = new FileHelper();
        //        //Validate(fileName, access_token);
        //        var finder = PreviewRelationOperator.Instance.GetModel(businessId);
        //        var attachment = AttachmentOperator.Instance.GetModel(finder.BusinessID);
        //        var fileInfo = _fileHelper.GetFileInfo(finder.DocumentFileName, finder.DocumentFileCode, attachment.IsUseV1);
        //        bool _updateEnabled = false;
        //        bool.TryParse(ConfigurationManager.AppSettings["updateEnabled"], out _updateEnabled);
        //        fileInfo.SupportsUpdate = _updateEnabled;
        //        fileInfo.UserCanWrite = _updateEnabled;
        //        fileInfo.SupportsLocks = _updateEnabled;
        //        //fileInfo.DownloadUrl = "http://d.youth.cn/shrgch/201612/W020161223334435271965.jpg";
        //        NLogTraceHelper.Instace.Info("获取文件，返回值：{0}", Newtonsoft.Json.JsonConvert.SerializeObject(fileInfo));

        //        return fileInfo;
        //    }
        //    catch (Exception e)
        //    {
        //        NLogTraceHelper.Instace.Error(e, "错误信息：{0}", e.Message);

        //        return null;
        //    }


        //}
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="businessId"></param>
        /// <param name="access_token"></param>
        /// <param name="wdEnableRoaming"></param>
        /// <returns></returns>
        [System.Web.Http.Route("files/{businessId}/contents")]
        public FileResult Get(string businessId, string access_token)
        {
            try
            {
                NLogTraceHelper.Instace.Info(string.Format("获取文件流：请求参数,businessId:{0},access_token", businessId, access_token));

                var finder = PreviewRelationOperator.Instance.GetModel(businessId);

                NLogTraceHelper.Instace.Info("找到记录，正在进行文件下载");
                var attachment = AttachmentOperator.Instance.GetModel(finder.BusinessID);
                var bytes = FileUploadHelper.DownLoadFileStream(finder.DocumentFileCode, attachment.IsUseV1);

                return new FileResult(finder.DocumentFileName, bytes, Request);

            }
            catch (Exception ex)
            {
                NLogTraceHelper.Instace.Error(ex, "错误信息：{0}", ex.Message);
                throw new BizException("获取文件内容错误");
            }

        }

    }
}