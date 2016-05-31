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

namespace BootstrapControllers
{
    public class PluginsController : Controller<Plugin>
    {
        public ActionResult Upload()
        {
            return View();
        }

        protected override string GetFilePath()
        {
            return "~/SimpleCMS/Data/Plugins";
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Plugins");
            if (fh.Save(Files))
            {
                return Content("");
            }
            return Content("Error");
        }

        public ActionResult RemoveFile(string[] fileNames)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Plugins");
            if (fh.Remove(fileNames))
            {
                return Content("");
            }
            return Content("Error");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddFileToObject(PluginFileViewModel pluginFile)
        {
            if (pluginFile != null)
            {
                var dbObj = db.Set<Plugin>().Where(x => x.Id == pluginFile.PluginId).FirstOrDefault();
                if (dbObj != null)
                {
                    var files = AddFiles(pluginFile.Files);
                    //var file = files.Where(x => !string.IsNullOrEmpty(x.Name)).FirstOrDefault();
                    //if (file != null)
                    //{
                    SimpleCMS.Models.File retFile = null;
                    foreach (var file in files)
                    {
                        var PluginFile = new PluginFile()
                        {
                            Plugin = dbObj,
                            File = file,
                        };
                        db.Files.Add(file);
                        db.Set<PluginFile>().Add(PluginFile);
                        dbObj.Files.Add(PluginFile);
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

        protected override FileUploadHandler CreateFileUploadHandler()
        {
            return new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Plugins");
        }

        protected override void AddViewBag(Plugin obj)
        {

        }

        private void AddViewBagCreateEdit(Plugin obj)
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
        //    var filtered = GetFiltered(db.Set<Plugin>(), filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new PluginViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<Plugin> GetFiltered(IQueryable<Plugin> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<Plugin>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    ("" + s.Name).ToLower().Contains(filter.ToLower())
                    || ("" + s.Description).ToLower().Contains(filter.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Set<Plugin>().Select(m => new PluginViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        protected override jQueryDataTableResultModel<Plugin> GetData(jQueryDataTableParamModel param, Func<IList<Plugin>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<Plugin>().FilterDeleted().ToList();
            IList<Plugin> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<Plugin>()
                    .FilterDeleted()
                    .Where(s =>
                    ("" + s.Name).ToLower().Contains(param.sSearch.ToLower())
                    || ("" + s.Description).ToLower().Contains(param.sSearch.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    )
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<Plugin>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new PluginViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description
                })
            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new PluginCreateOrEditViewModel()
            {
                
            };
            return Create<Plugin>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PluginCreateOrEditViewModel vmObj)
        {
            var uf = FileUploadHandler.UploadedFiles;
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new Plugin()
                {
                    //Id = vmObj.Id,
                    Name = vmObj.Name,
                    Description = vmObj.Description
                };
                var files = AddFiles(vmObj.Files);
                foreach (var file in files)
                {
                    var pluginFile = new PluginFile()
                    {
                        Plugin = dbObj,
                        File = file
                    };
                    db.Files.Add(file);
                    db.PluginFiles.Add(pluginFile);
                    dbObj.Files.Add(pluginFile);
                }
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<Plugin>().Where(x => x.Id == id).FirstOrDefault();
            PluginCreateOrEditViewModel viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new PluginCreateOrEditViewModel()
                {
                    Name = o.Name,
                    Description = o.Description
                };
                var files = new List<FileUploadInfo>();
                foreach (var f in o.Files)
                {
                    var image = db.Files.Where(x => x.Id == f.FileId).FirstOrDefault();
                    if (image != null && image.Length > 0)
                    {
                        var fileUploadInfo = new FileUploadInfo()
                        {
                            FileName = image.Name,
                            Extension = "",
                            UID = ""
                        };
                        files.Add(fileUploadInfo);
                    }
                }
                viewModel.Files = files;
            }
            return Edit<Plugin>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PluginCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<Plugin>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                dbObj.Name = vmObj.Name;
                dbObj.Description = vmObj.Description;
                //dbObj.Files.AddRange(AddImages(vmObj.Image));
                var files = AddFiles(vmObj.Files);
                foreach (var file in files)
                {
                    var pluginFile = new PluginFile()
                    {
                        Plugin = dbObj,
                        File = file,
                    };
                    db.Files.Add(file);
                    db.PluginFiles.Add(pluginFile);
                    dbObj.Files.Add(pluginFile);
                }
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<Plugin>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PluginViewModel dbObj)
        {
            return DeleteObject<Plugin, Plugin>(dbObj.Id);
        }

        [HttpPost]
        public ActionResult GetFileContent(int id)
        {
            //var dir = Server.MapPath("/Images");
            //var path = Path.Combine(dir, id + ".jpg");
            var file = db.Files.Where(x => x.Id == id).FirstOrDefault();
            if (file != null)
            {
                return Content(System.Text.Encoding.Default.GetString(file.Data));
            }
            return new EmptyResult();
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
        public ActionResult GetPluginFiles(int id)
        {
            var plugin = db.Plugins.Where(x => x.Id == id).FirstOrDefault();
            if (plugin != null)
            {
                return Content(JsonConvert.SerializeObject(plugin.Files.ToList(), Formatting.Indented));
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult SaveFileAsync(int id, string data)
        {
            var pluginFile = db.PluginFiles.Where(x => x.Id == id).FirstOrDefault();
            var file = pluginFile != null ? db.Files.Where(x => x.Id == pluginFile.FileId).FirstOrDefault() : null;
            if (pluginFile != null && file != null)
            {
                try
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(data);
                    System.IO.File.WriteAllBytes(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Plugins", file.Name)), bytes);
                    file.Data = bytes;
                    file.Length = bytes.Length;
                    file.ChangeEvent = UpdateChangeEvent(file.ChangeEventId);
                    db.Entry(file).State = EntityState.Modified;
                    //db.Files.Remove(file);
                    //db.PluginFiles.Remove(pluginFile);
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
            var pluginFile = db.PluginFiles.Where(x => x.Id == id).FirstOrDefault();
            var file = pluginFile != null ? db.Files.Where(x => x.Id == pluginFile.FileId).FirstOrDefault() : null;
            if (pluginFile != null && file != null)
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Plugins", file.Name)));
                    db.Files.Remove(file);
                    db.PluginFiles.Remove(pluginFile);
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