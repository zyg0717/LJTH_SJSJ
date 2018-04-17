using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Models;
using WebApplication.WebAPI.Models.FilterModels;
using WebApplication.WebAPI.Models.Helper;
using Framework.Data;
using Framework.Web.Json;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.Http;
using Lib.BLL;
using Lib.Common;
using Lib.Model.Filter;
using Lib.ViewModel;
using Plugin.Workflow;
using Plugin.Auth;
using Plugin.OAMessage;
using BPF.Workflow.Object;
using BPF.Workflow.Client;
using Aspose.Cells;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// 待办、已办相关接口
    /// </summary>
    [RoutePrefix("api/todos")]
    public class TodosController : BaseController
    {
        private static Dictionary<string, int> GetTodoBusinessIdList(int taskType)
        {
            //var flowType = 0;
            //if (taskType == 1)
            //{
            //    flowType = 0;
            //}
            //if (taskType == 2)
            //{
            //    flowType = 2;
            //}
            //if (taskType == 3)
            //{
            //    flowType = 4;
            //}
            //var result = OAMessageOperator.Instance.GetList(flowType, WebHelper.GetCurrentUser().LoginName);
            //return result.ToDictionary(x => x.FlowID, x => taskType);


            var user = WebHelper.GetCurrentUser(); ;
            WFTaskQueryResult result = null;
            switch (taskType)
            {
                case 1:
                    {
                        result = WFTaskSDK.QueryToDo(new WFTaskQueryFilter()
                        {
                            TaskUser = user.LoginName,
                            PageIndex = 0,
                            PageSize = 0
                        });
                    }
                    break;
                case 2:
                    {
                        result = WFTaskSDK.QueryDoneDistinct(new WFTaskQueryFilter()
                        {
                            TaskUser = user.LoginName,
                            PageIndex = 0,
                            PageSize = 0
                        });
                    }
                    break;
                default:
                    {
                        result = WFTaskSDK.QueryToDo(new WFTaskQueryFilter()
                        {
                            TaskUser = user.LoginName,
                            PageIndex = 0,
                            PageSize = 0
                        });
                    }
                    break;
            }
            Regex urlRegex = new Regex(@"(?:^|\?|&)businessid=(\S*)(?:&|$)", RegexOptions.IgnoreCase);
            Dictionary<string, int> businessids = new Dictionary<string, int>();
            foreach (var task in result.TaskList)
            {
                Match m = urlRegex.Match(task.TaskURL.ToLower());
                string businessid = string.Empty;
                if (m.Success)
                {
                    businessids.Add(m.Groups[1].Value, task.TaskAction);
                }
            }

            return businessids;
        }
        /// <summary>
        /// 查询待办、已办列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public IHttpActionResult LoadTodoList([FromBody] TodoFilter filter)
        {
            Dictionary<string, int> businessids = GetTodoBusinessIdList(filter.TaskType);
            var data = new Framework.Data.PartlyCollection<VTaskTodo>();
            if (businessids.Count > 0)
            {
                var queryFilter = new VTaskTodoFilter();
                queryFilter.BusinessIDS = businessids.Select(x => x.Key).ToList();

                var startTime = DateTime.MinValue;
                DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                if (filter.TimeRange.HasValue)
                {
                    switch (filter.TimeRange.Value)
                    {
                        case 1:
                            startTime = dateNow;
                            break;
                        case 2:
                            startTime = dateNow.AddDays(-1);
                            break;
                        case 3:
                            startTime = dateNow.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7);
                            break;
                        case 4:
                            startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
                            break;
                        case 5:
                            startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-3);
                            break;
                        case 6:
                            startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
                            break;
                    }
                    queryFilter.CreatorTimeStart = startTime;
                }
                queryFilter.TaskTitle = filter.TaskTitle;
                queryFilter.PageSize = filter.PageSize;
                queryFilter.PageIndex = filter.PageIndex;
                //queryFilter.EmployeeLoginName = WebHelper.GetCurrentUser().LoginName;
                data = VTaskTodoOperator.Instance.GetViewList(queryFilter);
            }
            var result = data.Select(x =>
            {
                var finder = businessids.FirstOrDefault(t => t.Key.Equals(x.BusinessID, StringComparison.CurrentCultureIgnoreCase));
                var todo = new Todo();
                todo.ConvertEntity(x, finder.Value);
                return todo;
            }).ToList();
            return BizResult(new ViewData<Todo>() { Data = result, TotalCount = data.TotalCount });
        }
        /// <summary>
        /// 获取任务实体
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("load/{businessId}")]
        public IHttpActionResult Load(string businessId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            var queryFilter = new VTaskTodoFilter();
            queryFilter.BusinessIDS = new List<string>() { businessId };
            var finder = VTaskTodoOperator.Instance.GetViewList(queryFilter).FirstOrDefault();
            if (finder == null)
            {
                throw new BizException("待办无效");
            }
            //if (finder.EmployeeLoginName == WebHelper.GetCurrentUser().LoginName)
            //{
            //    WorkflowBuilder.ReadWorkflowProcess(finder.BusinessID, new Lib.Model.Employee() { LoginName = finder.ReceiveLoginName });
            //}

            var todo = new Todo();
            todo.ConvertEntity(finder);
            if (!(todo.TaskStatus < 1 && WebHelper.GetCurrentUser().LoginName != todo.EmployeeLoginName))
            {
                //--填报文件
                var attachment = AttachmentOperator.Instance.GetLastModel("UploadTaskData", businessId);
                todo.ConvertTaskAttachment(attachment);
            }
            //--相关附件
            var attachments = AttachmentOperator.Instance.GetModel("UploadTaskAttachment", todo.TaskID).ToList();
            todo.ConvertTaskAttachmentList(attachments);
            var taskReportAttachments = AttachmentOperator.Instance.GetModel("UploadTaskAttach", businessId).ToList();
            todo.ConvertTaskReportAttachmentList(taskReportAttachments);
            return BizResult(todo);
        }
        /// <summary>
        /// 上传填报文件
        /// </summary>
        /// <param name="businessId"></param>
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
            var tuple = AttachmentOperator.Instance.CommonSetting();
            var model = AttachmentOperator.Instance.CommonUpload(businessId, tuple.Item1, tuple.Item2, tuple.Item3);
            TaskCollectionData dcd = new TaskCollectionData();
            using (TransactionScope scope = TransactionScopeFactory.Create())
            {
                string errorMessage = "";
                var result = TemplateOperator.Instance.ReadTaskData(businessId, tuple.Item2, tuple.Item3, out dcd, out errorMessage);
                if (result != 3)
                {
                    return BizResult(new
                    {
                        ResultType = result,
                        Message = errorMessage
                    });
                }
                AttachmentOperator.Instance.AddModel(model);
                scope.Complete();
            }
            var attachment = new Attachment();
            attachment.ConvertEntity(model);
            return BizResult(new
            {
                ResultType = 3,
                Attachment = attachment,
                Message = "上传成功",
                Sheets = dcd.Sheets.Select(x => new { SheetName = x.SheetName, SheetRowLength = x.Rows.Count }).ToList()
            });
        }
        /// <summary>
        /// 流程审批通过回调函数
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workflow/approve/{businessId}")]
        public IHttpActionResult Approve(string businessId, [FromBody]Todo todo)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            var result = TemplateTaskOperator.Instance.Approve(businessId, todo.TaskReportRemark, WebHelper.GetCurrentUser());
            return BizResult(result);
        }
        /// <summary>
        /// 流程审批不通过回调函数
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workflow/reject/{businessId}")]
        public IHttpActionResult Reject(string businessId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            var result = TemplateTaskOperator.Instance.Reject(businessId, WebHelper.GetCurrentUser());
            return BizResult(result);
        }
        /// <summary>
        /// 提交回调
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="todo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workflow/submit/{businessId}")]
        public IHttpActionResult Submit(string businessId, [FromBody]Todo todo)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            using (TransactionScope transaction = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                var tTask = TemplateTaskOperator.Instance.GetModel(businessId);
                var tci = TemplateConfigInstanceOperator.Instance.GetModel(tTask.TemplateConfigInstanceID);
                LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;
                var configs = TemplateConfigOperator.Instance.GetList(tci.TemplateID, null).ToList();
                var sheetConfigs = TemplateSheetOperator.Instance.GetList(tci.TemplateID).ToList();
                var attac = AttachmentOperator.Instance.GetLastModel("UploadTaskData", businessId);
                tTask.SubmitTime = DateTime.Now;
                tTask.FileName = attac.Name;
                tTask.FilePath = attac.AttachmentPath;
                tTask.Status = (int)ProcessStatus.Inprocess;
                tTask.Remark = todo.TaskReportRemark;
                tTask.ModifierLoginName = userinfo.LoginName;
                tTask.ModifierName = userinfo.CNName;
                tTask.ModifyTime = DateTime.Now;
                //TaskTemplateType=2时，更新二维表
                if (tci.TaskTemplateType == 2)
                {
                    //先获取模板数据
                    var attachment = AttachmentOperator.Instance.GetModelByCode(tci.TemplatePathFileCode);
                    var dcu = DataCollectUserOperator.Instance.GetModel(tTask.DataCollectUserID);
                    System.IO.Stream stream = FileUploadHelper.DownLoadFileStream(tci.TemplatePathFileCode, attachment.IsUseV1).ToStream();
                    ExcelEngine engine = new ExcelEngine();
                    Workbook templateBook = new Workbook(stream);
                    TemplateOperator.Instance.GetTemplateDateForUpdate(templateBook, sheetConfigs, dcu.UpdateArea, dcu.AreaValue);
                    var json = TemplateTaskOperator.Instance.ReadTaskDataForUpdateTask(templateBook, attac, configs, sheetConfigs);
                    tTask.Content = JsonHelper.Serialize(json);
                }
                else
                {
                    var json = TemplateTaskOperator.Instance.ReadTaskData(attac, configs, sheetConfigs);
                    tTask.Content = JsonHelper.Serialize(json);
                }
                TemplateTaskOperator.Instance.UpdateModel(tTask);
                //var result = TemplateTaskOperator.Instance.Approve(businessId, tTask.Remark, WebHelper.GetCurrentUser());
                transaction.Complete();
            }
            return BizResult(true);
        }
        /// <summary>
        /// 保存回调
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="todo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workflow/save/{businessId}")]
        public IHttpActionResult Save(string businessId, [FromBody]Todo todo)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessId) || !Guid.TryParse(businessId, out ret))
            {
                throw new BizException("参数错误");
            }
            using (TransactionScope transaction = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                var tTask = TemplateTaskOperator.Instance.GetModel(businessId);
                var tci = TemplateConfigInstanceOperator.Instance.GetModel(tTask.TemplateConfigInstanceID);
                LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;
                tTask.Remark = todo.TaskReportRemark;
                TemplateTaskOperator.Instance.UpdateModel(tTask);
                transaction.Complete();
            }
            return BizResult(true);
        }
    }
}