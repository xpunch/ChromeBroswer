using System.Collections.Generic;
using System.IO;

namespace Common.Utils
{
    /// <summary>
    ///     目录操作的帮助类
    /// </summary>
    public class DirectoryHelper
    {
        /// <summary>
        ///     返回指定目录下所有文件信息
        /// </summary>
        /// <param name="strDirectory">目录字符串</param>
        /// <returns></returns>
        public static List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            var listFiles = new List<FileInfo>(); //保存所有的文件信息
            var directory = new DirectoryInfo(strDirectory);
            var directoryArray = directory.GetDirectories();
            var fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (var directoryInfo in directoryArray)
            {
                //递归遍历
                var subFiles = GetAllFilesInDirectory(directoryInfo.FullName);
                listFiles.AddRange(subFiles);
            }
            return listFiles;
        }

        /// <summary>
        ///     从一个目录将其内容移动到另一目录
        /// </summary>
        /// <param name="directorySource">源目录</param>
        /// <param name="directoryTarget">目的目录</param>
        public void MoveFolderTo(string directorySource, string directoryTarget)
        {
            //检查是否存在目的目录
            if (!Directory.Exists(directoryTarget))
            {
                Directory.CreateDirectory(directoryTarget);
            }
            //先来移动文件
            var directoryInfo = new DirectoryInfo(directorySource);
            var files = directoryInfo.GetFiles();
            //移动所有文件
            foreach (var file in files)
            {
                //如果自身文件在运行，不能直接覆盖，需要重命名之后再移动
                if (File.Exists(Path.Combine(directoryTarget, file.Name)))
                {
                    if (File.Exists(Path.Combine(directoryTarget, file.Name + ".bak")))
                    {
                        File.Delete(Path.Combine(directoryTarget, file.Name + ".bak"));
                    }
                    File.Move(Path.Combine(directoryTarget, file.Name),
                        Path.Combine(directoryTarget, file.Name + ".bak"));
                }
                file.MoveTo(Path.Combine(directoryTarget, file.Name));
            }
            //最后移动目录
            var directoryInfoArray = directoryInfo.GetDirectories();
            foreach (var dir in directoryInfoArray)
            {
                MoveFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
            }
        }

        /// <summary>
        ///     从一个目录将其内容复制到另一目录
        /// </summary>
        /// <param name="directorySource">源目录</param>
        /// <param name="directoryTarget">目的目录</param>
        public void CopyFolderTo(string directorySource, string directoryTarget)
        {
            //检查是否存在目的目录
            try
            {
                if (Directory.Exists(directoryTarget))
                {
                    Directory.Delete(directoryTarget, true);
                }
            }
            catch
            {
                //This will not infulence the next step
            }
            if (!Directory.Exists(directoryTarget))
            {
                Directory.CreateDirectory(directoryTarget);
            }
            //先来复制文件
            var directoryInfo = new DirectoryInfo(directorySource);
            var files = directoryInfo.GetFiles();
            //复制所有文件
            foreach (var file in files)
            {
                file.CopyTo(Path.Combine(directoryTarget, file.Name));
            }
            //最后复制目录
            var directoryInfoArray = directoryInfo.GetDirectories();
            foreach (var dir in directoryInfoArray)
            {
                CopyFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
            }
        }

        /// <summary>
        ///     删除目录
        /// </summary>
        public static void DeleteDirectory(string directoryPath)
        {
            try
            {
                var files = GetAllFilesInDirectory(directoryPath);
                foreach (var file in files)
                {
                    file.Attributes = FileAttributes.Normal;
                }
                Directory.Delete(directoryPath, true);
            }
            catch
            {
                //do nothing
            }
        }
    }
}