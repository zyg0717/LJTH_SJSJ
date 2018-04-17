using Aspose.Cells;
using Framework.Web.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Common;
using Lib.ViewModel;
using System.Drawing;

namespace Lib.BLL.Builder
{
    public class TaskCollectionDataBuilder
    {
        private string _templateConfigInstanceID;
        private string _collectUserID;
        public TaskCollectionDataBuilder(string templateConfigInstanceID, string collectUserID)
        {
            this._templateConfigInstanceID = templateConfigInstanceID;
            this._collectUserID = collectUserID;
        }

        public Workbook BuildData(bool c1, bool c2, bool c3, bool c4, out string fileExt)
        {

            ExcelEngine engine = new ExcelEngine();
            var templateConfigInstance = TemplateConfigInstanceOperator.Instance.GetModel(this._templateConfigInstanceID);

            //string filePath = Path.Combine(ConstSet.TemplateBasePath, templateConfigInstance.TemplatePath);
            var attachment = AttachmentOperator.Instance.GetModelByCode(templateConfigInstance.TemplatePathFileCode);
            Workbook book = new Workbook(FileUploadHelper.DownLoadFileStream(templateConfigInstance.TemplatePathFileCode, attachment.IsUseV1).ToStream());

            var ext = templateConfigInstance.TemplatePathFileExt;
            if (ext == ".xlsx")
            {
                book.FileFormat = FileFormatType.Xlsx;
            }
            else
            {
                book.FileFormat = FileFormatType.Excel97To2003;
            }
            fileExt = ext;
            book.FileName = templateConfigInstance.TemplateConfigInstanceName;


            var tasks = TemplateTaskOperator.Instance.GetList(this._templateConfigInstanceID);


            if (!string.IsNullOrEmpty(this._collectUserID))
            {
                tasks = tasks.FindAll(p => p.DataCollectUserID.ToLower() == this._collectUserID.ToLower());
            }
            tasks = tasks.FindAll(x => x.Status == ProcessStatus.Approved.GetHashCode());
            //tasks = tasks.FindAll(p => p.Status == (int)ProcessStatus.Approved);


            List<TaskCollectionData> datas = new List<TaskCollectionData>();

            foreach (var item in tasks)
            {
                //TaskCollectionData data = JsonHelper.Deserialize<TaskCollectionData>(item.Content);
                //if (!string.IsNullOrEmpty(item.Content) && !string.IsNullOrEmpty(item.AuthTimeString))
                if (item.Status == Common.ProcessStatus.Approved.GetHashCode())
                {
                    var json = JsonHelper.Deserialize<TaskCollectionData>(item.Content);
                    json.Sheets.ForEach(x =>
                    {
                        x.Rows.ForEach(r =>
                         {
                             r.LoginName = item.EmployeeLoginName;
                             r.OrgName = item.OrgName;
                             r.UserName = item.EmployeeName;
                         });
                    });
                    datas.Add(json);
                }
            }

            TaskCollectionData data = CombineDatas(datas);

            var sheetConfigs = TemplateSheetOperator.Instance.GetList(templateConfigInstance.TemplateID).ToList();
            var configs = TemplateConfigOperator.Instance.GetList(templateConfigInstance.TemplateID, null).ToList();

            var insertColumnsCount = (c1 ? 1 : 0) + (c2 ? 1 : 0) + (c3 ? 1 : 0) + (c4 ? 1 : 0);

            #region 删除非汇总sheet
            //var sheetNameList = new List<string>();
            //for (int i = book.Worksheets.Count - 1; i >= 0; i--)
            //{
            //    if (sheetConfigs.Where(x => x.TemplateSheetName == book.Worksheets[i].Name).Count() == 0)
            //    {
            //        sheetNameList.Add(book.Worksheets[i].Name);
            //    }
            //}
            //foreach (var item in sheetNameList)
            //{
            //    book.Worksheets.RemoveAt(item);
            //}
            #endregion

            #region      清空模板原有数据  可能会引起填报数据的颜色跟表头一致
            //foreach (var sheet in data.Sheets)
            //{
            //    var sheetConfig = sheetConfigs.Find(x => x.TemplateSheetName == sheet.SheetName);
            //    if (sheetConfig != null)
            //    {
            //        var firstRow = sheetConfig.RowNum;
            //        var workSheet = book.Worksheets[sheet.SheetName];
            //        for (int i = workSheet.Cells.MaxRow; i > firstRow; i--)
            //        {
            //            workSheet.Cells.DeleteRow(i);
            //        }
            //    }
            //}
            #endregion

            //表示是否是空列
            var emptycolumn = new List<int>();

            foreach (var sheet in data.Sheets)
            {
                var sheetConfig = sheetConfigs.Find(x => x.TemplateSheetName == sheet.SheetName);
                if (sheetConfig != null)
                {
                    var firstColumn = sheetConfig.ColumnNum;
                    var firstRow = sheetConfig.RowNum;
                    var workSheet = book.Worksheets[sheet.SheetName];
                    var firstColumnStyle = workSheet.Cells[firstRow - 1, firstColumn - 1].GetStyle();
                    //var firstColumnStyle = workSheet.Cells[firstRow - 1, GetExcelFistrColumn(workSheet,firstRow) - 1].GetStyle();
                    workSheet.Cells.InsertColumns(firstColumn - 1, insertColumnsCount, true);
                    book.CalculateFormula();
                    var startIdx = firstColumn - 1;
                    if (c1)
                    {
                        workSheet.Cells[firstRow - 1, startIdx].PutValue("公司");
                        workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                        startIdx++;
                    }
                    if (c2)
                    {
                        workSheet.Cells[firstRow - 1, startIdx].PutValue("组织架构");
                        workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                        startIdx++;
                    }
                    if (c3)
                    {
                        workSheet.Cells[firstRow - 1, startIdx].PutValue("姓名");
                        workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                        startIdx++;
                    }
                    if (c4)
                    {
                        workSheet.Cells[firstRow - 1, startIdx].PutValue("账号");
                        workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                        startIdx++;
                    }

                    var style = workSheet.Cells[sheetConfig.RowNum, firstColumn - 1 + insertColumnsCount].GetStyle();

                    var currentConfigs = configs.FindAll(x => x.TemplateSheetID == sheetConfig.ID);

                    //获取没有列标题的列
                    for (int i = 0; i <= workSheet.Cells.MaxDataColumn; i++)
                    {
                        var flag = true;
                        for (int j = 1; j <= firstRow; j++)
                        {
                            if (workSheet.Cells[firstRow - j, i].StringValue.Trim() != "")
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                            emptycolumn.Add(i);
                    }


                    //没有示例的内容更汇总颜色
                    var styleDic = new Dictionary<int, Style>();
                    for (int i = 0; i <= workSheet.Cells.MaxColumn; i++)
                    {
                        var columnStyle = workSheet.Cells[firstRow, i].GetStyle();
                        if (workSheet.Cells[firstRow, i].StringValue == "")
                        {
                            columnStyle.ForegroundColor = Color.White;
                            columnStyle.Pattern = BackgroundType.Solid;
                        }
                        styleDic.Add(i, columnStyle);
                    }

                    int rowIndex = firstRow;
                    sheet.Rows.ForEach(row =>
                    {
                        var current = rowIndex;
                        workSheet.Cells.InsertRows(current, 1);
                        workSheet.Cells.CopyRow(workSheet.Cells, current + 1, current);
                        if (currentConfigs.Count > 0 && insertColumnsCount > 0)
                        {
                            Range r = workSheet.Cells.CreateRange(current, firstColumn - 1, 1, insertColumnsCount);
                            var styleFlag = new StyleFlag();
                            styleFlag.All = true;
                            r.ApplyStyle(style, styleFlag);
                        }

                        startIdx = firstColumn - 1;
                        if (c1)
                        {
                            workSheet.Cells[current, startIdx].PutValue("");
                            startIdx++;
                        }
                        if (c2)
                        {
                            workSheet.Cells[current, startIdx].PutValue(row.OrgName);
                            startIdx++;
                        }
                        if (c3)
                        {
                            workSheet.Cells[current, startIdx].PutValue(row.UserName);
                            startIdx++;
                        }
                        if (c4)
                        {
                            workSheet.Cells[current, startIdx].PutValue(row.LoginName);
                            startIdx++;
                        }

                        int cellIndex = 0;
                        currentConfigs.ForEach(x =>
                        {
                            if (cellIndex <= row.Cells.Count - 1)
                            {
                                var cell = row.Cells[cellIndex];
                                var excelCell =
                                    workSheet.Cells[current, cellIndex + (firstColumn - 1) + insertColumnsCount];
                                try
                                {

                                    switch (cell.Type)
                                    {
                                        case "Text":
                                            excelCell.PutValue(cell.Value);
                                            break;
                                        case "Number":
                                            excelCell.PutValue(Double.Parse(cell.Value));
                                            break;
                                        default:
                                            excelCell.PutValue(cell.Value);
                                            break;
                                    }

                                    if (cellIndex >= insertColumnsCount)
                                    {
                                        //将没有表头的列置空（排除第一列）
                                        if (emptycolumn.Contains(cellIndex) && rowIndex > firstRow)
                                        {
                                            workSheet.Cells[rowIndex, cellIndex].PutValue("");
                                            var s = workSheet.Cells[rowIndex, cellIndex].GetStyle();
                                            s.ForegroundColor = Color.White;
                                            s.Pattern = BackgroundType.Solid;
                                            s.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                            s.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                            s.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                            s.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                            workSheet.Cells[rowIndex, cellIndex].SetStyle(s);
                                        }
                                        //设置内容颜色
                                        else
                                        {
                                            styleDic[cellIndex].ForegroundColor = Color.White;
                                            styleDic[cellIndex].Pattern = BackgroundType.Solid;
                                            styleDic[cellIndex].Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                            styleDic[cellIndex].Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                            styleDic[cellIndex].Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                            styleDic[cellIndex].Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                            workSheet.Cells[rowIndex, cellIndex].SetStyle(styleDic[cellIndex]);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                //}
                            }
                            cellIndex++;
                        });
                        rowIndex++;
                    });
                    var currentRow = workSheet.Cells[rowIndex];
                    var start = rowIndex;
                    while (currentRow != null)
                    {
                        var list = new List<Cell>();
                        for (int j = 0; j <= workSheet.Cells.MaxDataColumn; j++)
                        {
                            list.Add(workSheet.Cells[rowIndex, j]);
                        }
                        if (
                            //list.Any(x => !string.IsNullOrEmpty(x.StringValue))
                            //&&
                            list.Any(x => x.IsMerged)
                            ||
                            list.All(x => string.IsNullOrEmpty(x.StringValue))
                        )
                        {
                            break;
                        }
                        rowIndex++;
                        currentRow = workSheet.Cells[rowIndex];
                    }
                    workSheet.Cells.DeleteRows(start, rowIndex - start);

                }
                emptycolumn = new List<int>();
            }

            //book.CalculateFormula();
            return book;
        }
        /// <summary>
        /// 用于公式、二维表的需求
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <param name="c4"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public Workbook BuildDataFormula(bool c1, bool c2, bool c3, bool c4, out string fileExt)
        {

            ExcelEngine engine = new ExcelEngine();
            var templateConfigInstance = TemplateConfigInstanceOperator.Instance.GetModel(this._templateConfigInstanceID);

            //string filePath = Path.Combine(ConstSet.TemplateBasePath, templateConfigInstance.TemplatePath);
            var attachment = AttachmentOperator.Instance.GetModelByCode(templateConfigInstance.TemplatePathFileCode);
            Workbook book = new Workbook(FileUploadHelper.DownLoadFileStream(templateConfigInstance.TemplatePathFileCode, attachment.IsUseV1).ToStream());

            var ext = templateConfigInstance.TemplatePathFileExt;
            if (ext == ".xlsx")
            {
                book.FileFormat = FileFormatType.Xlsx;
            }
            else
            {
                book.FileFormat = FileFormatType.Excel97To2003;
            }
            fileExt = ext;
            book.FileName = templateConfigInstance.TemplateConfigInstanceName;


            var tasks = TemplateTaskOperator.Instance.GetList(this._templateConfigInstanceID);


            if (!string.IsNullOrEmpty(this._collectUserID))
            {
                tasks = tasks.FindAll(p => p.DataCollectUserID.ToLower() == this._collectUserID.ToLower());
            }
            tasks = tasks.FindAll(x => x.Status == ProcessStatus.Approved.GetHashCode());
            //tasks = tasks.FindAll(p => p.Status == (int)ProcessStatus.Approved);


            List<TaskCollectionData> datas = new List<TaskCollectionData>();

            foreach (var item in tasks)
            {
                //TaskCollectionData data = JsonHelper.Deserialize<TaskCollectionData>(item.Content);
                //if (!string.IsNullOrEmpty(item.Content) && !string.IsNullOrEmpty(item.AuthTimeString))
                if (item.Status == Common.ProcessStatus.Approved.GetHashCode())
                {
                    var json = JsonHelper.Deserialize<TaskCollectionData>(item.Content);
                    json.Sheets.ForEach(x =>
                    {
                        x.Rows.ForEach(r =>
                        {
                            r.LoginName = item.EmployeeLoginName;
                            r.OrgName = item.OrgName;
                            r.UserName = item.EmployeeName;
                        });
                    });
                    datas.Add(json);
                }
            }

            TaskCollectionData data = CombineDatas(datas);

            var sheetConfigs = TemplateSheetOperator.Instance.GetList(templateConfigInstance.TemplateID).ToList();
            var configs = TemplateConfigOperator.Instance.GetList(templateConfigInstance.TemplateID, null).ToList();




            #region      清空模板原有数据  可能会引起填报数据的颜色跟表头一致
            //foreach (var sheet in data.Sheets)
            //{
            //    var sheetConfig = sheetConfigs.Find(x => x.TemplateSheetName == sheet.SheetName);
            //    if (sheetConfig != null)
            //    {
            //        var firstRow = sheetConfig.RowNum;
            //        var workSheet = book.Worksheets[sheet.SheetName];
            //        for (int i = workSheet.Cells.MaxRow; i >= firstRow; i--)
            //        {
            //            workSheet.Cells.DeleteRow(i);
            //        }
            //    }
            //}
            #endregion

            //表示是否是空列
            var emptycolumn = new List<int>();

            foreach (var sheet in data.Sheets)
            {
                var sheetConfig = sheetConfigs.Find(x => x.TemplateSheetName == sheet.SheetName);
                if (sheetConfig != null)
                {
                    var firstColumn = sheetConfig.ColumnNum;
                    var firstRow = sheetConfig.RowNum;
                    var workSheet = book.Worksheets[sheet.SheetName];
                    var firstColumnStyle = workSheet.Cells[firstRow - 1, firstColumn - 1].GetStyle();
                    var firstRowStyle = workSheet.Cells[firstRow, firstColumn].GetStyle();

                    //  var startIdx = firstColumn - 1;


                    var style = workSheet.Cells[sheetConfig.RowNum, firstColumn - 1].GetStyle();

                    var currentConfigs = configs.FindAll(x => x.TemplateSheetID == sheetConfig.ID);

                    //获取没有列标题的列(主要考虑合并单元格的情况)
                    for (int i = 0; i <= workSheet.Cells.MaxDataColumn; i++)
                    {
                        var flag = true;
                        //主要考虑到合并单元格
                        for (int j = 1; j <= firstRow; j++)
                        {
                            if (workSheet.Cells[firstRow - j, i].StringValue.Trim() != "")
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                            emptycolumn.Add(i);
                    }


                    //没有示例的内容更汇总颜色
                    var styleDic = new Dictionary<int, Style>();
                    for (int i = 0; i <= workSheet.Cells.MaxColumn; i++)
                    {
                        var columnStyle = workSheet.Cells[firstRow, i].GetStyle();
                        if (workSheet.Cells[firstRow, i].StringValue == "")
                        {
                            columnStyle.ForegroundColor = Color.White;
                            columnStyle.Pattern = BackgroundType.Solid;
                        }
                        styleDic.Add(i, columnStyle);
                    }

                    int rowIndex = firstRow;
                    sheet.Rows.ForEach(row =>
                    {
                        var current = rowIndex;
                        workSheet.Cells.InsertRows(current, 1);
                        workSheet.Cells.CopyRow(workSheet.Cells, current + 1, current);

                        int cellIndex = 0;
                        currentConfigs.ForEach(x =>
                        {
                            if (cellIndex <= row.Cells.Count - 1)
                            {
                                var cell = row.Cells[cellIndex];
                                var excelCell =
                                    workSheet.Cells[current, cellIndex + (firstColumn - 1)];
                                try
                                {

                                    switch (cell.Type)
                                    {
                                        case "Text":
                                            excelCell.PutValue(cell.Value);
                                            break;
                                        case "Number":
                                            excelCell.PutValue(Double.Parse(cell.Value));
                                            break;
                                        default:
                                            excelCell.PutValue(cell.Value);
                                            break;
                                    }


                                    //将没有表头的列置空（排除第一列）
                                    if (emptycolumn.Contains(cellIndex) && rowIndex > firstRow)
                                    {
                                        workSheet.Cells[rowIndex, cellIndex].PutValue("");
                                        var s = workSheet.Cells[rowIndex, cellIndex].GetStyle();
                                        s.ForegroundColor = Color.White;
                                        s.Pattern = BackgroundType.Solid;
                                        s.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                        s.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                        s.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                        s.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                        workSheet.Cells[rowIndex, cellIndex].SetStyle(s);
                                    }
                                    //设置内容颜色
                                    else
                                    {
                                        //设置公式
                                        if (x.IsFormula == 1)
                                        {
                                            if (cell.IsFormula)
                                            {
                                                excelCell.Formula = x.CellFormula.Replace("{R}", (1 + rowIndex).ToString());
                                                styleDic[cellIndex].Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                                styleDic[cellIndex].Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                                styleDic[cellIndex].Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                                styleDic[cellIndex].Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                                workSheet.Cells[rowIndex, cellIndex].SetStyle(styleDic[cellIndex]);
                                            }
                                            else
                                            {
                                                //破坏掉公式后，亮色显示背景色
                                                var noFormulaStyle = workSheet.Cells[rowIndex, cellIndex].GetStyle();
                                                noFormulaStyle.ForegroundColor = Color.Yellow;
                                                noFormulaStyle.Pattern = BackgroundType.Solid;
                                                noFormulaStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                                noFormulaStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                                noFormulaStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                                noFormulaStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                                workSheet.Cells[rowIndex, cellIndex].SetStyle(noFormulaStyle);
                                            }
                                        }
                                        //二维表高亮显示时，用值填充
                                        if (cell.IsShowColorForUpdateData)
                                        {
                                            excelCell.PutValue(cell.Value);
                                            var noFormulaStyle = workSheet.Cells[rowIndex, cellIndex].GetStyle();
                                            noFormulaStyle.ForegroundColor = Color.Yellow;
                                            noFormulaStyle.Pattern = BackgroundType.Solid;
                                            noFormulaStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
                                            noFormulaStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
                                            noFormulaStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
                                            noFormulaStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
                                            workSheet.Cells[rowIndex, cellIndex].SetStyle(noFormulaStyle);
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                                //}
                            }
                            cellIndex++;
                        });
                        rowIndex++;
                    });
                    var currentRow = workSheet.Cells[rowIndex];
                    var start = rowIndex;
                    while (currentRow != null)
                    {
                        var list = new List<Cell>();
                        for (int j = 0; j <= workSheet.Cells.MaxDataColumn; j++)
                        {
                            list.Add(workSheet.Cells[rowIndex, j]);
                        }
                        if (
                            //list.Any(x => !string.IsNullOrEmpty(x.StringValue))
                            //&&
                            list.Any(x => x.IsMerged)
                            ||
                            list.All(x => string.IsNullOrEmpty(x.StringValue))
                        )
                        {
                            break;
                        }
                        rowIndex++;
                        currentRow = workSheet.Cells[rowIndex];
                    }
                    workSheet.Cells.DeleteRows(start, rowIndex - start);

                    //重新计算后插入数据库
                    book.CalculateFormula();
                    InsertColumn(workSheet, sheet, firstColumn, firstRow, c1, c2, c3, c4, firstColumnStyle, firstRowStyle);
                }
                emptycolumn = new List<int>();
            }

            //book.CalculateFormula();
            return book;
        }

        private TaskCollectionData CombineDatas(List<TaskCollectionData> datas)
        {
            TaskCollectionData data = new TaskCollectionData();

            data.Sheets = new List<DataSheet>();

            foreach (TaskCollectionData item in datas)
            {
                foreach (var sheet in item.Sheets)
                {
                    if (data.Sheets.Find(p => p.SheetName == sheet.SheetName) == null)
                    {
                        data.Sheets.Add(new DataSheet() { SheetName = sheet.SheetName, Rows = new List<DataRows>() });
                    }

                    var dataSheet = data.Sheets.Find(p => p.SheetName == sheet.SheetName);

                    dataSheet.Rows.AddRange(sheet.Rows);
                }
            }

            return data;
        }

        #region 私有方法

        /// <summary>
        /// 获取excel 有效的第一列
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="firstRow"></param>
        /// <returns></returns>
        private int GetExcelFistrColumn(Worksheet workSheet, int firstRow)
        {
            var result = 0;
            //获取没有列标题的列
            for (int i = 0; i <= workSheet.Cells.MaxDataColumn; i++)
            {
                var flag = true;
                for (int j = 1; j <= firstRow; j++)
                {
                    if (workSheet.Cells[firstRow - j, i].StringValue.Trim() != "")
                    {
                        flag = false;
                        result = i;
                        break;
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 插入列
        /// </summary>
        /// <param name="ws"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private void InsertColumn(Worksheet workSheet, DataSheet sheet, int firstColumn, int firstRow, bool c1, bool c2, bool c3, bool c4, Style firstColumnStyle, Style firstRowStyle)
        {
            var insertColumnsCount = (c1 ? 1 : 0) + (c2 ? 1 : 0) + (c3 ? 1 : 0) + (c4 ? 1 : 0);
            workSheet.Cells.InsertColumns(firstColumn - 1, insertColumnsCount, true);
            var startIdx = firstColumn - 1;
            if (c1)
            {
                workSheet.Cells[firstRow - 1, startIdx].PutValue("公司");
                workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                startIdx++;
            }
            if (c2)
            {
                workSheet.Cells[firstRow - 1, startIdx].PutValue("组织架构");
                workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                startIdx++;
            }
            if (c3)
            {
                workSheet.Cells[firstRow - 1, startIdx].PutValue("姓名");
                workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                startIdx++;
            }
            if (c4)
            {
                workSheet.Cells[firstRow - 1, startIdx].PutValue("账号");
                workSheet.Cells[firstRow - 1, startIdx].SetStyle(firstColumnStyle);
                startIdx++;
            }
            var style = workSheet.Cells[firstRow, firstColumn - 1 + insertColumnsCount].GetStyle();
            int rowIndex = firstRow;
            sheet.Rows.ForEach(row =>
            {
                var current = rowIndex;
                startIdx = firstColumn - 1;
                if (c1)
                {
                    workSheet.Cells[current, startIdx].PutValue("");
                    workSheet.Cells[current, startIdx].SetStyle(firstRowStyle);
                    startIdx++;
                }
                if (c2)
                {
                    workSheet.Cells[current, startIdx].PutValue(row.OrgName);
                    workSheet.Cells[current, startIdx].SetStyle(firstRowStyle);
                    startIdx++;
                }
                if (c3)
                {
                    workSheet.Cells[current, startIdx].PutValue(row.UserName);
                    workSheet.Cells[current, startIdx].SetStyle(firstRowStyle);
                    startIdx++;
                }
                if (c4)
                {
                    workSheet.Cells[current, startIdx].PutValue(row.LoginName);
                    workSheet.Cells[current, startIdx].SetStyle(firstRowStyle);
                    startIdx++;
                }

                rowIndex++;
            });

        }

        #endregion

    }
}
