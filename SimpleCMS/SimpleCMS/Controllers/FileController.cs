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

namespace BootstrapControllers
{
    public class FileController : Controller<SimpleCMS.Models.File>
    {
        public override ActionResult Index()
        {
            return View(db.Set<SimpleCMS.Models.File>().Where(x => x.ContentType.StartsWith("image/")).ToList());
        }

        public ActionResult Upload()
        {
            return View();
        }

        protected override string GetFilePath()
        {
            return "~/SimpleCMS/Data/Images";
        }


        public ActionResult RemoveFile(string[] FileNames)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            if (fh.Remove(FileNames))
            {
                return Content("");
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            if (fh.Save(Files))
            {
                byte[] data = null;
                FileInfo fileInfo = null;
                foreach (var item in Files)
                {
                    ReadFileData(new FileUploadInfo() { FileName = Path.GetFileName(item.FileName), Extension = Path.GetExtension(item.FileName), UID = null }, out data, out fileInfo);
                    if (data != null && fileInfo != null)
                    {
                        var file = new SimpleCMS.Models.File()
                        {
                            Name = fileInfo.Name,
                            ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                            Length = fileInfo.Length,
                            Data = data,
                            ChangeEvent = CreateChangeEvent()
                        };
                        db.Files.Add(file);
                    }
                }
                db.SaveChanges();
                return Content("");
            }
            return Content("Error");
        }

        List<SimpleCMS.Models.File> AddFiles(IEnumerable<SimpleCMS.Models.FileUploadInfo> FileUploadInfo)
        {
            var Files = new List<SimpleCMS.Models.File>();

            if (FileUploadInfo == null)
                return Files;

            byte[] image = null;
            FileInfo fileInfo = null;
            foreach (var item in FileUploadInfo)
            {
                ReadFileData(item, out image, out fileInfo);
                if (image != null && fileInfo != null)
                {
                    var File = new SimpleCMS.Models.File()
                    {
                        Name = fileInfo.Name,
                        ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                        Length = fileInfo.Length,
                        Data = image,
                        ChangeEvent = CreateChangeEvent()
                    };
                    Files.Add(File);
                }
            }
            return Files;
        }

        private void ReadFileData(SimpleCMS.Models.FileUploadInfo o, out byte[] File, out FileInfo FileInfo)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            File = null;
            FileInfo = null;
            if (o != null && o.FileName != null)
            {
                var FilePath = fh.GetFilePhysicalPath(o.FileName);
                FileInfo = new FileInfo(FilePath);
                File = System.IO.File.ReadAllBytes(FilePath);
            }
        }

        protected override void AddViewBag(SimpleCMS.Models.File obj)
        {

        }

        private void AddViewBagCreateEdit(SimpleCMS.Models.File obj)
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
        //    var filtered = GetFiltered(db.Set<SimpleCMS.Models.File>(), filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new FileViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Data = m.Data,
        //        ContentType = m.ContentType,
        //        Length = m.Length,
        //        ChangeEvent = m.ChangeEvent
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<SimpleCMS.Models.File> GetFiltered(IQueryable<SimpleCMS.Models.File> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<SimpleCMS.Models.File>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    ("" + s.Name).ToLower().Contains(filter.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Set<SimpleCMS.Models.File>().Select(m => new FileViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Data = m.Data,
        //        ContentType = m.ContentType,
        //        Length = m.Length,
        //        ChangeEvent = m.ChangeEvent
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    IViewModelBase viewModel = null;
        //    AddViewBagCreateEdit(null);
        //    viewModel = new FileCreateOrEditViewModel()
        //    {

        //    };
        //    return Create<SimpleCMS.Models.File>("CreateOrEdit", viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(FileCreateOrEditViewModel vmObj)
        //{
        //    return CreateUsingViewModel(vmObj, (parent) =>
        //    {
        //        //Convert the ViewModel to DB Object (Model)
        //        var dbObj = new SimpleCMS.Models.File()
        //        {
        //            Name = vmObj.Name,
        //            Data = vmObj.Data,
        //            ContentType = vmObj.ContentType,
        //            Length = vmObj.Length,
        //            ChangeEvent = vmObj.ChangeEvent
        //        };
        //        var Files = AddFiles(vmObj.Files);
        //        foreach (var File in Files)
        //        {
        //            var FileFile = new FileFile()
        //            {
        //                File = dbObj,
        //                File = File
        //            };
        //            db.Files.Add(File);
        //            dbObj.Files.Add(File);
        //        }
        //        return dbObj;
        //    });
        //}

        //[HttpGet]
        //public ActionResult Edit(int? id)
        //{
        //    var o = db.Set<SimpleCMS.Models.File>().Where(x => x.Id == id).FirstOrDefault();
        //    ViewModelBase viewModel = null;
        //    if (o != null)
        //    {
        //        AddViewBag(o);
        //        viewModel = new SimpleCMS.Models.FileCreateOrEditViewModel()
        //        {
        //            Name = o.Name,
        //            Description = o.Description
        //        };
        //        var SimpleCMS.Models.Files = new List<SimpleCMS.Models.FileUploadInfo>();
        //        foreach (var f in o.SimpleCMS.Models.Files)
        //        {
        //            var image = db.SimpleCMS.Models.Files.Where(x => x.Id == f.SimpleCMS.Models.FileId).FirstOrDefault();
        //            if (image != null && image.Length > 0)
        //            {
        //                var SimpleCMS.Models.FileUploadInfo = new SimpleCMS.Models.FileUploadInfo()
        //                {
        //                    Name = image.Name,
        //                    Extension = "",
        //                    UID = ""
        //                };
        //                SimpleCMS.Models.Files.Add(SimpleCMS.Models.FileUploadInfo);
        //            }
        //        }
        //        ((SimpleCMS.Models.FileCreateOrEditViewModel)viewModel).SimpleCMS.Models.Files = SimpleCMS.Models.Files;
        //    }
        //    return Edit<SimpleCMS.Models.File>("CreateOrEdit", viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(FileCreateOrEditViewModel vmObj)
        //{
        //    return UpdateUsingViewModel(vmObj, (o) =>
        //    {
        //        //Convert the ViewModel to DB Object (Model)
        //        var dbObj = db.Set<SimpleCMS.Models.File>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
        //        //dbObj.SimpleCMS.Models.Files.AddRange(AddImages(vmObj.Image));
        //        var Files = AddFiles(vmObj.Files);
        //        foreach (var File in Files)
        //        {
        //            db.Files.Add(File);
        //            dbObj.Files.Add(FileFile);
        //        }
        //        return dbObj;
        //    });
        //}

        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    var obj = db.Set<SimpleCMS.Models.File>().Find(id);
        //    return Delete("Delete", obj);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(FileViewModel dbObj)
        //{
        //    return DeleteObject<SimpleCMS.Models.File, SimpleCMS.Models.File>(dbObj.Id);
        //}

        [HttpGet]
        public ActionResult GetFile(int id)
        {
            //var dir = Server.MapPath("/Images");
            //var path = Path.Combine(dir, id + ".jpg");
            var File = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (File != null)
            {
                return base.File(File.Data, File.ContentType);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveFileAsync(int id, string data)
        {
            var File = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (File != null)
            {
                try
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(data);
                    System.IO.File.WriteAllBytes(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", File.Name)), bytes);
                    File.Data = bytes;
                    File.Length = bytes.Length;
                    File.ChangeEvent = UpdateChangeEvent(File.ChangeEventId);
                    db.Entry(File).State = EntityState.Modified;
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
            var file = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (file != null)
            {
                try
                {
                    var filePath = Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    db.Files.Remove(file);
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
    }
}