using Aspose.Cells;
using Framework.Data;
using Framework.Web.Utility;
using Lib.BLL;
using Lib.BLL.Builder;
using Lib.Common;
using Lib.Model;
using Lib.Model.Filter;
using Lib.ViewModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web.Http;
using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Models;
using WebApplication.WebAPI.Models.FilterModels;
using WebApplication.WebAPI.Models.Helper;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// 任务相关接口
    /// </summary>
    [RoutePrefix("api/tasks")]
    public class TasksController : BaseController
    {
        public readonly string connectionName = "DBConnectionString";
        public readonly string CommonFilePath = System.AppDomain.CurrentDomain.BaseDirectory;
        ///// <summary>
        ///// 获取预览地址
        ///// </summary>
        ///// <param name="taskId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("template/download/preview/{taskId}")]
        //public IHttpActionResult PreviewTaskTemplate(string taskId)
        //{
        //    FilesController filesController = new FilesController();
        //    string businessType = "TaskTemplatePreviewAttach";
        //    Lib.Model.Attachment attachment = null;
        //    var finder = AttachmentOperator.Instance.GetLastModel(businessType, taskId);
        //    if (finder != null)
        //    {
        //        attachment = finder;
        //    }
        //    else
        //    {
        //        var fileName = "";
        //        var stream = LoadTaskTemplateStream(taskId, out fileName);
        //        attachment = AttachmentOperator.Instance.CommonUpload(taskId, businessType, fileName, stream);
        //    }
        //    AttachmentOperator.Instance.AddModel(attachment);
        //    var result = filesController.GetPreviewLinkToken(attachment.ID);
        //    return result;
        //}
        private MemoryStream LoadTaskTemplateStream(string taskId, out string fileName)
        {
            var ret = Guid.Empty;
            if (string.IsNullOrEmpty(taskId) || !Guid.TryParse(taskId, out ret))
            {
                throw new BizException("参数错误");
            }
            var task = TemplateTaskOperator.Instance.GetModel(taskId);
            var tci = TemplateConfigInstanceOperator.Instance.GetModel(task.TemplateConfigInstanceID);

            var attachment = AttachmentOperator.Instance.GetModelByCode(tci.TemplatePathFileCode);
            System.IO.Stream stream = FileUploadHelper.DownLoadFileStream(tci.TemplatePathFileCode, attachment.IsUseV1).ToStream();
            ExcelEngine engine = new ExcelEngine();
            Workbook taskBook = new Workbook(stream);

            #region 更新二维表时使用
            var dcu = DataCollectUserOperator.Instance.GetModel(task.DataCollectUserID);
            var templateSheets = TemplateSheetOperator.Instance.GetList(tci.TemplateID);
            if (tci.TaskTemplateType == 2)
            {
                TemplateOperator.Instance.GetTemplateDateForUpdate(taskBook, templateSheets, dcu.UpdateArea, dcu.AreaValue);
            }
            #endregion
            engine.SetCustomProperty(taskBook.Worksheets[0], "TempleteID", tci.TemplateID);
            MemoryStream taskMemStream = new MemoryStream();
            var format = TemplateOperator.Instance.GetFormatType(tci.TemplatePathFileExt);
            taskBook.Save(taskMemStream, format);
            fileName = string.Format("{0}-{1}（{2}）-{3}", tci.TemplateConfigInstanceName, task.EmployeeName, task.EmployeeLoginName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            fileName = string.Format("{0}{1}", fileName, tci.TemplatePathFileExt);
            return taskMemStream;
        }
        /// <summary>
        /// 下载任务模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("template/download/{taskId}")]
        public IHttpActionResult DownloadTaskTemplate(string taskId)
        {
            string fileName = "";
            MemoryStream stream = LoadTaskTemplateStream(taskId, out fileName);

            return new FileResult(fileName, stream.ToArray(), Request);
        }
        /// <summary>
        /// 根据条件获取任务集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public IHttpActionResult GetTaskList([FromBody] TaskFilter filter)
        {
            #region 计算时间范围
            DateTime date = DateTime.MinValue;
            DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            if (filter.TimeRange.HasValue)
            {
                switch (filter.TimeRange)
                {
                    case 1:
                        date = dateNow;
                        break;
                    case 2:
                        date = dateNow.AddDays(-1);
                        break;
                    case 3:
                        date = dateNow.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7);
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
                        //default:
                        //    date = dateNow;
                        //    break;
                }
            }
            #endregion

            VCollectUserTaskFilter queryFilter = new VCollectUserTaskFilter();
            queryFilter.TemplateConfigInstanceName = filter.TaskTitle;
            queryFilter.CreatorTimeStart = date;

            //var roles = RoleinfoOperator.Instance.GetBelongsToRoles(WebHelper.GetCurrentUser().EmployeeCode);
            if (!UserInfoOperator.Instance.IsAdmin() || filter.IsOnlySelf)
            {
                queryFilter.CreatorLoginName = WebHelper.GetCurrentUser().LoginName;
            }
            if (UserInfoOperator.Instance.IsAdmin() && !string.IsNullOrEmpty(filter.TaskCreatorLoginName))
            {
                queryFilter.CreatorLoginOrName = filter.TaskCreatorLoginName;
            }
            //if (!filter.IsOnlySelf)
            //{
            //    var startOrg = AuthHelper.GetPrivilegeStartOrg();
            //    queryFilter.UnitFullPath = startOrg;
            //}
            queryFilter.PageIndex = filter.PageIndex;
            queryFilter.PageSize = filter.PageSize;


            #region 计算任务类型
            switch (filter.TaskType)
            {
                case 1:
                    {
                        queryFilter.TaskTypeList = new List<int>() { 1, 3 };
                        queryFilter.StatusList = new List<int>() { 0, 1, 2, 3 };
                    }
                    break;
                case 2:
                    {
                        //queryFilter.TaskTypeList = new List<int>() { 1, 2, 3 };
                        queryFilter.StatusList = new List<int>() { 4 };
                    }
                    break;
                case 3:
                    {
                        queryFilter.TaskTypeList = new List<int>() { 2 };
                        queryFilter.StatusList = new List<int>() { 0, 1, 2, 3 };
                    }
                    break;
                    //default:
                    //    {
                    //        queryFilter.TaskTypeList = new List<int>() { 1, 3 };
                    //        queryFilter.StatusList = new List<int>() { 0, 1, 2, 3 };
                    //    }
                    //    break;
            }
            #endregion

            if (filter.TaskStatus.HasValue)
            {
                queryFilter.StatusList = new List<int>() { filter.TaskStatus.Value };
            }
            //queryFilter.TaskTypeList = filter.TaskType == 1 ? new List<int>() { 2 } : new List<int>() { 1, 3 };
            //queryFilter.StatusList = new List<int>() { 0, 1, 2, 3 };

            var data = VCollectUserTaskOperator.Instance.GetViewList(queryFilter);
            var tasks = data.Select(x =>
            {
                var task = new Task();
                task.ConvertEntity(x);
                return task;
            });
            return BizResult(new ViewData<Task>() { Data = tasks, TotalCount = data.TotalCount });

        }
        /// <summary>
        /// 根据ID获取任务实体
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("load/{taskId}")]
        public IHttpActionResult GetTaskById(string taskId)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(taskId) || !Guid.TryParse(taskId, out ret))
            {
                throw new BizException("参数错误");
            }
            var totalCount = 0;
            var finder = VCollectUserTaskOperator.Instance.GetViewList(new VCollectUserTaskFilter() { ID = Guid.Parse(taskId) }).FirstOrDefault();
            if (finder == null)
            {
                throw new BizException("任务查找失败");
            }
            var task = new Task();
            //转换实体
            task.ConvertEntity(finder);
            //加载附件集合
            var attachments = AttachmentOperator.Instance.GetList("UploadTaskAttachment", taskId);
            task.ConvertAttachment(attachments);
            //加载时间节点
            var timeNodes = TemplateConfigInstancePlanOperator.Instance.GetList(new TemplateConfigInstancePlanFilter() { TemplateConfigInstanceID = taskId }, out totalCount).ToList();
            timeNodes = timeNodes.OrderBy(x => x.TimeNode).ToList();
            task.ConvertTimeNodes(timeNodes);
            return BizResult(task);
        }
        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="task">任务实体</param>
        /// <param name="userNodes">任务人员节点集合</param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        public IHttpActionResult SaveTask([FromBody]Task task)
        {
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(task.TaskID);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            List<TaskUserNode> userNodes = task.UserNodes;
            return CommonSaveTask(task, userNodes, false);
        }
        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userNodes"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("submit")]
        public IHttpActionResult SubmitTask([FromBody]Task task)
        {
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(task.TaskID);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            List<TaskUserNode> userNodes = task.UserNodes;
            return CommonSaveTask(task, userNodes, true);
        }
        /// <summary>
        /// 终止任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("stop/{taskId}")]
        public IHttpActionResult StopTask(string taskId)
        {
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            TemplateConfigInstanceOperator.Instance.CancelTask(taskId);
            return BizResult(true);
        }
        /// <summary>
        /// 归档任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("filing/{taskId}")]
        public IHttpActionResult FilingTask(string taskId)
        {
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            var instance = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (instance.ProcessStatus == 1)
            {
                throw new BizException("进行中的任务不可以进行归档操作");
            }
            instance.ProcessStatus = 4;
            TemplateConfigInstanceOperator.Instance.UpdateModel(instance);
            return BizResult(true);
        }
        /// <summary>
        /// 下载汇总文件
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="corp">是否勾选公司</param>
        /// <param name="org">是否勾选组织架构</param>
        /// <param name="name">是否勾选姓名</param>
        /// <param name="username">是否勾选账号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("summary/{taskId}/{corp}/{org}/{name}/{username}")]
        public IHttpActionResult LoadSummaryData(string taskId, string corp, string org, string name, string username)
        {
            bool c1 = !string.IsNullOrEmpty(corp) && corp == "1";//公司
            bool c2 = !string.IsNullOrEmpty(org) && org == "1";//组织架构
            bool c3 = !string.IsNullOrEmpty(name) && name == "1";//姓名
            bool c4 = !string.IsNullOrEmpty(username) && username == "1";//账号

            TaskCollectionDataBuilder tdb = new TaskCollectionDataBuilder(taskId, null);
            string fileExt = ".xls";
            //Workbook dataBook = tdb.BuildData(c1, c2, c3, c4, out fileExt);
            Workbook dataBook = tdb.BuildDataFormula(c1, c2, c3, c4, out fileExt);
            SaveFormat format = SaveFormat.Excel97To2003;
            if (fileExt == ".xlsx")
            {
                format = SaveFormat.Xlsx;
            }
            MemoryStream stream = new MemoryStream();
            dataBook.Save(stream, format);
            stream.Seek(0, SeekOrigin.Begin);
            var fileName = string.Format("{0}{1}", dataBook.FileName, fileExt);
            //return BizResult(FileUploadHelper.GetPreviewUrlByStream(stream, fileName));
            return new FileResult(fileName, stream.ToArray(), Request);

        }
        /// <summary>
        /// 获取在线预览地址
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="corp"></param>
        /// <param name="org"></param>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("summarypreview/{taskId}/{corp}/{org}/{name}/{username}")]
        public IHttpActionResult LoadSummaryPreviewUrl(string taskId, string corp, string org, string name, string username)
        {
            bool c1 = !string.IsNullOrEmpty(corp) && corp == "1";//公司
            bool c2 = !string.IsNullOrEmpty(org) && org == "1";//组织架构
            bool c3 = !string.IsNullOrEmpty(name) && name == "1";//姓名
            bool c4 = !string.IsNullOrEmpty(username) && username == "1";//账号

            TaskCollectionDataBuilder tdb = new TaskCollectionDataBuilder(taskId, null);
            string fileExt = ".xls";
            Workbook dataBook = tdb.BuildData(c1, c2, c3, c4, out fileExt);
            SaveFormat format = SaveFormat.Excel97To2003;
            if (fileExt == ".xlsx")
            {
                format = SaveFormat.Xlsx;
            }
            MemoryStream stream = new MemoryStream();
            dataBook.Save(stream, format);
            stream.Seek(0, SeekOrigin.Begin);
            var fileName = string.Format("{0}{1}", dataBook.FileName, fileExt);
            var attachment = AttachmentOperator.Instance.CommonUpload(taskId, "DownLoadTaskSummaryFile", fileName, stream);
            return BizResult(string.Format("/api/attachments/orgstream/{0}", attachment.ID));

        }
        /// <summary>
        /// 打包相关附件
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("attachments/{taskId}")]
        public IHttpActionResult LoadTaskAttachmentList(string taskId)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);

            var attas = AttachmentOperator.Instance.GetAttachmentListByTask(task.ID, "UploadTaskAttach");

            string fileName = task.TemplateName + "_" + task.TemplateConfigInstanceName + "_其他附件";
            if (attas == null || attas.Count == 0)
            {
                throw new BizException("没有可下载的附件");
                //throw new HttpResponseException(new HttpResponseMessage()
                //{
                //    Content = new StringContent("没有可下载的附件"),
                //    StatusCode = HttpStatusCode.Conflict
                //});
            }
            var zip = ZipFile(attas, fileName, 1000);
            if (zip == null)
            {
                throw new BizException("没有可下载的附件");
            }
            return new FileResult(string.Format("{0}{1}", fileName, ".zip"), zip.ToArray(), Request);
        }
        //--------裴晓红--------
        /// <summary>
        /// 打包下载数据源文件
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("taskfile/{taskId}")]
        public IHttpActionResult LoadTaskFileList(string taskId)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);

            //var attas = AttachmentOperator.Instance.GetAttachmentListByTask(task.ID, "UploadTaskAttach");
            var tasks = TemplateTaskOperator.Instance.GetList(taskId);

            string fileName = task.TemplateName + "_" + task.TemplateConfigInstanceName + "_源文件";
            if (tasks == null || tasks.Count == 0)
            {
                throw new BizException("没有可下载的文件");
            }
            if (tasks.Count(x => !string.IsNullOrEmpty(x.FileName)) == 0)
            {
                throw new BizException("没有可下载的文件");
            }
            var zip = ZipFile(tasks, fileName, 1000);
            if (zip == null)
            {
                throw new BizException("没有可下载的文件");
            }
            return new FileResult(string.Format("{0}{1}", fileName, ".zip"), zip.ToArray(), Request);
        }
        //-----------------
        /// <summary>
        /// 通用任务提交方法
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userNodes"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        private IHttpActionResult CommonSaveTask(Task task, List<TaskUserNode> userNodes, bool isSubmit)
        {
            var taskID = task.TaskID;
            var isAdd = true;
            if (!string.IsNullOrEmpty(taskID))
            {
                var finder = VCollectUserTaskOperator.Instance.GetViewList(new VCollectUserTaskFilter() { ID = Guid.Parse(taskID) }).FirstOrDefault();
                if (finder != null)
                {
                    isAdd = false;
                }
            }
            task.TaskTimeNodes = task.TaskTimeNodes ?? new List<TaskTimeNode>();
            //验证
            /*
             * 模板ID必须填写
             * 任务名称必须填写
             * 验证附件列表在数据库中
             * circleType 只可以在1 2 3 的范围中
             * taskType 只可以在 1 2 3 的范围内
             * 已经发起的的任务不可以进行保存 或提交
             * 如果是计划任务 则circleType必须为 2 或者3
             * 如果是非计划任务 则circleType必须为1
             * 如果是计划任务 并且circleType=2 每日任务 必须提供计划开始时间 结束时间
             * 如果是计划任务 必须提供 计划小时 计划分钟数
             * 如果是circleType=3 自定义任务 必须提供时间节点集合 时间节点集合必须至少需要有1条
             * 如果是计划任务 任务开始时间至少应该是今日 开始时间要小于或结束时间
             * 计划时间范围只可以为一年的范围
             * 至少提供一个任务人员节点
             * 提供的人员节点均不离职
             */

            TemplateConfigInstance orgTask = null;
            Lib.Model.Template template = null;
            Guid ret = Guid.Empty;

            if (Guid.Empty == task.TaskTemplateID)
            {
                throw new BizException("请选择模板");
            }
            template = TemplateOperator.Instance.GetModel(task.TaskTemplateID.ToString());
            if (template == null)
            {
                throw new BizException("模板无效");
            }
            if (string.IsNullOrEmpty(task.TaskName))
            {
                throw new BizException("任务名称必填");
            }
            var circleType = new List<int>() { 1, 2, 3 };
            var taskType = new List<int>() { 1, 2, 3 };
            if (!taskType.Contains(task.TaskType))
            {
                throw new BizException("任务类型无效");
            }
            if (!circleType.Contains(task.TaskCircleType))
            {
                throw new BizException("任务周期类型无效");
            }
            if (!isAdd)
            {
                orgTask = TemplateConfigInstanceOperator.Instance.GetModel(task.TaskID.ToString());
                if (orgTask == null)
                {
                    throw new BizException("任务ID填写不正确");
                }
                if (orgTask.ProcessStatus != 0)
                {
                    throw new BizException("该任务已经发起或撤销，无法进行保存操作");
                }
                if (orgTask.TaskType != task.TaskType)
                {
                    throw new BizException("任务类型不可更改");
                }
                if (orgTask.CircleType != task.TaskCircleType)
                {
                    throw new BizException("任务周期类型不可更改");
                }
            }
            if (task.TaskType == 2)
                circleType = new List<int>() { 2, 3 };
            else
                circleType = new List<int>() { 1 };
            if (!circleType.Contains(task.TaskCircleType))
            {
                throw new BizException("任务周期类型不正确");
            }
            if (task.TaskType == 2)
            {
                if (task.TaskCircleType == 2 && (!task.PlanStart.HasValue || !task.PlanEnd.HasValue))
                {
                    throw new BizException("需填写周期任务日期范围");
                }
                if (task.PlanHour <= 0 || task.PlanHour > 23 || task.PlanMinute < 0 || task.PlanMinute > 59)
                {
                    throw new BizException("周期任务时间范围填写不正确");
                }
                if (task.TaskCircleType == 3 && task.TaskTimeNodes.Count == 0)
                {
                    throw new BizException("至少需要填写一个时间节点");
                }
                if (task.TaskCircleType == 3)
                {
                    task.PlanStart = task.TaskTimeNodes.Min(x => x.TimeNode);
                    task.PlanEnd = task.TaskTimeNodes.Max(x => x.TimeNode);
                }
                task.PlanStart = new DateTime(task.PlanStart.Value.Year, task.PlanStart.Value.Month, task.PlanStart.Value.Day);
                task.PlanEnd = new DateTime(task.PlanEnd.Value.Year, task.PlanEnd.Value.Month, task.PlanEnd.Value.Day);
                if (task.PlanStart < DateTime.Today && isAdd)
                {
                    throw new BizException("请选择今日及以后的日期");
                }
                if (task.PlanStart > task.PlanEnd)
                {
                    throw new BizException("日期范围不正确");
                }
                if ((task.PlanEnd.Value - task.PlanStart.Value).TotalDays > 365)
                {
                    throw new BizException("计划任务仅支持一年以内的范围");
                }
            }
            if (userNodes == null || userNodes.Count == 0)
            {
                throw new BizException("任务至少需要有一个人员节点");
            }
            var currentTaskUserList =
                   UserInfoOperator.Instance.GetWDUserInfoByLoginNameList(userNodes.Select(x => x.TaskUserLoginName).ToList());
            var NotNormalEmployeeStatusUserList =
                   currentTaskUserList.Where(x => !x.IsNormalEmployeeStatus).ToList();

            if (NotNormalEmployeeStatusUserList.Any() && !isSubmit)
            {
                string errorMsg = string.Format("用户：{0}，为非有效员工状态，无法进行该操作，请删除后再进行提交操作",
                    string.Join("、",
                        NotNormalEmployeeStatusUserList.Select(x => string.Format("{0}（{1}）", x.CNName, x.LoginName)))
                );
                throw new BizException(errorMsg);
            }
            //var result = Guid.NewGuid();
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                string needApprovalWorkflowID = System.Configuration.ConfigurationManager.AppSettings["ApprovalWorkflowID"];
                var wrklst = EnumOperator.Instance.GetList("WorkFolw").Where(p =>
                {
                    return task.IsNeedApprove == p.EnumCode.Equals(needApprovalWorkflowID, StringComparison.CurrentCultureIgnoreCase);

                }).FirstOrDefault();
                TemplateConfigInstance configInstance = new TemplateConfigInstance();
                var userinfo = WebHelper.GetCurrentUser(); ;
                if (isAdd)
                {
                    configInstance.ID = task.TaskID.ToString();
                    configInstance.UserName = userinfo.LoginName;
                    configInstance.EmployeeName = userinfo.CNName;
                    configInstance.CreateDate = DateTime.Now;
                    configInstance.CreatorLoginName = userinfo.LoginName;
                    configInstance.CreatorName = userinfo.CNName;
                    configInstance.IsDeleted = false;
                    //2表示更新二位表格
                    configInstance.TaskTemplateType = task.TaskTemplateType;
                }
                else
                {
                    configInstance = orgTask;
                    //result = Guid.Parse(orgTask.ID);
                }

                configInstance.TemplateConfigInstanceName = task.TaskName;
                configInstance.TemplateID = task.TaskTemplateID.ToString();
                configInstance.TemplateName = task.TaskTemplateName;
                configInstance.Remark = task.Remark;
                configInstance.WorkflowID = wrklst.EnumCode;
                configInstance.WorkflowInfo = wrklst.EnumName;
                configInstance.TemplatePath = template.TemplatePath;
                configInstance.CircleType = task.TaskCircleType;
                configInstance.TaskType = task.TaskType;
                configInstance.PlanBeginDate = task.PlanStart;
                configInstance.PlanEndDate = task.PlanEnd;
                configInstance.PlanHour = task.PlanHour;
                configInstance.PlanMinute = task.PlanMinute;
                configInstance.ModifierLoginName = userinfo.LoginName;
                configInstance.ModifierName = userinfo.CNName;
                configInstance.ModifyTime = DateTime.Now;
                configInstance.ProcessStatus = isSubmit ? 1 : 0;
                if (isAdd)
                {
                    TemplateConfigInstanceOperator.Instance.AddModel(configInstance);
                    task.TaskTimeNodes.ForEach(x =>
                    {
                        TemplateConfigInstancePlanOperator.Instance.AddModel(new TemplateConfigInstancePlan()
                        {
                            CreateDate = DateTime.Now,
                            CreatorName = configInstance.CreatorName,
                            CreatorLoginName = configInstance.CreatorLoginName,
                            ID = Guid.NewGuid().ToString(),
                            IsDeleted = false,
                            ModifierLoginName = configInstance.ModifierLoginName,
                            ModifierName = configInstance.ModifierName,
                            ModifyTime = configInstance.ModifyTime,
                            Status = 1,
                            SubTemplateConfigInstanceID = Guid.Empty,
                            TemplateConfigInstanceID = configInstance.ID,
                            TimeNode = x.TimeNode.Value,
                            SenderTime = new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day, configInstance.PlanHour,
                                configInstance.PlanMinute, 0)
                        });
                    });
                }
                else
                {
                    TemplateConfigInstanceOperator.Instance.UpdateModel(configInstance);
                    //--比较差异数据进行入库
                    var totalCount = 0;
                    var orgList = TemplateConfigInstancePlanOperator.Instance.GetList(new Lib.Model.Filter.TemplateConfigInstancePlanFilter() { TemplateConfigInstanceID = task.TaskID.ToString() }, out totalCount);
                    //按天计算记录
                    var addList = task.TaskTimeNodes.Where(x =>
                        orgList.All(t => new DateTime(t.TimeNode.Year, t.TimeNode.Month, t.TimeNode.Day) != new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day))
                    ).ToList();
                    var deleteList = orgList.Where(x =>
                    task.TaskTimeNodes.All(t => new DateTime(t.TimeNode.Value.Year, t.TimeNode.Value.Month, t.TimeNode.Value.Day) != new DateTime(x.TimeNode.Year, x.TimeNode.Month, x.TimeNode.Day))
                    ).ToList();
                    var editList = task.TaskTimeNodes.Where(x =>
                            orgList.Any(
                                t =>
                                    new DateTime(t.TimeNode.Year, t.TimeNode.Month, t.TimeNode.Day) ==
                                    new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day))
                    ).ToList();
                    addList.ForEach(x => TemplateConfigInstancePlanOperator.Instance.AddModel(new TemplateConfigInstancePlan()
                    {
                        CreateDate = DateTime.Now,
                        CreatorName = configInstance.CreatorName,
                        CreatorLoginName = configInstance.CreatorLoginName,
                        ID = Guid.NewGuid().ToString(),
                        IsDeleted = false,
                        ModifierLoginName = configInstance.ModifierLoginName,
                        ModifierName = configInstance.ModifierName,
                        ModifyTime = configInstance.ModifyTime,
                        Status = 1,
                        SubTemplateConfigInstanceID = Guid.Empty,
                        TemplateConfigInstanceID = configInstance.ID,
                        TimeNode = x.TimeNode.Value,
                        SenderTime = new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day, configInstance.PlanHour, configInstance.PlanMinute, 0)
                    }));
                    deleteList.ForEach(x => TemplateConfigInstancePlanOperator.Instance.RemoveModel(x.ID));
                    editList.ForEach(x =>
                    {
                        var f = orgList.FirstOrDefault(t =>
                            new DateTime(t.TimeNode.Year, t.TimeNode.Month, t.TimeNode.Day) ==
                                new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day)
                        );
                        if (f != null)
                        {
                            f.TimeNode = x.TimeNode.Value;
                            f.SenderTime = new DateTime(x.TimeNode.Value.Year, x.TimeNode.Value.Month, x.TimeNode.Value.Day, configInstance.PlanHour,
                                configInstance.PlanMinute, 0);
                            TemplateConfigInstancePlanOperator.Instance.UpdateModel(f);
                        }
                    });
                }
                if (isSubmit && task.TaskType == 3)
                {
                    var plan = TemplateConfigInstancePlanOperator.Instance.GetModelBySubTask(configInstance.ID);
                    if (plan == null || plan.Status != 2)
                    {
                        throw new Exception("保存子任务失败");
                    }
                    plan.Status = 3;
                    plan.SenderTime = DateTime.Now;
                    TemplateConfigInstancePlanOperator.Instance.UpdateModel(plan);
                }
                List<TaskUser> taskUserList = currentTaskUserList
                    .Select(x =>
                    {
                        return new TaskUser()
                        {
                            EmployeeCode = x.EmployeeCode,
                            OrgID = x.OrgID,
                            UnitID = x.UnitID,
                            UnitName = x.UnitName,
                            EmployeeName = x.CNName,
                            OrgName = x.OrgName,
                            UserName = x.LoginName,
                            TaskTemplateType = configInstance.TaskTemplateType,
                            AreaValue = userNodes.Where(v => v.TaskUserLoginName == x.LoginName).FirstOrDefault().AreaValue,
                            UpdateArea = userNodes.Where(v => v.TaskUserLoginName == x.LoginName).FirstOrDefault().UpdateArea
                        };
                    }).ToList();
                if (taskUserList.GroupBy(x => x.UserName).Any(e => e.Count() > 1))
                {
                    throw new BizException("人员节点信息重复，请仔细核对");
                }
                TemplateConfigInstanceOperator.Instance.UpdateTaskInfo(configInstance, taskUserList,
                       isSubmit);
                scope.Complete();
            }

            return BizResult(task.TaskID);
        }
        /// <summary>
        /// 附件压缩成内存流
        /// </summary>
        /// <param name="atts"></param>
        /// <param name="zipedFile"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private MemoryStream ZipFile(List<Lib.Model.Attachment> atts, string zipedFile, int blockSize)
        {
            string tmpFilePath = System.IO.Path.GetTempPath() + "\\" + Guid.NewGuid().ToString();
            string tmpZipFilePath = tmpFilePath + "\\" + Guid.NewGuid().ToString() + ".zip";
            //tmpZipFilePath = Path.ChangeExtension(tmpZipFilePath, "zip");
            using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(Encoding.GetEncoding("GB2312")))
            {
                var t = atts.GroupBy(p => p.CreatorLoginName);

                foreach (var itemKey in t)
                {
                    var folderName = string.Format("{0}({1})", itemKey.FirstOrDefault().CreatorName, itemKey.Key);
                    List<string> files = new List<string>();
                    string newPath = Path.Combine(tmpFilePath, folderName);
                    if (!Directory.Exists(newPath))
                        Directory.CreateDirectory(newPath);
                    zipFile.AddDirectory(newPath);
                    foreach (var item in itemKey)
                    {
                        string fileName = item.Name;
                        int index = 1;
                        while (files.Any(x => x.ToLower() == fileName.ToLower()))
                        {
                            var newFileName = BuilderNewFileName(fileName, index);
                            fileName = newFileName;
                            index++;
                        }
                        string filePath = Path.Combine(newPath, fileName);
                        File.WriteAllBytes(filePath, FileUploadHelper.DownLoadFileStream(item.AttachmentPath, item.IsUseV1));
                        zipFile.AddFile(filePath, folderName);
                        files.Add(fileName);
                    }
                }
                MemoryStream stream = new MemoryStream();
                zipFile.Save(stream);
                return stream;
            }
        }
        //---------------裴晓红-----------------------
        /// <summary>
        /// 源文件压缩成内存流
        /// </summary>
        /// <param name="atts"></param>
        /// <param name="zipedFile"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private MemoryStream ZipFile(List<Lib.Model.TemplateTask> tasks, string zipedFile, int blockSize)
        {
            string tmpFilePath = System.IO.Path.GetTempPath() + "\\" + Guid.NewGuid().ToString();
            string tmpZipFilePath = tmpFilePath + "\\" + Guid.NewGuid().ToString() + ".zip";
            using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(Encoding.GetEncoding("GB2312")))
            {
                var folderName = zipedFile;
                string newPath = Path.Combine(tmpFilePath, folderName);
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);
                zipFile.AddDirectory(newPath);
                List<string> files = new List<string>();
                foreach (var itemKey in tasks)
                {
                    if (string.IsNullOrEmpty(itemKey.FileName))
                        continue;
                    string fileName = itemKey.FileName;
                    int index = 1;
                    while (files.Any(x => x.ToLower() == fileName.ToLower()))
                    {
                        var newFileName = BuilderNewFileName(fileName, index);
                        fileName = newFileName;
                        index++;
                    }
                    string filePath = Path.Combine(newPath, fileName);
                    File.WriteAllBytes(filePath, FileUploadHelper.DownLoadFileStream(itemKey.FilePath, false));
                    zipFile.AddFile(filePath, folderName);
                    files.Add(fileName);
                }
                MemoryStream stream = new MemoryStream();
                zipFile.Save(stream);
                return stream;
            }
        }


        /// <summary>
        /// 构建文件名，如果存在同名的文件 则进行重命名操作
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string BuilderNewFileName(string fileName, int index)
        {
            var fix = string.Format("副本{0}-", index == 1 ? "" : string.Format("({0})", index));
            string newName = string.Format("{0}{1}", fix, fileName);
            return newName;
        }

        #region 下载审批流程 by  张永刚


        [HttpGet]
        [Route("taskapprovedlist/{taskId}")]
        public IHttpActionResult DownloadTaskApprovedList(string taskId)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);

            //var attas = AttachmentOperator.Instance.GetAttachmentListByTask(task.ID, "UploadTaskAttach");
            var tasks = TemplateTaskOperator.Instance.GetList(taskId);

            //string fileName = System.IO.Path.Combine(CommonFilePath, task.TemplateConfigInstanceName + "_审批流程.xlsx");


            var ds = DbHelper.RunSqlReturnDS(string.Format(@" EXEC P_GetTaskApprovedList '{0}' ", taskId), "DBConnectionString");
            if (ds == null || ds.Tables.Count < 1)
            {
                throw new BizException("没有可下载的文件");
            }
            //数据导出列
            string[] columnnames = new string[] { "序号", "接收人部门", "接收人", "填报日期", "审批日期", "审批流程" };

            ImportExcelCriteria iecriteria = new ImportExcelCriteria();
            iecriteria.data = ds.Tables[0];//导出的数据
            iecriteria.ColumnNames = columnnames;//excel显示列
            iecriteria.TempleName = "TaskApprovedListTemplate.xlsx";//模板名字
            iecriteria.ExcelName = task.TemplateConfigInstanceName + "_审批流程";//文件名字
            iecriteria.RowIndex = 1;//从第几行插入数据
            iecriteria.CellIndex = 0;//从第几列插入数据
            iecriteria.RecordStatus = "Create";//新增
            iecriteria.IsAddHj = false;
            Dictionary<string, string> dictionary = ExcelData(iecriteria);

            var ms = new MemoryStream();
            using (FileStream fileStream = new FileStream(dictionary["filePath"], FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                // 读取文件的 byte[]

                byte[] bytes = new byte[fileStream.Length];

                fileStream.Read(bytes, 0, bytes.Length);

                fileStream.Close();

                // 把 byte[] 转换成 Stream

                ms = new MemoryStream(bytes);

            }

            if (File.Exists(dictionary["filePath"]))
            {
                File.Delete(dictionary["filePath"]);
            }

            //return new FileResult(string.Format("{0}{1}", Guid.NewGuid(), ".xlsx"), ms.ToArray(), Request);
            //Request.setCharacterEncoding()
            return new FileResult(string.Format("{0}_审批流程{1}", task.TemplateConfigInstanceName, ".xlsx"), ms.ToArray(), Request);
        }


        private Dictionary<string, string> ExcelData(ImportExcelCriteria criteria)
        {
            string fileName = criteria.ExcelName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx"; // 文件名称
            string urlPath = Path.Combine(CommonFilePath, "File//Download//") + fileName; // 文件下载的URL地址，供给前台下载
            string filePath = Path.Combine(CommonFilePath, "File//ExcelTemplate//") + fileName; // 文件路径
            Dictionary<string, string> idictionary = new Dictionary<string, string>();
            idictionary.Add("urlPath", urlPath);
            idictionary.Add("filePath", filePath);

            // 1.检测是否存在文件夹，若不存在就建立个文件夹
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            ISheet sheet = null;//申明标签页
            string url = string.Empty;
            //把文件内容导入到工作薄当中，然后关闭文件
            if (criteria.RecordStatus == "Edit")
            {
                url = criteria.TempleName;
            }
            else
            {
                url = Path.Combine(CommonFilePath, "File//ExcelTemplate//") + criteria.TempleName;
            }
            FileStream fs = File.OpenRead(url);
            IWorkbook workbook = new XSSFWorkbook(fs);
            //设置单元格样式
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            fs.Close();

            if (workbook != null)
            {
                sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                IDataFormat dataformat = workbook.CreateDataFormat();
                //如果有数据就进行导出操作
                if (criteria.data.Rows.Count > 0)
                {
                    //读取数据
                    for (int i = 0; i < criteria.data.Rows.Count; i++)
                    {
                        //创建行
                        IRow row = sheet.CreateRow(i + criteria.RowIndex);
                        if (criteria.IsAddHj)
                        {
                            ICell cell = row.CreateCell(0);
                            cell.SetCellValue("合计");
                            style.DataFormat = dataformat.GetFormat("#,##0.00");
                            cell.CellStyle = style;
                        }
                        for (int j = 0; j < criteria.ColumnNames.Length; j++)
                        {
                            ICell cell = row.CreateCell(j + criteria.CellIndex);
                            cell.CellStyle = style;
                            string columnName = criteria.ColumnNames[j];
                            switch (criteria.data.Rows[i][columnName].GetType().Name)
                            {
                                case "String"://字符串类型
                                    var stringvalue = criteria.data.Rows[i][columnName];
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(stringvalue.ToString());
                                    break;
                                case "Decimal"://钱型
                                    //设置 数字格式，样式：123，234.00
                                    style.DataFormat = dataformat.GetFormat("#,##0.00");//分段添加，号
                                    var decimalzhi = criteria.data.Rows[i][columnName];
                                    cell.CellStyle = style;
                                    cell.SetCellValue(Convert.ToDouble(decimalzhi));
                                    break;
                                case "Int16"://整型
                                case "Int32":
                                case "Int64":
                                case "Byte":
                                    var intzhi = criteria.data.Rows[i][columnName];
                                    cell.SetCellValue(Convert.ToInt32(intzhi));
                                    break;
                                case "DBNull"://空值处理
                                    cell.SetCellValue("");
                                    break;
                                case "DateTime"://日期类型
                                    var datetimezhi = Convert.ToDateTime(criteria.data.Rows[i][columnName]).ToString("yyyy-MM-dd");
                                    cell.SetCellValue(datetimezhi);
                                    break;
                            }
                        }
                    }
                }
            }
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();

            // 5。返回路径
            return idictionary;
        }

        class ImportExcelCriteria
        {
            /// <summary>
            /// 数据源DataTable
            /// </summary>
            public DataTable data { get; set; }

            /// <summary>
            /// 数据插入的列，请按照页面显示顺序插入
            /// </summary>
            public string[] ColumnNames { get; set; }

            /// <summary>
            /// 模板带后缀的名字   注：模板存放在web程序下Contents/files 文件夹中
            /// </summary>
            public string TempleName { get; set; }

            /// <summary>
            /// 文件生成的名字
            /// </summary>
            public string ExcelName { get; set; }

            /// <summary>
            /// 从第几行插入数据，取决于模板
            /// </summary>
            public int RowIndex { get; set; }

            /// <summary>
            /// 从第几列插入数据，取决于模板
            /// </summary>
            public int CellIndex { get; set; }

            /// <summary>
            /// 状态：新增，编辑
            /// </summary>
            public string RecordStatus { get; set; }

            /// <summary>
            /// 是否增加合计列
            /// </summary>
            public bool IsAddHj { get; set; }
        }


        #endregion

    }
}
