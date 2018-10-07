using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyCode.Project.Infrastructure.Exports;

namespace MyCode.Project.WebApi
{
    public partial class Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Clear(); //清空无关信息
            //Response.BufferOutput = false; //完成整个响应后再发送
            //Response.Charset = "utf-8";//设置输出流的字符集
            //Response.AppendHeader("Content-Disposition", "attachment;filename=test.xls");//追加头信息
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//设置输出流的字符集
            //Response.ContentType = "application/vnd.ms-excel";//

            //创建2007的Excel

            //Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head>><body><table>");
            //for (int i = 0; i < 100; i++)
            //{
            //    Response.Write($"<tr><td>{i}</td></tr>");
            //     //var bytes = Encoding.UTF8.GetBytes(i.ToString());
            //     //Response.BinaryWrite(bytes);
            //}
            //Response.Write("</table></body></html>");



            // Response.End();//停止输出

            var list = new List<ClassA>();

            list.Add(new ClassA() { Title="2"});

            var excelConfig = new ExportExcelConfig();

            excelConfig.Properties.Add(new ExportExcelProperty() { Caption="标题",EntityProp="Title"});

            //var excelName = DateTime.Now.ToString("yyyyMMddHHmmss_fff") + ".xls";
            var excelName = "20180918155457_019.xls";

            excelConfig.CreateExcelFile(list, excelName);
            //excelConfig.CreateExcelFile(list, excelName);
        }
    }

    public class ClassA
    {
        public string Title { get; set; }
    }
}