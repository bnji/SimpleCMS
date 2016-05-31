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
using System.Drawing;

namespace BootstrapControllers
{
    public class BlogItemController : Controller<PageContent>
    {
        protected override string GetImagesPath()
        {
            return "~/SimpleCMS/Data/Images";
        }
        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files, int pageContentId)
        {
            var pageContent = repository.Get(pageContentId);
            if (pageContent == null)
                return new EmptyResult();

            var images = base.SaveImages(Files);
            if (images != null && images.Count > 0)
            {
                //var files = AddFiles(vmObj.Files);
                foreach (var imageInfo in images)
                {
                    var pageContentFile = new PageContentFile()
                    {
                        ImageInfo = imageInfo,
                        Index = db.PageContentFiles.FilterDeleted().Where(x => x.PageContentId == pageContentId).Count(),
                        PageContent = pageContent,
                        ChangeEvent = CreateChangeEvent()
                    };
                    pageContent.BlogItemContent.Files.Add(pageContentFile);
                    //db.Entry(pageContent).State = EntityState.Modified;
                    Exception exception = null;
                    repository.Save(out exception);
                }
                return Content("");
            }
            return Content("Error");
        }

        public ActionResult RemoveFile(string[] fileNames)
        {
            var fh = new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
            if (fh.Remove(fileNames))
            {
                return Content("");
            }
            return Content("Error");
        }

