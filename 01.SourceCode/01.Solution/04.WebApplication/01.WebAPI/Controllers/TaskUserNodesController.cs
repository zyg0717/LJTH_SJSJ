using Aspose.Cells;
using WebApplication.WebAPI.Controllers.Base;
using WebApplication.WebAPI.Models;
using WebApplication.WebAPI.Models.FilterModels;
using WebApplication.WebAPI.Models.Helper;
using Framework.Data;
using Framework.Web.MVC;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Http;
using Lib.BLL;
using Lib.Model;
using Lib.Model.Filter;
using Lib.ViewModel;

namespace WebApplication.WebAPI.Controllers
{
    /// <summary>
    /// 任务人员节点相关信息接口
    /// </summary>
    [RoutePrefix("api/usernodes")]
    public class TaskUserNodesController : BaseController
    {
        /// <summary>
        /// 解析excel批量导入数据（仅解析，实际结果需自己处理）
        /// </summary>
        /// <param name="businessid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import/{businessid}")]
        public IHttpActionResult ImportTaskUserNode(string businessid)
        {
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(businessid) || !Guid.TryParse(businessid, out ret))
            {
                throw new BizException("参数错误");
            }
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(businessid);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            var request = HttpContext.Current.Request;
            var setting = AttachmentOperator.Instance.CommonSetting();
            var model = AttachmentOperator.Instance.CommonUpload(businessid, setting.Item1, setting.Item2, setting.Item3);
            var importLength = 0;
            var successLength = 0;
            var errorLength = 0;
            var repeatLength = 0;
            var dataStatus = 0;
            var dataStatusMsg = "";
            var successUserList = new List<TaskUserNode>();
            var errorDataList = new List<Entity<string>>();
            var repeatUserList = new List<LoginUserInfo>();
            try
            {
                var collectUserList = DataCollectUserOperator.Instance.GetList(businessid).ToList();

                //批量追加任务人员
                //解析上传上来的excel文件  获取第一列所有的行(不包含表头)
                Workbook book = new Workbook(setting.Item3);
                var sheet = book.Worksheets[0];
                List<string> accountList = new List<string>();
                for (int i = 0; i <= sheet.Cells.MaxDataRow; i++)
                {
                    var cell = sheet.Cells[i, 0];
                    var cellValue = cell.StringValue.ToLower();
                    if (!string.IsNullOrEmpty(cellValue) && !accountList.Contains(cellValue))
                    {
                        accountList.Add(cell.StringValue);
                    }
                }
                var loginUserList = UserInfoOperator.Instance.GetWDUserInfoByLoginNameList(accountList);


                errorDataList = accountList.Where(x => loginUserList.All(e => e.LoginName.ToLower() != x.ToLower())).Select(x =>
                {
                    var entity = new Entity<string>
                    {
                        Data = "用户不存在",
                        Message = "用户不存在",
                        Status = -1
                    };
                    return entity;
                }).ToList();
                repeatUserList =
                    loginUserList.Where(x => collectUserList.Any(t => t.UserName.ToLower() == x.LoginName))
                        .ToList();
                successUserList =
                    loginUserList.Where(
                        x =>
                        repeatUserList.All(t => t.LoginName.ToLower() != x.LoginName.ToLower())
                        ).Select(x =>
                        {
                            TaskUserNode node = new TaskUserNode();
                            node.ApproveDate = null;
                            node.BusinessID = null;
                            node.ReceiveDate = null;
                            node.TaskUserDeparment = x.UnitName;
                            node.TaskUserLoginName = x.LoginName;
                            node.TaskUserName = x.CNName;
                            node.TaskUserPosition = x.jobName;
                            return node;
                        }).ToList();
                importLength = accountList.Count;
                successLength = successUserList.Count;
                errorLength = errorDataList.Count;
                repeatLength = repeatUserList.Count;
                dataStatus = 1;
                dataStatusMsg = "解析成功";
            }
            catch (Exception ex)
            {
                dataStatus = 0;
                dataStatusMsg = ex.Message;
            }
            var data = new
            {
                status = dataStatus,
                importlength = importLength,
                successlength = successLength,
                errorlength = errorLength,
                repeatlength = repeatLength,
                successuserlist = successUserList,
                errordatalist = errorDataList,
                repeatuserlist = repeatUserList,
                statusmsg = dataStatusMsg
            };
            return BizResult(data);
        }

