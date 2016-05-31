namespace SimpleCMS.Migrations
{
    using DAL;
    using Models;
    using Models.Blog;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;

    //DropCreateDatabaseIfModelChanges<SimpleCMS.DAL.BootstrapContext>// 
    internal sealed class DALConfiguration : DbMigrationsConfiguration<BootstrapContext>
    {
        public DALConfiguration()
        {
            //AutomaticMigrationsEnabled = false;
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BootstrapContext db)
        {
            //  This method will be called after migrating to the latest version.
            AddLanguage(db, "fo", "Faroese");
            AddLanguage(db, "en", "English");
            AddLanguage(db, "da", "Danish");
            AddContentType(db, "BlogItem", "_BlogItem", "Vanlig blog grein");
            AddContentType(db, "BlogItem", "_BlogItemFrontPage", "Blog grein til forsíðu");
            AddContentType(db, "BlogItem", "_BlogItemLargeImage", "Blog grein við stórari mynd");
            AddContentType(db, "PageItem", "_PageItem", "Vanlig síða");
            AddContentType(db, "ImageContent", "_ImageContent", "Mynd");
            AddContentType(db, "BlogList", "_BlogList", "Listi við greinum");
            AddContentType(db, "BlogList", "_BlogListAjax", "Listi við greinum við nýtslu av AJAX");
            AddContentType(db, "BlogList", "_BlogListAjaxInfiniteScroll", "Listi við greinum í minnir um Facebook");
            AddContentType(db, "Layout", "_Layout", "Layout");
            AddContentType(db, "Layout", "_LayoutMenu", "Layout Menu");
            AddContentType(db, "Layout", "_Footer", "Footer");
            AddContentType(db, "Helper", "_Pagination", "Pagination");
            AddContentType(db, "Helper", "_ContentTranslationMissing", "Content Translation Missing");
            AddContentType(db, "Helper", "_ViewTemplateMissing", "View Template Missing");
            AddContentType(db, "Plugin", "_FBInit", "Facebook Configuration");
            AddContentType(db, "Plugin", "_RRSSB", "Ridiculously Responsive Social Sharing Buttons");
            AddContentType(db, "ContactForm", "_ContactForm", "Contact Form");
            var siteName = "Simple CMS ASP.NET Website";
            var siteDesc = "Simple CMS ASP.NET Website";
            var defaultLanguage = "fo";
            var defaultFrontpage = "frontpage";
            if (db.BlogWebsiteSettings.FirstOrDefault() == null)
            {
                db.BlogWebsiteSettings.Add(new Models.Blog.BlogWebsiteSettings()
                {
                    Name = siteName,
                    Description = siteDesc,
                    PostsPerPage = 10,
                    DefaultLanguage = db.ContentTranslations.Where(x => x.Language == defaultLanguage).FirstOrDefault(),
                    DefaultFrontPage = defaultFrontpage
                });
            }
            if (db.BlogFeedSettings.FirstOrDefault() == null)
            {
                db.BlogFeedSettings.Add(new Models.Blog.BlogFeedSettings()
                {
                    Title = siteName,
                    Description = siteDesc + " RSS",
                    BaseUri = "http://localhost:44308",
                    UriName = "rss"
                });
            }
            db.SaveChanges();
            for (int i = 1; i <= 50; i++)
            {
                AddPage(db, "page-" + i);
            }
            AddPage(db, "rss", false, true);
            AddPage(db, "post", false);
            db.SaveChanges();
            AddPlugin(db, "fitvid", new string[] { "jquery.fitvids.js", "fitvid.demo.js" });
            AddPlugin(db, "rrssb", new string[] { "rrssb.min.js", "rrssb.css" });
            AddPlugin(db, "test", new string[] { "test.js", "test.css" });
            base.Seed(db);
            db.SaveChanges();
        }

        private void AddLanguage(BootstrapContext db, string language, string description)
        {
            if (db.ContentTranslations.Where(x => x.Language == language).FirstOrDefault() == null)
            {
                db.ContentTranslations.Add(new Models.Blog.ContentTranslation()
                {
                    Language = language,
                    Description = description
                });
            }
            db.SaveChanges();
        }

        private void AddPlugin(BootstrapContext db, string name, string[] fileNames)
        {
            if (db.Plugins.Where(x => x.Name.Equals(name)).FirstOrDefault() == null)
            {
                var plugin = new Plugin()
                {
                    Name = name,
                    Description = ""
                };
                foreach (var fileName in fileNames)
                {
                    var file = CreatePluginFile(db, fileName);
                    plugin.Files.Add(new Models.Blog.PluginFile()
                    {
                        Plugin = plugin,
                        File = file
                    });
                    db.Files.Add(file);
                }
                db.Plugins.Add(plugin);
            }
        }

        private void AddPage(BootstrapContext db, string pageName, bool isBlogItem = true, bool isRssFeed = false)
        {
            if (db.Pages.Where(x => x.Name.ToLower().Equals(pageName)).FirstOrDefault() == null)
            {
                var permaLink = new Permalink()
                {
                    Name = pageName,
                    Type = null,
                    ChangeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now }
                };
                db.Permalinks.Add(permaLink);
                Page page = new Page()
                {
                    Name = pageName,
                    Title = pageName,
                    Permalink = permaLink,
                    IncludeInMenu = false,
                    IsDraft = false,
                    IsPublished = true,
                    IsRssFeed = false,
                    ChangeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now }
                };
                PageContent pageContent = null;
                if (!isRssFeed && isBlogItem)
                {
                    var contentData = new BlogItemContentData()
                    {
                        Title = pageName,
                        Description = "",
                        Body = pageName,
                        Excerpt = "",
                        Translation = db.ContentTranslations.Where(x => x.Language.ToLower().Equals("fo")).FirstOrDefault()
                    };
                    db.BlogItemContentDatas.Add(contentData);
                    var pageContentInfo = new PageContentInfo()
                    {
                        HasRSS = true,
                        IsDraft = false,
                        IsPublished = true,
                        PublishedBy = "bha",
                        PublishedOn = DateTime.Now
                    };
                    db.PageContentInfos.Add(pageContentInfo);
                    var blogItemContent = new BlogItemContent()
                    {
                        PageContentInfo = pageContentInfo
                    };
                    blogItemContent.ContentData.Add(contentData);
                    db.BlogItemContents.Add(blogItemContent);
                    pageContent = new PageContent()
                    {
                        ViewTemplate = isBlogItem ? db.ViewTemplates.Where(x => x.Name.ToLower().Equals("_BlogItem")).FirstOrDefault() : null,
                        Page = page,
                        PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogItem")).FirstOrDefault(),
                        ChangeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now }
                    };
                    if (isBlogItem)
                    {
                        pageContent.BlogItemContent = blogItemContent;
                    }
                    db.PageContents.Add(pageContent);
                }
                if (isRssFeed && !isBlogItem)
                {
                    page.IsRssFeed = true;
                    pageContent = new PageContent()
                    {
                        Page = page,
                        PageContentType = db.PageContentTypes.Where(x => x.Name.Equals("BlogList")).FirstOrDefault(),
                        ChangeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now }
                    };
                    db.PageContents.Add(pageContent);
                }
                //if (isBlogItem)
                //{
                //    page.Content.Add(pageContent);
                //}
                db.Pages.Add(page);
                db.SaveChanges();
            }
        }

        private void AddContentType(BootstrapContext db, string pageContentTypeName, string fileName, string description)
        {
            var pageContentType = db.PageContentTypes.Where(x => x.Name.ToLower().Equals(pageContentTypeName.ToLower())).FirstOrDefault();
            if (pageContentType == null)
            {
                pageContentType = new Models.Blog.PageContentType()
                {
                    Name = pageContentTypeName,
                    Description = description
                };
                db.PageContentTypes.Add(pageContentType);
            }
            if (db.ViewTemplates.Where(x => x.Name.ToLower().Equals(fileName.ToLower())).FirstOrDefault() == null)
            {
                var file = CreateBlogViewTemplateFile(db, fileName);
                var viewTemplate = new Models.Blog.ViewTemplate()
                {
                    Name = file.Name,
                    Description = "",
                    PageContentType = pageContentType
                };
                viewTemplate.Files.Add(new Models.Blog.ViewTemplateFile()
                {
                    ViewTemplate = viewTemplate,
                    File = file
                });
                db.Files.Add(file);
                db.ViewTemplates.Add(viewTemplate);
            }
            db.SaveChanges();
        }

        private File CreateBlogViewTemplateFile(BootstrapContext db, string name)
        {
            System.IO.FileInfo fileInfo = null;
            //if (IsOnAzure())
            //{
            //    fileInfo = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/Views/Shared/BlogViewTemplates/" + name + ".cshtml"));
            //}
            //else
            //{
            //    fileInfo = new System.IO.FileInfo(@"..\Views\Shared\BlogViewTemplates\" + name + ".cshtml");
            //}
            fileInfo = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/SimpleCMS/Views/Shared/BlogViewTemplates/" + name + ".cshtml"));
            var data = System.IO.File.ReadAllBytes(fileInfo.FullName);
            var changeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now };
            db.ChangeEvents.Add(changeEvent);
            return new Models.File()
            {
                Name = name,
                ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                Length = fileInfo.Length,
                Data = data,
                ChangeEvent = changeEvent
            };
        }

        private File CreatePluginFile(BootstrapContext db, string fileName)
        {
            System.IO.FileInfo fileInfo = null;
            //if (IsOnAzure())
            //{
            //    fileInfo = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/SimpleCMS/Data/Plugins/" + fileName));
            //}
            //else
            //{
            //    fileInfo = new System.IO.FileInfo(@"..\Data\Plugins\" + fileName);
            //}
            fileInfo = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/SimpleCMS/Data/Plugins/" + fileName));
            var data = System.IO.File.ReadAllBytes(fileInfo.FullName);
            var changeEvent = new ChangeEvent() { CreatedByUserIdCode = "bha", CreatedOn = DateTime.Now, UpdatedByUserIdCode = "bha", UpdatedOn = DateTime.Now };
            db.ChangeEvents.Add(changeEvent);
            return new Models.File()
            {
                Name = fileName,
                ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                Length = fileInfo.Length,
                Data = data,
                ChangeEvent = changeEvent
            };
        }

        private bool IsOnAzure()
        {
            return !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        }
    }
}