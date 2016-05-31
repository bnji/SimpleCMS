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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BootstrapControllers
{
    public class PageContentTypeController : Controller<PageContentType>
    {
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

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //private DataSourceResult GetFiltered(string filter, DataSourceRequest request)
        //{
        //    var listFiltered = db.Set<PageContentType>() as IQueryable<PageContentType>;
        //    if (filter.Length > 1)
        //    {
        //        //DateTime dt = DateTime.Now;
        //        //DateTime.TryParseExact(filter, "dMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        //        listFiltered = listFiltered.Where(s => s.Name.ToLower().Equals(filter.ToLower())
        //                                || s.Description.ToLower().Contains(filter.ToLower())
        //        );
        //    }
        //    return GetFiltered(request, listFiltered);
        //}

        //private DataSourceResult GetFiltered(DataSourceRequest request, IQueryable<PageContentType> list)
        //{
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = list.FilterDeleted().Select(m => new PageContentTypeViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Description = m.Description
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        protected override jQueryDataTableResultModel<PageContentType> GetData(jQueryDataTableParamModel param, Func<IList<PageContentType>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<PageContentType>().FilterDeleted().ToList();
            IList<PageContentType> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<PageContentType>()
                    .FilterDeleted()
                    .Where(s => s.Name.ToLower().Equals(param.sSearch.ToLower()) || s.Description.ToLower().Contains(param.sSearch.ToLower()))
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<PageContentType>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new PageContentTypeViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description
                })
            };
        }

        protected override void AddViewBag(PageContentType PageContentType)
        {
            //ViewBag.PageContentTypeId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
            //{
            //    Text = m.Name,
            //    Value = m.Id.ToString()
            //}), "Value", "Text", PageContentType == null ? 1 : PageContentType.PageContentTypeId);
        }

        [HttpGet]
        public ActionResult Create(int? parentId)
        {
            IViewModelBase viewModel = null;
            //AddViewBag(o);
            viewModel = new PageContentTypeCreateOrEditViewModel()
            {
                
            };
            return Edit<PageContentType>("CreateOrEdit", viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageContentTypeCreateOrEditViewModel o)
        {
            return CreateUsingViewModel(o, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new PageContentType()
                {
                    Name = o.Name,
                    Description = o.Description
                };
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = db.PageContentTypes.Where(x => x.Id == id).FirstOrDefault();
            IViewModelBase viewModel = null;
            if (obj != null)
            {
                AddViewBag(obj);
                viewModel = new PageContentTypeCreateOrEditViewModel()
                {
                    Name = obj.Name,
                    Description = obj.Description
                };
            }
            return Edit<PageContentType>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageContentTypeCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.PageContentTypes.Where(x => x.Id == vmObj.Id).FirstOrDefault();
                dbObj.Name = vmObj.Name;
                dbObj.Description = vmObj.Description;
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.PageContentTypes.Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PageItemContentViewModel dbObj)
        {
            return DeleteObject<PageContentType, PageContentType>(dbObj.Id);
        }

        //TODO: Implement new custom functonality
        public JsonResult Find([DataSourceRequest] DataSourceRequest request, string filter)
        {
            filter = Request.Params["filter[filters][0][value]"];
            var listFiltered = db.PageContentTypes as IQueryable<PageContentType>;
            if (filter.Length >= 1)
            {
                listFiltered = listFiltered.Where(s =>
                    ("" + s.Name).ToLower().Contains(filter.ToLower())
                    || ("" + s.Description).ToLower().Contains(filter.ToLower())
                    );
            }
            var result = listFiltered.ToList().Select(m => new PageContentTypeViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            });
            return Json(result.ToDataSourceResult(request).Data, JsonRequestBehavior.AllowGet);
        }
    }
}