        /// <summary>
        /// 获取人员节点信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public IHttpActionResult GetTaskUserNodeList([FromBody] TaskUserNodeFilter filter)
        {
            if (filter.TaskID == Guid.Empty)
            {
                throw new BizException("参数错误");
            }
            var dataCollectList = DataCollectUserOperator.Instance.GetListWithLastTask(new VCollectUserFilter()
            {
                TemplateConfigInstanceID = filter.TaskID.ToString()
            });
            var pendingCount = 0;
            var dataList = dataCollectList.SubCollection.FindAll(x =>
            {
                var statusExist = false;
                if (filter.Status == 0)
                {
                    statusExist = true;
                }
                else if (filter.Status == 1)
                {
                    statusExist = x.Status == 2;
                }
                else
                {
                    statusExist = x.Status != 2;
                }
                pendingCount += x.Status != 2 ? 1 : 0;
                return
                 //状态还没加上 等老吴签入修改了实体类以后进行
                 statusExist &&
                 (string.IsNullOrEmpty(filter.LoginOrName) || x.UserName.IndexOf(filter.LoginOrName) >= 0 || x.EmployeeName.IndexOf(filter.LoginOrName) >= 0);
            });
            return BizResult(
                  new
                  {
                      Nodes = dataList.Select(x =>
                      {
                          var userNode = new TaskUserNode();
                          userNode.ConvertEntity(x);
                          return userNode;
                      }).ToList(),
                      Total = dataCollectList.SubCollection.Count,
                      Pending = pendingCount
                  });
        }

        /// <summary>
        /// 追加人员节点
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userNames"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("append/{taskId}")]
        public IHttpActionResult AppendUser(string taskId, [FromBody] List<string> userNames)
        {
            var finder = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (finder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(finder.CreatorLoginName);
            }
            var taskUserNodes = new List<TaskUser>();
            string errorMessage = null;
            List<LoginUserInfo> userList = null;
            var isValidate = CommonValidation(taskId, userNames, out userList, out errorMessage);
            if (!isValidate)
            {
                throw new BizException(errorMessage);
            }
            var dcuList = DataCollectUserOperator.Instance.GetList(taskId).ToList();
            var existUserList = dcuList.Where(x => userList.Any(t => t.LoginName.Equals(x.UserName, StringComparison.CurrentCultureIgnoreCase))).ToList();
            if (existUserList.Count > 0)
            {
                throw new BizException(string.Format("用户：{0}，已属于该任务，无法重复添加", string.Join("、", existUserList.Select(t => t.UserName))));
            }
            taskUserNodes.AddRange(userList.Select(x =>
            {
                return new TaskUser()
                {
                    EmployeeCode = x.EmployeeCode,
                    OrgID = x.OrgID,
                    UnitID = x.UnitID,
                    UnitName = x.UnitName,
                    EmployeeName = x.CNName,
                    OrgName = x.OrgName,
                    UserName = x.LoginName
                };
            }));
            taskUserNodes.AddRange(dcuList.Select(x =>
            {
                return new TaskUser()
                {
                    EmployeeCode = x.EmployeeCode,
                    OrgID = x.OrgID,
                    UnitID = x.UnitID,
                    UnitName = x.UnitName,
                    EmployeeName = x.EmployeeName,
                    OrgName = x.OrgName,
                    UserName = x.UserName
                };
            }).ToList());
            using (TransactionScope scope = TransactionScopeFactory.Create())
            {
                var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
                var tuple = TemplateConfigInstanceOperator.Instance.UpdateTaskInfo(task, taskUserNodes, true);
                task.ProcessStatus = 1;
                TemplateConfigInstanceOperator.Instance.UpdateModel(task);
                scope.Complete();
            }
            return BizResult(true);
        }
        /// <summary>
        /// 删除人员节点
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userNames"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("remove/{taskId}")]
        public IHttpActionResult RemoveUser(string taskId, [FromBody] List<string> userNames)
        {
            var existFinder = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (existFinder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(existFinder.CreatorLoginName);
            }
            string errorMessage = null;
            List<LoginUserInfo> userList = null;
            var isValidate = CommonValidation(taskId, userNames, out userList, out errorMessage);
            if (!isValidate)
            {
                throw new BizException(errorMessage);
            }
            var dcuList = DataCollectUserOperator.Instance.GetList(taskId).ToList();
            //不在用户集合中的数据就是剩余的不删除的数据
            //如果该数据集合为0 则说明没有用户  不可以提交
            if (!dcuList.Any(x => !userList.Any(u => u.LoginName.Equals(x.UserName, StringComparison.CurrentCultureIgnoreCase))))
            {
                throw new BizException("不可以删除任务中的全部人员节点");
            }
            var notExistUserList = userNames.Where(x => !dcuList.Any(t => t.UserName.Equals(x, StringComparison.CurrentCultureIgnoreCase))).ToList();
            if (notExistUserList.Count > 0)
            {
                throw new BizException(string.Format("用户：{0}，不属于该任务，无法进行删除操作", string.Join("、", notExistUserList)));
            }
            using (TransactionScope scope = TransactionScopeFactory.Create())
            {
                var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
                userList.ForEach(x =>
                {
                    var finder = dcuList.FirstOrDefault(d => d.UserName.Equals(x.LoginName, StringComparison.CurrentCultureIgnoreCase));
                    if (finder != null)
                    {
                        var itemTask = TemplateTaskOperator.Instance.GetLastTask(finder.ID);
                        if (itemTask != null)
                        {
                            TemplateConfigInstanceOperator.Instance.CancelCollectTask(itemTask);
                        }
                        finder.ProcessStatus = 1;
                        DataCollectUserOperator.Instance.UpdateModel(finder);
                    }
                });
                task.ProcessStatus = 1;
                TemplateConfigInstanceOperator.Instance.UpdateModel(task);
                scope.Complete();
            }

            return BizResult(true);
        }

