using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Infrastructure.Common
{
	public class EncryptHelper
	{
		//DES加密秘钥，要求为8位  
		private const string desKey = "looxierp";
		//默认密钥向量  
		private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

		//秘钥过期的秒数
		private static int expireSecond = 36000;

        #region Encrypt(加密，带过期时间)
        /// <summary>  
        /// DES加密  
        /// </summary>  
        /// <param name="encryptString">待加密的字符串，未加密成功返回原串</param>  
        /// <returns></returns>  
        public static string Encrypt(string encryptString)
		{
			var totalSecond = Convert.ToInt64((DateTime.Now - DateTime.Parse("2000-01-01")).TotalSeconds);

			encryptString = encryptString + "___" + totalSecond;

			byte[] rgbKey = Encoding.UTF8.GetBytes(desKey);
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
			DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();
			string returnStr = Convert.ToBase64String(mStream.ToArray());
			returnStr = returnStr.Replace('+', '@');
			return System.Web.HttpUtility.UrlEncode(returnStr);
		}
        #endregion

        #region Decrypt(解密，带过期时间)
        /// <summary>  
        /// DES解密  
        /// </summary>  
        /// <param name="decryptString">待解密的字符串，未解密成功返回原串</param>  
        /// <returns></returns>  
        public static string Decrypt(string decryptString)
		{
			decryptString = System.Web.HttpUtility.UrlDecode(decryptString);
			decryptString = decryptString.Replace('@', '+');
			byte[] rgbKey = Encoding.UTF8.GetBytes(desKey);
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Convert.FromBase64String(decryptString);
			DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();

			var returnStr = Encoding.UTF8.GetString(mStream.ToArray());
			var arrStr = returnStr.Split(new char[3] { '_', '_', '_' }, StringSplitOptions.RemoveEmptyEntries);

			if (arrStr.Length < 2)
			{
				throw new BaseException("非法加密字符串：" + decryptString);
			}
			var totalSecond = Convert.ToInt64(arrStr[1]);
			var currentTotalSecond = Convert.ToInt64((DateTime.Now - DateTime.Parse("2000-01-01")).TotalSeconds);
			if (currentTotalSecond - totalSecond > expireSecond)
			{
				throw new BaseException("该加密字符串已过期");
			}
			return arrStr[0];
		}
        #endregion

        #region GetMD5(获取大写的MD5签名结果)
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        #endregion

        #region MD5Encrypt(MD5加密)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr">需要加密文本</param>
        /// <param name="charset">字符编码默认UTF8</param>
        /// <returns></returns>
        public static string MD5Encrypt(string encypStr, string charset = "UTF-8")
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("UTF-8").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);
            retStr = System.BitConverter.ToString(outputBye);
            return retStr;
        }
        #endregion

        #region SHA1Hash(SHA加密)
        /// <summary>
        /// 加密方法，通过SHA1加密后再转Base64
        /// 注意，这个加密属于不可逆的加密，一般用于密码加密使用
        /// </summary>
        /// <param name="input">需要加密的文本</param>
        /// <returns></returns>
        public static string SHA1Hash(string input)
        {
            HashAlgorithm algorithm = SHA1.Create();
            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
        #endregion

        #region EncryptToSHA1()
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncryptToSHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        #endregion

        #region Base64Encrypt
        /// <summary>
        /// Base64加密方法
        /// </summary>
        /// <param name="input">需要加密的文本</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }
        #endregion

        #region Base64Decrypt
        /// <summary>
        /// Base64解密方法
        /// </summary>
        /// <param name="input">需要解密的文本</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
        #endregion

    }
}
