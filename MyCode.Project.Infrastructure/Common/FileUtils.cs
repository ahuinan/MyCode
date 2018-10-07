using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;

[Serializable]
public class FileItem
{
    public FileItem()
    { }

    #region 私有字段
    private string _Name;
    private string _FullName;
    private DateTime _CreationDate;
    private bool _IsFolder;
    private long _Size;
    private DateTime _LastAccessDate;
    private DateTime _LastWriteDate;
    private int _FileCount;
    private int _SubFolderCount;
    #endregion

    #region 公有属性
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    /// <summary>
    /// 文件或目录的完整目录
    /// </summary>
    public string FullName
    {
        get { return _FullName; }
        set { _FullName = value; }
    }

    /// <summary>
    ///  创建时间
    /// </summary>
    public DateTime CreationDate
    {
        get { return _CreationDate; }
        set { _CreationDate = value; }
    }

    public bool IsFolder
    {
        get { return _IsFolder; }
        set { _IsFolder = value; }
    }

    /// <summary>
    /// 文件大小
    /// </summary>
    public long Size
    {
        get { return _Size; }
        set { _Size = value; }
    }

    /// <summary>
    /// 上次访问时间
    /// </summary>
    public DateTime LastAccessDate
    {
        get { return _LastAccessDate; }
        set { _LastAccessDate = value; }
    }

    /// <summary>
    /// 上次读写时间
    /// </summary>
    public DateTime LastWriteDate
    {
        get { return _LastWriteDate; }
        set { _LastWriteDate = value; }
    }

    /// <summary>
    /// 文件个数
    /// </summary>
    public int FileCount
    {
        get { return _FileCount; }
        set { _FileCount = value; }
    }

    /// <summary>
    /// 目录个数
    /// </summary>
    public int SubFolderCount
    {
        get { return _SubFolderCount; }
        set { _SubFolderCount = value; }
    }
    #endregion
}

public class FileUtils
{
    #region 构造函数
    private static string strRootFolder;
    static FileUtils()
    {
        // strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
        // strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));

        strRootFolder = GetPhysicalPath("/File");
    }
    #endregion

    #region 目录
    /// <summary>
    /// 读根目录
    /// </summary>
    public static string GetRootPath()
    {
        return strRootFolder;
    }

    /// <summary>
    /// 写根目录
    /// </summary>
    public static void SetRootPath(string path)
    {
        strRootFolder = path;
    }

    /// <summary>
    /// 读取目录列表
    /// </summary>
    public static List<FileItem> GetDirectoryItems()
    {
        return GetDirectoryItems(strRootFolder);
    }

    /// <summary>
    /// 读取目录列表
    /// </summary>
    public static List<FileItem> GetDirectoryItems(string path)
    {
        List<FileItem> list = new List<FileItem>();
        string[] folders = Directory.GetDirectories(path);
        foreach (string s in folders)
        {
            FileItem item = new FileItem();
            DirectoryInfo di = new DirectoryInfo(s);
            item.Name = di.Name;
            item.FullName = di.FullName;
            item.CreationDate = di.CreationTime;
            item.IsFolder = false;
            list.Add(item);
        }
        return list;
    }
    #endregion

    #region 文件
    /// <summary>
    /// 读取文件列表
    /// </summary>
    public static List<FileItem> GetFileItems()
    {
        return GetFileItems(strRootFolder);
    }