        /// <summary>
        /// 重发人员节点
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userNames"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("repeat/{taskId}")]
        public IHttpActionResult RepeatUser(string taskId, [FromBody] List<string> userNames)
        {
            var existFinder = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (existFinder != null)
            {
                Helper.Common.CommonValidation.ValidateRoleRight(existFinder.CreatorLoginName);
            }
            string errorMessage = null;
            List<LoginUserInfo> userList = null;
            var isValidate = CommonValidation(taskId, userNames, out userList, out errorMessage);
            if (!isValidate)
            {
                throw new BizException(errorMessage);
            }
            var dcuList = DataCollectUserOperator.Instance.GetList(taskId).ToList();
            var notExistUserList = userNames.Where(x => !dcuList.Any(t => t.UserName.Equals(x, StringComparison.CurrentCultureIgnoreCase))).ToList();
            if (notExistUserList.Count > 0)
            {
                throw new BizException(string.Format("用户：{0}，不属于该任务，无法进行重发操作", string.Join("、", notExistUserList)));
            }
            using (TransactionScope scope = TransactionScopeFactory.Create())
            {
                var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
                userList.ForEach(x =>
                {
                    var finder = dcuList.FirstOrDefault(d => d.UserName.Equals(x.LoginName, StringComparison.CurrentCultureIgnoreCase));
                    if (finder != null)
                    {
                        var itemTask = TemplateTaskOperator.Instance.GetLastTask(finder.ID);
                        if (itemTask != null)
                        {
                            TemplateConfigInstanceOperator.Instance.CancelCollectTask(itemTask);
                        }
                        TemplateConfigInstanceOperator.Instance.StartCollectTask(task.WorkflowID, task.TemplateConfigInstanceName, task, finder, new LoginUserInfo() { LoginName = finder.UserName });
                    }
                });
                task.ProcessStatus = 1;
                TemplateConfigInstanceOperator.Instance.UpdateModel(task);
                scope.Complete();
            }
            return BizResult(true);
        }

        /// <summary>
        /// 通用验证函数
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userNames"></param>
        /// <param name="result">数据库查询结果</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CommonValidation(string taskId, List<string> userNames, out List<LoginUserInfo> result, out string errorMessage)
        {
            errorMessage = null;
            result = new List<LoginUserInfo>();
            Guid ret = Guid.Empty;
            if (string.IsNullOrEmpty(taskId) || !Guid.TryParse(taskId, out ret))
            {
                errorMessage = "参数错误";
                return false;
            }
            if (userNames == null || userNames.Count == 0)
            {
                errorMessage = "提交参数为空，无法进行操作";
                return false;
            }
            //验证是否存在特殊字符
            var filters = userNames.Select(x =>
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
                return reg.Replace(x, "");
            }).ToList();
            if (filters.Any(x => x.Length > 0))
            {
                errorMessage = "提交参数存在特殊字符,请确认后重新填写";
                return false;
            }
            var task = TemplateConfigInstanceOperator.Instance.GetModel(taskId);
            if (task == null)
            {
                errorMessage = "任务ID无效";
                return false;
            }
            var userList = UserInfoOperator.Instance.GetWDUserInfoByLoginNameList(userNames);
            var errorUserNames = userNames.Where(x => !userList.Any(e => e.LoginName.Equals(x, StringComparison.CurrentCultureIgnoreCase))).ToList();
            if (errorUserNames.Count() > 0)
            {
                errorMessage = string.Format("账号：{0}无效，请检查", string.Join("、", errorUserNames));
                return false;
            }
            errorMessage = null;
            result = userList;
            return true;
        }
    }
}