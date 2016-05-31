//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SimpleCMS.Models;
//using SimpleCMS.DAL;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
//using SimpleCMS.Interfaces;
//using SimpleCMS.ViewModels;
//using SimpleCMS;
//using SimpleCMS.Models.Blog;

//namespace BootstrapControllers
//{
//    public class ImageContentController : Controller<PageContent>
//    {
//        protected override DataSourceResult Get(int id, DataSourceRequest request)
//        {
//            return null;
//        }

//        protected override void AddViewBag(PageContent PageContent)
//        {
//            //ViewBag.CountryId = new SelectList(db.Countries.ToList().Select(m => new SelectListItem
//            //{
//            //    Text = m.Name,
//            //    Value = m.Id.ToString()
//            //}), "Value", "Text", PageContent == null ? 1 : PageContent.CountryId);
//        }

//        [HttpGet]
//        public ActionResult Create(int? parentId)
//        {
//            ViewModelBase viewModel = null;
//            //AddViewBag(o);
//            var parent = db.Pages.Where(x => x.Id == parentId.Value).FirstOrDefault();
//            var viewTemplate = db.ViewTemplates.Where(x => x.Name.Equals("_ImageContent")).FirstOrDefault();
//            viewModel = new ImageContentCreateOrEditViewModel()
//            {
//                ParentId = parentId,
//                ViewTemplateId = viewTemplate.Id,
//                ViewTemplate = viewTemplate,
//                Index = parent != null ? parent.Content.FilterDeleted().Count() + 1 : 0
//            };
//            return Edit<PageContent>("CreateOrEdit", viewModel);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(ImageContentCreateOrEditViewModel vmObj)
//        {
//            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
//            if (parent == null)
//            {
//                return new EmptyResult();
//            }
//            return CreateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
//            {
//                //Convert the ViewModel to DB Object (Model)
//                //var translation = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();

//                var imageContentData = new ImageContentData()
//                {
//                    Caption = vmObj.Caption,
//                    Description = vmObj.Description,
//                    TranslationId = db.BlogWebsiteSettings.FirstOrDefault().DefaultLanguageId
//                    //Translation = translation
//                };
//                db.ImageContentDatas.Add(imageContentData);
//                var pageContentInfo = new PageContentInfo()
//                {
//                    HasRSS = vmObj.HasRSS,
//                    IsDraft = vmObj.IsDraft,
//                    IsPublished = vmObj.IsPublished,
//                    PublishedBy = vmObj.PublishedBy,
//                    PublishedOn = vmObj.PublishedOn,
//                    ActiveFrom = vmObj.ActiveFrom,
//                    ActiveTo = vmObj.ActiveTo
//                };
//                db.PageContentInfos.Add(pageContentInfo);
//                var imageContent = new ImageContent()
//                {
//                    Id = vmObj.Id,
//                    Image = vmObj.ImageHtml,
//                    Height = vmObj.Height,
//                    Width = vmObj.Width,
//                    PageContentInfo = pageContentInfo,
//                    ChangeEvent = CreateChangeEvent()
//                };
//                imageContent.ContentData.Add(imageContentData);
//                db.ImageContents.Add(imageContent);
//                var pageContent = new PageContent()
//                {
//                    ImageContent = imageContent,
//                    Index = vmObj.Index,
//                    //ViewName = vmObj.ViewName,
//                    ViewTemplateId = vmObj.ViewTemplateId,
//                    PageId = vmObj.ParentId,
//                    PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("ImageContent")).FirstOrDefault()
//                };
//                db.PageContents.Add(pageContent);

//                return pageContent;
//            });
//        }

//        [HttpGet]
//        public ActionResult Edit(int? id, string language)
//        {
//            var o = db.Set<PageContent>().Where(x => x.Id == id).FirstOrDefault();
//            IViewModelBase viewModel = null;
//            if (o != null && o.ImageContent != null && o.ImageContent.PageContentInfo != null)
//            {
//                var imageContentData = o.ImageContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
//                var languageFound = db.ContentTranslations.Where(x => x.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
//                var languageExists = languageFound != null;
//                if (imageContentData == null && languageExists)
//                {
//                    imageContentData = new ImageContentData()
//                    {
//                        Caption = "",
//                        Description = "",
//                        Translation = languageFound
//                    };
//                }

