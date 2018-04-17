
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using System.Transactions;
using System.Web;
using Framework.Core;
using Framework.Web.Security.Authentication;
using Lib.Common;
using Lib.Model.Filter;
using Lib.ViewModel;
using Framework.Web.Utility;

namespace Lib.BLL
{
    /// <summary>
    /// TemplateConfigInstance对象的业务逻辑操作
    /// </summary>
    public class TemplateConfigInstanceOperator : BizOperatorBase<TemplateConfigInstance>
    {

        #region Generate Code

        public static readonly TemplateConfigInstanceOperator Instance = PolicyInjection.Create<TemplateConfigInstanceOperator>();

        /// <summary>
        /// 是否已开始使用系统
        /// 如果有过创建任务或创建过模板就算使用过系统
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsStartToUse(string userID)
        {
            return _templateconfiginstanceAdapter.IsStartToUse(userID);
        }

        private static TemplateConfigInstanceAdapter _templateconfiginstanceAdapter = AdapterFactory.GetAdapter<TemplateConfigInstanceAdapter>();

        protected override BaseAdapterT<TemplateConfigInstance> GetAdapter()
        {
            return _templateconfiginstanceAdapter;
        }

        public Tuple<List<TemplateTask>, List<TemplateTask>, List<DataCollectUser>, List<DataCollectUser>> UpdateTaskInfo(TemplateConfigInstance instance, List<TaskUser> currentTaskUser, bool withProcess)
        {
            var taskAddList = new List<TemplateTask>();
            var taskDeleteList = new List<TemplateTask>();
            var dataCollectUserAddList = new List<DataCollectUser>();
            var dataCollectUserDeleteList = new List<DataCollectUser>();
            using (TransactionScope transaction = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                var orgList = DataCollectUserOperator.Instance.GetList(instance.ID).ToList();
                var newList = currentTaskUser;

                var addList = newList.FindAll(x => !orgList.Any(o => o.UserName == x.UserName));
                var deleteList = orgList.FindAll(x => !newList.Any(n => n.UserName == x.UserName));
                var editList = orgList.FindAll(x => newList.Any(n => n.UserName == x.UserName));
                string flowCode = instance.WorkflowID;
                string taskTitle = instance.TemplateConfigInstanceName;


                var currentTaskList = TemplateTaskOperator.Instance.GetList(instance.ID);
                #region 删除
                deleteList.ForEach(x =>
                {
                    dataCollectUserDeleteList.Add(x);
                    if (currentTaskList.Any(t => t.DataCollectUserID == x.ID))
                    {
                        x.ProcessStatus = 1;
                        DataCollectUserOperator.Instance.UpdateModel(x);
                    }
                    else
                    {
                        DataCollectUserOperator.Instance.RemoveModel(x.ID);
                    }
                });
                #endregion

                #region 新增
                addList.ForEach(x =>
                {
                    DataCollectUser dUsers = new DataCollectUser();
                    var wdUser = UserInfoOperator.Instance.GetWDUserInfoByLoginName(x.UserName);
                    if (wdUser != null)
                    {
                        //todo:发送流程。
                        dUsers.ID = Guid.NewGuid().ToString();
                        dUsers.TemplateID = instance.TemplateID;
                        dUsers.TemplateName = instance.TemplateName;
                        dUsers.UserName = wdUser.LoginName;
                        dUsers.EmployeeCode = wdUser.EmployeeCode;
                        dUsers.OrgID = wdUser.OrgID;
                        dUsers.OrgName = wdUser.OrgName;
                        dUsers.UnitID = wdUser.UnitID;
                        dUsers.UnitName = wdUser.UnitName;
                        dUsers.EmployeeName = wdUser.CNName;
                        dUsers.TemplateConfigInstanceID = instance.ID;
                        dUsers.CreateDate = DateTime.Now;
                        dUsers.CreatorLoginName = WebHelper.GetCurrentUser().LoginName;
                        dUsers.CreatorName = WebHelper.GetCurrentUser().CNName;
                        dUsers.IsDeleted = false;
                        dUsers.ModifierLoginName = WebHelper.GetCurrentUser().LoginName;
                        dUsers.ModifierName = WebHelper.GetCurrentUser().CNName;
                        dUsers.ModifyTime = DateTime.Now;

                        #region 用于二维表更新
                        dUsers.TaskTemplateType = instance.TaskTemplateType;
                        dUsers.UpdateArea = x.UpdateArea;
                        dUsers.AreaValue = x.AreaValue;
                        #endregion

                        DataCollectUserOperator.Instance.AddModel(dUsers);
                        dataCollectUserAddList.Add(dUsers);
                    }
                });
                #endregion

                #region 编辑
                editList.ForEach(x =>
                {
                    var v = currentTaskUser.Where(y => y.UserName == x.UserName &&(y.AreaValue!=x.AreaValue||y.UpdateArea!=x.UpdateArea)).FirstOrDefault();
                    if (v != null)
                    {
                        x.AreaValue = v.AreaValue;
                        x.UpdateArea = v.UpdateArea;
                        DataCollectUserOperator.Instance.UpdateModel(x);
                    }
                });
                #endregion


                var currentCollectUserList = DataCollectUserOperator.Instance.GetList(instance.ID);
                if (withProcess)
                {
                    //StartCollectTask(model.WorkflowID, model.TemplateConfigInstanceName, instance, x, wdUser);
                    #region 计算流程变更状态
                    var addTaskList = currentCollectUserList.Where(x => !currentTaskList.Any(t => t.DataCollectUserID == x.ID));
                    var deleteTaskList = currentTaskList.Where(x => !currentCollectUserList.Any(d => d.ID == x.DataCollectUserID));
                    deleteTaskList.ForEach(x =>
                    {
                        TemplateConfigInstanceOperator.Instance.CancelCollectTask(x);
                        taskDeleteList.Add(x);
                    });
                    addTaskList.ForEach(x =>
                    {
                        var task = StartCollectTask(flowCode, taskTitle, instance, x, new LoginUserInfo()
                        {
                            EmployeeCode = x.EmployeeCode,
                            OrgID = x.OrgID,
                            OrgName = x.OrgName,
                            LoginName = x.UserName,
                            CNName = x.EmployeeName
                        });
                        if (task != null)
                        {
                            taskAddList.Add(task);
                            var finder = dataCollectUserAddList.Find(c => c.ID == task.DataCollectUserID);
                            if (finder != null)
                            {
                                finder.LastTaskID = task.ID;
                                finder.AuthTimeString = task.AuthTimeString;
                                finder.SubmitTimeString = task.SubmitTimeString;
                            }
                        }
                    });
                    #endregion
                }
                transaction.Complete();
            }
            return Tuple.Create(taskAddList, taskDeleteList, dataCollectUserAddList, dataCollectUserDeleteList);
        }