    /// <summary>
    /// 读取文件列表
    /// </summary>
    public static List<FileItem> GetFileItems(string path)
    {
        List<FileItem> list = new List<FileItem>();
        string[] files = Directory.GetFiles(path);
        foreach (string s in files)
        {
            FileItem item = new FileItem();
            FileInfo fi = new FileInfo(s);
            item.Name = fi.Name;
            item.FullName = fi.FullName;
            item.CreationDate = fi.CreationTime;
            item.IsFolder = true;
            item.Size = fi.Length;
            list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="fullName">全路径</param>
    /// <returns></returns>
    public static bool CreateFile(string fullName)
    {
        try
        {
            FileStream fs = File.Create(fullName);
            fs.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }



    /// <summary>
    /// 创建文件
    /// </summary>
    public static void  CreateFile( string path,string content)
    {
        FileInfo file = new FileInfo(path);

        using (FileStream stream = file.Create())
        {
            using (StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
            }
        }
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    public static string OpenText(string parentName)
    {
        StreamReader sr = File.OpenText(parentName);
        StringBuilder output = new StringBuilder();
        string rl;
        while ((rl = sr.ReadLine()) != null)
        {
            output.Append(rl);
        }
        sr.Close();
        return output.ToString();
    }

    /// <summary>
    /// 读取文件信息
    /// </summary>
    public static FileItem GetItemInfo(string path)
    {
        FileItem item = new FileItem();
        if (Directory.Exists(path))
        {
            DirectoryInfo di = new DirectoryInfo(path);
            item.Name = di.Name;
            item.FullName = di.FullName;
            item.CreationDate = di.CreationTime;
            item.IsFolder = true;
            item.LastAccessDate = di.LastAccessTime;
            item.LastWriteDate = di.LastWriteTime;
            item.FileCount = di.GetFiles().Length;
            item.SubFolderCount = di.GetDirectories().Length;
        }
        else
        {
            FileInfo fi = new FileInfo(path);
            item.Name = fi.Name;
            item.FullName = fi.FullName;
            item.CreationDate = fi.CreationTime;
            item.LastAccessDate = fi.LastAccessTime;
            item.LastWriteDate = fi.LastWriteTime;
            item.IsFolder = false;
            item.Size = fi.Length;
        }
        return item;
    }

    /// <summary>
    /// 写入一个新文件，在文件中写入内容，然后关闭文件。如果目标文件已存在，则改写该文件。 
    /// </summary>
    public static bool WriteAllText(string parentName, string contents)
    {
       
            File.WriteAllText(parentName, contents, Encoding.UTF8);
            return true;
     
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    public static bool DeleteFile(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    public static bool MoveFile(string oldPath, string newPath)
    {
        try
        {
            File.Move(oldPath, newPath);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region 文件夹
    /// <summary>
    /// 创建文件夹
    /// </summary>
    public static void CreateFolder(string name, string parentName)
    {
        DirectoryInfo di = new DirectoryInfo(parentName);
        di.CreateSubdirectory(name);
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    public static bool DeleteFolder(string path)
    {
        try
        {
            Directory.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 移动文件夹
    /// </summary>
    public static bool MoveFolder(string oldPath, string newPath)
    {
        try
        {
            Directory.Move(oldPath, newPath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    public static bool CopyFolder(string source, string destination)
    {
        try
        {
            String[] files;
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar) destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            files = Directory.GetFileSystemEntries(source);
            foreach (string element in files)
            {
                if (Directory.Exists(element))
                    CopyFolder(element, destination + Path.GetFileName(element));
                else
                    File.Copy(element, destination + Path.GetFileName(element), true);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region 检测文件
    /// <summary>
    /// 判断是否为安全文件名
    /// </summary>
    /// <param name="str">文件名</param>
    public static bool IsSafeName(string strExtension)
    {
        strExtension = strExtension.ToLower();
        if (strExtension.LastIndexOf(".") >= 0)
        {
            strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
        }
        else
        {
            strExtension = ".txt";
        }
        string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png", ".rar", ".zip" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  判断是否为不安全文件名
    /// </summary>
    /// <param name="str">文件名、文件夹名</param>
    public static bool IsUnsafeName(string strExtension)
    {
        strExtension = strExtension.ToLower();
        if (strExtension.LastIndexOf(".") >= 0)
        {
            strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
        }
        else
        {
            strExtension = ".txt";
        }
        string[] arrExtension = { ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx", ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", ".mdf", ".resx", ".xsd" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  判断是否为可编辑文件
    /// </summary>
    /// <param name="str">文件名、文件夹名</param>
    public static bool IsCanEdit(string strExtension)
    {
        strExtension = strExtension.ToLower();

        if (strExtension.LastIndexOf(".") >= 0)
        {
            strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
        }
        else
        {
            strExtension = ".txt";
        }
        string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region GetPhysicalPath(获取物理路径)
    /// <summary>
    /// 获取物理路径
    /// </summary>
    /// <param name="relativePath">相对路径</param>
    /// <returns></returns>
    public static string GetPhysicalPath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return string.Empty;
        }
        if (HttpContext.Current == null)
        {
            if (relativePath.StartsWith("~"))
            {
                relativePath = relativePath.Remove(0, 2);
            }
            return Path.GetFullPath(relativePath);
        }
        if (relativePath.StartsWith("~"))
        {
            return HttpContext.Current.Server.MapPath(relativePath);
        }
        if (relativePath.StartsWith("/") || relativePath.StartsWith("\\"))
        {
            return HttpContext.Current.Server.MapPath("~" + relativePath);
        }
        return HttpContext.Current.Server.MapPath("~/" + relativePath);
    }
    #endregion

    #region IsFileExists(检查某个文件是否真的存在)
    /// <summary>
    /// 检查某个文件是否真的存在
    /// </summary>
    /// <param name="path">需要检查的文件的路径(包括路径的文件全名)</param>
    /// <returns>返回true则表示存在，false为不存在</returns>
    public static bool IsFileExists(string path)
    {

         return File.Exists(path);

    }
    #endregion

    #region IsDirectoryExists(检查文件目录是否真的存在)
    /// <summary>
    /// 检查文件目录是否真的存在
    /// </summary>
    /// <param name="path">需要检查的文件目录</param>
    /// <returns>返回true则表示存在，false为不存在</returns>
    public static bool IsDirectoryExists(string path)
    {
        try
        {
            return Directory.Exists(Path.GetDirectoryName(path));
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region FindLineTextFromFile(查找文件中是否存在匹配的内容)
    /// <summary>
    /// 查找文件中是否存在匹配的内容
    /// </summary>
    /// <param name="fileInfo">查找的文件流信息</param>
    /// <param name="lineTxt">在文件中需要查找的行文本</param>
    /// <param name="lowerUpper">是否区分大小写，true为区分，false为不区分</param>
    /// <returns>返回true则表示存在，false为不存在</returns>
    public static bool FindLineTextFromFile(FileInfo fileInfo, string lineTxt, bool lowerUpper = false)
    {
        bool isTrue = false; //表示没有查询到信息
        try
        {
            //首先判断文件是否存在
            if (fileInfo.Exists)
            {
                var streamReader = new StreamReader(fileInfo.FullName);
                do
                {
                    string readLine = streamReader.ReadLine(); //读取的信息
                    if (String.IsNullOrEmpty(readLine))
                    {
                        break;
                    }
                    if (lowerUpper)
                    {
                        if (readLine.Trim() != lineTxt.Trim())
                        {
                            continue;
                        }
                        isTrue = true;
                        break;
                    }
                    if (readLine.Trim().ToLower() != lineTxt.Trim().ToLower())
                    {
                        continue;
                    }
                    isTrue = true;
                    break;
                } while (streamReader.Peek() != -1);
                streamReader.Close(); //继承自IDisposable接口，需要手动释放资源
            }
        }
        catch (Exception)
        {
            isTrue = false;
        }
        return isTrue;
    }
    #endregion

    public const string FileKey = "ihlih*0037JOHT*)(PIJY*(()JI^)IO%"; //加密密钥

    #region FileEncryptInfo(对文件进行加密)
    /// <summary>
    /// 对文件进行加密
    /// 调用:FileEncryptHelper.FileEncryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
    /// </summary>
    /// <param name="fileOriginalPath">需要加密的文件路径</param>
    /// <param name="fileFinshPath">加密完成后存放的文件路径</param>
    /// <param name="fileKey">文件密钥</param>
    public static void FileEncryptInfo(string fileOriginalPath, string fileFinshPath, string fileKey)
    {
        //分组加密算法的实现
        using (var fileStream = new FileStream(fileOriginalPath, FileMode.Open))
        {
            var buffer = new Byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length); //得到需要加密的字节数组
            //设置密钥，密钥向量，两个一样，都是16个字节byte
            var rDel = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(fileKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryptoTransform = rDel.CreateEncryptor();
            byte[] cipherBytes = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
            using (var fileSEncrypt = new FileStream(fileFinshPath, FileMode.Create, FileAccess.Write))
            {
                fileSEncrypt.Write(cipherBytes, 0, cipherBytes.Length);
            }
        }
    }
    #endregion

    #region FileDecryptInfo(对文件进行解密)
    /// <summary>
    /// 对文件进行解密
    /// 调用:FileEncryptHelper.FileDecryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
    /// </summary>
    /// <param name="fileFinshPath">传递需要解密的文件路径</param>
    /// <param name="fileOriginalPath">解密后文件存放的路径</param>
    /// <param name="fileKey">密钥</param>
    public static void FileDecryptInfo(string fileFinshPath, string fileOriginalPath, string fileKey)
    {
        using (var fileStreamIn = new FileStream(fileFinshPath, FileMode.Open, FileAccess.Read))
        {
            using (var fileStreamOut = new FileStream(fileOriginalPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var rDel = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(fileKey),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (var cryptoStream = new CryptoStream(fileStreamOut, rDel.CreateDecryptor(),
                    CryptoStreamMode.Write))
                {
                    var bufferLen = 4096;
                    var buffer = new byte[bufferLen];
                    int bytesRead;
                    do
                    {
                        bytesRead = fileStreamIn.Read(buffer, 0, bufferLen);
                        cryptoStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }
    }
    #endregion

    #region GetSolutionPath(得到解决方案目录适用于命令行控制台)
    public static string GetSolutionPath()
    {
        // return AppDomain.CurrentDomain.SetupInformation.ApplicationBase

        return Path.GetFullPath(@"../../../");
    }
    #endregion
}