using Aspose.Cells;
using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Models;
using WebApplication.WebAPI.Models.FilterModels;
using WebApplication.WebAPI.Models.Helper;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Lib.BLL;
using Lib.BLL.Builder;
using Lib.Common;
using Lib.Model.Filter;
using Lib.ViewModel.TemplateViewModel;
using Plugin.Auth;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// 模板相关
    /// </summary>
    [RoutePrefix("api/templates")]
    public class TemplatesController : BaseController
    {
        /// <summary>
        /// 获取OWA在线预览地址
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [Route("download/preview")]
        [HttpPost]
        public IHttpActionResult OWAPreviewLink(string templateID)
        {
            var entity = new { };
            return BizResult(entity);
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [Route("download/{templateId}")]
        [HttpGet]
        public IHttpActionResult Download(string templateId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(templateId) || !Guid.TryParse(templateId, out ret))
            {
                throw new BizException("参数错误");
            }
            MemoryStream mockStream = new MemoryStream();
            var templete = TemplateOperator.Instance.GetModel(templateId);
            var fileCode = templete.TemplatePathFileCode;
            if (string.IsNullOrEmpty(fileCode))
            {
                throw new BizException("模板数据存在问题，未找到有效远程文件");
            }
            var attachment = AttachmentOperator.Instance.GetModelByCode(fileCode);
            ExcelEngine engine = new ExcelEngine();
            var stream = FileUploadHelper.DownLoadFileStream(fileCode, attachment.IsUseV1).ToStream();
            stream.Seek(0, SeekOrigin.Begin);
            Workbook taskBook = new Workbook(stream);

            var fileExt = templete.TemplatePathFileExt;
            var format = TemplateOperator.Instance.GetFormatType(fileExt);
            if (format == SaveFormat.Unknown)
            {
                throw new BizException("未知文件格式");
            }
            MemoryStream taskMemStream = new MemoryStream();
            taskBook.Save(taskMemStream, format);
            taskMemStream.Seek(0, SeekOrigin.Begin);
            var fileName = string.Format("{0}{1}", templete.TemplateName, fileExt);
            return new FileResult(fileName, taskMemStream.ToArray(), Request);
        }
        /// <summary>
        /// 分析模板（用户首次上传本地模板）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("analysis")]
        public IHttpActionResult AnalysisTemplate()
        {
            var tuple = AttachmentOperator.Instance.CommonSetting();
            var model = AttachmentOperator.Instance.CommonUpload(Guid.NewGuid().ToString(), tuple.Item1, tuple.Item2, tuple.Item3);
            AttachmentOperator.Instance.AddModel(model);
            var attachment = new Attachment();
            attachment.ConvertEntity(model);
            //by文件流生成配置信息（分析时因为还没有设置数据区域，所以默认第三行第一列）
            var tuples = TemplateOperator.Instance.ReadSheetDataFromStream(tuple.Item3, new List<Tuple<string, int, int>>());
            var fileFormat = TemplateOperator.Instance.GetFormatType(model);
            if (fileFormat == SaveFormat.Unknown)
            {
                throw new BizException("文件格式未知");
            }
            //使用配置和文件流生成业务数据便于前台展示
            List<ViewSheet> list = TemplateOperator.Instance.ReadExcelData(tuple.Item3, fileFormat, tuples.Item1, tuples.Item2, new List<Lib.Model.TemplateConfigSelect>());
            return BizResult(new
            {
                Attachment = attachment,
                Data = list
            });
        }
        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("load/{templateId}")]
        public IHttpActionResult Load(string templateId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(templateId) || !Guid.TryParse(templateId, out ret))
            {
                throw new BizException("参数无效");
            }
            var template = TemplateOperator.Instance.GetViewList(new VTemplateFilter()
            {
                TemplateID = templateId
            }).FirstOrDefault();
            var attachment = AttachmentOperator.Instance.GetLastModel("UploadModelAttach", templateId);
            if (attachment == null && template.IsImport == 0)
            {
                //为了适配老数据
                ExcelTempleteBuilder builder = new ExcelTempleteBuilder(templateId);
                var fileExt = "";
                //初始化出excel对象
                var wb = builder.InitExcelTemplete(out fileExt);
                var format = TemplateOperator.Instance.GetFormatType(fileExt);
                MemoryStream tmpStream = new MemoryStream();
                wb.Save(tmpStream, format);
                var attach = AttachmentOperator.Instance.CommonUpload(templateId, "UploadModelAttach", string.Format("{0}{1}", template.TemplateName, fileExt), tmpStream);
                AttachmentOperator.Instance.AddModel(attach);
                attachment = attach;
            }
            var fileFormat = TemplateOperator.Instance.GetFormatType(attachment.FileExt);
            var stream = FileUploadHelper.DownLoadFileStream(attachment.AttachmentPath, attachment.IsUseV1).ToStream();
            var sheets = TemplateSheetOperator.Instance.GetList(templateId).ToList();
            var configs = TemplateConfigOperator.Instance.GetList(templateId, null).ToList();
            var selects = TemplateConfigSelectOperator.Instance.GetList(templateId).ToList();
            //使用配置和文件流生成业务数据便于前台展示
            List<ViewSheet> list = TemplateOperator.Instance.ReadExcelData(stream, fileFormat, sheets, configs, selects);
            Template model = new Template();
            model.ConvertEntity(template);
            model.sheets = list;
            return BizResult(new
            {
                Attachment = attachment,
                Model = model
            });
        }
        /// <summary>
        /// 设置数据区域
        /// </summary>
        /// <param name="attachmentId">分析模板时返回的附件实体中的ID</param>
        /// <param name="ranges">数据区域集合 （name,row,column）</param>
        /// <returns></returns>
        [HttpPost]
        [Route("setrange/{attachmentId}")]
        public IHttpActionResult SetDataRange(string attachmentId, [FromBody] List<DataRangeSetting> ranges)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(attachmentId) || !Guid.TryParse(attachmentId, out ret))
            {
                throw new BizException("参数无效");
            }
            var attachment = AttachmentOperator.Instance.GetModel(attachmentId);
            if (attachment == null)
            {
                throw new BizException("文件查找失败");
            }
            var fileStream = FileUploadHelper.DownLoadFileStream(attachment.AttachmentPath, attachment.IsUseV1).ToStream();
            var fileFormat = TemplateOperator.Instance.GetFormatType(attachment);
            //by文件流生成配置信息（分析时因为还没有设置数据区域，所以默认第三行第一列）

            var dataRangeList =
                ranges.Select(x =>
                {
                    return Tuple.Create(x.name, x.row, x.column);
                }).ToList();
            if (dataRangeList.Any(x => string.IsNullOrEmpty(x.Item1)))
            {
                throw new BizException("工作表名称不可为空");
            }
            if (dataRangeList.Any(x => x.Item2 < 2 || x.Item3 < 1))
            {
                throw new BizException("数据区域存在错误");
            }
            if (dataRangeList.GroupBy(x => x.Item1).Any(x => x.Count() > 1))
            {
                throw new BizException("工作表名称不可重复");
            }
            var tuples = TemplateOperator.Instance.ReadSheetDataFromStream(fileStream, dataRangeList);
           
            //使用配置和文件流生成业务数据便于前台展示
            // List<ViewSheet> list = TemplateOperator.Instance.ReadExcelData(fileStream, fileFormat, tuples.Item1, tuples.Item2, new List<Lib.Model.TemplateConfigSelect>());
            List<ViewSheet> list = TemplateOperator.Instance.ReadExcelData(fileStream, fileFormat, tuples.Item1, new List<Lib.Model.TemplateConfig>(), new List<Lib.Model.TemplateConfigSelect>());
            list = list.Where(x => dataRangeList.Exists(r => r.Item1 == x.name)).ToList();
            //如果没有输入列表，系统添加默认列，系统添加默认列
            var flag = 1;

            foreach (var viewSheet in list)
            {
                foreach (var column in viewSheet.columns)
                {
                    if (column.name == "")
                    {
                        column.name = "".PadLeft(flag);
                        flag++;
                    }
                }

            }
            return BizResult(new
            {
                Attachment = attachment,
                Data = list
            });
        }
        /// <summary>
        /// 预览模板
        /// 需要将附件ID以及页面填写过的业务数据对象一起提交过来用以生成预览模板
        /// </summary>
        /// <param name="renderType">呈现类型（0 在线模板 1 克隆模板 2 本地模板）</param>
        /// <param name="model">页面配置信息</param>
        [HttpPost]
        [Route("preview/{renderType}")]
        public IHttpActionResult Preview(int renderType, [FromBody] Template model)
        {
            var fileName = "";
            var stream = CommonPreview(renderType, model, out fileName);
            return new FileResult(fileName, stream.ToArray(), Request);

        }
        private MemoryStream CommonPreview(int renderType, [FromBody] Template model, out string fileName)
        {
            string attachmentId = model.AttachmentId;
            var views = model.sheets;
            string errorMessage = "";
            var ret = CommonExcelValidate(views, out errorMessage);
            if (!ret)
            {
                throw new BizException(errorMessage);
                //throw new HttpResponseException(new HttpResponseMessage()
                //{
                //    StatusCode = System.Net.HttpStatusCode.Conflict,
                //    Content = new StringContent(errorMessage)
                //});
            }
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var template = new Lib.Model.Template();
            template.ID = Guid.NewGuid().ToString();
            var tuple = TemplateOperator.Instance.ReadSheetDataFromBizModel(template, renderType, views);
            Lib.Model.Attachment attachment = null;
            if (!string.IsNullOrEmpty(attachmentId))
            {
                attachment = AttachmentOperator.Instance.GetModel(attachmentId);
            }
            var ext = attachment == null ? ".xlsx" : attachment.FileExt;
            fileName = string.Format("PREVIEW-{0}({1}){2}", WebHelper.GetCurrentUser().CNName, WebHelper.GetCurrentUser().LoginName, ext);
            template.TemplateName = fileName;

            ExcelTempleteBuilder builder = new ExcelTempleteBuilder();
            //初始化出excel对象
            var wb = builder.InitExcel(renderType == 2, attachment, template, tuple.Item1, tuple.Item2, tuple.Item3);
            wb = TemplateOperator.Instance.WriteSampleData(wb, views, renderType);
            //获取文档格式
            var format = TemplateOperator.Instance.GetFormatType(attachment);
            //写到内存流中
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, format);
            return stream;
        }
        /// <summary>
        /// 使用在线预览
        /// </summary>
        /// <param name="renderType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("preview/online/{renderType}")]
        //public IHttpActionResult PreviewOnline(int renderType, [FromBody] Template model)
        //{
        //    FilesController filesController = new FilesController();
        //    var fileName = "";
        //    var stream = CommonPreview(renderType, model, out fileName);
        //    var attachment = AttachmentOperator.Instance.CommonUpload(Guid.NewGuid().ToString(), "PreviewOnlineTemplateAttach", fileName, stream);
        //    AttachmentOperator.Instance.AddModel(attachment);

        //    return BizResult(new { result = attachment.ID, IsUseV1 = false });
        //}
        private bool CommonExcelValidate(List<ViewSheet> views, out string errorMessage)
        {
            errorMessage = "";
            if (views == null || views.Count == 0)
            {
                errorMessage = "至少要有一个工作表";
                return false;
            }
            if (views.Any(x => string.IsNullOrEmpty(x.name)))
            {
                errorMessage = "工作表名称不可为空";
                return false;
            }
            if (views.Any(x => x.firstrow < 2 || x.firstcolumn < 1))
            {
                errorMessage = "数据区域存在错误";
                return false;
            }
            if (views.GroupBy(x => x.name).Any(x => x.Count() > 1))
            {
                errorMessage = "工作表名称不可重复";
                return false;
            }
            if (views.Any(x => x.columns == null || !x.columns.Any()))
            {
                errorMessage = "必须设置列信息";
                return false;
            }
            if (views.Any(x => x.columns == null || x.columns.All(c => string.IsNullOrEmpty(c.name))))
            {
                var sheetNames = string.Join("、", views.Where(x => x.columns == null || x.columns.All(c => string.IsNullOrEmpty(c.name))).Select(x => x.name).ToArray());
                errorMessage = sheetNames + " 工作表中至少要有一列";
                return false;
            }
            var sameNameColumns = views.Where(x =>
                 {
                     var columns = x.columns.Where(c => !string.IsNullOrEmpty(c.name)).GroupBy(c => c.name);
                     return columns.Any(c => c.Count() > 1);
                 }
             );
           
            if (sameNameColumns.Any())
            {
                var messageList = new List<string>();
                foreach (var item in sameNameColumns)
                {
                    var repeaterColumns = item.columns.Where(x => !string.IsNullOrEmpty(x.name)).GroupBy(x => x.name).Where(y => y.Count() > 1).Select(x => x.Key).ToList();
                    messageList.Add(string.Format("【{0}】存在相同的列名 【{1}】", item.name, string.Join("、", repeaterColumns)));
                }
                errorMessage = string.Format("【{0}】存在相同的列名 ", string.Join("  |  ", sameNameColumns.Select(c => c.name)));
                errorMessage = string.Join(",", messageList);
                return false;
            }
            //验证不可以跳列
            var validates = views.Where(x =>
            {
                var lastUseColumnIndex = x.columns.FindLastIndex(c => !string.IsNullOrEmpty(c.name));
                int i = 0;

                bool hasSkipColumn = false;
                x.columns.ForEach(c =>
                {
                    if (!hasSkipColumn)
                    {
                        var currentHasContent = !string.IsNullOrEmpty(c.name);
                        var preHasContent = currentHasContent;
                        var nextHasContent = currentHasContent;
                        if (i > 0 && i < lastUseColumnIndex)
                        {
                            preHasContent = !string.IsNullOrEmpty(x.columns[i - 1].name);
                            nextHasContent = !string.IsNullOrEmpty(x.columns[i + 1].name);
                        }
                        if (preHasContent != currentHasContent || nextHasContent != currentHasContent)
                        {
                            hasSkipColumn = true;
                        }
                    }
                    i++;
                });
                return hasSkipColumn;
            });
            if (validates.Any())
            {
                errorMessage = string.Format("【{0}】存在未连续填写的列信息", string.Join("、", validates.Select(x => x.name)));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="renderType"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save/{renderType}")]
        public IHttpActionResult Save(int renderType, [FromBody] Template viewModel)
        {
            //var finder = TemplateOperator.Instance.GetModel(viewModel.TemplateID);
            //if (finder != null)
            //{
            //    Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            //}
            string attachmentId = viewModel.AttachmentId;
            return CommonSave(attachmentId, renderType, viewModel, true);
        }
        /// <summary>
        /// 通用保存方法
        /// </summary>
        /// <param name="attachmentId">附件ID</param>
        /// <param name="renderType">呈现模式</param>
        /// <param name="viewModel">业务数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns></returns>
        private IHttpActionResult CommonSave(string attachmentId, int renderType, Template viewModel, bool isAdd)
        {
            if (string.IsNullOrEmpty(viewModel.TemplateName))
            {
                throw new BizException("请填写模板名称");
            }
            if (isAdd)
            {
                var exist = TemplateOperator.Instance.CheckExist(WebHelper.GetCurrentUser().LoginName, viewModel.TemplateName);
                if (exist)
                {
                    throw new BizException("模板名称已存在，无法保存");
                }
            }
            string errorMessage = "";
            var views = viewModel.sheets;
            var ret = CommonExcelValidate(views, out errorMessage);
            if (!ret)
            {
                throw new BizException(errorMessage);
            }
            var userInfo = WebHelper.GetCurrentUser(); ;
            Lib.Model.Attachment attachment = null;
            if (!string.IsNullOrEmpty(attachmentId))
            {
                attachment = AttachmentOperator.Instance.GetModel(attachmentId);
                if (attachment == null)
                {
                    throw new BizException("文件查找失败，请确认文件正确已正确上传");
                }
            }
            var template = new Lib.Model.Template();
            if (isAdd)
            {
                template.ID = Guid.NewGuid().ToString();
                template.ActualUnitID = userInfo.actualUnitID;
                template.ActualUnitName = userInfo.ActualUnitName;
                template.CreatorTime = DateTime.Now;
                template.CreatorLoginName = userInfo.LoginName;
                template.CreatorName = userInfo.CNName;
                template.IsDeleted = false;
                template.IsPrivate = 1;
                template.ModifierLoginName = userInfo.LoginName;
                template.ModifierName = userInfo.CNName;
                template.ModifyTime = DateTime.Now;
                template.OrgID = userInfo.OrgID;
                template.OrgName = userInfo.OrgName;
                template.Range = null;
                template.TemplatePath = "";
                template.UnitID = userInfo.UnitID;
                template.UnitName = userInfo.UnitName;

                template.IsImport = renderType == 2 ? 1 : 0;
                template.Status = 0;
            }
            else
            {
                template = TemplateOperator.Instance.GetModel(viewModel.TemplateID);
            }
            template.TemplateName = viewModel.TemplateName;
            var tuple = TemplateOperator.Instance.ReadSheetDataFromBizModel(template, renderType, views);

            ExcelTempleteBuilder builder = new ExcelTempleteBuilder();
            //初始化出excel对象
            var wb = builder.InitExcel(renderType == 2, attachment, template, tuple.Item1, tuple.Item2, tuple.Item3);
            wb = TemplateOperator.Instance.WriteSampleData(wb, views, renderType);

            //获取文档格式
            var format = TemplateOperator.Instance.GetFormatType(attachment);
            //写到内存流中
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, format);


            //保存内存流到文件系统中
            string templateFileName = "";
            if (attachment != null)
            {
                templateFileName = attachment.Name;
            }
            else
            {
                templateFileName = string.Format("{0}.xlsx", template.TemplateName);
            }

            var model = AttachmentOperator.Instance.CommonUpload(template.ID, "UploadModelAttach", templateFileName, stream);
            template.TemplatePath = string.Format("{0}|{1}", model.AttachmentPath, model.FileExt);
            if (!isAdd)
            {
                TemplateOperator.Instance.UpdateRalationByID(template.ID);
            }
            AttachmentOperator.Instance.AddModel(model);
            //入库所有对象
            TemplateOperator.Instance.AddModel(template);
            tuple.Item1.ForEach(x =>
            {
                TemplateSheetOperator.Instance.AddModel(x);
            });
            tuple.Item2.ForEach(x =>
            {
                TemplateConfigOperator.Instance.AddModel(x);
            });
            tuple.Item3.ForEach(x =>
            {
                TemplateConfigSelectOperator.Instance.AddModel(x);
            });
            //返回模板ID
            return BizResult(template.ID);
        }
        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update/{templateId}")]
        public IHttpActionResult Update(string templateId, [FromBody] Template viewModel)
        {
            var finder = TemplateOperator.Instance.GetModel(templateId);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(templateId) || !Guid.TryParse(templateId, out ret))
            {
                throw new BizException("参数无效");
            }
            var template = TemplateOperator.Instance.GetModel(templateId);
            var attachment = AttachmentOperator.Instance.GetLastModel("UploadModelAttach", templateId);
            return CommonSave(attachment.ID, template.IsImport == 1 ? 2 : 0, viewModel, false);
        }
        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public IHttpActionResult LoadTemplateData([FromBody] TemplateFilter filter)
        {
            var queryFilter = new VTemplateFilter();
            if (!UserInfoOperator.Instance.IsAdmin() || filter.IsOnlySelf)
            {
                queryFilter.CreatorLoginName = WebHelper.GetCurrentUser().LoginName;
            }
            //如果查看的为非自己的数据 则需要数据权限
            //if (!filter.IsOnlySelf)
            //{
            //    var startOrg = AuthHelper.GetPrivilegeStartOrg();
            //    queryFilter.UnitFullPath = startOrg;
            //}
            queryFilter.CreatorLoginOrName = filter.TemplateCreateLoginOrName;

            #region 获取时间范围
            DateTime date = DateTime.MinValue.ValidateSqlMinDate();
            if (filter.TimeRange.HasValue)
            {
                switch (filter.TimeRange)
                {
                    case 1:
                        date = DateTime.Today;
                        break;
                    case 2:
                        date = DateTime.Now.AddDays(-1);
                        break;
                    case 3:
                        date = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7);
                        break;
                    case 4:
                        date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
                        break;
                    case 5:
                        date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-3);
                        break;
                    case 6:
                        date = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
                        break;
                }
            }

            #endregion
            queryFilter.TemplateName = filter.TemplateName;
            queryFilter.CreatorTimeStart = date;
            queryFilter.PageIndex = filter.PageIndex;
            queryFilter.PageSize = filter.PageSize;
            //如果不是加载本地导入模板 则只查询非本地导入模板
            if (!filter.WithImport)
            {
                queryFilter.IsImport = 0;
            }
            var data = TemplateOperator.Instance.GetViewList(queryFilter);
            var templates = data.Select(x =>
              {
                  Template template = new Template();
                  template.ConvertEntity(x);
                  return template;
              }).ToList();
            return BizResult(new ViewData<Template>() { Data = templates, TotalCount = data.TotalCount });
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{templateId}")]
        public IHttpActionResult Delete(string templateId)
        {
            var finder = TemplateOperator.Instance.GetModel(templateId);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(templateId) || !Guid.TryParse(templateId, out ret))
            {
                throw new BizException("参数无效");
            }
            var template = TemplateOperator.Instance.GetModel(templateId);
            if (template == null)
            {
                throw new BizException("参数错误");
            }
            template.Status = 1;
            TemplateOperator.Instance.UpdateModel(template);
            return BizResult(true);
        }
    }
}