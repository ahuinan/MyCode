using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.POIFS.FileSystem;

namespace MyCode.Project.Infrastructure.Exports
{
    /// <summary>
    /// 导出Excel配置类
    /// </summary>
    public class ExportExcelConfig
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public object Condition { get; set; }

        /// <summary>
        /// Excel中的标题和实体属性中的字段Map，例如：标题->Title
        /// </summary>
        public List<ExportExcelProperty> Properties { get; }

        /// <summary>
        /// 初始化一个<see cref="ExportExcelConfig"/>类型的实例
        /// </summary>
        public ExportExcelConfig()
        {
            Properties=new List<ExportExcelProperty>();
        }

        /// <summary>
        /// 根据数据实体和配置生成相应的Excel文件
        /// </summary>
        /// <param name="sourceData">数据源</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string CreateExcelFile(IList sourceData,string fileName)
        {
            //获取列表第一个元素的数据类型
            Type type = null;
            if (sourceData.Count > 0)
            {
                type = sourceData[0].GetType();
            }

            //创建2007的Excel
            HSSFWorkbook workbook = null;

            //如果文件存在，则直接获取采用追加的方式追加进去
            string filePath = GetDownloadPath(fileName);

            string absFilePath = FileUtils.GetPhysicalPath(filePath);

            ISheet sheet = null;

            IRow row = null;

            //如果已经存在该文件，则说明已经产生过了，那么采用追加的方式
            if (File.Exists(absFilePath))
            {
                using (var fs = new FileStream(absFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    POIFSFileSystem ps = new POIFSFileSystem(fs);

                    workbook = new HSSFWorkbook(ps);

                    sheet = workbook.GetSheetAt(0);

                    fs.Close();
                    fs.Dispose();
                    //row = sheet.CreateRow(sheet.LastRowNum + 1);
                }//读取流
            }
            else
            {
                // (absFilePath);
                workbook = new HSSFWorkbook();

                sheet = workbook.CreateSheet();

                //设置标题列样式
                ICellStyle headStyle = workbook.CreateCellStyle();
                headStyle.VerticalAlignment = VerticalAlignment.Center;
                headStyle.Alignment = HorizontalAlignment.Center;

                //设置字体-加粗字体
                IFont headFont = workbook.CreateFont();
                headFont.IsBold = true;

                row = sheet.CreateRow(0);

                //创建标题列
                for (int i = 0; i < this.Properties.Count; i++)
                {
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(this.Properties[i].Caption);
                    cell.CellStyle = headStyle;
                    cell.CellStyle.SetFont(headFont);
                }
            }


            //创建内容列
            for (int i = 0; i < sourceData.Count ; i++)
            {
                var entity = sourceData[i];
                //创建新的内容行
                row = sheet.CreateRow(i + sheet.LastRowNum + 1);

                //根据配置的属性列创建对应单元格
                for (int j = 0; j < this.Properties.Count; j++)
                {
                    var prop = this.Properties[j].EntityProp;

                    //储存内容值
                    object value = null;

                    //检查是否存在，如果存在则表示是拼接数据
                    if (prop.IndexOf("+", StringComparison.Ordinal) > -1)
                    {
                        var joinProps = prop.Split('+');
                        for (int jp = 0; jp < joinProps.Length; jp++)
                        {
                            var jprop = joinProps[jp];
                            var newValue = this.GetPropValue(jprop, type, entity);
                            if (value != null)
                            {
                                if (newValue != null)
                                {
                                    //这里只智齿字符串的累加，主要解决地址合并省市区的情况
                                    value = value.ToString() + this.Properties[j].JoinPropChar + newValue.ToString();
                                }
                            }
                            else
                            {
                                value = newValue;
                            }
                        }
                    }
                    else
                    {
                        value = this.GetPropValue(prop, type, entity);
                    }

                    //根据数据类型填写对应的单元格
                    if (value == null)
                    {
                        row.CreateCell(j).SetCellValue("");
                    }
                    else if (value is int)
                    {
                        row.CreateCell(j).SetCellValue((int)value);
                    }
                    else if (value is long)
                    {
                        row.CreateCell(j).SetCellValue((long)value);
                    }
                    else if (value is decimal)
                    {
                        row.CreateCell(j).SetCellValue(Convert.ToDouble(value));
                    }
                    else if (value is double)
                    {
                        row.CreateCell(j).SetCellValue((double)value);
                    }
                    else if (value is DateTime)
                    {
                        row.CreateCell(j).SetCellValue(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (value is bool)
                    {
                        row.CreateCell(j).SetCellValue((bool)value);
                    }
                    else
                    {
                        row.CreateCell(j).SetCellValue(value.ToString());
                    }
                }
            }


            using (FileStream fs = File.OpenWrite(absFilePath))
            {
                workbook.Write(fs);
                fs.Close();
                fs.Dispose();
            }

            workbook = null;
            return filePath;
        }

        /// <summary>
        /// 获取下载路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private string GetDownloadPath(string fileName)
        {
            string dirPath = "/download/exceltemp/";
            string absFilePath = FileUtils.GetPhysicalPath(dirPath);
            if (!Directory.Exists(absFilePath))
            {
                Directory.CreateDirectory(absFilePath);
            }
            //string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_fff");
            //string filePath = Path.Combine(dirPath, fileName + "_" + newFileName + ".xls");
            //return filePath;
            return Path.Combine(dirPath, fileName);
        }

        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="prop">属性名</param>
        /// <param name="type">类型</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        private object GetPropValue(string prop, Type type, object entity)
        {
            object value = null;
            //如果存在.号则表示是复杂类型属性，需要依次去获取数据
            if (prop.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                var props = prop.Split('.');
                //获取第一级实体
                value = type.GetProperty(props[0]).GetValue(entity);
                for (int p = 1; p < props.Length; p++)
                {
                    //如果值为空，则直接退出
                    if (value == null)
                    {
                        break;
                    }
                    Type childType = value.GetType();
                    value = childType.GetProperty(props[p]).GetValue(value);
                }
            }
            else
            {
                value = type.GetProperty(prop).GetValue(entity);
            }
            return value;
        }
    }
}
