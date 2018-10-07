using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.QrCode;

namespace MyCode.Project.Infrastructure.Common
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="codeNumber">要生成二维码的字符串</param>     
        /// <param name="size">大小尺寸</param>
        /// <returns>二维码图片</returns>
        public static Bitmap Create_ImgCode(string codeNumber, int size)
        {
            //创建二维码生成类
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片            
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);
            return image;
        }
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="content">生成内容</param>
        /// <returns></returns>
        public static Bitmap CreateImgCode(string content)
        {
            BarcodeWriter writer=new BarcodeWriter();
            writer.Format=BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options=new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 255;
            options.Height = 255;
            //设置二维码的边距，单位不是固定像素
            options.Margin = 1;
            writer.Options = options;
            Bitmap image = writer.Write(content);
            return image;
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="bitMapDic"></param>
        /// <returns></returns>
        public static Bitmap MergerImg(Dictionary<string, Bitmap> bitMapDic)
        {
            if (bitMapDic == null || bitMapDic.Count == 0)
                throw new Exception("图片数不能够为0");
            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap backgroudImg = new Bitmap(bitMapDic.Count * 12, 16);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            int j = 0;
            foreach (KeyValuePair<string, Bitmap> entry in bitMapDic)
            {
                Bitmap map = entry.Value;
                g.DrawImage(map, j * 11, 0, map.Width, map.Height);
                j++;
            }
            g.Dispose();
            return backgroudImg;
        }

        /// <summary>
        /// 调用此函数后使此两种图片合并
        /// </summary>
        /// <param name="imgBack">粘贴的源图片</param>
        /// <param name="img">粘贴的目标图片</param>
        /// <param name="x">左边距</param>
        /// <param name="y">上边距</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static Image CombinImage(Image imgBack, Image img, float x,float y,float width,float height)
        {
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            Graphics g = Graphics.FromImage(imgBack);
            //加框
            g.FillRectangle(System.Drawing.Brushes.Transparent, x - 1, y - 1, width + 2, height + 2);
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);单位：像素
            g.DrawImage(img, x, y, width, height);
            g.Dispose();
            //GC.Collect();
            return imgBack;
        }

        /// <summary>
        /// 从网络下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image DownLoadNetImage(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;

            req.ContentType = "image/jpg";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            System.IO.Stream stream = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                var image = System.Drawing.Image.FromStream(stream);
                return image;

            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
        
        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image CutEllipse(Image img, int x, int y, int width, int height)
        {
            //截图画板
            Bitmap bm = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bm);
            //创建截图路径（类似Ps里的路径）
            GraphicsPath gpath = new GraphicsPath();
            gpath.AddEllipse(x, y, width, height);//圆形

            //设置画板的截图路径
            g.SetClip(gpath);

            //对图片进行截图
            g.DrawImage(img, x, y);
            g.Dispose();

            return bm;
        }

    }
}
