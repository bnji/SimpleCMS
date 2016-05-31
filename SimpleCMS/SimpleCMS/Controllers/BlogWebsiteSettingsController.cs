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
    public class BlogWebsiteSettingsController : Controller<BlogWebsiteSettings>
    {
        public override ActionResult Index()
        {
            return RedirectToAction("Details", new { @id = 1 });
        }

        protected override void AddViewBag(BlogWebsiteSettings obj)
        {

        }

        private void AddViewBagCreateEdit(BlogWebsiteSettings obj)
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
        //    var filtered = GetFiltered(db.Set<BlogWebsiteSettings>(), filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new BlogWebsiteSettingsViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Description = m.Description,
        //        PostsPerPage = m.PostsPerPage,
        //        DefaultFrontPage = m.DefaultFrontPage
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<BlogWebsiteSettings> GetFiltered(IQueryable<BlogWebsiteSettings> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<BlogWebsiteSettings>;
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
        //    var result = db.Set<BlogWebsiteSettings>().Select(m => new BlogWebsiteSettingsViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Description = m.Description,
        //        PostsPerPage = m.PostsPerPage,
        //        DefaultFrontPage = m.DefaultFrontPage,
        //        DefaultLanguageDescription = m.DefaultLanguage.Description,
        //        DefaultLanguageId = m.DefaultLanguageId.Value
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new BlogWebsiteSettingsCreateOrEditViewModel()
            {

            };
            return Create<BlogWebsiteSettings>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogWebsiteSettingsCreateOrEditViewModel vmObj)
        {
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new BlogWebsiteSettings()
                {
                    //Id = vmObj.Id,
                    Name = vmObj.Name,
                    Description = vmObj.Description,
                    PostsPerPage = vmObj.PostsPerPage,
                    DefaultFrontPage = vmObj.DefaultFrontPage,
                    DefaultLanguageId = vmObj.DefaultLanguageId
                };
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<BlogWebsiteSettings>().Where(x => x.Id == id).FirstOrDefault();
            ViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new BlogWebsiteSettingsCreateOrEditViewModel()
                {
                    Name = o.Name,
                    Description = o.Description,
                    PostsPerPage = o.PostsPerPage,
                    DefaultFrontPage = o.DefaultFrontPage,
                    DefaultLanguageId = o.DefaultLanguageId.Value
                };
            }
            return Edit<BlogWebsiteSettings>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogWebsiteSettingsCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<BlogWebsiteSettings>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                dbObj.Name = vmObj.Name;
                dbObj.Description = vmObj.Description;
                dbObj.PostsPerPage = vmObj.PostsPerPage;
                dbObj.DefaultFrontPage = vmObj.DefaultFrontPage;
                dbObj.DefaultLanguageId = vmObj.DefaultLanguageId;
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<BlogWebsiteSettings>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BlogWebsiteSettingsViewModel dbObj)
        {
            return DeleteObject<BlogWebsiteSettings, BlogWebsiteSettings>(dbObj.Id);
        }
    }
}