        public TemplateTask StartCollectTask(string flowCode, string taskTitle, TemplateConfigInstance configInstance, DataCollectUser dUsers, LoginUserInfo wdUser)
        {
            var currentUser = UserInfoOperator.Instance.GetWDUserInfoByLoginName(wdUser.LoginName);
            TemplateTask task = new TemplateTask();
            task.ID = Guid.NewGuid().ToString();
            task.DataCollectUserID = dUsers.ID;
            task.EmployeeCode = currentUser.EmployeeCode;
            task.OrgID = currentUser.OrgID;
            task.OrgName = currentUser.UnitName;
            task.EmployeeLoginName = currentUser.LoginName;
            task.EmployeeName = currentUser.CNName;
            task.TemplateConfigInstanceID = configInstance.ID;
            task.Status = (int)Lib.Common.ProcessStatus.Draft;
            task.CreateDate = DateTime.Now;
            task.CreatorLoginName = WebHelper.GetCurrentUser().LoginName;
            task.CreatorName = WebHelper.GetCurrentUser().CNName;
            task.IsDeleted = false;
            task.ModifierLoginName = WebHelper.GetCurrentUser().LoginName;
            task.ModifierName = WebHelper.GetCurrentUser().CNName;
            task.ModifyTime = DateTime.Now;


            Employee startUser = new Employee { LoginName = currentUser.LoginName };
            Employee lastUser = new Employee { LoginName = configInstance.UserName };
            var data = new { flowCode = flowCode, taskID = task.ID, startUser = startUser, lastUser = lastUser, taskTitle = taskTitle, approvalContent = "" };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            AutoProcess _autoprocess = new AutoProcess()
            {
                BusinessType = "StartProcess",
                BusinessID = task.ID,
                Parameters = json
            };
            AutoProcessOperator.Instance.AddAutoproces(_autoprocess);
            //WorkFlowUtil.StartProcess(flowCode, task.ID, startUser, lastUser, taskTitle, "");
            TemplateTaskOperator.Instance.AddModel(task);
            return task;
        }

