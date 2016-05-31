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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BootstrapControllers
{
    public class ContentTranslationController : Controller<ContentTranslation>
    {
        private void AddViewBagCreateEdit(ContentTranslation obj)
        {
            //ViewBag.CountryId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
            //{
            //    Text = m.Code + " - " + m.Name,
            //    Value = m.Id.ToString()
            //}), "Value", "Text", obj != null ? new Nullable<int>(obj.CountryId) : null);
        }

        protected override jQueryDataTableResultModel<ContentTranslation> GetData(jQueryDataTableParamModel param, Func<IList<ContentTranslation>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<ContentTranslation>().FilterDeleted().ToList();
            IList<ContentTranslation> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<ContentTranslation>()
                    .FilterDeleted()
                    .Where(s =>
                    ("" + s.Language).ToLower().Contains(param.sSearch.ToLower())
                    || ("" + s.Description).ToLower().Contains(param.sSearch.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    )
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<ContentTranslation>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new ContentTranslationViewModel()
                {
                    Id = m.Id,
                    Language = m.Language,
                    Description = m.Description
                })
            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new ContentTranslationCreateOrEditViewModel()
            {

            };
            return Create<ContentTranslation>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContentTranslationCreateOrEditViewModel vmObj)
        {
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = new ContentTranslation()
                {
                    Language = vmObj.Language,
                    Description = vmObj.Description
                };
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<ContentTranslation>().Where(x => x.Id == id).FirstOrDefault();
            ViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);
                viewModel = new ContentTranslationCreateOrEditViewModel()
                {
                    Language = o.Language,
                    Description = o.Description
                };
            }
            return Edit<ContentTranslation>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContentTranslationCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<ContentTranslation>().Where(x => x.Id == vmObj.Id).FirstOrDefault();
                dbObj.Language = vmObj.Language;
                dbObj.Description = vmObj.Description;
                return dbObj;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<ContentTranslation>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ContentTranslationViewModel dbObj)
        {
            return DeleteObject<ContentTranslation, ContentTranslation>(dbObj.Id);
        }


        //TODO: Implement new custom functonality
        public JsonResult Find([DataSourceRequest] DataSourceRequest request, string filter)
        {
            filter = Request.Params["filter[filters][0][value]"];
            var listFiltered = db.ContentTranslations as IQueryable<ContentTranslation>;
            if (filter.Length >= 2)
            {
                listFiltered = listFiltered.Where(s =>
                    ("" + s.Language).ToLower().Contains(filter.ToLower())
                    || s.Description.ToLower().Contains(filter.ToLower()));
            }
            var result = listFiltered.ToList().Select(m => new ContentTranslationViewModel()
            {
                Id = m.Id,
                Language = m.Language,
                Description = m.Description
            });
            return Json(result.ToDataSourceResult(request).Data, JsonRequestBehavior.AllowGet);
        }
    }
}