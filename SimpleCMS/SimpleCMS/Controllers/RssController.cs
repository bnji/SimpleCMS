using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using SimpleCMS.DAL;
using SimpleCMS.ViewModels;
using SimpleCMS.Models;
using System.Xml;
using System.Xml.Linq;
using SimpleCMS.Models.Blog;
using SimpleCMS;

namespace BootstrapControllers
{
    //[RequireHttps]
    [AllowAnonymous]
    // https://msdn.microsoft.com/en-us/library/bb412174(v=vs.110).aspx
    // https://damieng.com/blog/2010/04/26/creating-rss-feeds-in-asp-net-mvc
    public class RssController : Controller<BlogFeedSettings>
    {
        public static Rss20FeedFormatter GetRssFeed(BootstrapContext db, string type, string language = "fo", int itemsCount = 10)
        {
            var feedSettings = db.BlogFeedSettings.FirstOrDefault();
            var baseUrl = feedSettings.BaseUri;
            SyndicationFeed feed = new SyndicationFeed()
            {
                Title = new TextSyndicationContent(feedSettings.Title, TextSyndicationContentKind.Html),
                Description = new TextSyndicationContent(feedSettings.Description),
                BaseUri = new Uri(feedSettings.BaseUri + "/" + feedSettings.UriName)
            };
            // TODO: 
            feed.Authors.Add(new SyndicationPerson("someone@microsoft.com"));
            feed.Categories.Add(new SyndicationCategory("How To Sample Code"));
            List<SyndicationItem> items = new List<SyndicationItem>();
            var blogItems = db.PageContents.FilterDeleted().Where(x => x.BlogItemContent != null && x.BlogItemContent.ContentData != null && x.BlogItemContent.PageContentInfo.HasRSS && x.BlogItemContent.ContentData.Where(y => y.Translation.Language.ToLower().Equals(("" + language).ToLower())).FirstOrDefault() != null).OrderByDescending(x => x.ChangeEvent.CreatedOn).Take(itemsCount * 5).ToList();
            var c = 0;
            if (blogItems != null && blogItems.Count > 0)
            {
                do
                {
                    var blogItem = blogItems[c];
                    if (blogItem != null && !blogItem.ChangeEvent.IsDeleted)
                    {
                        var blogData = blogItem.BlogItemContent.ContentData.Where(x => x.Translation.Language.ToLower().Equals(("" + language).ToLower())).FirstOrDefault();
                        if (blogData != null)
                        {
                            var syndicationItem = new SyndicationItem()
                            {
                                Title = new TextSyndicationContent(blogData.Title, TextSyndicationContentKind.Plaintext),
                                Content = new TextSyndicationContent(blogData.Body, TextSyndicationContentKind.Html),
                                Summary = new TextSyndicationContent(blogData.Excerpt),
                                BaseUri = new Uri(baseUrl + "/" + blogItem.Page.Permalink.Name),
                                Id = ("" + blogData.Id),
                                LastUpdatedTime = blogItem.ChangeEvent.UpdatedOn,
                                PublishDate = blogItem.BlogItemContent.PageContentInfo.PublishedOn.Value
                            };
                            syndicationItem.Authors.Add(new SyndicationPerson()
                            {
                                Name = blogItem.PageContentInfo.PublishedBy
                                //Email = ""
                            });
                            //items.Add(syndicationItem);
                            items.Add(new SyndicationItem(
                                blogData.Title,
                                new TextSyndicationContent(HttpUtility.HtmlDecode(blogData.Body), TextSyndicationContentKind.Html),
                                new Uri(baseUrl + "/" + blogItem.Page.Permalink.Name),
                                ("" + blogData.Id),
                                DateTime.Now)
                            );
                        }
                        c++;
                    }
                } while (c < itemsCount);
            }
            feed.Items = items;
            //XNamespace atom = "http://www.w3.org/2005/Atom";
            //feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName), atom.NamespaceName);
            var formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;
            return formatter;
        }

        [HttpGet]
        public ActionResult Index(string type = "rss", string language = "fo", int itemsCount = 10)
        {
            //var langName = SharedLibrary.Web.CookieHelper.GetCultureName(Request);
            //langName = !string.IsNullOrEmpty(langName) ? langName : "fo";
            //return new FeedResult(GetRssFeed(db, "rss", langName, 10));
            return new FeedResult(GetRssFeed(db, type, language, itemsCount));
        }

        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var langName = SharedLibrary.Web.CookieHelper.GetCultureName(Request);
        //    langName = !string.IsNullOrEmpty(langName) ? langName : "fo";
        //    return new FeedResult(GetRssFeed(db, "rss", langName, 10));
        //}

        protected override void AddViewBag(BlogFeedSettings obj)
        {
        }
    }
}