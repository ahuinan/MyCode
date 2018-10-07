using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.Infrastructure.Common
{
    /// <summary>
    /// 生成验证码的类
    /// </summary>
    public static class ValidateCodeUtils
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public static string CreateValidateCode(int pMax)
        {
            pMax = (pMax < 2 || pMax > 6) ? 4 : pMax;
            int[] randMembers = new int[pMax];
            int[] validateNums = new int[pMax];
            StringBuilder validateNumberStr = new StringBuilder();
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, Int32.MaxValue - pMax * 10000);
            int[] seeks = new int[pMax];
            for (int i = 0; i < pMax; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < pMax; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, pMax);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < pMax; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < pMax; i++)
            {
                validateNumberStr.Append(validateNums[i]);
            }
            return validateNumberStr.ToString();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode"></param>
        public static byte[] CreateValidateGraphic(string validateCode, Color bgColor)
        {
            //Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Bitmap image = new Bitmap(115, 45);
            Graphics g = Graphics.FromImage(image);
            using (image)
            {
                using (g)
                {
                    //生成随机生成器
                    Random random = new Random();
                    //清空图片背景色
                    g.Clear(bgColor);
                    //画图片的干扰线
                    for (int i = 0; i < 25; i++)
                    {
                        int x1 = random.Next(image.Width);
                        int x2 = random.Next(image.Width);
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);
                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }
                    Font font = new Font("Arial", 20, (FontStyle.Bold | FontStyle.Italic));
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                        Color.Blue, Color.DarkRed, 1.2f, true);
                    g.DrawString(validateCode, font, brush, 6, 5);
                    //画图片的前景干扰点
                    for (int i = 0; i < 60; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);
                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }
                    //画图片的边框线
                    g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                    //保存图片数据
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Jpeg);
                    //输出图片流
                    return stream.ToArray();
                }
            }
        }

        public static string[] GenerateCheckCode()
        {
            string[] result = new string[2];

            int intFirst, intSec, intTemp;

            string checkCode = String.Empty;

            System.Random random = new Random();

            intFirst = random.Next(1, 10);
            intSec = random.Next(1, 10);
            switch (random.Next(1, 3).ToString())
            {
                case "2":
                    if (intFirst < intSec)
                    {
                        intTemp = intFirst;
                        intFirst = intSec;
                        intSec = intTemp;
                    }
                    checkCode = intFirst + "-" + intSec + "=";
                    result[0] = checkCode;
                    result[1] = intFirst - intSec + "";
                    break;
                default:
                    checkCode = intFirst + "+" + intSec + "=";
                    result[0] = checkCode;
                    result[1] = intFirst + intSec + "";
                    break;
            }
            return result;
        }

        #region 产生波形滤镜效果
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;
        private static System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色  
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色  
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        #endregion

        public static byte[] CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return null;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 15.0)), 25);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器  
                Random random = new Random();

                //清空图片背景色  
                g.Clear(Color.White);

                //画图片的背景噪音线  
                for (int i = 0; i < 12; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 1, 1);

                //画图片的边框线  
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 检查图片验证码
        /// </summary>
        /// <returns></returns>
        public static bool ValidateCode(string code)
        {
            try
            {
                //从请求上下文中获取用户的登录信息。
                HttpRequest request = HttpContext.Current.Request;
                if (request.Cookies.AllKeys.Contains("ImageCode"))
                {
                    //获取后并且解析cookie中的验证码信息
                    HttpCookie cookie = request.Cookies["ImageCode"];
                    if (cookie.Value == EncryptHelper.SHA1Hash(code))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                //这里出现异常直接忽略
            }
             
            return false;
        }

        /// <summary>
        /// 保存图片验证码。
        /// </summary>
        /// <returns></returns>
        public static void SaveValidateCode(string code)
        {
            //将验证码加密后储存到cookie中
             HttpContext.Current.Response.Cookies.Add(new HttpCookie("ImageCode", EncryptHelper.SHA1Hash(code)));
        }
    }

    /// <summary>
    /// 随机图片
    /// </summary>
    public sealed class RandomImage : IDisposable
    {
        //property
        public string Text
        {
            get { return _text; }
        }
        public Bitmap Image
        {
            get { return _image; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        //Private variable
        private string _text;
        private int _width;
        private int _height;
        private Bitmap _image;
        private readonly Random _random = new Random();

        ~RandomImage()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                _image.Dispose();
        }

        private void SetDimensions(int width, int height)
        {
            _width = width <= 80 ? 100 : width;
            _height = height <= 22 ? 22 : height;
        }

        /// <summary>
        /// 产生图片
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void GenerateImage(string s, int width, int height)
        {
            _text = s;
            SetDimensions(width, height);
            SaveImage();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="pMax">The p maximum.</param>
        /// <returns></returns>
        public String GenerateRandomCode(Int32 pMax)
        {
            pMax = (pMax < 2 || pMax > 6) ? 4 : pMax;
            int[] randMembers = new int[pMax];
            int[] validateNums = new int[pMax];
            StringBuilder validateNumberStr = new StringBuilder();
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, Int32.MaxValue - pMax * 10000);
            int[] seeks = new int[pMax];
            for (int i = 0; i < pMax; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < pMax; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, pMax);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < pMax; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < pMax; i++)
            {
                validateNumberStr.Append(validateNums[i]);
            }
            return validateNumberStr.ToString();
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        private void SaveImage()
        {
            Bitmap bitmap = new Bitmap
              (_width, _height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, _width, _height);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            do
            {
                fontSize--;
                font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
                size = g.MeasureString(_text, font);
            } while (size.Width > rect.Width);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            GraphicsPath path = new GraphicsPath();
            path.AddString(_text, font.FontFamily, (int)font.Style, 33.8256f, rect, format);
            float v = 4F;
            PointF[] points =
          {
                new PointF(_random.Next(rect.Width) / v, _random.Next(
                   rect.Height) / v),
                new PointF(rect.Width - _random.Next(rect.Width) / v, 
                    _random.Next(rect.Height) / v),
                new PointF(_random.Next(rect.Width) / v, 
                    rect.Height - _random.Next(rect.Height) / v),
                new PointF(rect.Width - _random.Next(rect.Width) / v,
                    rect.Height - _random.Next(rect.Height) / v)
          };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.SkyBlue);
            g.FillPath(hatchBrush, path);
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = _random.Next(rect.Width);
                int y = _random.Next(rect.Height);
                int w = _random.Next(m / 50);
                int h = _random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            _image = bitmap;
        }
    }
}