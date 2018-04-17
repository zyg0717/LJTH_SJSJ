
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using Framework.Core;
using Lib.ViewModel.TemplateViewModel;
using Aspose.Cells;
using System.IO;
using Lib.ViewModel;
using Lib.Model.Filter;
using Lib.Common;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System.Text.RegularExpressions;

namespace Lib.BLL
{
    /// <summary>
    /// Template对象的业务逻辑操作
    /// </summary>
    public class TemplateOperator : BizOperatorBase<Template>
    {
        /// <summary>
        /// 读取上传的任务填报数据
        /// 返回值 1 上传文件来源不正确 2 上传文件数据填写不正确 3上传成功
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <param name="views"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int ReadTaskData(string businessId, string fileName, Stream stream, out TaskCollectionData views, out string errorMessage)
        {
            views = new TaskCollectionData();
            errorMessage = "";
            var itemTask = TemplateTaskOperator.Instance.GetModel(businessId);
            var task = TemplateConfigInstanceOperator.Instance.GetModel(itemTask.TemplateConfigInstanceID);
            ExcelEngine engine = new ExcelEngine();
            Workbook databook = new Workbook(stream);

            string templateID = engine.GetStringCustomProperty(databook.Worksheets[0], "TempleteID");
            if (!templateID.Equals(task.TemplateID, StringComparison.CurrentCultureIgnoreCase))
            {
                errorMessage = "请使用系统下载的文件上传";
                return 1;
            }
            var configs = TemplateConfigOperator.Instance.GetList(task.TemplateID, null).ToList();
            var sheets = TemplateSheetOperator.Instance.GetList(task.TemplateID).ToList();
            try
            {
                var data = TemplateTaskOperator.Instance.ReadTaskData(stream, configs, sheets);
                views = data;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return 2;
            }
            return 3;
        }


