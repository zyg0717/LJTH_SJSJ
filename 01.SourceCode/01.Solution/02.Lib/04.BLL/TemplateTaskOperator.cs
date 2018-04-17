
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using Framework.Core;
using System.Web;
using Framework.Web.Security.Authentication;
using Lib.Common;
using Aspose.Cells;
using Framework.Web;
using Lib.ViewModel;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Web.Utility;
using Plugin.Workflow;
using Lib.BLL;
using System.Transactions;
using Framework.Data;

namespace Lib.BLL
{
    /// <summary>
    /// TemplateTask对象的业务逻辑操作
    /// </summary>
    public class TemplateTaskOperator : BizOperatorBase<TemplateTask>
    {
        private string GetCellValue(Worksheet item, int row, int column, Model.TemplateConfig cConfig)
        {
            if (string.IsNullOrEmpty(item.Cells[row, column].StringValue))
            {
                return "";
            }
            string res = "";
            try
            {
                var cell = item.Cells[row, column];
                if (cell.IsMerged)
                {
                    var range = cell.GetMergedRange();
                    cell = item.Cells[range.FirstRow, range.FirstColumn];
                }
                switch (cConfig.FieldType)
                {
                    case "Text":
                        res = cell.StringValue;
                        break;
                    case "Number":
                        int dot = cConfig.Digit;
                        //string format = string.Format("{{0:F{0}}}", dot);
                        res = cell.DoubleValue.ToString("F" + dot);
                        break;
                    case "DateTiem":
                        res = cell.DateTimeValue.ToString("yyyy-MM-dd");
                        break;
                    default:
                        res = cell.StringValue;
                        break;
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return res;
        }

        public TaskCollectionData ReadTaskData(Stream stream, List<Model.TemplateConfig> configs, List<TemplateSheet> sheetConfigs)
        {
            LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;

            Workbook dataBook = new Workbook(stream);
            TaskCollectionData tcd = new TaskCollectionData();
            tcd.Sheets = new List<DataSheet>();
            StringBuilder sb = new StringBuilder();
            sheetConfigs.ForEach(sheetconfig =>
            {
                DataSheet sheet = new DataSheet();
                sheet.SheetName = sheetconfig.TemplateSheetName;
                sheet.Rows = new List<DataRows>();
                var firstRow = sheetconfig.RowNum;
                var firstColumn = sheetconfig.ColumnNum;
                var currentSheet = dataBook.Worksheets[sheet.SheetName];
                if (currentSheet != null)
                {
                    var currentConfigs = configs.FindAll(x => x.TemplateSheetID == sheetconfig.ID).OrderBy(x => x.SortIndex).ToList();
                    for (int i = 0; i <= currentSheet.Cells.MaxDataRow; i++)
                    {
                        if (i >= firstRow)
                        {
                            DataRows dr = new DataRows();
                            dr.Cells = new List<RowCells>();
                            //获取数据
                            for (int j = 0; j <= currentSheet.Cells.MaxDataColumn; j++)
                            {
                                if (j >= firstColumn - 1)
                                {
                                    int cellIndex = j - (firstColumn - 1);
                                    RowCells cell = new RowCells();
                                    cell.Index = cellIndex;
                                    if (currentConfigs.Count >= cellIndex + 1)
                                    {
                                        var config = currentConfigs[cellIndex];
                                        var cellValue = GetCellValue(currentSheet, i, j, config);
                                        cell.Type = config.FieldType;
                                        cell.Formula = config.CellFormula;
                                        cell.IsFormula = currentSheet.Cells[i, j].IsFormula;
                                        cell.Value = cellValue;
                                        dr.Cells.Add(cell);
                                    }
                                }
                            }
                            if (dr.Cells.Count == 0 || !dr.Cells.Any(x => !string.IsNullOrEmpty(x.Value)))
                            {
                                goto BreakLoop;
                            }
                            var hasError = dr.Cells.FindAll(cell =>
                            {
                                var config = currentConfigs[cell.Index];
                                return config != null && config.IsRequired == 1 && string.IsNullOrEmpty(cell.Value);
                            })
                             .Select(cell =>
                             {
                                 var config = currentConfigs[cell.Index];
                                 return config;
                             })
                             .ToList();
                            if (hasError.Count > 0)
                            {
                                sb.AppendLine(string.Format("【{1}】数据列：【{0}】为必填列", string.Join(",", hasError.GroupBy(c => c.FieldName).Select(c => c.Key)), currentSheet.Name));
                            }
                            sheet.Rows.Add(dr);
                        }
                        continue;
                        BreakLoop:
                        {
                            break;
                        }
                    }
                    tcd.Sheets.Add(sheet);
                }
            });
            if (sb.Length > 0)
            {
                sb.AppendLine("请仔细核对");
                throw new Exception(sb.ToString());
            }
            var noDataSheets = tcd.Sheets.Where(t => !t.Rows.Any()).ToList();
            if (noDataSheets.Any())
            {
                throw new Exception(string.Format("【{0}】未检测到有效数据行", string.Join("、", noDataSheets.Select(s => s.SheetName))));
            }
            return tcd;
        }

        public TaskCollectionData ReadTaskData(Attachment attac, List<Model.TemplateConfig> configs, List<TemplateSheet> sheetConfigs)
        {
            LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;
            var bytes = FileUploadHelper.DownLoadFileStream(attac.AttachmentPath, attac.IsUseV1);
            Stream stream = bytes.ToStream();
            stream.Seek(0, SeekOrigin.Begin);
            return ReadTaskData(stream, configs, sheetConfigs);
        }

        #region Generate Code

        public static readonly TemplateTaskOperator Instance = PolicyInjection.Create<TemplateTaskOperator>();

        private static TemplateTaskAdapter _templatetaskAdapter = AdapterFactory.GetAdapter<TemplateTaskAdapter>();

        protected override BaseAdapterT<TemplateTask> GetAdapter()
        {
            return _templatetaskAdapter;
        }



        public IList<TemplateTask> GetList()
        {
            IList<TemplateTask> result = _templatetaskAdapter.GetList();
            return result;
        }

        public string AddModel(TemplateTask data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateTask GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public TemplateTask GetTaskModel(string configInstanceID, string DataCollectUserID)
        {
            return _templatetaskAdapter.GetTaskModel(configInstanceID, DataCollectUserID);
        }
        public TemplateTask TryGetTaskModel(string configInstanceID, string DataCollectUserID)
        {
            return _templatetaskAdapter.TryGetTaskModel(configInstanceID, DataCollectUserID);
        }


        public string UpdateModel(TemplateTask data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public bool Reject(string businessID, LoginUserInfo currentUser)
        {
            var tTask = TemplateTaskOperator.Instance.GetModel(businessID);
            if (currentUser == null)
            {
                currentUser = WebHelper.GetCurrentUser(); ;
            }
            tTask.AuthTime = DateTime.Now;
            tTask.Status = (int)Common.ProcessStatus.Reject;
            tTask.ModifierLoginName = currentUser.LoginName;
            tTask.ModifierName = currentUser.CNName;
            tTask.ModifyTime = DateTime.Now;
            TemplateTaskOperator.Instance.UpdateModel(tTask);
            return true;
        }
        public bool Approve(string businessId, string TaskReportRemark, LoginUserInfo currentUser)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                var tTask = TemplateTaskOperator.Instance.GetModel(businessId);
                if (currentUser == null)
                {
                    currentUser = WebHelper.GetCurrentUser(); ;
                }
                tTask.AuthTime = DateTime.Now;
                tTask.Status = (int)Common.ProcessStatus.Approved;
                tTask.ModifierLoginName = currentUser.LoginName;
                tTask.ModifierName = currentUser.CNName;
                tTask.ModifyTime = DateTime.Now;
                if (!string.IsNullOrEmpty(TaskReportRemark))
                {
                    tTask.Remark = TaskReportRemark;
                }
                TemplateTaskOperator.Instance.UpdateModel(tTask);
                //var instance = TemplateConfigInstanceOperator.Instance.GetModel(tTask.TemplateConfigInstanceID);
                //WorkflowBuilder.ApproveWorkflowProcess(tTask.ID, instance.WorkflowID, instance.TemplateConfigInstanceName, new Employee() { LoginName = tTask.EmployeeLoginName }, "");
                scope.Complete();
            }
            return true;
        }

        public string RemoveModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            string result = base.RemoveObject(id);
            return result;
        }

        public List<TemplateTask> SearchUserTask(string taskID, string unitName, string employeeName, int feedBack)
        {
            return _templatetaskAdapter.SearchUserTask(taskID, unitName, employeeName, feedBack);
        }

        public TemplateTask GetLastTask(string collectDataUserID)
        {
            return _templatetaskAdapter.GetLastTask(collectDataUserID);
        }

        public TemplateTask GetModel(string userName, string templateConfigInstanceID)
        {
            return _templatetaskAdapter.GetModel(userName, templateConfigInstanceID);
        }

        public List<TemplateTask> GetList(string configInstanceID)
        {
            return _templatetaskAdapter.GetModel(configInstanceID);
        }
        public List<TemplateTask> GetDataUserIDByID(string TempConfigInstanceID)
        {
            return _templatetaskAdapter.GetDataUserIDByID(TempConfigInstanceID);
        }
        public TemplateTask GetByConfigInstaceID(string configInstanceID)
        {
            return _templatetaskAdapter.GetByConfigInstaceID(configInstanceID);
        }

        public void RemoveByDataCollectUserID(string dataCollectUserID)
        {
            _templatetaskAdapter.RemoveByDataCollectUserID(dataCollectUserID);
        }


        #endregion

        #region 用于更新二维表

        public TaskCollectionData ReadTaskDataForUpdateTask(Workbook attachmentWoorBook, Attachment attac, List<Model.TemplateConfig> configs, List<TemplateSheet> sheetConfigs)
        {
            LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;
            var bytes = FileUploadHelper.DownLoadFileStream(attac.AttachmentPath, attac.IsUseV1);
            Stream stream = bytes.ToStream();
            stream.Seek(0, SeekOrigin.Begin);
            return ReadTaskDataForUpdateTask(attachmentWoorBook, stream, configs, sheetConfigs);
        }

        public TaskCollectionData ReadTaskDataForUpdateTask(Workbook attachmentWoorBook, Stream stream, List<Model.TemplateConfig> configs, List<TemplateSheet> sheetConfigs)
        {
            LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;

            Workbook dataBook = new Workbook(stream);
            TaskCollectionData tcd = new TaskCollectionData();
            tcd.Sheets = new List<DataSheet>();
            StringBuilder sb = new StringBuilder();
            sheetConfigs.ForEach(sheetconfig =>
            {
                DataSheet sheet = new DataSheet();
                sheet.SheetName = sheetconfig.TemplateSheetName;
                sheet.Rows = new List<DataRows>();
                var firstRow = sheetconfig.RowNum;
                var firstColumn = sheetconfig.ColumnNum;
                var currentSheet = dataBook.Worksheets[sheet.SheetName];
                var templateSheet = attachmentWoorBook.Worksheets[sheet.SheetName];
                if (currentSheet != null)
                {
                    var currentConfigs = configs.FindAll(x => x.TemplateSheetID == sheetconfig.ID).OrderBy(x => x.SortIndex).ToList();
                    for (int i = 0; i <= currentSheet.Cells.MaxDataRow; i++)
                    {
                        if (i >= firstRow)
                        {
                            DataRows dr = new DataRows();
                            dr.Cells = new List<RowCells>();
                            //获取数据
                            for (int j = 0; j <= currentSheet.Cells.MaxDataColumn; j++)
                            {
                                if (j >= firstColumn - 1)
                                {
                                    int cellIndex = j - (firstColumn - 1);
                                    RowCells cell = new RowCells();
                                    cell.Index = cellIndex;
                                    if (currentConfigs.Count >= cellIndex + 1)
                                    {
                                        var config = currentConfigs[cellIndex];
                                        var cellValue = GetCellValue(currentSheet, i, j, config);
                                        var templateCellValue = GetCellValue(templateSheet, i, j, config);
                                        cell.Type = config.FieldType;
                                        cell.Formula = config.CellFormula;
                                        cell.IsFormula = currentSheet.Cells[i, j].IsFormula;
                                        cell.Value = cellValue;
                                        //用于更新二维表的值不等于模板值时，高亮色显示
                                        cell.IsShowColorForUpdateData = cellValue != templateCellValue;

                                        dr.Cells.Add(cell);
                                    }
                                }
                            }
                            if (dr.Cells.Count == 0 || !dr.Cells.Any(x => !string.IsNullOrEmpty(x.Value)))
                            {
                                goto BreakLoop;
                            }
                            var hasError = dr.Cells.FindAll(cell =>
                            {
                                var config = currentConfigs[cell.Index];
                                return config != null && config.IsRequired == 1 && string.IsNullOrEmpty(cell.Value);
                            })
                             .Select(cell =>
                             {
                                 var config = currentConfigs[cell.Index];
                                 return config;
                             })
                             .ToList();
                            if (hasError.Count > 0)
                            {
                                sb.AppendLine(string.Format("【{1}】数据列：【{0}】为必填列", string.Join(",", hasError.GroupBy(c => c.FieldName).Select(c => c.Key)), currentSheet.Name));
                            }
                            sheet.Rows.Add(dr);
                        }
                        continue;
                        BreakLoop:
                        {
                            break;
                        }
                    }
                    tcd.Sheets.Add(sheet);
                }
            });
            if (sb.Length > 0)
            {
                sb.AppendLine("请仔细核对");
                throw new Exception(sb.ToString());
            }
            var noDataSheets = tcd.Sheets.Where(t => !t.Rows.Any()).ToList();
            if (noDataSheets.Any())
            {
                throw new Exception(string.Format("【{0}】未检测到有效数据行", string.Join("、", noDataSheets.Select(s => s.SheetName))));
            }
            return tcd;
        }
        #endregion

    }
}

