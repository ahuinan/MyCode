using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MyCode.Project.Infrastructure.Common
{
    /// <summary>
    /// EXCEL工具类
    /// </summary>
    public class ExcelUtils
    {
        /// <summary>
        /// 工作簿的定义
        /// </summary>
        private IWorkbook _iWorkbook;

        /// <summary>
        /// 文件流初始化对象
        /// </summary>
        /// <param name="stream">传入流对象</param>
        public ExcelUtils(Stream stream)
        {
            _iWorkbook = CreateWorkBook(stream);
        }

        /// <summary>
        /// 传入文件名称读取Excel文件中的信息
        /// </summary>
        /// <param name="fileName">传递的文件名称(包含路径)</param>
        public ExcelUtils(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                _iWorkbook = CreateWorkBook(fileStream);
            }
        }

        /// <summary>
        /// 使用try-Catch创建一个工作薄对象
        /// </summary>
        /// <param name="stream">传递过来创建的工作薄对象(流文件)</param>
        /// <returns></returns>
        private IWorkbook CreateWorkBook(Stream stream)
        {
            try
            {
                return new XSSFWorkbook(stream);
            }
            catch
            {
                return new HSSFWorkbook(stream);
            }
        }

        /// <summary>
        /// 读取Excel中默认第一张sheet的集合
        /// </summary>
        /// <typeparam name="T">需要读取的Excel实体集合和Excel中的字段对应</typeparam>
        /// <param name="fields">excel中各个列，以此要转换成为对象的字段</param>
        /// <returns>返回List集合对象</returns>
        public List<T> ExcelToList<T>(List<string> fields) where T : class, new()
        {
            return ExportToList<T>(_iWorkbook.GetSheetAt(0), fields);
        }

        /// <summary>
        /// 将Excel中第一个Sheet中的数据信息转换成List集合
        /// </summary>
        /// <typeparam name="T">需要读取的Excel实体集合和Excel中的字段对应</typeparam>
        /// <param name="iSheet">表示Excel中第一个Sheet</param>
        /// <param name="fields">excel中各个列，以此要转换成为对象的字段</param>
        /// <returns></returns>
        public List<T> ExportToList<T>(ISheet iSheet, List<string> fields) where T : class, new()
        {
            List<T> list = new List<T>();
            //循环遍历每一行的数据
            for (int i = iSheet.FirstRowNum + 1, len = iSheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = iSheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                //循环遍历列数据
                for (int j = 0, len2 = fields.Count; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell == null)
                    {
                        continue;
                    }
                    object cellValue;
                    switch (cell.CellType)
                    {
                        case CellType.String: //文本
                            cellValue = cell.StringCellValue;
                            break;
                        case CellType.Numeric: //数值
                            cellValue = Convert.ToString(cell.NumericCellValue);
                            break;
                        case CellType.Boolean: //bool类型
                            cellValue = cell.BooleanCellValue;
                            break;
                        case CellType.Blank: //空值
                            cellValue = "";
                            break;
                        default:
                            cellValue = "";
                            break;
                    }
                    typeof(T).GetProperty(fields[j]).SetValue(t, cellValue, null);
                }
                list.Add(t);
            }
            return list;
        }

        public List<string> ExportFirstRowData(ISheet iSheet)
        {
            List<string> list = new List<string>();
            if (iSheet.LastRowNum > 0)
            {
                IRow row = iSheet.GetRow(0);
                foreach (var item in row.Cells)
                {
                    list.Add(item.StringCellValue.Trim());
                }
            }

            return list;
        }

        public List<string> ExportFirstRowData()
        {
            return ExportFirstRowData(_iWorkbook.GetSheetAt(0));
        }

        /// <summary>
        /// 读取Excel中默认第一张sheet的集合
        /// </summary>
        /// <typeparam name="T">需要读取的Excel实体集合和Excel中的字段对应</typeparam>
        /// <param name="fields">excel中各个列，以此要转换成为对象的字段</param>
        /// <returns>返回List集合对象</returns>
        public List<Dictionary<string, object>> ExcelToList()
        {
            return ExportToList(_iWorkbook.GetSheetAt(0));
        }

        /// <summary>
        /// 将Excel中第一个Sheet中的数据信息转换成List集合
        /// </summary>
        /// <typeparam name="T">需要读取的Excel实体集合和Excel中的字段对应</typeparam>
        /// <param name="iSheet">表示Excel中第一个Sheet</param>
        /// <param name="fields">excel中各个列，以此要转换成为对象的字段</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ExportToList(ISheet iSheet)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            //获取第一行数据。一般是标题数据
            List<string> columns = this.ExportFirstRowData();

            //循环遍历每一行的数据
            for (int i = iSheet.FirstRowNum + 1, len = iSheet.LastRowNum + 1; i < len; i++)
            {
                Dictionary<string, object> t = new Dictionary<string, object>();
                IRow row = iSheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                bool isAllNull = true;
                //循环遍历列数据
                for (int j = 0, len2 = columns.Count; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell == null)
                    {
                        t.Add(columns[j].Trim(), null);
                        continue;
                    }
                    object cellValue;
                    switch (cell.CellType)
                    {
                        case CellType.String: //文本
                            cellValue = cell.StringCellValue.Trim();
                            break;
                        case CellType.Numeric: //数值
                            cellValue = Convert.ToString(cell.NumericCellValue);
                            break;
                        case CellType.Boolean: //bool类型
                            cellValue = cell.BooleanCellValue;
                            break;
                        case CellType.Blank: //空值
                            cellValue = null;
                            break;
                        case CellType.Formula:
                            if (cell.CachedFormulaResultType == CellType.Numeric)
                            {
                                cellValue = Convert.ToString(cell.NumericCellValue);
                            }
                            else
                            {
                                cellValue = cell.StringCellValue;
                            }
                            break;
                        default:
                            cellValue = null;
                            break;
                    }
                    //是否全部内容为空
                    isAllNull = cellValue == null && isAllNull;
                    t.Add(columns[j].Trim(), cellValue);
                }

                if (!isAllNull)
                {
                    list.Add(t);
                }
            }
            return list;
        }

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="headers">需要导出的列的列头</param>
        /// <param name="cellKes">需要导出的对应的列字段</param>
        /// <param name="strFileName">保存位置</param>
        public static void Export(DataTable dtSource, string strHeaderText, string[] headers, string[] cellKes, string strFileName)
        {
            // 将需要导出的数据导到excel中并生成文件流
            using (MemoryStream ms = Export(dtSource, strHeaderText, headers, cellKes))
            {
                // 将文件流写入文件
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="headers">需要导出的列的列头</param>
        /// <param name="cellKes">需要导出的对应的列字段</param>
        /// <returns></returns>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText, string[] headers, string[] cellKes)
        {
            // excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            // 创建一个sheet页，已strHeaderText命名
            ISheet sheet = workbook.CreateSheet(strHeaderText);

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                //si.Author = "文件作者信息"; //填加xls文件作者信息
                //si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                //si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                //si.Comments = "作者信息"; //填加xls文件作者信息
                //si.Title = "标题信息"; //填加xls文件标题信息
                //si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            // 日期的样式
            ICellStyle dateStyle = workbook.CreateCellStyle();
            // 日期的格式化
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            // 字体样式
            IFont datafont = workbook.CreateFont();
            // 字体大小
            datafont.FontHeightInPoints = 11;
            dateStyle.SetFont(datafont);
            // 边框
            dateStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            dateStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            // 其他数据的样式
            ICellStyle cellStyle = workbook.CreateCellStyle();
            // 字体样式
            IFont cellfont = workbook.CreateFont();
            // 字体大小
            cellfont.FontHeightInPoints = 11;
            cellStyle.SetFont(cellfont);
            // 边框
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            // 总的列数
            int colNum = headers.Length;
            // 每个列的宽度
            int[] arrColWidth = new int[colNum];
            // 初始化列的宽度为列头的长度，已需要显示的列头的名字长度计算
            for (int i = 0; i < headers.Length; i++)
            {
                arrColWidth[i] = Encoding.GetEncoding(936).GetBytes(headers[i]).Length * 3;
            }

            // 循环数据，取每列数据最宽的作为该列的宽度
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < cellKes.Length; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][cellKes[j]].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            // 记录生成的行数
            int rowIndex = 0;

            // DataTable中的列信息
            DataColumnCollection columns = dtSource.Columns;
            // 循环所有的行，向sheet页中添加数据
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    // 如果不是第一行，则创建一个新的sheet页
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        // 在当前sheet页上创建第一行
                        IRow headerRow = sheet.CreateRow(0);
                        // 该行的高度
                        headerRow.HeightInPoints = 50;
                        // 设置第一列的值
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        // 设置列的样式
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        // 内容居中显示
                        headStyle.Alignment = HorizontalAlignment.Center;
                        // 字体样式
                        IFont font = workbook.CreateFont();
                        // 字体大小
                        font.FontHeightInPoints = 20;
                        // 粗体显示
                        font.Boldweight = 700;
                        // 字体颜色
                        font.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;

                        headStyle.SetFont(font);
                        // 边框
                        headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                        // 设置单元格的样式
                        headerRow.GetCell(0).CellStyle = headStyle;

                        // 设置该行每个单元格的样式
                        for (int i = 1; i < colNum; i++)
                        {
                            headerRow.CreateCell(i);
                            headerRow.GetCell(i).CellStyle = headStyle;
                        }

                        // 合并单元格
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, colNum - 1));
                    }
                    #endregion


                    #region 列头及样式
                    {
                        // 创建第二行
                        IRow headerRow = sheet.CreateRow(1);
                        // 该行的高度
                        headerRow.HeightInPoints = 28;
                        // 列的样式
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        // 单元格内容居中显示
                        headStyle.Alignment = HorizontalAlignment.Center;
                        // 字体样式
                        IFont font = workbook.CreateFont();
                        // 字体大小
                        font.FontHeightInPoints = 12;
                        // 粗体
                        font.Boldweight = 700;
                        // 字体颜色
                        font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                        headStyle.SetFont(font);
                        // 边框
                        headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                        // 设置每列的样式和值
                        for (int i = 0; i < headers.Length; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(headers[i]);
                            headerRow.GetCell(i).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(i, (arrColWidth[i] + 1) * 256);
                        }
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                // 创建新的一行
                IRow dataRow = sheet.CreateRow(rowIndex);
                // 该行的高度
                dataRow.HeightInPoints = 23;
                // 循环需要写入的每列数据
                for (int i = 0; i < cellKes.Length; i++)
                {
                    // 创建列
                    ICell newCell = dataRow.CreateCell(i);
                    // 获取DataTable中该列对象
                    DataColumn column = columns[cellKes[i]];
                    // 该列的值
                    string drValue = row[column].ToString();

                    // 根据值得类型分别处理之后赋值
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);

                            newCell.CellStyle = cellStyle;
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);

                            newCell.CellStyle = cellStyle;
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);

                            newCell.CellStyle = cellStyle;
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);

                            newCell.CellStyle = cellStyle;
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");

                            newCell.CellStyle = cellStyle;
                            break;
                        default:
                            newCell.SetCellValue("");

                            newCell.CellStyle = cellStyle;
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="headers">需要导出的列的列头</param>
        /// <param name="cellKes">需要导出的对应的列字段</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string[] headers, string[] cellKes, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText, headers, cellKes).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        /// 导出Excel到Web文件
        /// </summary>
        /// <param name="table">源数据</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="headers">需要导出的列的列头</param>
        /// <param name="cellKes">需要导出的对应的列字段</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportToWeb(DataTable table, string strHeaderText, string[] headers, string[] cellKes,
            string strFileName)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Create))
            {
                var ms = Export(table, strHeaderText, headers, cellKes);
                ms.WriteTo(file);
                file.Flush();
            }
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}
