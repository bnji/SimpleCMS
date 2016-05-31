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

namespace BootstrapControllers
{
    public class BlogListController : Controller<PageContent>
    {

        protected override void AddViewBag(PageContent PageContent)
        {
            //ViewBag.CountryId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
            //{
            //    Text = m.Name,
            //    Value = m.Id.ToString()
            //}), "Value", "Text", PageContent == null ? 1 : PageContent.CountryId);
        }

        [HttpGet]
        public ActionResult Create(int? parentId)
        {
            IViewModelBase viewModel = null;
            //AddViewBag(o);
            var viewTemplate = db.ViewTemplates.Where(x => x.Name.Equals("_BlogList")).FirstOrDefault();
            viewModel = new BlogListContentCreateOrEditViewModel()
            {
                ParentId = parentId,
                ViewTemplateId = viewTemplate.Id,
                ViewTemplate = viewTemplate
            };
            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogListContentCreateOrEditViewModel vmObj)
        {
            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return CreateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                //Convert the ViewModel to DB Object (Model)
                var pageContent = new PageContent()
                {
                    Index = vmObj.Index,
                    //ViewName = vmObj.ViewName,
                    ViewTemplateId = vmObj.ViewTemplateId,
                    PageId = vmObj.ParentId,
                    PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogList")).FirstOrDefault()
                };
                db.PageContents.Add(pageContent);
                return pageContent;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id, string language)
        {
            var o = db.Set<PageContent>().Where(x => x.Id == id).FirstOrDefault();
            IViewModelBase viewModel = null;
            if (o != null)
            {
                //AddViewBag(o);
                viewModel = new BlogListContentCreateOrEditViewModel()
                {
                    Id = o.Id,
                    ParentId = o.PageId,
                    Index = o.Index,
                    ViewTemplate = o.ViewTemplate,
                    ViewTemplateId = o.ViewTemplateId
                };
            }
            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogListContentCreateOrEditViewModel vmObj)
        {
            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return UpdateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                var pageContent = db.PageContents.Where(x => x.Id == vmObj.Id).FirstOrDefault();

                if (pageContent != null)
                {
                    pageContent.Index = vmObj.Index;
                    //PageContent.ViewName = vmObj.ViewName;
                    pageContent.ViewTemplateId = vmObj.ViewTemplateId;
                }
                return pageContent;
            });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var obj = db.Set<PageContent>().Find(id);
            return Delete("Delete", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BlogListContentViewModel dbObj)
        {
            return DeleteObject<PageContent, PageContent>(dbObj.Id);
        }

        [HttpPost]
        public JsonResult GetPaginatedList(string pageNumber, int paginationCount, string culture)
        {
            var pagination = new Pagination(db.PageContents.FilterDeleted().Where(x => x.BlogItemContent != null), db.BlogWebsiteSettings.FirstOrDefault().PostsPerPage, pageNumber, paginationCount);
            return Json(new PaginationViewModel<PageContentViewModel>()
            {
                List = pagination.List.Select(m => new PageContentViewModel()
                {
                    Id = m.Id,
                    Title = m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault() != null ? m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault().Title : null,
                    Body = m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault() != null ? HttpUtility.HtmlDecode(m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault().Body) : null,
                    Excerpt = m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault() != null ? m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault().Excerpt : null,
                    Description = m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault() != null ? m.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(culture)).FirstOrDefault().Description : null,
                    PageContentInfo = m.PageContentInfo,
                    Page = new PageViewModel(m.Page)
                }),
                PageNumber = pagination.PageNumber,
                PageCount = pagination.PageCount,
                PerPage = pagination.PerPage,
                BackIndex = pagination.BackIndex,
                ForwardIndex = pagination.ForwardIndex,
                From = pagination.From,
                To = pagination.To
            });
        }

    }
}