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
    public class PageItemController : Controller<PageContent>
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
            var parent = db.Pages.Where(x => x.Id == parentId.Value).FirstOrDefault();
            var viewTemplate = new Repository<ViewTemplate>().GetAll().Where(x => x.Name.Equals("_PageItem")).FirstOrDefault();
            viewModel = new PageItemContentCreateOrEditViewModel()
            {
                ParentId = parentId,
                PublishedOn = DateTime.Now,
                IsPublished = true,
                //HasRSS = false,
                ViewTemplateId = viewTemplate.Id,
                ViewTemplate = viewTemplate,
                Index = parent != null ? parent.Content.FilterDeleted().Count() + 1 : 0
            };
            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageItemContentCreateOrEditViewModel vmObj)
        {
            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return CreateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                //Convert the ViewModel to DB Object (Model)
                //var translation = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();

                var pageItemContentData = new PageItemContentData()
                {
                    Title = vmObj.Title,
                    Body = vmObj.BodyHtml,
                    //Keywords = vmObj.Keywords,
                    //Translation = translation
                    TranslationId = db.BlogWebsiteSettings.FirstOrDefault().DefaultLanguageId
                };
                db.PageItemContentDatas.Add(pageItemContentData);
                var pageContentInfo = new PageContentInfo()
                {
                    HasRSS = false,// vmObj.HasRSS,
                    IsDraft = vmObj.IsDraft,
                    IsPublished = vmObj.IsPublished,
                    PublishedBy = vmObj.PublishedBy,
                    PublishedOn = vmObj.PublishedOn,
                    ActiveFrom = vmObj.ActiveFrom,
                    ActiveTo = vmObj.ActiveTo
                };
                db.PageContentInfos.Add(pageContentInfo);
                var pageItemContent = new PageItemContent()
                {
                    Id = vmObj.Id,
                    //Title = vmObj.Title,
                    //Body = vmObj.BodyHtml,
                    //Keywords = vmObj.Keywords,
                    //Description = vmObj.Description,
                    PageContentInfo = pageContentInfo
                };
                pageItemContent.ContentData.Add(pageItemContentData);
                db.PageItemContents.Add(pageItemContent);
                var pageContent = new PageContent()
                {
                    PageItemContent = pageItemContent,
                    //ImageContent = null,
                    //Index = vmObj.Index,
                    //ViewName = vmObj.ViewName,
                    ViewTemplateId = vmObj.ViewTemplateId,
                    PageId = vmObj.ParentId,
                    PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("PageItem")).FirstOrDefault()
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
            if (o != null && o.PageItemContent != null && o.PageItemContent.PageContentInfo != null)
            {
                var pageItemContentData = o.PageItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
                var languageFound = db.ContentTranslations.Where(x => x.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
                var languageExists = languageFound != null;
                if (pageItemContentData == null && languageExists)
                {
                    pageItemContentData = new PageItemContentData()
                    {
                        Title = "",
                        Body = "",
                        Translation = languageFound,
                    };
                }

                //AddViewBag(o);
                viewModel = new PageItemContentCreateOrEditViewModel()
                {
                    Id = o.Id,
                    ParentId = o.PageId,
                    //Index = o.Index,
                    //ViewName = o.ViewTemplate != null ? o.ViewTemplate.Name : "_BlogItem",
                    ContentTranslation = languageFound,
                    ContentTranslationId = languageFound.Id,
                    ViewTemplate = o.ViewTemplate,
                    ViewTemplateId = o.ViewTemplateId,
                    Title = pageItemContentData.Title,// o.PageItemContent.Title,
                    Body = pageItemContentData.Body,// o.PageItemContent.Body,
                    //Excerpt = o.PageItemContent.Excerpt,
                    //Keywords = o.PageItemContent.Keywords,
                    //Description = o.PageItemContent.Description,
                    //HasRSS = o.PageItemContent.PageContentInfo.HasRSS,
                    IsDraft = o.PageItemContent.PageContentInfo.IsDraft,
                    IsPublished = o.PageItemContent.PageContentInfo.IsPublished,
                    PublishedBy = o.PageItemContent.PageContentInfo.PublishedBy,
                    PublishedOn = o.PageItemContent.PageContentInfo.PublishedOn,
                    ActiveFrom = o.PageItemContent.PageContentInfo.ActiveFrom,
                    ActiveTo = o.PageItemContent.PageContentInfo.ActiveTo
                };
            }
            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageItemContentCreateOrEditViewModel vmObj)
        {
            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return UpdateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                var pageContent = db.PageContents.Where(x => x.Id == vmObj.Id).FirstOrDefault();
                var pageItemContent = pageContent != null ? pageContent.PageItemContent : null;
                var pageContentInfo = pageItemContent != null ? pageItemContent.PageContentInfo : null;
                var pageItemContentData = pageItemContent != null && pageItemContent.ContentData != null ? pageItemContent.ContentData.Where(x => x.Translation != null && x.Translation.Id == vmObj.ContentTranslationId).FirstOrDefault() : null;

                var languageFound = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();
                var languageExists = languageFound != null;
                if (pageItemContentData == null && languageExists)
                {
                    pageItemContentData = new PageItemContentData()
                    {
                        Title = "",
                        Body = "",
                        Translation = languageFound
                    };
                    pageContent.PageItemContent.ContentData.Add(pageItemContentData);
                }

                if (pageContent != null && pageItemContent != null && pageContentInfo != null && pageItemContentData != null)
                {
                    //pageContentInfo.HasRSS = vmObj.HasRSS;
                    pageContentInfo.IsDraft = vmObj.IsDraft;
                    pageContentInfo.IsPublished = vmObj.IsPublished;
                    pageContentInfo.PublishedBy = vmObj.PublishedBy;
                    pageContentInfo.PublishedOn = vmObj.PublishedOn;
                    pageContentInfo.ActiveFrom = vmObj.ActiveFrom;
                    pageContentInfo.ActiveTo = vmObj.ActiveTo;

                    pageItemContentData.Title = vmObj.Title;
                    pageItemContentData.Body = vmObj.BodyHtml;

                    //pageItemContent.Title = vmObj.Title;
                    //pageItemContent.Body = vmObj.BodyHtml;
                    //pageItemContent.Excerpt = vmObj.Excerpt;
                    //pageItemContent.Keywords = vmObj.Keywords;
                    //pageItemContent.Description = vmObj.Description;
                    pageItemContent.PageContentInfo = pageContentInfo;

                    pageContent.PageItemContent = pageItemContent;
                    //pageContent.ImageContent = null;
                    //pageContent.Index = vmObj.Index;
                    //PageContent.ViewName = vmObj.ViewName;
                    pageContent.ViewTemplateId = vmObj.ViewTemplateId;
                    //PageContent.PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogItem")).FirstOrDefault()
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
        public ActionResult Delete(PageItemContentViewModel dbObj)
        {
            return DeleteObject<PageContent, PageContent>(dbObj.Id);
        }

    }
}