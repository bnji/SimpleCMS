using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
using Newtonsoft.Json;
using System.Drawing;

namespace BootstrapControllers
{
    public class ImageController : Controller<ImageInfo>
    {
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            if (fh.Save(Files))
            {
                //var files = AddFiles(vmObj.Files);
                foreach (var fileInfo in FileUploadHandler.UploadedFiles)
                {
                    var file = new SimpleCMS.Models.File()
                    {
                        Name = fileInfo.Name,
                        Length = fileInfo.Length,
                        ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                        Data = System.IO.File.ReadAllBytes(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", fileInfo.Name))),
                        ChangeEvent = CreateChangeEvent()
                    };
                    var dbObj = new ImageInfo()
                    {
                        Width = -1,
                        Height = -1,
                        Description = string.Empty,
                        File = file,
                        ChangeEvent = CreateChangeEvent()
                    };
                    using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)), FileMode.Open, FileAccess.Read))
                    {
                        using (Image img = Image.FromStream(stream: fs,
                                                            useEmbeddedColorManagement: false,
                                                            validateImageData: false))
                        {
                            float width = img.PhysicalDimension.Width;
                            float height = img.PhysicalDimension.Height;
                            //float hresolution = img.HorizontalResolution;
                            //float vresolution = img.VerticalResolution;
                            dbObj.Width = (int)width;
                            dbObj.Height = (int)height;
                        }
                        fs.Close();
                    }
                    db.Files.Add(file);
                    db.Images.Add(dbObj);
                    db.SaveChanges();
                }
                return Content("");
            }
            return Content("Error");
        }

        public ActionResult RemoveFile(string[] fileNames)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            if (fh.Remove(fileNames))
            {
                return Content("");
            }
            return Content("Error");
        }

        //List<SimpleCMS.Models.File> AddFiles(IEnumerable<FileUploadInfo> fileUploadInfo)
        //{
        //    var files = new List<SimpleCMS.Models.File>();

        //    if (fileUploadInfo == null)
        //        return files;

        //    byte[] image = null;
        //    FileInfo imageInfo = null;
        //    foreach (var item in fileUploadInfo)
        //    {
        //        ReadFileData(item, out image, out imageInfo);
        //        if (image != null && imageInfo != null)
        //        {
        //            var file = new SimpleCMS.Models.File()
        //            {
        //                Name = imageInfo.Name,
        //                ContentType = MimeMapping.GetMimeMapping(imageInfo.FullName),
        //                Length = imageInfo.Length,
        //                Data = image,
        //                ChangeEvent = CreateChangeEvent()
        //            };
        //            files.Add(file);
        //        }
        //    }
        //    return files;
        //}

        //private void ReadFileData(FileUploadInfo o, out byte[] file, out FileInfo fileInfo)
        //{
        //    var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
        //    file = null;
        //    fileInfo = null;
        //    if (o != null && o.Name != null)
        //    {
        //        var filePath = fh.GetFilePhysicalPath(o.Name);
        //        fileInfo = new FileInfo(filePath);
        //        file = System.IO.File.ReadAllBytes(filePath);
        //    }
        //}

        protected override FileUploadHandler CreateFileUploadHandler()
        {
            return new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
        }

        protected override void AddViewBag(ImageInfo obj)
        {

        }

        private void AddViewBagCreateEdit(ImageInfo obj)
        {
            //ViewBag.CountryId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
            //{
            //    Text = m.Code + " - " + m.Name,
            //    Value = m.Id.ToString()
            //}), "Value", "Text", obj != null ? new Nullable<int>(obj.CountryId) : null);
        }


        //public override ActionResult Read([DataSourceRequest] DataSourceRequest request, int? id)
        //{
        //    var filter = "";
        //    if (request.Filters.Count > 0)
        //    {
        //        filter = (request.Filters[0] as Kendo.Mvc.FilterDescriptor).Value as string;
        //    }
        //    request.Filters.Clear();
        //    return Json(GetFiltered(filter, request));
        //}

        //private DataSourceResult GetFiltered(string filter, DataSourceRequest request)
        //{
        //    var filtered = GetFiltered(db.Images, filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new ImageInfoViewModel()
        //    {
        //        Id = m.Id,
        //        Width = m.Width,
        //        Height = m.Height,
        //        Description = m.Description,
        //        Name = m.File != null ? m.File.Name : string.Empty,
        //        Length = m.File != null ? m.File.Length : -1,
        //        ContentType = m.File != null ? m.File.ContentType : string.Empty
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<ImageInfo> GetFiltered(IQueryable<ImageInfo> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<ImageInfo>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    ("" + s.Description).ToLower().Contains(filter.ToLower())
                    || (s.File != null && ("" + s.File.Name).ToLower().Contains(filter.ToLower()))
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Images.FilterDeleted().Select(m => new ImageInfoViewModel()
        //    {
        //        Id = m.Id,
        //        Width = m.Width,
        //        Height = m.Height,
        //        Description = m.Description,
        //        Name = m.File != null ? m.File.Name : string.Empty,
        //        Length = m.File != null ? m.File.Length : -1,
        //        ContentType = m.File != null ? m.File.ContentType : string.Empty
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        protected override jQueryDataTableResultModel<ImageInfo> GetData(jQueryDataTableParamModel param, Func<IList<ImageInfo>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<ImageInfo>().FilterDeleted().ToList();
            IList<ImageInfo> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<ImageInfo>()
                    .FilterDeleted()
                    .Where(s =>
                    ("" + s.Description).ToLower().Contains(param.sSearch.ToLower())
                    || (s.File != null && ("" + s.File.Name).ToLower().Contains(param.sSearch.ToLower()))
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    )
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<ImageInfo>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new ImageInfoViewModel()
                {
                    Id = m.Id,
                    Width = m.Width,
                    Height = m.Height,
                    Description = m.Description,
                    Name = m.File != null ? m.File.Name : string.Empty,
                    Length = m.File != null ? m.File.Length : -1,
                    ContentType = m.File != null ? m.File.ContentType : string.Empty
                })
            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new ImageInfoCreateViewModel()
            {
                Files = new List<FileUploadInfo>()
            };
            return Create<ImageInfo>("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImageInfoCreateViewModel vmObj)
        {
            ActionResult result = null;
            var files = AddFiles(vmObj.Files);
            foreach (var file in files)
            {
                result = CreateUsingViewModel(vmObj, (parent) =>
                {
                    var dbObj = new ImageInfo()
                    {
                        Width = -1,
                        Height = -1,
                        Description = string.Empty,
                        ChangeEvent = CreateChangeEvent()
                    };
                    using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)), FileMode.Open, FileAccess.Read))
                    {
                        using (Image img = Image.FromStream(stream: fs,
                                                            useEmbeddedColorManagement: false,
                                                            validateImageData: false))
                        {
                            float width = img.PhysicalDimension.Width;
                            float height = img.PhysicalDimension.Height;
                            //float hresolution = img.HorizontalResolution;
                            //float vresolution = img.VerticalResolution;
                            dbObj.Width = (int)width;
                            dbObj.Height = (int)height;
                        }
                        fs.Close();
                    }
                    //var imageInfoFile = new ImageInfoFile()
                    //{
                    //    ImageInfo = dbObj,
                    //    File = file
                    //};
                    //dbObj.ImageInfoFile = imageInfoFile;
                    db.Files.Add(file);
                    //db.ImageInfoFiles.Add(imageInfoFile);
                    db.Images.Add(dbObj);
                    return dbObj;
                }, null, "Create");
            }
            return result;
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<ImageInfo>().Where(x => x.Id == id).FirstOrDefault();
            ViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new ImageInfoEditViewModel()
                {
                    //Width = o.Width,
                    //Height = o.Height,
                    Description = o.Description
                    //Images = new List<FileUploadInfo>()
                };
                //if (o != null && o.ImageFile != null && o.ImageFile.File.Length > 0)
                //{
                //    var images = new List<FileUploadInfo>();
                //    var fileUploadInfo = new FileUploadInfo()
                //    {
                //        Name = o.ImageFile.File.Name,
                //        Extension = "",
                //        UID = ""
                //    };
                //    images.Add(fileUploadInfo);
                //    ((ImageInfoEditViewModel)viewModel).Images = images;
                //}
            }
            return Edit<ImageInfo>("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageInfoEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<ImageInfo>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                //dbObj.Width = vmObj.Width;
                //dbObj.Height = vmObj.Height;
                dbObj.Description = vmObj.Description;

                //var imageInfoFile = new ImageInfoFile()
                //{
                //    ImageInfo = dbObj,
                //    File = file
                //};
                //db.Files.Add(file);
                //db.ImageInfoFiles.Add(imageInfoFile);
                //db.Images.Add(dbObj);

                //var files = AddFiles(vmObj.Files);
                //foreach (var file in files)
                //{
                //    var ImageInfoFile = new ImageInfoFile()
                //    {
                //        ImageInfo = dbObj,
                //        File = file,
                //    };
                //    db.Files.Add(file);
                //    db.ImageInfoFiles.Add(ImageInfoFile);
                //    dbObj.Files.Add(ImageInfoFile);
                //}
                return dbObj;
            }, null, "Edit");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<ImageInfo>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ImageInfoViewModel dbObj)
        {
            var imageInfo = db.Images.Where(x => x.Id == dbObj.Id).FirstOrDefault();
            if (imageInfo != null)
            {
                try
                {
                    //System.IO.File.Delete(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", imageInfo.File.Name)));
                    DeleteObject<SimpleCMS.Models.File, ImageInfo>(imageInfo.File.Id);
                    DeleteObject<ImageInfo, ImageInfo>(imageInfo.Id);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new EmptyResult();// DeleteObject<ImageInfo, ImageInfo>(dbObj.Id);
        }

        [HttpGet]
        public ActionResult GetFile(int id)
        {
            //var dir = Server.MapPath("/Images");
            //var path = Path.Combine(dir, id + ".jpg");
            var file = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (file != null)
            {
                return base.File(file.Data, file.ContentType);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveFileAsync(int id, string data)
        {
            var file = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (file != null)
            {
                try
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(data);
                    System.IO.File.WriteAllBytes(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)), bytes);
                    file.Data = bytes;
                    file.Length = bytes.Length;
                    file.ChangeEvent = UpdateChangeEvent(file.ChangeEventId);
                    db.Entry(file).State = EntityState.Modified;
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteFileAsync(int id)
        {
            var imageInfo = db.Images.Where(x => x.Id == id).FirstOrDefault();
            if (imageInfo != null)
            {
                try
                {
                    //System.IO.File.Delete(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", imageInfo.File.Name)));
                    DeleteObject<SimpleCMS.Models.File, ImageInfo>(imageInfo.File.Id);
                    DeleteObject<ImageInfo, ImageInfo>(imageInfo.File.Id);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new EmptyResult();

            //var ImageInfoFile = db.ImageInfoFiles.Where(x => x.Id == id).FirstOrDefault();
            //var file = ImageInfoFile != null ? db.Files.Where(x => x.Id == ImageInfoFile.FileId).FirstOrDefault() : null;
            //if (ImageInfoFile != null && file != null)
            //{
            //    try
            //    {
            //        System.IO.File.Delete(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)));
            //        db.Files.Remove(file);
            //        db.ImageInfoFiles.Remove(ImageInfoFile);
            //        db.SaveChanges();
            //        return new HttpStatusCodeResult(HttpStatusCode.OK);
            //    }
            //    catch (Exception)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            //}
            //return new EmptyResult();
        }
    }
}