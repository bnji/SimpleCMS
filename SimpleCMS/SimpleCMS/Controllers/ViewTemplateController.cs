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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace BootstrapControllers
{
    public class ViewTemplateController : Controller<ViewTemplate>
    {
        public ActionResult Upload()
        {
            return View();
        }

        protected override string GetFilePath()
        {
            return "~/Views/Shared/BlogViewTemplates";
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/Views/Shared/BlogViewTemplates");
            if (fh.Save(Files))
            {
                return Content("");
            }
            return Content("Error");
        }

        public ActionResult RemoveFile(string[] fileNames)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/Views/Shared/BlogViewTemplates");
            if (fh.Remove(fileNames))
            {
                return Content("");
            }
            return Content("Error");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddFileToObject(ViewTemplateViewModel viewTemplate)
        {
            if (viewTemplate != null)
            {
                var dbObj = db.Set<ViewTemplate>().Where(x => x.Id == viewTemplate.Id).FirstOrDefault();
                if (dbObj != null)
                {
                    var files = AddFiles(viewTemplate.Files);
                    //var file = files.Where(x => !string.IsNullOrEmpty(x.Name)).FirstOrDefault();
                    //if (file != null)
                    //{
                    SimpleCMS.Models.File retFile = null;
                    foreach (var file in files)
                    {
                        var viewTemplateFile = new ViewTemplateFile()
                        {
                            ViewTemplate = dbObj,
                            File = file,
                        };
                        db.Files.Add(file);
                        db.ViewTemplateFiles.Add(viewTemplateFile);
                        dbObj.Files.Add(viewTemplateFile);
                        retFile = file;
                        //}
                        try
                        {
                            db.SaveChanges();
                            //return new HttpStatusCodeResult(HttpStatusCode.OK);
                            //Response.ContentType = file.ContentType;
                            //Response.Headers.Add("ContentType", file.ContentType);
                            //Response.Headers.Add("ContentLength", "" + file.Length);
                            //return base.File(file.Data, file.ContentType);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    return Json(retFile);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        List<SimpleCMS.Models.File> AddFiles(IEnumerable<FileUploadInfo> fileUploadInfo)
        {
            var files = new List<SimpleCMS.Models.File>();

            if (fileUploadInfo == null)
                return files;

            byte[] data = null;
            FileInfo fileInfo = null;
            foreach (var item in fileUploadInfo)
            {
                ReadFileData(item, out data, out fileInfo);
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
                    files.Add(file);
                }
            }
            return files;
        }

        private void ReadFileData(FileUploadInfo o, out byte[] file, out FileInfo fileInfo)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/Views/Shared/BlogViewTemplates");
            file = null;
            fileInfo = null;
            if (o != null && o.FileName != null)
            {
                var filePath = fh.GetFilePhysicalPath(o.FileName);
                fileInfo = new FileInfo(filePath);
                file = System.IO.File.ReadAllBytes(filePath);
            }
        }

        protected override void AddViewBag(ViewTemplate obj)
        {

        }

        private void AddViewBagCreateEdit(ViewTemplate obj)
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
        //    var filtered = GetFiltered(db.Set<ViewTemplate>(), filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new ViewTemplateViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        PageContentType = m.PageContentType,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<ViewTemplate> GetFiltered(IQueryable<ViewTemplate> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<ViewTemplate>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    ("" + s.Name).ToLower().Contains(filter.ToLower())
                    || ("" + s.Description).ToLower().Contains(filter.ToLower())
                    || (s.PageContentType != null && ("" + s.PageContentType.Name).ToLower().Contains(filter.ToLower()))
                    || (s.PageContentType != null && ("" + s.PageContentType.Description).ToLower().Contains(filter.ToLower()))
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Set<ViewTemplate>().Select(m => new ViewTemplateViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        PageContentType = m.PageContentType,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        protected override jQueryDataTableResultModel<ViewTemplate> GetData(jQueryDataTableParamModel param, Func<IList<ViewTemplate>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<ViewTemplate>().FilterDeleted().ToList();
            IList<ViewTemplate> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<ViewTemplate>()
                    .FilterDeleted()
                    .Where(s =>
                    ("" + s.Name).ToLower().Contains(param.sSearch.ToLower())
                    || ("" + s.Description).ToLower().Contains(param.sSearch.ToLower())
                    || (s.PageContentType != null && ("" + s.PageContentType.Name).ToLower().Contains(param.sSearch.ToLower()))
                    || (s.PageContentType != null && ("" + s.PageContentType.Description).ToLower().Contains(param.sSearch.ToLower()))
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    )
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<ViewTemplate>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new ViewTemplateViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    PageContentType = m.PageContentType,
                    Description = m.Description
                })
            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new ViewTemplateCreateOrEditViewModel()
            {
                Files = new List<FileUploadInfo>()
            };
            return Create<ViewTemplate>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewTemplateCreateOrEditViewModel vmObj)
        {
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new ViewTemplate()
                {
                    //Id = vmObj.Id,
                    //Name = vmObj.Name,
                    Description = vmObj.Description,
                    PageContentTypeId = vmObj.PageContentTypeId
                };
                var files = AddFiles(vmObj.Files);
                foreach (var file in files)
                {
                    var ViewTemplateFile = new ViewTemplateFile()
                    {
                        ViewTemplate = dbObj,
                        File = file
                    };
                    db.Files.Add(file);
                    db.ViewTemplateFiles.Add(ViewTemplateFile);
                    dbObj.Files.Add(ViewTemplateFile);
                    dbObj.Name = Path.GetFileNameWithoutExtension(file.Name);
                }
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<ViewTemplate>().Where(x => x.Id == id).FirstOrDefault();
            ViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new ViewTemplateCreateOrEditViewModel()
                {
                    Name = o.Name,
                    Description = o.Description,
                    PageContentTypeId = o.PageContentTypeId,
                    PageContentTypeName = o.PageContentType != null ? o.PageContentType.Name : string.Empty
                };
                var files = new List<FileUploadInfo>();
                foreach (var f in o.Files)
                {
                    var file = db.Files.Where(x => x.Id == f.FileId).FirstOrDefault();
                    if (file != null && file.Length > 0)
                    {
                        var fileUploadInfo = new FileUploadInfo()
                        {
                            FileName = file.Name,
                            Extension = "",
                            UID = ""
                        };
                        files.Add(fileUploadInfo);
                    }
                }
                ((ViewTemplateCreateOrEditViewModel)viewModel).Files = files;
            }
            return Edit<ViewTemplate>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewTemplateCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<ViewTemplate>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                //dbObj.Name = vmObj.Name;
                dbObj.Description = vmObj.Description;
                //dbObj.Files.AddRange(AddImages(vmObj.Image));
                var files = AddFiles(vmObj.Files);
                foreach (var file in files)
                {
                    var viewTemplateFile = new ViewTemplateFile()
                    {
                        ViewTemplate = dbObj,
                        File = file,
                    };
                    db.Files.Add(file);
                    db.ViewTemplateFiles.Add(viewTemplateFile);
                    dbObj.Files.Add(viewTemplateFile);
                    dbObj.Name = Path.GetFileNameWithoutExtension(file.Name);
                }
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<ViewTemplate>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewTemplateViewModel dbObj)
        {
            return DeleteObject<ViewTemplate, ViewTemplate>(dbObj.Id);
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

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveFileAsync(ViewTemplateDataViewModel obj)//int id, string data)
        {
            var viewTemplateFile = db.ViewTemplateFiles.Where(x => x.Id == obj.Id).FirstOrDefault();
            var file = viewTemplateFile != null ? db.Files.Where(x => x.Id == viewTemplateFile.FileId).FirstOrDefault() : null;
            if (viewTemplateFile != null && file != null)
            {
                try
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(obj.Data);
                    System.IO.File.WriteAllBytes(Server.MapPath(Path.Combine("~/SimpleCMS/Views/Shared/BlogViewTemplates", file.Name + ".cshtml")), bytes);
                    file.Data = bytes;
                    file.Length = bytes.Length;
                    file.ChangeEvent = UpdateChangeEvent(file.ChangeEventId);
                    db.Entry(file).State = EntityState.Modified;
                    //db.Files.Remove(file);
                    //db.ViewTemplateFiles.Remove(ViewTemplateFile);
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
            var ViewTemplateFile = db.ViewTemplateFiles.Where(x => x.Id == id).FirstOrDefault();
            var file = ViewTemplateFile != null ? db.Files.Where(x => x.Id == ViewTemplateFile.FileId).FirstOrDefault() : null;
            if (ViewTemplateFile != null && file != null)
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath(Path.Combine("~/Views/Shared/BlogViewTemplates", file.Name + ".cshtml")));
                    db.Files.Remove(file);
                    db.ViewTemplateFiles.Remove(ViewTemplateFile);
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


        //TODO: Implement new custom functonality
        public JsonResult Find([DataSourceRequest] DataSourceRequest request, string filter, string pageContentTypeName, bool canFindAnyViewTemplate = false)
        {
            filter = Request.Params["filter[filters][0][value]"];
            var pageContentType = db.PageContentTypes.FilterDeleted().Where(x => x.Name == pageContentTypeName).FirstOrDefault();
            var listFiltered = db.ViewTemplates as IQueryable<ViewTemplate>;
            if (filter.Length >= 1)
            {
                listFiltered = listFiltered.Where(s =>
                    (canFindAnyViewTemplate || s.PageContentType.Name.Equals(pageContentType.Name))
                    &&
                    (("" + s.Name).ToLower().Contains(filter.ToLower()) || ("" + s.Description).ToLower().Contains(filter.ToLower()))
                    );
            }
            var result = listFiltered.ToList().Select(m => new ViewTemplateViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            });
            return Json(result.ToDataSourceResult(request).Data, JsonRequestBehavior.AllowGet);
        }
    }
}