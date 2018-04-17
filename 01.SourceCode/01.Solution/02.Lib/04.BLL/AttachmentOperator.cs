
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using Framework.Core;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Lib.Common;
using System.Web;
using System.IO;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;

namespace Lib.BLL
{
    /// <summary>
    /// TemplateAttachment对象的业务逻辑操作
    /// </summary>
    public class AttachmentOperator : BizOperatorBase<Attachment>
    {
        /// <summary>
        /// 获取通用上传表单配置
        /// 返回值为Tuple类型 Item1为action Item2为文件名 Item3为文件流
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string, Stream> CommonSetting()
        {
            var request = HttpContext.Current.Request;
            var action = request.Form["action"];
            var file = request.Files["FileData"];
            var pathIndex = file.FileName.LastIndexOf('\\') >= 0 ? file.FileName.LastIndexOf('\\') + 1 : 0;
            var fileName = file.FileName.Substring(pathIndex);
            var fileStream = file.InputStream;
            fileStream.Seek(0, SeekOrigin.Begin);
            return Tuple.Create(action, fileName, fileStream);
        }
        /// <summary>
        /// 通用上传文件方法
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="action"></param>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public Lib.Model.Attachment CommonUpload(string businessId, string action, string fileName, Stream fileStream)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            var userInfo = WebHelper.GetCurrentUser();;
            var fileCode = FileUploadHelper.UploadFile(fileStream, fileName);
            var attachment = new Lib.Model.Attachment();
            attachment.ID = Guid.NewGuid().ToString();
            attachment.Name = fileName;
            attachment.AttachmentPath = fileCode;
            attachment.BusinessID = businessId;
            attachment.BusinessType = action;
            attachment.IsUseV1 = false;
            attachment.CreateDate = DateTime.Now;
            attachment.CreatorLoginName = userInfo.LoginName;
            attachment.CreatorName = userInfo.CNName;
            attachment.IsDeleted = false;
            attachment.ModifierLoginName = userInfo.LoginName;
            attachment.ModifierName = userInfo.CNName;
            attachment.ModifyTime = DateTime.Now;
            attachment.FileSize = FileUploadHelper.GetContentLength(fileStream.Length);
            return attachment;
        }

        #region Generate Code

        public static readonly AttachmentOperator Instance = PolicyInjection.Create<AttachmentOperator>();

        private static AttachmentAdapter _templateattachmentAdapter = AdapterFactory.GetAdapter<AttachmentAdapter>();

        public List<Attachment> GetList(string bussinessType, string bussinessId)
        {
            return _templateattachmentAdapter.GetList(bussinessType, bussinessId);
        }
        public List<Attachment> GetAttachmentListByTask(string TemplateConfigInstanceID, string businessType)
        {
            return _templateattachmentAdapter.GetAttachmentListByTask(TemplateConfigInstanceID, businessType);
        }

        public Attachment GetModelByCode(string fileCode)
        {
            return _templateattachmentAdapter.GetModelByCode(fileCode);
        }

        protected override BaseAdapterT<Attachment> GetAdapter()
        {
            return _templateattachmentAdapter;
        }

        public Attachment GetLastModel(string bussinessType, string bussinessId)
        {
            return _templateattachmentAdapter.GetLastModel(bussinessType, bussinessId);
        }

        public IList<Attachment> GetList()
        {
            IList<Attachment> result = _templateattachmentAdapter.GetList();
            return result;
        }

        public string AddModel(Attachment data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public Attachment GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(Attachment data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public string RemoveModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            string result = base.RemoveObject(id);
            return result;
        }

        public IList<Attachment> GetModel(string businessType, string businessID)
        {
            IList<Attachment> result = _templateattachmentAdapter.GetModel(businessType, businessID);
            return result;
        }

        public void AddAndUpdateModel(Attachment attach)
        {
            _templateattachmentAdapter.AddAndUpdateModel(attach);
        }

        #endregion
    }
}

