using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;
using System.Xml;
using System.Web;

namespace SimpleCMS.Controllers.API
{
    public class RSSController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string url, string type = null)
        {
            var newsItems = new List<NewsItem>();
            try
            {
                var r = XmlReader.Create(url);
                var d = SyndicationFeed.Load(r);
                foreach (SyndicationItem item in d.Items)
                {
                    newsItems.Add(new NewsItem()
                    {
                        Date = item.PublishDate.DateTime.ToShortDateString(),
                        Title = item.Title.Text,
                        Content = HttpUtility.HtmlDecode(item.Summary.Text)
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            return Request.GetResponse(newsItems, type);
        }

        public class NewsItem
        {
            public string Date { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }
    }
}
