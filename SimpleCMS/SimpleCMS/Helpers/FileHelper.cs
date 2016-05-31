using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.IO
{
    public class FileHelper
    {
        public static List<FileInfo> GetFiles(string path)
        {
            return GetFiles(new DirectoryInfo(path), null);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo dirInfo, List<FileInfo> _files)
        {
            if (_files == null)
            {
                _files = new List<FileInfo>();
            }
            foreach (var file in dirInfo.GetFiles())
            {
                try
                {
                    _files.Add(file);
                }
                catch (Exception)
                {
                    //Console.WriteLine("Could not read file " + file.Name + " reason:" + e.Message);
                }
            }
            foreach (var dir in dirInfo.GetDirectories())
            {
                GetFiles(new DirectoryInfo(dir.FullName), _files);
            }
            return _files;
        }
    }
}
