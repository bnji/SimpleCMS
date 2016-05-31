using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleCMS.Models;
using SimpleCMS.DAL;
using SimpleCMS.ViewModels;
using SimpleCMS;
using SimpleCMS.Models.Blog;
using System.IO;
using SimpleCMS.Models.KendoEditor;

namespace BootstrapControllers
{
    public class ImageBrowserController : Controller<ImageInfo>
    {
        private const string contentFolderRoot = "~/SimpleCMS/Data/";
        private const string prettyName = "Images/";
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private readonly SimpleCMS.Models.KendoEditor.DirectoryBrowser directoryBrowser;
        private readonly SimpleCMS.Models.KendoEditor.ThumbnailCreator thumbnailCreator;

        public ImageBrowserController()
        {
            directoryBrowser = new SimpleCMS.Models.KendoEditor.DirectoryBrowser();
            thumbnailCreator = new SimpleCMS.Models.KendoEditor.ThumbnailCreator();
        }

        public string ContentPath
        {
            get
            {
                return Path.Combine(contentFolderRoot, prettyName);
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }

        public virtual JsonResult GetPictures(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeRead(path))
            {
                try
                {
                    directoryBrowser.Server = Server;
                    var result = new List<SimpleCMS.Models.KendoEditor.FileBrowserEntry>();
                    using (var db = new BootstrapContext())
                    {
                        var images = db.Images;
                        var imagesFiltered = images.FilterDeleted().ToList();
                        var imagesSelected = imagesFiltered.Select(m => new
                        {
                            name = m.File != null ? m.File.Name : string.Empty,
                            size = m.File != null ? m.File.Length : -1
                        });
                        foreach (var image in imagesSelected)
                        {
                            var filePath = Path.Combine(Server.MapPath(path), image.name);
                            if (System.IO.File.Exists(filePath))
                            {
                                result.Add(new SimpleCMS.Models.KendoEditor.FileBrowserEntry()
                                {
                                    Name = image.name,
                                    Size = image.size,
                                    Type = EntryType.File
                                });
                            }
                        }
                    }
                    return Json(result.Select(m => new
                    {
                        name = m.Name,
                        size = m.Size,
                        type = "f"
                    }), JsonRequestBehavior.AllowGet);
                    //var result = directoryBrowser
                    //    .GetContent(path, DefaultFilter)
                    //    .Select(f => new
                    //    {
                    //        name = f.Name,
                    //        type = f.Type == EntryType.File ? "f" : "d",
                    //        size = f.Size
                    //    });
                    //return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new HttpException(404, "File Not Found");
                }
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeThumbnail(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    Response.AddFileDependency(physicalPath);

                    return CreateThumbnail(physicalPath);
                }
                else
                {
                    throw new HttpException(404, "File Not Found");
                }
            }
            else
            {
                throw new HttpException(403, "Forbidden");
            }
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new SimpleCMS.Models.KendoEditor.ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/png";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            path = NormalizePath(path);

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = CombinePaths(path, name);
                if (type.ToLowerInvariant() == "f")
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(null);
            }
            throw new HttpException(404, "File Not Found");
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, SimpleCMS.Models.KendoEditor.FileBrowserEntry entry)
        {
            path = NormalizePath(path);
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            {
                var physicalPath = Path.Combine(Server.MapPath(path), name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(null);
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                var filePath = Path.Combine(Server.MapPath(path), fileName);
                file.SaveAs(filePath);

                var fileInfo = new FileInfo(filePath);
                //var bytes = System.Text.Encoding.Default.GetBytes(filein  );
                var bytes = System.IO.File.ReadAllBytes(filePath);

                var imageInfo = new ImageInfo()
                {
                    Width = -1,
                    Height = -1,
                    Description = string.Empty,
                    ChangeEvent = CreateChangeEvent()
                };
                using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", fileInfo.Name)), FileMode.Open, FileAccess.Read))
                {
                    using (var img = System.Drawing.Image.FromStream(stream: fs,
                                                        useEmbeddedColorManagement: false,
                                                        validateImageData: false))
                    {
                        float width = img.PhysicalDimension.Width;
                        float height = img.PhysicalDimension.Height;
                        //float hresolution = img.HorizontalResolution;
                        //float vresolution = img.VerticalResolution;
                        imageInfo.Width = (int)width;
                        imageInfo.Height = (int)height;
                    }
                    fs.Close();
                }
                var f = new SimpleCMS.Models.File()
                {
                    Name = fileInfo.Name,
                    Length = fileInfo.Length,
                    Data = bytes,
                    ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                    ChangeEvent = CreateChangeEvent()
                };
                db.Files.Add(f);
                db.Images.Add(imageInfo);
                db.SaveChanges();

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            throw new HttpException(403, "Forbidden");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeImage(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    const string contentType = "image/png";
                    return File(System.IO.File.OpenRead(physicalPath), contentType);
                }
            }

            throw new HttpException(403, "Forbidden");
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }

        protected override void AddViewBag(ImageInfo obj)
        {
            throw new NotImplementedException();
        }
    }
}
