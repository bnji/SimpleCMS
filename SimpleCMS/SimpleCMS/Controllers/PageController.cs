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
using System.Text;
using SimpleCMS;
using Newtonsoft.Json;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BootstrapControllers
{
    public class PageController : Controller<Page>
    {
        [AllowAnonymous]
        public ActionResult PermaLink(string permalinkName, string type = "html", string language = "fo")
        {
            var all = repository.GetAll();
            var o = all.Where(x => x.Permalink.Name.Equals(permalinkName)).FirstOrDefault();
            if (!HttpContext.Request.IsAuthenticated)
            {
                o = all.FilterPublished().Where(x => x.Permalink.Name.Equals(permalinkName)).FirstOrDefault();
            }
            if (o != null)
            {
                if (o.IsRssFeed)
                {
                    //var langName = SharedLibrary.Web.CookieHelper.GetCultureName(Request);
                    //langName = !string.IsNullOrEmpty(langName) ? langName : "fo";
                    return new FeedResult(RssController.GetRssFeed(db, "rss", language, 10));
                }
                //else if (o.Name.ToLower().Equals(db.BlogWebsiteSettings.FirstOrDefault().DefaultFrontPage))
                //{
                //    return RedirectToRoute(o.Name);
                //}
                if (type.ToLower().Equals("json"))
                {
                    var json = JsonConvert.SerializeObject(o, Formatting.Indented);
                    return Content(json, "application/json");
                    //return Json(o, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View(o);
                }
            }
            return RedirectToAction("Index");
        }

        protected override void AddViewBag(Page Page)
        {
            //ViewBag.CountryId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
            //{
            //    Text = m.Name,
            //    Value = m.Id.ToString()
            //}), "Value", "Text", Page == null ? 1 : Page.CountryId);
        }

        private void AddViewBagCreateEdit(Page obj)
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
        //    var filtered = GetFiltered(db.Set<Page>(), filter, 1);
        //    //return GetFiltered(request, filtered);
        //    // Avoid the circular reference by creating a View Model object and skiping the Customer property
        //    var result = filtered.FilterDeleted().Select(m => new PageViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Title = m.Title,
        //        Description = m.Description,
        //        Keywords = m.Keywords,
        //        Tags = m.Tags,
        //        //Content = m.Content,
        //        ParentId = m.Parent != null ? new Nullable<int>(m.Parent.Id) : null,
        //        //Pages = m.Pages,
        //        PermalinkId = m.PermalinkId,
        //        Permalink = m.Permalink,
        //        TemplateName = m.TemplateName,
        //        IncludeInMenu = m.IncludeInMenu,
        //        IsDraft = m.IsDraft,
        //        IsPublished = m.IsPublished,
        //        ActiveFrom = m.ActiveFrom,
        //        ActiveTo = m.ActiveTo,
        //        IsRssFeed = m.IsRssFeed,
        //        ChangeEventId = m.ChangeEventId,
        //        ChangeEvent = m.ChangeEvent
        //        //PageContentTypes = db.PageContentTypes.ToList()
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        IQueryable<Page> GetFiltered(IQueryable<Page> list, string filter, int minItems)
        {
            var filtered = list as IQueryable<Page>;
            if (filter.Length >= minItems)
            {
                //DateTime dt = DateTime.UtcNow;
                //DateTime.TryParseExact(filter, "dMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                filtered = filtered.Where(s =>
                    s.Name.ToLower().Equals(filter.ToLower())
                    || ("" + s.Title).ToLower().Contains(filter.ToLower())
                    //|| (s.Keywords != null && s.Keywords.Split(' ').Contains(filter))
                    //|| ("" + s.Description).ToLower().Contains(filter.ToLower())
                    //|| DateTime.Compare(DateTime.UtcNow, dt) >= 0
                    );
            }
            return filtered;
        }

        //protected override DataSourceResult Get(int id, DataSourceRequest request)
        //{
        //    var result = db.Set<Page>().Select(m => new PageViewModel()
        //    {
        //        Id = m.Id,
        //        Name = m.Name,
        //        Title = m.Title,
        //        Description = m.Description,
        //        Keywords = m.Keywords,
        //        Tags = m.Tags,
        //        //Content = m.Content,
        //        ParentId = m.Parent != null ? new Nullable<int>(m.Parent.Id) : null,
        //        //Pages = m.Pages,
        //        PermalinkId = m.PermalinkId,
        //        Permalink = m.Permalink,
        //        TemplateName = m.TemplateName,
        //        IncludeInMenu = m.IncludeInMenu,
        //        IsDraft = m.IsDraft,
        //        IsPublished = m.IsPublished,
        //        ActiveFrom = m.ActiveFrom,
        //        ActiveTo = m.ActiveTo,
        //        IsRssFeed = m.IsRssFeed,
        //        ChangeEventId = m.ChangeEventId,
        //        ChangeEvent = m.ChangeEvent
        //    });
        //    return result.ToDataSourceResult(request);
        //}

        protected override jQueryDataTableResultModel<Page> GetData(jQueryDataTableParamModel param, Func<IList<Page>, IEnumerable<dynamic>> transform)
        {
            var allPages = db.Set<Page>().FilterDeleted().ToList();
            IList<Page> filteredPages;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPages = db.Set<Page>()
                    .FilterDeleted()
                    .Where(c => c.Name.Contains(param.sSearch) || c.Title.Contains(param.sSearch))
                    .ToList();
            }
            else
            {
                filteredPages = allPages.ToList();
            }
            return new jQueryDataTableResultModel<Page>()
            {
                AllResults = allPages,
                FilteredResults = filteredPages,
                SelectedResults = filteredPages.Select(m => new PageViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Title = m.Title,
                    Description = m.Description,
                    Keywords = m.Keywords,
                    Tags = m.Tags,
                    //Content = m.Content,
                    ParentId = m.Parent != null ? new Nullable<int>(m.Parent.Id) : null,
                    //Pages = m.Pages,
                    PermalinkId = m.PermalinkId,
                    Permalink = m.Permalink,
                    TemplateName = m.TemplateName,
                    IncludeInMenu = m.IncludeInMenu,
                    IsDraft = m.IsDraft,
                    IsPublished = m.IsPublished,
                    ActiveFrom = m.ActiveFrom,
                    ActiveTo = m.ActiveTo,
                    IsRssFeed = m.IsRssFeed,
                    ChangeEventId = m.ChangeEventId,
                    ChangeEvent = m.ChangeEvent
                })
            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            IViewModelBase viewModel = null;
            AddViewBagCreateEdit(null);
            viewModel = new PageCreateOrEditViewModel()
            {
                IsDraft = false,
                IsPublished = true,
                IsRssFeed = false
            };
            return Create<Page>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageCreateOrEditViewModel vmObj)
        {
            return CreateUsingViewModel(vmObj, (parent) =>
            {
                var permalinkName = ValidatePermalinkName(vmObj.PermalinkName, "");
                if (string.IsNullOrEmpty(permalinkName))
                {
                    return null;
                }
                else
                {
                    var permalink = new Permalink()
                    {
                        Name = permalinkName,
                        Type = null,
                        ChangeEvent = CreateChangeEvent()
                    };
                    db.Permalinks.Add(permalink);
                    //Convert the ViewModel to DB Object (Model)
                    var dbObj = new Page()
                    {
                        Id = vmObj.Id,
                        Name = vmObj.Name,
                        Title = vmObj.Title,
                        Description = vmObj.Description,
                        Keywords = vmObj.Keywords,
                        Tags = vmObj.Tags,
                        //Content = vmObj.Content.ToList(),
                        Parent = vmObj.Parent,
                        //Pages = vmObj.Pages.ToList(),
                        PermalinkId = vmObj.PermalinkId,
                        Permalink = permalink,
                        TemplateName = vmObj.TemplateName,
                        IncludeInMenu = vmObj.IncludeInMenu,
                        IsDraft = vmObj.IsDraft,
                        IsPublished = vmObj.IsPublished,
                        ActiveFrom = vmObj.ActiveFrom,
                        ActiveTo = vmObj.ActiveTo,
                        IsRssFeed = vmObj.IsRssFeed,
                        ChangeEventId = vmObj.ChangeEventId,
                        ChangeEvent = vmObj.ChangeEvent
                    };
                    return dbObj;
                }
            }, null, "CreateOrEdit");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var o = db.Set<Page>().Where(x => x.Id == id).FirstOrDefault();
            IViewModelBase viewModel = null;
            if (o != null)
            {
                AddViewBag(o);

                viewModel = new PageCreateOrEditViewModel()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Title = o.Title,
                    Description = o.Description,
                    Keywords = o.Keywords,
                    Tags = o.Tags,
                    //Content = o.Content,
                    Parent = o.Parent,
                    ParentId = o.Parent != null ? new Nullable<int>(o.Parent.Id) : null,
                    ParentName = o.Parent != null ? o.Parent.Name : string.Empty,
                    //Pages = o.Pages,
                    PermalinkId = o.PermalinkId,
                    Permalink = o.Permalink,
                    PermalinkName = o.Permalink != null ? o.Permalink.Name : string.Empty,
                    TemplateName = o.TemplateName,
                    IncludeInMenu = o.IncludeInMenu,
                    IsDraft = o.IsDraft,
                    IsPublished = o.IsPublished,
                    ActiveFrom = o.ActiveFrom,
                    ActiveTo = o.ActiveTo,
                    IsRssFeed = o.IsRssFeed,
                    ChangeEventId = o.ChangeEventId,
                    ChangeEvent = o.ChangeEvent
                };
            }
            return Edit<Page>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageCreateOrEditViewModel vmObj)
        {
            return UpdateUsingViewModel(vmObj, (o) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var dbObj = db.Set<Page>().Where(x => x.Id == vmObj.Id).FirstOrDefault();

                var newPermalink = ValidatePermalinkName(vmObj.PermalinkName, dbObj.Permalink.Name);
                dbObj.Permalink.Name = !string.IsNullOrEmpty(newPermalink) ? newPermalink : dbObj.Permalink.Name;
                dbObj.Name = vmObj.Name;
                dbObj.Title = vmObj.Title;
                dbObj.Description = vmObj.Description;
                dbObj.Keywords = vmObj.Keywords;
                dbObj.Tags = vmObj.Tags;
                dbObj.ParentId = !string.IsNullOrEmpty(vmObj.ParentName) ? vmObj.ParentId : null;
                dbObj.TemplateName = vmObj.TemplateName;
                dbObj.IncludeInMenu = vmObj.IncludeInMenu;
                dbObj.IsDraft = vmObj.IsDraft;
                dbObj.IsPublished = vmObj.IsPublished;
                dbObj.ActiveFrom = vmObj.ActiveFrom;
                dbObj.ActiveTo = vmObj.ActiveTo;
                dbObj.IsRssFeed = vmObj.IsRssFeed;
                dbObj.ChangeEvent = UpdateChangeEvent(dbObj.ChangeEventId);

                return dbObj;

            }, null, "CreateOrEdit");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<Page>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PageViewModel dbObj)
        {
            return DeleteObject<Page, Page>(dbObj.Id);
        }

        //TODO: Implement new custom functonality
        public JsonResult Find([DataSourceRequest] DataSourceRequest request, string filter)
        {
            filter = Request.Params["filter[filters][0][value]"];
            var listFiltered = db.Set<Page>() as IQueryable<Page>;
            if (filter.Length >= 2)
            {
                listFiltered = listFiltered.Where(s =>
                    ("" + s.Name).ToLower().Contains(filter.ToLower())
                    || ("" + s.Title).ToLower().Contains(filter.ToLower())
                    || (s.Keywords != null && s.Keywords.Contains(filter))
                    || s.Description.ToLower().Contains(filter.ToLower()));
            }
            var result = listFiltered.ToList().Select(m => new PageViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Title = m.Title,
                Description = m.Description,
                Keywords = m.Keywords,
                Tags = m.Tags,
                ParentId = m.Parent != null ? new Nullable<int>(m.Parent.Id) : null,
                PermalinkId = m.PermalinkId,
                Permalink = m.Permalink,
                TemplateName = m.TemplateName,
                IncludeInMenu = m.IncludeInMenu,
                IsDraft = m.IsDraft,
                IsPublished = m.IsPublished,
                ActiveFrom = m.ActiveFrom,
                ActiveTo = m.ActiveTo,
                IsRssFeed = m.IsRssFeed,
                ChangeEventId = m.ChangeEventId,
                ChangeEvent = m.ChangeEvent
            });
            return Json(result.ToDataSourceResult(request).Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CleanUrl(string text, string current)
        {
            return Json(ValidatePermalinkName(text, current), JsonRequestBehavior.AllowGet);
        }

        private string ValidatePermalinkName(string text, string current)
        {
            var notAllowed = new List<string>() { 
                "pagecontent",
                "page",
                "viewtemplate",
                "plugins",
                "file",
                "contenttranslation",
                "pagecontenttype",
                "setculture",
                "account",
                "index",
                "login",
                "logout",
                "language",
                //"rss",
                //"feed",
                "mail",
                "signup",
                "signout",
                "newsletter"
            };
            var result = ("" + text).Slugify();
            if (notAllowed.Contains(result.ToLower()))
            {
                result = string.Empty;
            }
            if (db.Permalinks.Where(x => !x.Name.ToLower().Equals(current) && x.Name.ToLower().Equals(result)).FirstOrDefault() != null)
            {
                result = string.Empty;
            }
            return result;
        }

        [HttpPost]
        public JsonResult SetPageContentIndex(int pageContentId, int newIndex)
        {
            var pageContent = db.PageContents.Where(x => x.Id == pageContentId).FirstOrDefault();
            if (pageContent != null)
            {
                pageContent.Index = newIndex;
                db.Entry(pageContent).State = EntityState.Modified;
                var result = db.SaveChanges();
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            return Json("null", JsonRequestBehavior.DenyGet);
        }

    }
}