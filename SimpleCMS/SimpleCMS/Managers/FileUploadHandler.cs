using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public class FileUploadHandler
    {
        public string Folder { get; set; }
        public HttpServerUtilityBase Server { get; private set; }
        public HttpSessionStateBase Session { get; private set; }
        public bool UseSessionID { get; private set; }
        public static List<FileInfo> UploadedFiles = new List<FileInfo>();

        public FileUploadHandler(HttpServerUtilityBase _server, HttpSessionStateBase _session, bool _useSessionID = true, string _folder = null)
        {
            Folder = !string.IsNullOrEmpty(_folder) ? _folder : "~/App_Data/Files";
            //Folder = Path.Combine("~/App_Data/Files", !string.IsNullOrEmpty(_folder) ? _folder : "");
            //if (!Directory.Exists(Folder))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(Server.MapPath(Folder));
            //    }
            //    catch (Exception)
            //    {
            //        Folder = Path.Combine("~/App_Data/Files");
            //    }
            //}
            Server = _server;
            Session = _session;
            UseSessionID = _useSessionID;
            //UploadedFiles = new List<FileInfo>();
        }

        public bool Save(IEnumerable<Models.FileUploadInfo> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                UploadedFiles.Clear();
                foreach (var file in files)
                {
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var fileName = Path.GetFileName(file.FileName);
                        var sessionFolder = GetSessionFolder();
                        var physicalPath = Path.Combine(sessionFolder, fileName);
                        var fileInfo = new FileInfo(physicalPath);
                        if (fileInfo != null && fileInfo.Exists)
                        {
                            //file.SaveAs(physicalPath);
                            var fileData = System.IO.File.ReadAllBytes(fileInfo.FullName);
                            File.WriteAllBytes(physicalPath, fileData);
                        }
                        if (Path.GetExtension(file.FileName) == ".zip")
                        {
                            using (var zip = ZipFile.Read(physicalPath))
                            {
                                var outputDir = Path.Combine(sessionFolder, Path.GetFileNameWithoutExtension(file.FileName));
                                if (!Directory.Exists(outputDir))
                                {
                                    Directory.CreateDirectory(outputDir);
                                }
                                foreach (ZipEntry e in zip)
                                {
                                    e.Extract(outputDir, ExtractExistingFileAction.OverwriteSilently);
                                    UploadedFiles.Add(new FileInfo(Path.Combine(outputDir, e.FileName)));
                                }
                            }
                            try
                            {
                                File.Delete(physicalPath);
                            }
                            catch (Exception)
                            {
                                // todo: handle
                            }
                        }
                        else
                        {
                            UploadedFiles.Add(fileInfo);
                        }
                    }
                }
            }
            return true;
        }

        public bool Save(IEnumerable<HttpPostedFileBase> files)
        {
            var _files = new List<Models.FileUploadInfo>();
            foreach (var f in files)
            {
                // Some browsers send file names with full path.
                // We are only interested in the file name.
                var fileName = Path.GetFileName(f.FileName);
                var sessionFolder = GetSessionFolder();
                var physicalPath = Path.Combine(sessionFolder, fileName);
                var fileInfo = new FileInfo(physicalPath);
                f.SaveAs(physicalPath);
                _files.Add(new Models.FileUploadInfo()
                {
                    FileName = f.FileName,
                    Extension = Path.GetExtension(f.FileName),
                    UID = null
                });
            }
            return Save(_files);
            //// The Name of the Upload component is "files"
            //if (files != null)
            //{
            //    UploadedFiles.Clear();
            //    foreach (var file in files)
            //    {
            //        // Some browsers send file names with full path.
            //        // We are only interested in the file name.
            //        var fileName = Path.GetFileName(file.FileName);
            //        var sessionFolder = GetSessionFolder();
            //        var physicalPath = Path.Combine(sessionFolder, fileName);
            //        var fileInfo = new FileInfo(physicalPath);
            //        file.SaveAs(physicalPath);
            //        if (Path.GetExtension(file.FileName) == ".zip")
            //        {
            //            using (var zip = ZipFile.Read(physicalPath))
            //            {
            //                var outputDir = Path.Combine(sessionFolder, Path.GetFileNameWithoutExtension(file.FileName));
            //                if (!Directory.Exists(outputDir))
            //                {
            //                    Directory.CreateDirectory(outputDir);
            //                }
            //                foreach (ZipEntry e in zip)
            //                {
            //                    e.Extract(outputDir, ExtractExistingFileAction.OverwriteSilently);
            //                    UploadedFiles.Add(new FileInfo(Path.Combine(outputDir, e.FileName)));
            //                }
            //            }
            //            try
            //            {
            //                File.Delete(physicalPath);
            //            }
            //            catch (Exception)
            //            {
            //                // todo: handle
            //            }
            //        }
            //        else
            //        {
            //            UploadedFiles.Add(fileInfo);
            //        }
            //    }
            //}
            //return true;
        }

        public bool Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var sessionFolder = GetSessionFolder();
                    var physicalPath = Path.Combine(sessionFolder, fileName);
                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            return true;
        }

        public string GetFilePath(string name)
        {
            var fileName = Path.GetFileName(name);
            var physicalPath = Path.Combine(Server.MapPath(Folder), fileName);
            return physicalPath;
        }

        public string GetSessionFolder()
        {
            //var sessionOrder = SessionHandler.Instance.Read(Session, Request);
            //if (sessionOrder != null)
            //{
            try
            {
                var sessionFolder = UseSessionID ? Path.Combine(Server.MapPath(Folder), Session.SessionID) : Server.MapPath(Folder);
                Directory.CreateDirectory(sessionFolder);
                return sessionFolder;
            }
            catch (Exception)
            {
                // todo: handle
            }
            //}
            return Server.MapPath(Folder);
        }

        public string GetFilePhysicalPath(string fileName)
        {
            var sessionFolder = GetSessionFolder();
            return Path.Combine(sessionFolder, fileName);
        }
    }
}