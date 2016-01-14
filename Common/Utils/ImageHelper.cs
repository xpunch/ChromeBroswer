using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using log4net;

namespace Common.Utils
{
    /// <summary>
    /// 图片处理的帮助类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// Logo目录
        /// </summary>
        private static readonly string _iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Icons/");

        /// <summary>
        /// 存放临时Logo的目录
        /// </summary>
        private static readonly string _tempIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Icons/Temp");

        /// <summary>
        /// Logo目录
        /// </summary>
        public static string IconPath
        {
            get
            {
                if (!Directory.Exists(_iconPath))
                {
                    Directory.CreateDirectory(_iconPath);
                }
                return _iconPath;
            }
        }

        /// <summary>
        /// 存放临时Logo的目录
        /// </summary>
        public static string TempIconPath
        {
            get
            {
                if (!Directory.Exists(_tempIconPath))
                {
                    Directory.CreateDirectory(_tempIconPath);
                }
                return _tempIconPath;
            }
        } 

        public const string FavIconFile = "favicon.ico";

        public const string DefaultIconFile = "defaultIcon.png";

        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public byte[] GetPictureData(string imagepath)
        {
            ////根据图片文件的路径使用文件流打开，并保存为byte[] 
            var fs = new FileStream(imagepath, FileMode.Open); //可以是其他重载方法 
            var byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }

        public bool DownloadImage(string url, string savePath)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = request.GetResponse().GetResponseStream();
                if (response != null)
                {
                    var img = Image.FromStream(response);
                    img.Save(savePath);
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggor.Error(exception);
            }

            return false;
        }
    }
}