        /// <summary>
        /// 根据后缀名获取文件格式
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public SaveFormat GetFormatType(string ext)
        {
            SaveFormat format = SaveFormat.Unknown;
            if (".xls".Equals(ext, StringComparison.CurrentCultureIgnoreCase))
            {
                format = SaveFormat.Excel97To2003;
            }
            if (".xlsx".Equals(ext, StringComparison.CurrentCultureIgnoreCase))
            {
                format = SaveFormat.Xlsx;
            }
            return format;
        }
        /// <summary>
        /// 通过附件实体获取文件格式
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public SaveFormat GetFormatType(Attachment attachment)
        {
            if (attachment == null)
            {
                return SaveFormat.Xlsx;
            }
            var fileExt = attachment.FileExt;
            return GetFormatType(fileExt);
        }
        /// <summary>
        /// 通过文件流以及配置信息读取转换格式后的业务数据
        /// </summary>
        /// <param name="stream">excel文件流</param>
        /// <param name="formatType">文件格式</param>
        /// <param name="sheetDatas">sheet配置集合</param>
        /// <param name="configList">列配置集合</param>
        /// <param name="rangeList">列下拉列表配置集合</param>
        /// <returns></returns>
        public List<ViewSheet> ReadExcelData(Stream stream, SaveFormat formatType, List<TemplateSheet> sheetDatas, List<Model.TemplateConfig> configList, List<Model.TemplateConfigSelect> rangeList)
        {
            Workbook dodelDatabook = new Workbook(stream);
            var list = new List<ViewSheet>();

            int defaultFirstColumn = 1;
            int defaultFirstRow = 3;

            foreach (Worksheet item in dodelDatabook.Worksheets)
            {
                if (!item.IsVisible)
                    continue;

                var sheetData = sheetDatas.FirstOrDefault(x => x.TemplateSheetName == item.Name);
                if (sheetData == null)
                {
                    continue;
                }
                if (item.Cells.MaxDataRow >= 0 && item.Cells.MaxDataColumn >= 0)
                {
                    ViewSheet sheet = new ViewSheet();
                    sheet.changeflag = true;
                    sheet.edit = false;
                    sheet.firstcolumn = defaultFirstColumn;
                    sheet.firstrow = defaultFirstRow;

                    sheet.code = item.Name;
                    sheet.name = item.Name;
                    sheet.title = item.Cells[0, 0].StringValue;
                    sheet.remark = "";
                    sheet.columns = new List<ViewColumn>();
                    sheet.firstrow = sheetData.RowNum + 1;
                    sheet.firstcolumn = sheetData.ColumnNum;
                    sheet.remark = sheetData.TemplateSheetRemark;
                    sheet.title = sheetData.TemplateSheetTitle;
                    List<Model.TemplateConfig> configs = configList.FindAll(x => x.TemplateSheetID == sheetData.ID);
                    configs = configs.OrderBy(x => x.SortIndex).ToList();
                    for (int i = 0; i <= item.Cells.MaxDataColumn; i++)
                    {
                        if (i >= sheet.firstcolumn - 1)
                        {
                            ViewColumn column = new ViewColumn();
                            column.bgcolor = "255,255,255";
                            column.code = "";
                            column.digit = 0;
                            column.required = false;
                            column.sort = i;
                            column.type = "Text";
                            column.name = item.Cells[sheet.firstrow - 1 - 1, i].StringValue;


                            //获取单元格公式
                            column.isformula = item.Cells[sheet.firstrow - 1, i].IsFormula;
                            if (column.isformula)
                            {
                                column.tempformula = item.Cells[sheet.firstrow - 1, i].Formula;
                                column.cellformula = GetFormulaForTempFormula(column.tempformula);
                            }

                            var currentCell = item.Cells[sheet.firstrow - 1 - 1, i];
                            var cellValue = GetColumnSpanHeaderText(item, currentCell);

                            column.name = cellValue;


                            if (configs.Count > i + 1 - sheet.firstcolumn)
                            {
                                var config = configs[i + 1 - sheet.firstcolumn];
                                column.bgcolor = config.BGColor;
                                column.code = "";
                                column.digit = config.Digit;
                                column.required = config.IsRequired == 1;
                                column.sort = i;
                                column.type = config.FieldType;
                                column.name = config.FieldName;
                                var selectList = rangeList.FindAll(x => x.TemplateSheetID == sheetData.ID && x.TemplateConfigID == config.ID);
                                //TemplateConfigSelectOperator.Instance.GetList(templateID, sheetData.ID, config.ID);
                                if (selectList != null && selectList.Count > 0)
                                {
                                    column.range = string.Join(",", selectList.OrderBy(x => x.SortIndex).Select(x => x.SelectedValue).ToList());
                                }
                                //获取单元格公式
                                column.isformula = config.IsFormula == 1;
                                if (column.isformula)
                                {
                                    column.tempformula = config.TempFormula;
                                    column.cellformula = config.CellFormula;
                                }
                            }
                            sheet.columns.Add(column);
                            if (sheet.columns.Count > 0)
                            {
                                sheet.columns[0].select = true;
                            }
                        }
                    }
                    var rows = TemplateOperator.Instance.ReadDataRowFromWorksheet(item, sheet.columns, sheet.firstrow, sheet.firstcolumn, 3);
                    sheet.rows = rows;
                    list.Add(sheet);
                    if (list.Count > 0)
                    {
                        list[0].select = true;
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 读取任务下发后上传的填报数据
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="stream"></param>
        /// <param name="fullFileName"></param>
        /// <param name="validateTemplate"></param>
        /// <returns></returns>
        public object ReadUploadTaskData(string businessID, Stream stream, string fullFileName, bool validateTemplate = true)
        {
            object res = null;
            int status = 0;
            int dataCount = 0;
            var bytes = stream.ToBytes();
            LoginUserInfo userinfo = WebHelper.GetCurrentUser(); ;


            ExcelEngine engine = new ExcelEngine();
            Workbook databook = new Workbook(stream);

            //Lib.Log.LogHelper.Instance.Info("读取至aspose成功");
            string templeteID = engine.GetStringCustomProperty(databook.Worksheets[0], "TempleteID");

            Framework.Core.Log.LogHelper.Instance.Info(string.Format("正在检测excel中的自定义属性,TemplateID:{0}", templeteID));

            var task = TemplateTaskOperator.Instance.GetModel(businessID);

            //Lib.Log.LogHelper.Instance.Info("读取下发任务成功");
            var dcu = DataCollectUserOperator.Instance.GetModel(task.DataCollectUserID);
            var tci = TemplateConfigInstanceOperator.Instance.GetModel(dcu.TemplateConfigInstanceID);

            var configs = TemplateConfigOperator.Instance.GetList(tci.TemplateID, null).ToList();

            var sheetConfigs = TemplateSheetOperator.Instance.GetList(tci.TemplateID).ToList();


            Attachment attach = new Attachment();

            if (templeteID.ToLower() != dcu.TemplateID.ToLower() && validateTemplate)
            {
                Framework.Core.Log.LogHelper.Instance.Info(string.Format("当前获取到的任务TemplateID:{0}", dcu.TemplateID));

                status = -100;

                res = new { Attachment = attach, Status = status };
            }
            else
            {
                try
                {
                    attach.ID = Guid.NewGuid().ToString();
                    var pathIndex = fullFileName.LastIndexOf('\\') >= 0 ? fullFileName.LastIndexOf('\\') + 1 : 0;
                    var fileName = fullFileName.Substring(pathIndex);
                    stream.Seek(0, SeekOrigin.Begin);

                    //--这里保存的文件格式有问题
                    string fileCode = FileUploadHelper.UploadFileStream(stream, bytes.Length, fileName);

                    attach.Name = fileName;// fileName.Substring(0, fileName.LastIndexOf('.'));
                    attach.AttachmentPath = fileCode;// System.IO.Path.Combine(DateTime.Now.ToString("yyyyMMdd"), fileID + extName);
                    attach.BusinessID = businessID;
                    attach.BusinessType = "UploadTaskData";
                    attach.IsUseV1 = false;

                    attach.CreateDate = DateTime.Now;
                    attach.CreatorLoginName = userinfo.LoginName;
                    attach.CreatorName = userinfo.CNName;
                    attach.IsDeleted = false;
                    attach.ModifierLoginName = userinfo.LoginName;
                    attach.ModifierName = userinfo.CNName;
                    attach.ModifyTime = DateTime.Now;


                    attach.FileSize = FileUploadHelper.GetContentLength(bytes.Length);



                    status = 100;
                    try
                    {
                        var data = TemplateTaskOperator.Instance.ReadTaskData(attach, configs, sheetConfigs);
                        dataCount = data.Sheets.Sum(x => x.Rows.Count);
                        if (dataCount == 0)
                        {
                            res = new { Attachment = attach, Status = -102, DataCount = 0 };
                        }
                        else
                        {
                            AttachmentOperator.Instance.AddAndUpdateModel(attach);
                            res = new { Attachment = attach, Status = status, DataCount = data.Sheets.Select(x => new { SheetName = x.SheetName, DataCount = x.Rows.Count }) };
                        }
                    }
                    catch (Exception e)
                    {
                        Framework.Core.Log.LogHelper.Instance.Error(e);
                        res = new { Attachment = attach, Status = -101, ErrorMessage = e.Message };
                    }
                }
                catch (Exception e)
                {
                    status = 0;
                    Framework.Core.Log.LogHelper.Instance.Error(e);
                    res = new { Attachment = attach, Status = status };
                }
            }
            return res;
        }

        public bool CheckExist(string loginName, string templateName)
        {
            bool exist = _templateAdapter.CheckExist(loginName, templateName);
            return exist;
        }

        public Workbook WriteSampleData(Workbook wb, List<ViewSheet> views, int renderType)
        {
            if (renderType == 0 || renderType == 1)
            {
                views = views ?? new List<ViewSheet>();
                views.ForEach(x =>
            {
                var sheet = wb.Worksheets[x.name];
                if (sheet != null)
                {
                    int rowIndex = 0;
                    x.rows = x.rows ?? new List<ViewRow>();
                    x.rows.ForEach(row =>
                    {
                        int cellIndex = 0;
                        row.cells = row.cells ?? new List<ViewCell>();
                        row.cells.ForEach(cell =>
                        {
                            string cellValue = cell.content;
                            var column = x.columns[cellIndex];
                            if (string.IsNullOrEmpty(column.name) && (renderType == 0 || renderType == 1))
                            {
                                return;
                            }
                            sheet.Cells[rowIndex + x.firstrow - 1, cellIndex + x.firstcolumn - 1].PutValue(cellValue);
                            cellIndex++;
                        });
                        rowIndex++;
                    });
                }
            });
            }
            return wb;
        }

        /// <summary>
        /// 获取数据行数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columns"></param>
        /// <param name="firstRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="fixRowCount"></param>
        /// <returns></returns>
        public List<ViewRow> ReadDataRowFromWorksheet(Worksheet sheet, List<ViewColumn> columns, int firstRow, int firstColumn, int fixRowCount)
        {
            columns = columns ?? new List<ViewColumn>();
            var rows = new List<ViewRow>();
            for (int i = 0; i <= sheet.Cells.MaxDataRow; i++)
            {
                if (i >= firstRow - 1)
                {
                    ViewRow row = new ViewRow();
                    row.cells = new List<ViewCell>();
                    for (int j = 0; j <= sheet.Cells.MaxDataColumn; j++)
                    {
                        if (j >= firstColumn - 1)
                        {
                            var cel = sheet.Cells[i, j];
                            if (cel.IsMerged)
                            {
                                goto BREAKLOOP;
                            }
                            ViewCell cell = new ViewCell();
                            cell.content = cel.StringValue;
                            row.cells.Add(cell);
                        }
                    }
                    if (!row.cells.Any(x => !string.IsNullOrEmpty(x.content)))
                    {
                        goto BREAKLOOP;
                    }
                    rows.Add(row);
                }
                continue;
                BREAKLOOP:
                break;
            }
            if (rows.Count < fixRowCount)
            {
                var newRowCount = fixRowCount - rows.Count;
                for (int i = 0; i < newRowCount; i++)
                {
                    ViewRow row = new ViewRow();
                    row.cells = new List<ViewCell>();
                    for (int j = 0; j < columns.Count; j++)
                    {
                        var cell = new ViewCell();
                        cell.content = "";
                        row.cells.Add(cell);
                    }
                    rows.Add(row);
                }
            }
            return rows;
        }
        public Cell GetMergeCell(Cell cell)
        {
            var currentCell = cell;
            var currentRowIndex = currentCell.Row;
            var currentColumnIndex = currentCell.Column;
            var range = currentCell.GetMergedRange();
            if (currentCell.IsMerged && (currentCell.Row != range.FirstRow || currentCell.Column != range.FirstColumn))
            {
                //列合并
                if (range.ColumnCount > 1)
                {
                    currentColumnIndex = range.FirstColumn;
                }
                //行合并
                if (range.RowCount > 1)
                {
                    currentRowIndex = range.FirstRow;
                }
                currentCell = range.Worksheet.Cells[currentRowIndex, currentColumnIndex];
            }
            return currentCell;
        }
        public string GetColumnSpanHeaderText(Worksheet sheet, Cell currentCell)
        {
            var currentRowIndex = currentCell.Row;
            var currentColumnIndex = currentCell.Column;
            var range = currentCell.GetMergedRange();
            var cellValue = currentCell.StringValue;
            //如果当前列至少在第三行（第一行是标题 第二行是标题起始 如果是在第二行就没必要合并列文本）
            if (currentCell.Row > 1)
            {
                currentCell = sheet.Cells[currentCell.Row - 1, currentColumnIndex];
                while (currentCell.IsMerged && currentCell.Row > 0)
                {
                    var r = currentCell.GetMergedRange();
                    currentCell = sheet.Cells[r.FirstRow, r.FirstColumn];
                    cellValue = currentCell.StringValue + "-" + cellValue;
                    if (currentCell.Row == 0 || currentCell.Column == 0)
                    {
                        break;
                    }
                    currentCell = sheet.Cells[currentCell.Row - 1, currentCell.Column];
                }
            }
            return cellValue;
        }

        /// <summary>
        /// 根据文件流读取excel配置信息
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="tuple">配置信息集合tuple说明（item1：sheet名称 item2：首行 item3：首列）</param>
        /// <returns></returns>
        public Tuple<List<TemplateSheet>, List<Model.TemplateConfig>> ReadSheetDataFromStream(Stream stream, List<Tuple<string, int, int>> tuple)
        {
            List<TemplateSheet> sheets = new List<TemplateSheet>();
            List<Model.TemplateConfig> configs = new List<Model.TemplateConfig>();
            Workbook workbook = new Workbook(stream);
            var defaultColumn = 1;
            var defaultRow = 3;

            foreach (Worksheet wb in workbook.Worksheets)
            {
                if (!wb.IsVisible)
                    continue;
                TemplateSheet sheet = new TemplateSheet();
                var item = tuple.Find(x => x.Item1 == wb.Name);
                var column = defaultColumn;
                var row = defaultRow;
                if (item != null)
                {
                    //column = item.Item3;
                    row = item.Item2;
                }
                sheet.ColumnNum = column;
                sheet.ID = Guid.NewGuid().ToString();
                sheet.RowNum = row - 1;
                sheet.Status = 0;
                sheet.TemplateSheetName = wb.Name;
                sheet.TemplateSheetRemark = "";
                sheet.TemplateSheetTitle = wb.Cells[0, 0].StringValue;
                List<Model.TemplateConfig> items = new List<Model.TemplateConfig>();
                for (int i = 0; i <= wb.Cells.MaxDataColumn; i++)
                {
                    //if (i >= column - 1)
                    if (i >= 0)
                    {
                        var columnModel = new Model.TemplateConfig();
                        columnModel.BGColor = "255,255,255";
                        columnModel.Digit = 0;
                        columnModel.FieldType = "Text";
                        columnModel.FiledLength = 0;
                        columnModel.HasSelectedValue = 0;
                        columnModel.ID = Guid.NewGuid().ToString();
                        columnModel.IsRequired = 0;
                        columnModel.SortIndex = i;
                        columnModel.TemplateSheetID = sheet.ID;
                        columnModel.TemplateSheetName = sheet.TemplateSheetName;

                        var currentCell = wb.Cells[row - 1 - 1, i];
                        var cellValue = GetColumnSpanHeaderText(wb, currentCell);
                        columnModel.FieldName = cellValue;
                        configs.Add(columnModel);
                    }
                }
                sheets.Add(sheet);
            }
            return Tuple.Create(sheets, configs);
        }
        /// <summary>
        /// 根据提交的业务数据实体读取excel配置信息
        /// </summary>
        /// <param name="template">模板实体</param>
        /// <param name="renderType">渲染类型 1在线编辑 2克隆 3本地模板</param>
        /// <param name="views">业务数据实体</param>
        /// <returns></returns>
        public Tuple<List<TemplateSheet>, List<Model.TemplateConfig>, List<TemplateConfigSelect>> ReadSheetDataFromBizModel(Template template, int renderType, List<ViewSheet> views)
        {
            views = views ?? new List<ViewSheet>();
            var userInfo = WebHelper.GetCurrentUser(); ;
            List<TemplateSheet> sheets = new List<TemplateSheet>();
            List<Model.TemplateConfig> configs = new List<Model.TemplateConfig>();
            List<TemplateConfigSelect> selects = new List<TemplateConfigSelect>();
            foreach (var item in views)
            {
                TemplateSheet sheet = new TemplateSheet();
                sheet.ID = Guid.NewGuid().ToString();
                sheet.TemplateSheetName = item.name;
                sheet.TemplateSheetTitle = item.title;
                sheet.TemplateSheetRemark = item.remark;
                sheet.RowNum = item.firstrow - 1;
                sheet.ColumnNum = item.firstcolumn;
                sheet.TemplateID = template.ID;
                sheet.TemplateName = template.TemplateName;
                int index = 1;
                item.columns = item.columns ?? new List<ViewColumn>();
                foreach (var config in item.columns)
                {
                    //如果是在线编辑或克隆模板  遇到第一个空列则视为后面没有其他列  这里需要在客户端做下验证 不可以跳格填写内容
                    if (string.IsNullOrEmpty(config.name) && (renderType == 0 || renderType == 1))
                    {
                        continue;
                    }
                    Model.TemplateConfig tconfig = new Model.TemplateConfig();
                    tconfig.ID = Guid.NewGuid().ToString();
                    tconfig.FieldName = config.name;
                    tconfig.FieldType = config.type;
                    tconfig.FiledLength = 0;
                    tconfig.Digit = config.digit;
                    tconfig.BGColor = config.bgcolor;
                    tconfig.HasSelectedValue = !string.IsNullOrEmpty(config.range) && config.range.Length > 0 ? 1 : 0;
                    tconfig.IsDeleted = false;
                    tconfig.IsRequired = config.required ? 1 : 0;
                    tconfig.SortIndex = index;
                    tconfig.TemplateSheetID = sheet.ID;
                    tconfig.TemplateSheetName = sheet.TemplateSheetName;
                    tconfig.TemplateID = template.ID;
                    tconfig.TemplateName = template.TemplateName;

                    //设置公式
                    tconfig.IsFormula = config.isformula ? 1 : 0;
                    if (config.isformula)
                    {
                        tconfig.TempFormula = config.tempformula;
                        tconfig.CellFormula = config.cellformula;
                    }


                    if (!string.IsNullOrEmpty(config.range))
                    {
                        var rangeVal = config.range.Replace('，', ',').Split(',');

                        int rindex = 0;
                        foreach (var rVal in rangeVal)
                        {
                            TemplateConfigSelect configselect = new TemplateConfigSelect();
                            configselect.TemplateConfigID = tconfig.ID;
                            configselect.SelectedValue = rVal;
                            configselect.SortIndex = ++rindex;
                            configselect.TemplateSheetID = sheet.ID;
                            configselect.TemplateSheetName = sheet.TemplateSheetName;
                            configselect.TemplateID = template.ID;
                            configselect.TemplateName = template.TemplateName;
                            selects.Add(configselect);
                        }
                    }
                    configs.Add(tconfig);
                    index++;
                }
                sheets.Add(sheet);
            }
            return Tuple.Create(sheets, configs, selects);
        }
        /// <summary>
        /// 通过文件流获取业务数据实体
        /// **该方法已过时
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="templateID"></param>
        /// <param name="isPriview"></param>
        /// <returns></returns>
        [Obsolete("方法已过时，推荐使用ReadExcelData方法")]
        public List<ViewSheet> LoadTemplateDataByStream(Stream stream, string templateID, bool isPriview)
        {
            Workbook dodelDatabook = new Workbook(stream);
            var list = new List<ViewSheet>();

            int defaultFirstColumn = 1;
            int defaultFirstRow = 3;

            var sheetDatas = TemplateSheetOperator.Instance.GetList(templateID);
            foreach (Worksheet item in dodelDatabook.Worksheets)
            {
                if (!item.IsVisible)
                    continue;
                if (item.Cells.MaxDataRow >= 0 && item.Cells.MaxDataColumn >= 0)
                {
                    ViewSheet sheet = new ViewSheet();

                    sheet.firstcolumn = defaultFirstColumn;
                    sheet.firstrow = defaultFirstRow;

                    sheet.code = item.Name;
                    sheet.name = item.Name;
                    sheet.title = item.Cells[0, 0].StringValue;
                    sheet.remark = "";
                    sheet.columns = new List<ViewColumn>();
                    var sheetData = sheetDatas.FirstOrDefault(x => x.TemplateSheetName == item.Name);
                    if (sheetData != null)
                    {
                        sheet.firstrow = sheetData.RowNum + 1;
                        sheet.firstcolumn = sheetData.ColumnNum;
                        sheet.remark = sheetData.TemplateSheetRemark;
                        sheet.title = sheetData.TemplateSheetTitle;
                    }
                    if (sheetData == null && !isPriview)
                    {
                        continue;
                    }
                    List<Model.TemplateConfig> configs = new List<Model.TemplateConfig>();
                    if (sheetData != null)
                    {
                        configs = TemplateConfigOperator.Instance.GetList(templateID, sheetData.ID).ToList();
                    }
                    configs = configs.OrderBy(x => x.SortIndex).ToList();
                    for (int i = 0; i <= item.Cells.MaxDataColumn; i++)
                    {
                        if (i >= sheet.firstcolumn - 1)
                        {
                            ViewColumn column = new ViewColumn();
                            column.bgcolor = "255,255,255";
                            column.code = "";
                            column.digit = 0;
                            column.required = false;
                            column.sort = i;
                            column.type = "Text";
                            column.name = item.Cells[sheet.firstrow - 1 - 1, i].StringValue;
                            if (configs.Count >= i + 1)
                            {
                                var config = configs[i];
                                column.bgcolor = config.BGColor;
                                column.code = "";
                                column.digit = config.Digit;
                                column.required = config.IsRequired == 1;
                                column.sort = i;
                                column.type = config.FieldType;

                                var currentCell = item.Cells[sheet.firstrow - 1 - 1, i];
                                var currentRowIndex = currentCell.Row;
                                var currentColumnIndex = currentCell.Column;
                                var range = currentCell.GetMergedRange();
                                if (currentCell.IsMerged && (currentCell.Row != range.FirstRow || currentCell.Column != range.FirstColumn))
                                {
                                    //列合并
                                    if (range.ColumnCount > 1)
                                    {
                                        currentColumnIndex = range.FirstColumn;
                                    }
                                    //行合并
                                    if (range.RowCount > 1)
                                    {
                                        currentRowIndex = range.FirstRow;
                                    }
                                    currentCell = item.Cells[currentRowIndex, currentColumnIndex];
                                }
                                var cellValue = currentCell.StringValue;
                                //如果当前列至少在第三行（第一行是标题 第二行是标题起始 如果是在第二行就没必要合并列文本）
                                if (currentCell.Row > 1)
                                {
                                    currentCell = item.Cells[currentCell.Row - 1, currentColumnIndex];
                                    while (currentCell.IsMerged && currentCell.Row > 0)
                                    {
                                        var r = currentCell.GetMergedRange();
                                        currentCell = item.Cells[r.FirstRow, r.FirstColumn];
                                        cellValue = currentCell.StringValue + "-" + cellValue;
                                        currentCell = item.Cells[currentCell.Row - 1, currentCell.Column];
                                    }
                                }
                                column.name = cellValue;

                                var selectList = TemplateConfigSelectOperator.Instance.GetList(templateID, sheetData.ID, config.ID);
                                if (selectList != null && selectList.Count > 0)
                                {
                                    column.range = string.Join(",", selectList.OrderBy(x => x.SortIndex).Select(x => x.SelectedValue).ToList());
                                }
                            }
                            sheet.columns.Add(column);
                        }
                    }
                    var rows = TemplateOperator.Instance.ReadDataRowFromWorksheet(item, sheet.columns, sheet.firstrow, sheet.firstcolumn, 3);
                    sheet.rows = rows;
                    list.Add(sheet);
                }
            }
            return list;
        }

        #region Generate Code

        public static readonly TemplateOperator Instance = PolicyInjection.Create<TemplateOperator>();

        private static TemplateAdapter _templateAdapter = AdapterFactory.GetAdapter<TemplateAdapter>();
        private static VTemplateAdapter _vTemplateAdapter = AdapterFactory.GetAdapter<VTemplateAdapter>();

        public PartlyCollection<VTemplate> GetViewList(VTemplateFilter filter)
        {
            return _vTemplateAdapter.GetList(filter);
        }

        protected override BaseAdapterT<Template> GetAdapter()
        {
            return _templateAdapter;
        }

        public IList<Template> GetList()
        {
            IList<Template> result = _templateAdapter.GetList();
            return result;
        }
        public List<Template> GetMyModelList()
        {
            return _templateAdapter.GetMyModelList();
        }
        public IList<Template> GetListByLoginName(string loginName)
        {
            IList<Template> result = _templateAdapter.GetListByLoginName(loginName);
            return result;
        }



        public int UpdateRalationByID(string templateID)
        {
            return _templateAdapter.UpdateRalationByID(templateID);
        }

        public string AddModel(Template data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public Template GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(Template data)
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

        public List<Template> SearchTemplate(string name, string loginName, bool isWithIsImport = true)
        {
            return _templateAdapter.SearchTemplate(name, loginName, isWithIsImport);
        }
        public List<Template> SearchTemplateSelf(string name, string loginName)
        {
            return _templateAdapter.SearchTemplateSelf(name, loginName);
        }

        #endregion

        #region 私有方法
        private string GetFormulaForTempFormula(string tempFormula)
        {
            var regex = @"([A-Z])(\d+)";
            return Regex.Replace(tempFormula, regex, @"$1{R}");
        }
        #endregion


        #region 私有方法

        /// <summary>
        /// 更新二维表时下载模板，删除非自己的数据(因为Workbook 是引用类型，所以类型用了void)
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="templateSheets"></param>
        /// <param name="updateArea"></param>
        /// <param name="areaValue"></param>
        public void GetTemplateDateForUpdate(Workbook wb, IList<Lib.Model.TemplateSheet> templateSheets, string updateArea, string areaValue)
        {
            foreach (var templateSheet in templateSheets)
            {
                var workSheet = wb.Worksheets[templateSheet.TemplateSheetName];

                for (int i = workSheet.Cells.MaxDataRow + 1; i > templateSheet.RowNum; i--)
                {
                    if (workSheet.Cells[updateArea + i.ToString()].StringValue != areaValue)
                        workSheet.Cells.DeleteRow(i - 1);
                }
            }
        }
        #endregion
    }
}

