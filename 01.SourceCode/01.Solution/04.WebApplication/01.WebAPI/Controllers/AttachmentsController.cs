using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Helper.Log;
using WebApplication.WebAPI.Models;
using WebApplication.WebAPI.Models.FilterModels;
using WebApplication.WebAPI.Models.Helper;
using Framework.Data;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Transactions;
using System.Web;
using System.Web.Http;
using Lib.BLL;
using Lib.Common;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// 附件相关接口
    /// </summary>
    [RoutePrefix("api/attachments")]
    public class AttachmentsController : BaseController
    {
        [HttpGet]
        [Route("stream/{attachmentId}")]
        public IHttpActionResult DownLoadStream(string attachmentId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(attachmentId) || !Guid.TryParse(attachmentId, out ret))
            {
                throw new BizException("参数错误");
            }
            var finder = AttachmentOperator.Instance.GetModel(attachmentId);
            if (finder == null)
            {
                throw new BizException("附件查找失败");
            }
            var bytes = FileUploadHelper.DownLoadFileStream(finder.AttachmentPath, false);
            return new FileResult(finder.AttachmentFullName, bytes, Request);
        }

        /// <summary>
        /// 下载附件（重定向方式，用于直接浏览器打开方式）
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("download/{attachmentId}")]
        public IHttpActionResult DownLoadFile(string attachmentId)
        {
            //string downloadUrl = null;
            //downloadUrl = string.Format("/api/attachments/stream/{0}", attachmentId);
            //return Redirect(downloadUrl);
            return DownLoadStream(attachmentId);
        }
        /// <summary>
        /// 通过主键获取附件相关信息
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("load/{attachmentId}")]
        public IHttpActionResult Get(string attachmentId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(attachmentId) || !Guid.TryParse(attachmentId, out ret))
            {
                throw new BizException("参数错误");
            }
            var finder = AttachmentOperator.Instance.GetModel(attachmentId);
            if (finder == null)
            {
                throw new BizException("附件查找失败");
            }
            Models.Attachment attachment = new Models.Attachment();
            attachment.ConvertEntity(finder);
            return BizResult(attachment);
        }
        /// <summary>
        /// 按业务ID以及业务分类查找附件集合
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query/{businessId}/{businessType}")]
        public IHttpActionResult LoadAttachmentList(string businessId, string businessType)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret) || string.IsNullOrEmpty(businessType))
            {
                throw new BizException("参数错误");
            }
            var attachmentList = AttachmentOperator.Instance.GetList(businessType, businessId);
            var attachments = attachmentList.Select(x =>
              {
                  var attachment = new Models.Attachment();
                  attachment.ConvertEntity(x);
                  return attachment;
              }).ToList();
            return BizResult(attachments);
        }
        /// <summary>
        /// 附件上传方法（该方法无任何业务操作仅对附件进行上传并入库）
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload/{businessId}")]
        public IHttpActionResult Upload(string businessId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            var attachment = new Models.Attachment();
            var setting = AttachmentOperator.Instance.CommonSetting();
            var model = AttachmentOperator.Instance.CommonUpload(businessId, setting.Item1, setting.Item2, setting.Item3);
            AttachmentOperator.Instance.AddModel(model);
            attachment.ConvertEntity(model);
            return BizResult(attachment);
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachmentId">附件ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{attachmentId}")]
        public IHttpActionResult DeleteAttachment(string attachmentId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(attachmentId) || !Guid.TryParse(attachmentId, out ret))
            {
                throw new BizException("参数错误");
            }
            var finder = AttachmentOperator.Instance.GetModel(attachmentId);
            if (finder == null)
            {
                throw new BizException("附件查找失败");
            }
            if (!finder.CreatorLoginName.Equals(WebHelper.GetCurrentUser().LoginName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new BizException("该附件不属于您");
            }
            finder.IsDeleted = true;
            var currentUser = WebHelper.GetCurrentUser(); ;
            finder.ModifierLoginName = currentUser.LoginName;
            finder.ModifierName = currentUser.CNName;
            finder.ModifyTime = DateTime.Now;
            AttachmentOperator.Instance.UpdateModel(finder);
            return BizResult(true);
        }
        //[System.Web.Http.Route("getfile/{attachmentId}")]
        //[HttpPost]
        //public IHttpActionResult GetPreviewLink(string attachmentId)
        //{
        //    return BizResult(new { result = "" });
        //    var attachment = AttachmentOperator.Instance.GetModel(attachmentId);
        //    if (attachment.IsUseV1)
        //    {
        //        var stream = FileUploadHelper.DownLoadFileStream(attachment.AttachmentPath, attachment.IsUseV1).ToStream();
        //        if (FileUploadHelper.IsUseV1)
        //        {
        //            var accessToken = "";
        //            var previewRelation = new Lib.Model.PreviewRelation();
        //            previewRelation.BusinessID = attachmentId;
        //            previewRelation.CreateDate = DateTime.Now;
        //            var currentUser = WebHelper.GetCurrentUser(); ;
        //            previewRelation.CreatorLoginName = currentUser.LoginName;
        //            previewRelation.CreatorName = currentUser.CNName;
        //            previewRelation.ID = Guid.NewGuid().ToString();
        //            previewRelation.DocumentByteLength = stream.Length;
        //            previewRelation.DocumentFileCode = attachment.AttachmentPath;
        //            previewRelation.DocumentFileName = attachment.Name;
        //            var link = OWAHelper.Instance.GetDocumentLink(attachment.Name, previewRelation.ID, out accessToken);
        //            previewRelation.AccessToken = accessToken;
        //            previewRelation.DocumentLink = link;
        //            PreviewRelationOperator.Instance.AddModel(previewRelation);
        //            return BizResult(new { result = previewRelation.ID, IsUseV1 = true, LinkToV2 = false });
        //        }
        //        else
        //        {
        //            var url = FileUploadHelper.GetPreviewUrlByStream(stream, attachment.Name);
        //            return BizResult(new { result = url, IsUseV1 = true, LinkToV2 = true });
        //        }

        //    }
        //    else
        //    {
        //        NLogTraceHelper.Instace.Debug("当前用户：{0}", HttpContext.Current.User.Identity.Name);
        //        var url = FileUploadHelper.GetPreviewUrl(attachment.AttachmentPath);
        //        return BizResult(new { result = url, IsUseV1 = false });
        //    }
        //}
    }
}