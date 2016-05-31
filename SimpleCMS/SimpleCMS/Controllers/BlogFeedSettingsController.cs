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
    public class BlogFeedSettingsController : Controller<BlogFeedSettings>
    {
        public override ActionResult Index()
        {
            return RedirectToAction("Details", new { @id = 1 });
        }

        protected override void AddViewBag(BlogFeedSettings obj)
        {

        }

        private void AddViewBagCreateEdit(BlogFeedSettings obj)
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
        //    var filtered = GetFiltered(db.Set<BlogFeedSettings>(), filter, 2);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new BlogFeedSettingsViewModel()
        //    {
        //        Id = m.Id,
        //        Title = m.Title,
        //        BaseUri = m.BaseUri,
        //        UriName = m.UriName,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<BlogFeedSettings> GetFiltered(IQueryable<BlogFeedSettings> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<BlogFeedSettings>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    ("" + s.Title).ToLower().Contains(filter.ToLower())
                    || ("" + s.Description).ToLower().Contains(filter.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Set<BlogFeedSettings>().Select(m => new BlogFeedSettingsViewModel()
        //    {
        //        Id = m.Id,
        //        Title = m.Title,
        //        BaseUri = m.BaseUri,
        //        UriName = m.UriName,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new BlogFeedSettingsCreateOrEditViewModel()
            {

            };
            return Create<BlogFeedSettings>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogFeedSettingsCreateOrEditViewModel vmObj)
        {
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new BlogFeedSettings()
                {
                    //Id = vmObj.Id,
                    Title = vmObj.Title,
                    BaseUri = vmObj.BaseUri,
                    UriName = vmObj.UriName,
                    Description = vmObj.Description
                };
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<BlogFeedSettings>().Where(x => x.Id == id).FirstOrDefault();
            ViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new BlogFeedSettingsCreateOrEditViewModel()
                {
                    Title = o.Title,
                    BaseUri = o.BaseUri,
                    UriName = o.UriName,
                    Description = o.Description
                };
            }
            return Edit<BlogFeedSettings>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogFeedSettingsCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<BlogFeedSettings>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                dbObj.Title = vmObj.Title;
                dbObj.BaseUri = vmObj.BaseUri;
                dbObj.UriName = vmObj.UriName;
                dbObj.Description = vmObj.Description;
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<BlogFeedSettings>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BlogFeedSettingsViewModel dbObj)
        {
            return DeleteObject<BlogFeedSettings, BlogFeedSettings>(dbObj.Id);
        }
    }
}