        public int CancelTask(string id)
        {
            int result = 0;
            using (TransactionScope transaction = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                TemplateConfigInstance tem = TemplateConfigInstanceOperator.Instance.GetModel(id);
                //--终止任务 将该任务的流程状态设置为2
                tem.ProcessStatus = 2;
                TemplateConfigInstanceOperator.Instance.UpdateModel(tem);
                if (tem.TaskType == 3)
                {
                    var plan = TemplateConfigInstancePlanOperator.Instance.GetModelBySubTask(tem.ID);
                    if (plan != null)
                    {
                        plan.Status = 4;
                        TemplateConfigInstancePlanOperator.Instance.UpdateModel(plan);
                    }
                }
                if (tem.TaskType != 2)
                {
                    //找到该任务下所有的task 取消流程
                    var taskList = TemplateTaskOperator.Instance.GetList(tem.ID);
                    taskList.ForEach(x =>
                    {
                        TemplateConfigInstanceOperator.Instance.CancelCollectTask(x, false);
                    });
                }
                result = 100;
                transaction.Complete();
            }
            return result;
        }

        public bool CancelCollectTask(TemplateTask task, bool withDeleted = true)
        {
            var content = string.Format("{0}删除{1}填报任务,流程作废.", WebHelper.GetCurrentUser().CNName, task.EmployeeName);

            var data = new { taskID = task.ID, currentUser = new Employee { LoginName = task.EmployeeLoginName }, approvalContent = content };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            AutoProcess _autoprocess = new AutoProcess()
            {
                BusinessType = "CancelProcess",
                BusinessID = task.ID,
                Parameters = json
            };

            AutoProcessOperator.Instance.AddAutoproces(_autoprocess);
            //WorkFlowUtil.CancelProcess(task.ID, new UserInfo { UserLoginID = task.EmployeeLoginName }, content);
            if (withDeleted)
            {
                task.ProcessStatus = 1;
                TemplateTaskOperator.Instance.UpdateModel(task);
            }
            return true;
        }

        public string base64Decode(string data)
        {
            try
            {

                byte[] datas = System.Convert.FromBase64String(data);
                string strPath = System.Text.Encoding.GetEncoding("UTF-8").GetString(datas);
                return strPath;

            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        public PartlyCollection<VCollectUserTask> SearchTask(string TempName, string time, int PageIndex, int PageSize, bool isWithFinish)
        {
            if (!string.IsNullOrEmpty(TempName))
            {
                TempName = HttpUtility.UrlDecode(TempName.Trim());
            }
            else
            {
                TempName = string.Empty;
            }
            if (!string.IsNullOrEmpty(time))
            {
                time = base64Decode(HttpUtility.UrlDecode(time.Trim()));
            }
            DateTime date = DateTime.MinValue;
            DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            switch (time)
            {
                case "1":
                    date = dateNow;
                    break;
                case "2":
                    date = dateNow.AddDays(-1);
                    break;
                case "3":
                    date = dateNow.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7);
                    break;
                case "4":
                    date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
                    break;
                case "5":
                    date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-3);
                    break;
                case "6":
                    date = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
                    break;
            }
            VCollectUserTaskFilter filter = new VCollectUserTaskFilter();
            filter.TemplateConfigInstanceName = TempName;
            filter.CreatorTimeStart = date;
            filter.CreatorLoginName = WebHelper.GetCurrentUser().LoginName;
            filter.PageIndex = PageIndex;
            filter.PageSize = PageSize;
            var statusList = new List<int>();
            statusList.Add(0);
            statusList.Add(1);
            statusList.Add(2);
            statusList.Add(3);
            if (isWithFinish)
            {
                statusList.Add(4);
            }
            filter.StatusList = statusList;
            var result = VCollectUserTaskOperator.Instance.GetViewList(filter);
            return result;
            //return new {DataList = result, TotalCount = result.TotalCount};
        }

        public IList<TemplateConfigInstance> GetList()
        {
            IList<TemplateConfigInstance> result = _templateconfiginstanceAdapter.GetList();
            return result;
        }

        public string AddModel(TemplateConfigInstance data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateConfigInstance GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(TemplateConfigInstance data)
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

        public List<TemplateConfigInstance> GetList(string loginName, string name, string time, string endTime)
        {
            return _templateconfiginstanceAdapter.GetList(loginName, name, time, endTime);
        }


        #endregion


        public void ScheduleTask(int topCount)
        {
            _templateconfiginstanceAdapter.ScheduleTask(topCount);
        }
        public void DoneTask()
        {
            _templateconfiginstanceAdapter.DoneTask();
        }

        public List<TemplateConfigInstance> LoadClearTaskList(int topCount)
        {
            return _templateconfiginstanceAdapter.LoadClearTaskList(topCount);
        }

        public List<TemplateConfigInstance> LoadPendingDoneTaskNotifyList(int topCount)
        {
            return _templateconfiginstanceAdapter.LoadPendingDoneTaskNotifyList(topCount);
        }
    }
}