        protected override string GetFilePath()
        {
            return "~/SimpleCMS/Data/Images";
        }

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
            var viewTemplate = db.ViewTemplates.Where(x => x.Name.Equals("_BlogItem")).FirstOrDefault();
            viewModel = new BlogItemContentCreateOrEditViewModel()
            {
                ParentId = parentId,
                PublishedOn = DateTime.Now,
                IsPublished = true,
                HasRSS = true,
                ViewTemplateId = viewTemplate.Id,
                ViewTemplate = viewTemplate,
                Index = parent != null ? parent.Content.FilterDeleted().Count() + 1 : 0
                //ContentTranslationId = db.BlogWebsiteSettings.FirstOrDefault().DefaultLanguageId
            };

            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogItemContentCreateOrEditViewModel vmObj)
        {
            //var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            //if (parent == null)
            //{
            //    return new EmptyResult();
            //}
            //return CreateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            //{
            //    //Convert the ViewModel to DB Object (Model)
            //    var pageContent = new PageContent()
            //    {
            //        Index = vmObj.Index,
            //        //ViewName = vmObj.ViewName,
            //        ViewTemplateId = vmObj.ViewTemplateId,
            //        PageId = vmObj.ParentId,
            //        PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogList")).FirstOrDefault()
            //    };
            //    db.PageContents.Add(pageContent);
            //    return pageContent;
            //});


            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return CreateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                //Convert the ViewModel to DB Object (Model)
                //var translation = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();

                var blogItemContentData = new BlogItemContentData()
                {
                    Title = vmObj.Title,
                    Body = vmObj.Body,// vmObj.BodyHtml,
                    Excerpt = vmObj.Excerpt,
                    //Keywords = vmObj.Keywords,
                    Description = vmObj.Description,
                    TranslationId = db.BlogWebsiteSettings.FirstOrDefault().DefaultLanguageId
                    //Translation = translation
                };
                db.BlogItemContentDatas.Add(blogItemContentData);
                var pageContentInfo = new PageContentInfo()
                {
                    HasRSS = vmObj.HasRSS,
                    IsDraft = vmObj.IsDraft,
                    IsPublished = vmObj.IsPublished,
                    PublishedBy = vmObj.PublishedBy,
                    PublishedOn = vmObj.PublishedOn,
                    ActiveFrom = vmObj.ActiveFrom,
                    ActiveTo = vmObj.ActiveTo
                };
                db.PageContentInfos.Add(pageContentInfo);
                var blogItemContent = new BlogItemContent()
                {
                    Id = vmObj.Id,
                    //Title = vmObj.Title,
                    //Body = vmObj.BodyHtml,
                    //Excerpt = vmObj.Excerpt,
                    ////Keywords = vmObj.Keywords,
                    //Description = vmObj.Description,
                    PageContentInfo = pageContentInfo
                };
                blogItemContent.ContentData.Add(blogItemContentData);
                db.BlogItemContents.Add(blogItemContent);
                var pageContent = new PageContent()
                {
                    BlogItemContent = blogItemContent,
                    Index = vmObj.Index,
                    //ViewName = vmObj.ViewName,
                    ViewTemplateId = vmObj.ViewTemplateId,
                    PageId = vmObj.ParentId,
                    PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogItem")).FirstOrDefault()
                };
                var index = 0;
                //var files = AddFiles(vmObj.Files);
                //foreach (var file in files)
                //{
                //    db.Files.Add(file);
                //    var imageInfo = new ImageInfo()
                //    {
                //        Description = "",
                //        Height = -1,
                //        Width = -1,
                //        ChangeEvent = CreateChangeEvent()
                //    };
                //    using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)), FileMode.Open, FileAccess.Read))
                //    {
                //        using (Image img = Image.FromStream(stream: fs,
                //                                            useEmbeddedColorManagement: false,
                //                                            validateImageData: false))
                //        {
                //            float width = img.PhysicalDimension.Width;
                //            float height = img.PhysicalDimension.Height;
                //            //float hresolution = img.HorizontalResolution;
                //            //float vresolution = img.VerticalResolution;
                //            imageInfo.Width = (int)width;
                //            imageInfo.Height = (int)height;
                //        }
                //        fs.Close();
                //    }
                //    db.Images.Add(imageInfo);
                //    var pageContentFile = new PageContentFile()
                //    {
                //        PageContent = pageContent,
                //        ImageInfo = imageInfo,
                //        Index = index,
                //        ChangeEvent = CreateChangeEvent()
                //    };
                //    db.PageContentFiles.Add(pageContentFile);
                //    pageContent.BlogItemContent.Files.Add(pageContentFile);
                //    index++;
                //}
                //db.PageContents.Add(pageContent);


                var images = base.SaveImages(vmObj.Files);
                if (images != null && images.Count > 0)
                {
                    foreach (var imageInfo in images)
                    {
                        var pageContentFile = new PageContentFile()
                        {
                            ImageInfo = imageInfo,
                            Index = index,
                            PageContent = pageContent,
                            ChangeEvent = CreateChangeEvent()
                        };
                        pageContent.BlogItemContent.Files.Add(pageContentFile);
                        db.PageContents.Add(pageContent);
                        //db.SaveChanges();
                        index++;
                    }
                }

                return pageContent;
            });
        }

        [HttpGet]
        public ActionResult Edit(int? id, string language)
        {
            var o = db.Set<PageContent>().Where(x => x.Id == id).FirstOrDefault();
            BlogItemContentCreateOrEditViewModel viewModel = null;
            if (o != null && o.BlogItemContent != null && o.BlogItemContent.PageContentInfo != null)
            {
                var blogItemContentData = o.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
                var languageFound = db.ContentTranslations.Where(x => x.Language.ToLower().Equals(language.ToLower())).FirstOrDefault();
                var languageExists = languageFound != null;
                if (blogItemContentData == null && languageExists)
                {
                    blogItemContentData = new BlogItemContentData()
                    {
                        Title = "",
                        Body = "",
                        Description = "",
                        Excerpt = "",
                        Translation = languageFound
                    };
                }

                //AddViewBag(o);
                viewModel = new BlogItemContentCreateOrEditViewModel()
                {
                    Id = o.Id,
                    ParentId = o.PageId,
                    Index = o.Index,
                    ContentTranslation = languageFound,
                    ContentTranslationId = languageFound.Id,
                    //ViewName = o.ViewTemplate != null ? o.ViewTemplate.Name : "_BlogItem",
                    ViewTemplate = o.ViewTemplate,
                    ViewTemplateId = o.ViewTemplateId,
                    Title = blogItemContentData.Title,// o.BlogItemContent.Title,
                    Body = blogItemContentData.Body,//o.BlogItemContent.Body,
                    Excerpt = blogItemContentData.Excerpt,//o.BlogItemContent.Excerpt,
                    //Keywords = o.BlogItemContent.Keywords,
                    Description = blogItemContentData.Description,//o.BlogItemContent.Description,
                    HasRSS = o.BlogItemContent.PageContentInfo.HasRSS,
                    IsDraft = o.BlogItemContent.PageContentInfo.IsDraft,
                    IsPublished = o.BlogItemContent.PageContentInfo.IsPublished,
                    PublishedBy = o.BlogItemContent.PageContentInfo.PublishedBy,
                    PublishedOn = o.BlogItemContent.PageContentInfo.PublishedOn,
                    ActiveFrom = o.BlogItemContent.PageContentInfo.ActiveFrom,
                    ActiveTo = o.BlogItemContent.PageContentInfo.ActiveTo,
                    PageContentFiles = (o != null && o.BlogItemContent != null) ? o.BlogItemContent.Files : new List<PageContentFile>()
                };
                var files = new List<FileUploadInfo>();
                foreach (var f in o.BlogItemContent.Files)
                {
                    if (f.ImageInfo != null)
                    {
                        var image = db.Files.Where(x => x.Id == f.ImageInfo.FileId).FirstOrDefault();
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
                }
                viewModel.Files = files;
            }
            return Edit<PageContent>("CreateOrEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogItemContentCreateOrEditViewModel vmObj)
        {
            var parent = db.Pages.Where(x => x.Id == vmObj.ParentId).FirstOrDefault();
            if (parent == null)
            {
                return new EmptyResult();
            }
            return UpdateRelatedObjectUsingViewModel<Page, PageContent>(parent.Id, vmObj, (p) =>
            {
                var pageContent = db.PageContents.Where(x => x.Id == vmObj.Id).FirstOrDefault();
                var blogItemContent = pageContent != null ? pageContent.BlogItemContent : null;
                var pageContentInfo = blogItemContent != null ? blogItemContent.PageContentInfo : null;
                var blogItemContentData = blogItemContent != null && blogItemContent.ContentData != null ? blogItemContent.ContentData.Where(x => x.Translation != null && x.Translation.Id == vmObj.ContentTranslationId).FirstOrDefault() : null;

                var languageFound = db.ContentTranslations.Where(x => x.Id == vmObj.ContentTranslationId).FirstOrDefault();
                var languageExists = languageFound != null;
                if (blogItemContentData == null && languageExists)
                {
                    blogItemContentData = new BlogItemContentData()
                    {
                        Title = "",
                        Body = "",
                        Description = "",
                        Excerpt = "",
                        Translation = languageFound,
                    };
                    pageContent.BlogItemContent.ContentData.Add(blogItemContentData);
                }

                if (pageContent != null && blogItemContent != null && pageContentInfo != null && blogItemContentData != null)
                {
                    pageContentInfo.HasRSS = vmObj.HasRSS;
                    pageContentInfo.IsDraft = vmObj.IsDraft;
                    pageContentInfo.IsPublished = vmObj.IsPublished;
                    pageContentInfo.PublishedBy = vmObj.PublishedBy;
                    pageContentInfo.PublishedOn = vmObj.PublishedOn;
                    pageContentInfo.ActiveFrom = vmObj.ActiveFrom;
                    pageContentInfo.ActiveTo = vmObj.ActiveTo;

                    blogItemContentData.Title = vmObj.Title;
                    blogItemContentData.Body = vmObj.Body;// vmObj.BodyHtml;
                    blogItemContentData.Excerpt = vmObj.Excerpt;
                    blogItemContentData.Description = vmObj.Description;

                    //blogItemContent.Title = vmObj.Title;
                    //blogItemContent.Body = vmObj.BodyHtml;
                    //blogItemContent.Excerpt = vmObj.Excerpt;
                    //blogItemContent.Keywords = vmObj.Keywords;
                    //blogItemContent.Description = vmObj.Description;

                    blogItemContent.PageContentInfo = pageContentInfo;

                    //var files = AddFiles(vmObj.Files);
                    //var index = 0;
                    //foreach (var file in files)
                    //{
                    //    db.Files.Add(file);
                    //    var imageInfo = new ImageInfo()
                    //    {
                    //        Description = "",
                    //        Height = -1,
                    //        Width = -1,
                    //        File = file
                    //    };
                    //    db.Images.Add(imageInfo);
                    //    using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", file.Name)), FileMode.Open, FileAccess.Read))
                    //    {
                    //        using (Image img = Image.FromStream(stream: fs,
                    //                                            useEmbeddedColorManagement: false,
                    //                                            validateImageData: false))
                    //        {
                    //            float width = img.PhysicalDimension.Width;
                    //            float height = img.PhysicalDimension.Height;
                    //            //float hresolution = img.HorizontalResolution;
                    //            //float vresolution = img.VerticalResolution;
                    //            imageInfo.Width = (int)width;
                    //            imageInfo.Height = (int)height;
                    //        }
                    //        fs.Close();
                    //    }
                    //    var pageContentFile = new PageContentFile()
                    //    {
                    //        PageContent = pageContent,
                    //        ImageInfo = imageInfo,
                    //        Index = index,
                    //        ChangeEvent = CreateChangeEvent()
                    //    };
                    //    db.PageContentFiles.Add(pageContentFile);
                    //    blogItemContent.Files.Add(pageContentFile);
                    //    index++;
                    //}

                    pageContent.BlogItemContent = blogItemContent;
                    //pageContent.ImageContent = null;
                    pageContent.Index = vmObj.Index;
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
        public ActionResult Delete(BlogItemContentViewModel dbObj)
        {
            return DeleteObject<PageContent, PageContent>(dbObj.Id);
        }

        protected override FileUploadHandler CreateFileUploadHandler()
        {
            return new FileUploadHandler(Server, Session, false, "~/SimpleCMS/Data/Images");
        }

        //TODO: Implement new custom image functonality
        //[HttpPost]
        //public ActionResult Images_Read([DataSourceRequest] DataSourceRequest request, int? id = null)
        //{
        //    var o = db.PageContents.FilterDeleted().Where(x => x.Id == id).FirstOrDefault();
        //    if (o != null && o.BlogItemContent != null)
        //    {
        //        var files = o.BlogItemContent.Files;
        //        var filesFiltered = files.FilterDeleted();
        //        var r = filesFiltered.OrderBy(x => x.Index).Select(m => new BlogItemContentImagesViewModel()
        //        {
        //            Id = m.Id,
        //            Index = m.Index,
        //            PageContentId = m.PageContentId,
        //            ImageInfoId = m.ImageInfo != null ? m.ImageInfo.Id : -1,
        //            Width = m.ImageInfo != null ? m.ImageInfo.Width : -1,
        //            Height = m.ImageInfo != null ? m.ImageInfo.Height : -1,
        //            Description = m.ImageInfo != null ? m.ImageInfo.Description : String.Empty,
        //            FileId = (m.ImageInfo != null && m.ImageInfo.File != null) ? m.ImageInfo.File.Id : -1,
        //            Name = (m.ImageInfo != null && m.ImageInfo.File != null) ? m.ImageInfo.File.Name : string.Empty,
        //            ContentType = (m.ImageInfo != null && m.ImageInfo.File != null) ? m.ImageInfo.File.ContentType : string.Empty,
        //            Length = (m.ImageInfo != null && m.ImageInfo.File != null) ? m.ImageInfo.File.Length : -1
        //        });
        //        return Json(r.ToDataSourceResult(request));
        //    }
        //    return new EmptyResult();
        //}


        //TODO: Implement new custom image functonality
        //[HttpPost]
        //public ActionResult Images_Remove([DataSourceRequest] DataSourceRequest request, int? id = null)
        //{
        //    var o = db.PageContentFiles.FilterDeleted().Where(x => x.Id == id).FirstOrDefault();
        //    if (o != null && o.ImageInfo != null && o.ImageInfo.File != null)
        //    {
        //        //System.IO.File.Delete(Server.MapPath(Path.Combine("~/SimpleCMS/Data/Images", o.ImageInfo.File.Name)));
        //        //DeleteObject<SimpleCMS.Models.File, PageContentFile>(o.ImageInfo.File.Id);
        //        //DeleteObject<ImageInfo, PageContentFile>(o.ImageInfo.Id);
        //        return DeleteObject<PageContentFile, PageContent>(id.Value);
        //    }
        //    return new EmptyResult();
        //}

        [HttpPost]
        public JsonResult SetBlogItemImageIndex(int id, int newIndex, int oldIndex)
        {
            var o2 = db.PageContentFiles.FilterDeleted().Where(x => x.Index == newIndex).FirstOrDefault();
            if (o2 != null)
            {
                o2.Index = oldIndex;
                db.Entry(o2).State = EntityState.Modified;
                db.SaveChanges();
            }
            var o = db.PageContentFiles.FilterDeleted().Where(x => x.Id == id).FirstOrDefault();
            if (o != null)
            {

                o.Index = newIndex;
                db.Entry(o).State = EntityState.Modified;
                var result = db.SaveChanges();
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            return Json("null", JsonRequestBehavior.DenyGet);
        }

    }
}