//                //AddViewBag(o);
//                viewModel = new ImageContentCreateOrEditViewModel()
//                {
//                    Id = o.Id,
//                    ParentId = o.PageId,
//                    Index = o.Index,
//                    ContentTranslation = languageFound,
//                    ContentTranslationId = languageFound.Id,
//                    //ViewName = o.ViewTemplate != null ? o.ViewTemplate.Name : "_BlogItem",
//                    ViewTemplate = o.ViewTemplate,
//                    ViewTemplateId = o.ViewTemplateId,
//                    Image = o.ImageContent.Image,
//                    Width = o.ImageContent.Width,
//                    Height = o.ImageContent.Height,
//                    Caption = imageContentData.Caption,
//                    Description= imageContentData.Description,
//                    //Keywords = o.BlogItemContent.Keywords,
//                    HasRSS = o.ImageContent.PageContentInfo.HasRSS,
//                    IsDraft = o.ImageContent.PageContentInfo.IsDraft,
//                    IsPublished = o.ImageContent.PageContentInfo.IsPublished,
//                    PublishedBy = o.ImageContent.PageContentInfo.PublishedBy,
//                    PublishedOn = o.ImageContent.PageContentInfo.PublishedOn,
//                    ActiveFrom = o.ImageContent.PageContentInfo.ActiveFrom,
//                    ActiveTo = o.ImageContent.PageContentInfo.ActiveTo
//                };
//            }
//            return Edit<PageContent>("CreateOrEdit", viewModel);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(ImageContentCreateOrEditViewModel vmObj)
//        {
//            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
//            if (parent == null)
//            {
//                return new EmptyResult();
//            }
//            return UpdateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
//            {
//                var pageContent = db.PageContents.Where(x => x.Id == vmObj.Id).FirstOrDefault();
//                var imageContent = pageContent != null ? pageContent.ImageContent : null;
//                var pageContentInfo = imageContent != null ? imageContent.PageContentInfo : null;
//                var imageContentData = imageContent != null && imageContent.ContentData != null ? imageContent.ContentData.Where(x => x.Translation != null && x.Translation.Id == vmObj.ContentTranslationId).FirstOrDefault() : null;

//                var languageFound = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();
//                var languageExists = languageFound != null;
//                if (imageContentData == null && languageExists)
//                {
//                    imageContentData = new ImageContentData()
//                    {
//                        Caption = "",
//                        Description = "",
//                        Translation = languageFound,
//                    };
//                    pageContent.ImageContent.ContentData.Add(imageContentData);
//                }

//                if (pageContent != null && imageContent != null && pageContentInfo != null && imageContentData != null)
//                {
//                    pageContentInfo.HasRSS = vmObj.HasRSS;
//                    pageContentInfo.IsDraft = vmObj.IsDraft;
//                    pageContentInfo.IsPublished = vmObj.IsPublished;
//                    pageContentInfo.PublishedBy = vmObj.PublishedBy;
//                    pageContentInfo.PublishedOn = vmObj.PublishedOn;
//                    pageContentInfo.ActiveFrom = vmObj.ActiveFrom;
//                    pageContentInfo.ActiveTo = vmObj.ActiveTo;

//                    imageContentData.Caption = vmObj.Caption;
//                    imageContentData.Description = vmObj.Description;


//                    imageContent.Image = vmObj.ImageHtml;
//                    imageContent.PageContentInfo = pageContentInfo;
//                    pageContent.ImageContent = imageContent;
//                    pageContent.Index = vmObj.Index;
//                    //PageContent.ViewName = vmObj.ViewName;
//                    pageContent.ViewTemplateId = vmObj.ViewTemplateId;
//                    //PageContent.PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogItem")).FirstOrDefault()
//                }
//                return pageContent;
//            });
//        }

//        [HttpGet]
//        public ActionResult Delete(int? id)
//        {
//            var obj = db.Set<PageContent>().Find(id);
//            return Delete("Delete", obj);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(ImageContentViewModel dbObj)
//        {
//            return DeleteObject<PageContent, PageContent>(dbObj.Id);
//        }

//    }
//}