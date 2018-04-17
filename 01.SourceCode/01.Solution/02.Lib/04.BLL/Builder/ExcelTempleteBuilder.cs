using Aspose.Cells;
using Framework.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Common;
using Lib.Model;

namespace Lib.BLL.Builder
{
    public class ExcelTempleteBuilder
    {
        private string _templeteID;
        public ExcelTempleteBuilder(string templeteID)
        {
            this._templeteID = templeteID;
        }
        public ExcelTempleteBuilder()
        {

        }
        /// <summary>
        /// 初始化excel
        /// </summary>
        /// <param name="isFile">是否为文件模式（对应是否为ismport）</param>
        /// <param name="attachment"></param>
        /// <param name="template"></param>
        /// <param name="sheets"></param>
        /// <param name="configs"></param>
        /// <param name="selects"></param>
        /// <returns></returns>
        public Workbook InitExcel(bool isFile, Attachment attachment, Template template, List<TemplateSheet> sheets, List<TemplateConfig> configs, List<TemplateConfigSelect> selects)
        {
            ExcelEngine engine = new ExcelEngine();
            Workbook wb = null;
            if (!isFile)
            {
                wb = new Workbook();
                wb.Worksheets.Clear();
            }
            else
            {
                var stream = FileUploadHelper.DownLoadFileStream(attachment.AttachmentPath,attachment.IsUseV1).ToStream();
                stream.Seek(0, SeekOrigin.Begin);
                wb = new Workbook(stream);
            }
            #region init style

            StyleFlag styleFlag = new StyleFlag() { All = true, Borders = true };
            CellsColor bcolor = wb.CreateCellsColor();
            bcolor.ColorIndex = 36;

            CellsColor ccolor = wb.CreateCellsColor();
            ccolor.ColorIndex = 36;

            CellsColor tcolor = wb.CreateCellsColor();
            tcolor.ColorIndex = 36;

            Style style = wb.Styles[wb.Styles.Add()];
            style.Font.Size = 18;
            style.Font.Name = "微软雅黑";
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            style.VerticalAlignment = Aspose.Cells.TextAlignmentType.Fill;
            style.ForegroundColor = System.Drawing.Color.White;
            style.Pattern = BackgroundType.Solid;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; ;
            style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; ;
            style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            style.VerticalAlignment = TextAlignmentType.Center;
            style.HorizontalAlignment = TextAlignmentType.Center;


            Style markStyle = wb.Styles[wb.Styles.Add()];
            markStyle.Font.Size = 15;
            markStyle.Font.Name = "微软雅黑";
            markStyle.IsTextWrapped = true;
            markStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
            markStyle.VerticalAlignment = Aspose.Cells.TextAlignmentType.Fill;
            markStyle.ForegroundColor = System.Drawing.Color.Yellow;
            markStyle.Pattern = BackgroundType.Solid;
            markStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.None;

            markStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.None;

            markStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.None;

            markStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.None;
            markStyle.VerticalAlignment = TextAlignmentType.Center;
            markStyle.HorizontalAlignment = TextAlignmentType.Left;



            Style contentStyle = wb.Styles[wb.Styles.Add()];
            //contentStyle.IsLocked = false;
            contentStyle.Font.Name = "Arial";
            contentStyle.Font.Size = 10;
            contentStyle.Pattern = BackgroundType.Solid;
            //contentStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            // contentStyle.Borders[BorderType.TopBorder].Color = System.Drawing.Color.FromArgb(155, 194, 230);
            contentStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; ;
            contentStyle.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.White;
            contentStyle.IsTextWrapped = true;
            contentStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; ;
            contentStyle.Borders[BorderType.LeftBorder].Color = bcolor.Color;
            contentStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            contentStyle.Borders[BorderType.RightBorder].Color = bcolor.Color;
            contentStyle.VerticalAlignment = TextAlignmentType.Center;
            contentStyle.HorizontalAlignment = TextAlignmentType.Left;


            Style contentStyle1 = wb.Styles[wb.Styles.Add()];
            //contentStyle1.IsLocked = false;
            contentStyle1.Font.Name = "Arial";
            contentStyle1.Font.Size = 10;
            //221, 235, 247
            contentStyle1.ForegroundColor = ccolor.Color;
            contentStyle1.Pattern = BackgroundType.Solid;
            //contentStyle1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //contentStyle1.Borders[BorderType.TopBorder].Color = System.Drawing.Color.FromArgb(155, 194, 230);
            contentStyle1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            contentStyle1.Borders[BorderType.BottomBorder].Color = bcolor.Color;
            contentStyle1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; ;
            contentStyle1.Borders[BorderType.LeftBorder].Color = bcolor.Color;
            contentStyle1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            contentStyle1.Borders[BorderType.RightBorder].Color = bcolor.Color;
            contentStyle1.VerticalAlignment = TextAlignmentType.Center;
            contentStyle1.HorizontalAlignment = TextAlignmentType.Left;


            Style titleStyle = wb.Styles[wb.Styles.Add()];
            //contentStyle.IsLocked = false;

            titleStyle.Font.Size = 12;
            titleStyle.Font.Name = "微软雅黑";
            titleStyle.ForegroundColor = tcolor.Color;
            titleStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
            titleStyle.VerticalAlignment = Aspose.Cells.TextAlignmentType.Fill;
            titleStyle.Pattern = BackgroundType.Solid;
            titleStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            titleStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; ;
            titleStyle.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            titleStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; ;
            titleStyle.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            titleStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            titleStyle.VerticalAlignment = TextAlignmentType.Center;
            titleStyle.HorizontalAlignment = TextAlignmentType.Center;
            #endregion

            foreach (var sheet in sheets)
            {
                int beforeCount = wb.Worksheets.Count;
                //重名异常，忽略
                try
                {
                    wb.Worksheets.Add(sheet.TemplateSheetName);
                }
                catch (Exception e)
                {
                    int afterCount = wb.Worksheets.Count;
                    if (afterCount > beforeCount)
                    {
                        wb.Worksheets.RemoveAt(wb.Worksheets.Count - 1);
                    }
                }
                var itemConfigs = configs.Where(x => x.TemplateSheetID == sheet.ID).ToList();
                //var configs = TemplateConfigOperator.Instance.GetList(sheet.TemplateID, sheet.ID);
                itemConfigs = itemConfigs.OrderBy(p => p.SortIndex).ToList();
                int startIndex = sheet.ColumnNum - 1;
                if (!isFile)
                {
                    Range r = wb.Worksheets[sheet.TemplateSheetName].Cells.CreateRange(0, startIndex, sheet.RowNum - 1, itemConfigs.Count);
                    r.Merge();
                    r[0, 0].PutValue(sheet.TemplateSheetTitle);
                    r.ColumnWidth = 13.88;
                    r.RowHeight = 48.75;
                    //r[0, 0].SetStyle(style);

                    if (sheet.TemplateSheetRemark != null && sheet.TemplateSheetRemark != "")
                    {
                        Range rr = wb.Worksheets[sheet.TemplateSheetName].Cells.CreateRange(11, startIndex, 5, itemConfigs.Count);
                        rr.Merge();
                        rr[0, 0].PutValue(sheet.TemplateSheetRemark);
                        rr[0, 0].SetStyle(markStyle);
                    }
                }


                if (itemConfigs.Count > 0)
                {
                    Range dataRange = wb.Worksheets[sheet.TemplateSheetName].Cells.CreateRange(sheet.RowNum, sheet.ColumnNum - 1, 9, itemConfigs.Count);
                    dataRange.Name = "DataName" + sheet.TemplateSheetName;

                }


                foreach (var config in itemConfigs)
                {
                    Style cStyle = new Style();

                    #region 格式枚举
                    //Value Type    Format String

                    //0    General General

                    //1    Decimal     0

                    //2    Decimal     0.00

                    //3    Decimal	 #,##0

                    //4    Decimal	 #,##0.00

                    //5    Currency    $#,##0;$-#,##0

                    //6    Currency    $#,##0;[Red]$-#,##0

                    //7    Currency    $#,##0.00;$-#,##0.00

                    //8    Currency    $#,##0.00;[Red]$-#,##0.00

                    //9    Percentage  0 %

                    //10   Percentage  0.00 %

                    //11   Scientific  0.00E+00

                    //12   Fraction	 # ?/?

                    //13   Fraction	 # /

                    //14   Date m/ d / yy

                    //15   Date d-mmm - yy

                    //16   Date d-mmm

                    //17   Date mmm-yy

                    //18   Time h:mm AM/ PM

                    //19   Time h:mm: ss AM/ PM

                    //20   Time h:mm

                    //21   Time h:mm: ss

                    //22   Time m/ d / yy h: mm

                    //37   Currency	 #,##0;-#,##0

                    //38   Currency	 #,##0;[Red]-#,##0

                    //39   Currency	 #,##0.00;-#,##0.00

                    //40   Currency	 #,##0.00;[Red]-#,##0.00

                    //41   Accounting _ * #,##0_ ;_ * "_ ;_ @_

                    //42   Accounting _ $* #,##0_ ;_ $* "_ ;_ @_

                    //43   Accounting _ * #,##0.00_ ;_ * "??_ ;_ @_

                    //44   Accounting _ $* #,##0.00_ ;_ $* "??_ ;_ @_

                    //45   Time mm:ss

                    //46   Time h :mm: ss

                    //47   Time mm:ss.0

                    //48   Scientific	 ##0.0E+00

                    //49   Text    @
                    #endregion

                    // Obtain the existing Validations collection.
                    ValidationCollection validations = wb.Worksheets[sheet.TemplateSheetName].Validations;



                    // Create a validation object adding to the collection list.
                    Validation validation = validations[validations.Add()];

                    CellArea area;
                    switch (config.FieldType)
                    {

                        case "Text":
                            cStyle.Number = 49;
                            break;
                        case "Number":
                            cStyle.Number = 1;
                            if (config.Digit != 0)
                            {
                                cStyle.Custom = "0.".PadRight(config.Digit + 2, '0');
                            }
                            // Set the validation type.
                            validation.Type = ValidationType.Decimal;

                            // Specify the operator.
                            validation.Operator = OperatorType.Between;

                            // Set the lower and upper limits.
                            validation.Formula1 = Decimal.MinValue.ToString();
                            validation.Formula2 = Decimal.MaxValue.ToString();

                            // Set the error message.
                            validation.ErrorMessage = "";

                            // Specify the validation area of cells.
                            area.StartRow = sheet.RowNum;
                            area.EndRow = 1048575;
                            area.StartColumn = startIndex;
                            area.EndColumn = startIndex;

                            // Add the area.
                            validation.AreaList.Add(area);

                            break;
                        case "DateTiem":
                            cStyle.Number = 15;
                            cStyle.Custom = "yyyy-m-d";

                            // Set the data validation type.
                            validation.Type = ValidationType.Date;

                            // Set the operator for the data validation
                            validation.Operator = OperatorType.Between;

                            // Set the value or expression associated with the data validation.
                            validation.Formula1 = "1970-1-1";

                            // The value or expression associated with the second part of the data validation.
                            validation.Formula2 = "2099-12-31";

                            // Enable the error.
                            validation.ShowError = true;

                            // Set the validation alert style.
                            validation.AlertStyle = ValidationAlertType.Stop;

                            // Set the title of the data-validation error dialog box
                            validation.ErrorTitle = "r";

                            // Set the data validation error message.
                            validation.ErrorMessage = "";

                            // Set and enable the data validation input message.
                            validation.InputMessage = "";
                            validation.IgnoreBlank = true;
                            validation.ShowInput = true;

                            // Set a collection of CellArea which contains the data validation settings.

                            area.StartRow = sheet.RowNum;
                            area.EndRow = 1048575;
                            area.StartColumn = startIndex;
                            area.EndColumn = startIndex;

                            validation.AreaList.Add(area);

                            break;
                    }


                    styleFlag = new StyleFlag() { NumberFormat = true, VerticalAlignment = true, HorizontalAlignment = true, FontSize = true, FontColor = true };

                    var colors = config.BGColor.Split(',');
                    cStyle.ForegroundColor = System.Drawing.Color.FromArgb(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]));
                    cStyle.Pattern = BackgroundType.Solid;
                    if (!isFile)
                    {
                        wb.Worksheets[sheet.TemplateSheetName].Cells.ApplyColumnStyle(startIndex, cStyle, styleFlag);

                        wb.Worksheets[sheet.TemplateSheetName].Cells[0, 0].SetStyle(style);
                        wb.Worksheets[sheet.TemplateSheetName].Cells[sheet.RowNum - 1, startIndex].PutValue(config.FieldName);
                        wb.Worksheets[sheet.TemplateSheetName].Cells[sheet.RowNum - 1, startIndex].SetStyle(titleStyle);
                    }
                    if (config.BGColor != "255,255,255")
                    {
                        wb.Worksheets[sheet.TemplateSheetName].Cells[sheet.RowNum - 1, startIndex].SetStyle(cStyle);
                    }


                    var selectedValues = selects.FindAll(x => x.TemplateConfigID == config.ID);// TemplateConfigSelectOperator.Instance.GetList(sheet.TemplateID, sheet.ID, config.ID);
                    if (selectedValues.Count > 0)
                    {
                        var hidSheet = wb.Worksheets["勿删除"];

                        if (hidSheet == null)
                        {
                            hidSheet = wb.Worksheets.Add("勿删除");
                        }

                        hidSheet.IsVisible = false;
                        Range range = hidSheet.Cells.CreateRange(50, itemConfigs.Count + 10 + startIndex, selectedValues.Count, 1);

                        range.Name = "MyRange" + sheet.TemplateSheetName + startIndex;
                        int i = 0;
                        foreach (var item in selectedValues)
                        {
                            range[i, 0].PutValue(item.SelectedValue);
                            i++;
                        }

                        validation = validations[validations.Add()];


                        validation.Type = Aspose.Cells.ValidationType.List;


                        validation.Operator = OperatorType.None;


                        validation.InCellDropDown = true;


                        validation.Formula1 = "=MyRange" + sheet.TemplateSheetName + startIndex;


                        validation.ShowError = true;


                        validation.AlertStyle = ValidationAlertType.Stop;


                        validation.ErrorTitle = "Error";

                        validation.ErrorMessage = "";

                        area.StartRow = sheet.RowNum;
                        area.EndRow = 1048575;
                        area.StartColumn = startIndex;
                        area.EndColumn = startIndex;


                        validation.AreaList.Add(area);
                    }

                    startIndex++;
                }
                if (!isFile)
                {
                    wb.Worksheets[sheet.TemplateSheetName].AutoFilter.SetRange(sheet.RowNum - 1, sheet.ColumnNum - 1, itemConfigs.Count - 1);
                }
            }

            wb.FileName = template.TemplateName;
            if (wb.Worksheets.Count > 0 && wb.Worksheets[0].IsVisible)
            {
                wb.Worksheets.ActiveSheetIndex = 0;
            }
            return wb;
        }

        public Workbook InitExcelTemplete(out string fileExt)
        {
            Workbook book = null;
            fileExt = ".xlsx";
            if (!string.IsNullOrEmpty(_templeteID))
            {
                var template = TemplateOperator.Instance.GetModel(this._templeteID);
                var attach = AttachmentOperator.Instance.GetModel("UploadModelAttach", this._templeteID).FirstOrDefault();
                if (attach != null)
                {
                    fileExt = attach.FileExt;
                }
                var sheets = TemplateSheetOperator.Instance.GetList(this._templeteID).ToList();
                var configs = TemplateConfigOperator.Instance.GetList(this._templeteID, null).ToList();
                var selects = TemplateConfigSelectOperator.Instance.GetList(this._templeteID).ToList();
                book = InitExcel(template.IsImport == 1, attach, template, sheets, configs, selects);
            }
            return book;
        }
    }
}
