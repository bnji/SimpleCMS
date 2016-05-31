using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace SimpleCMS
{
    public class FeedResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }

        private readonly SyndicationFeedFormatter feed;
        public SyndicationFeedFormatter Feed
        {
            get { return feed; }
        }

        public FeedResult(SyndicationFeedFormatter feed)
        {
            this.feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/rss+xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (feed != null)
            {
                //var settings = new XmlWriterSettings
                //{
                //    NewLineHandling = NewLineHandling.None,
                //    Indent = true,
                //    Encoding = Encoding.UTF32,
                //    ConformanceLevel = ConformanceLevel.Document,
                //    OmitXmlDeclaration = true
                //};
                //var buffer = new StringBuilder();
                //var cachedOutput = new StringWriter(buffer);
                //using (var writer = XmlWriter.Create(cachedOutput, settings))
                //{
                //    if (writer != null)
                //    {
                //        feed.WriteTo(writer);
                //        writer.Close();
                //    }
                //}

                //var xmlDoc = XDocument.Parse(buffer.ToString());
                //foreach (var element in xmlDoc.Descendants("channel").First().Descendants("item").Descendants("description"))
                //    VerifyCdataHtmlEncoding(buffer, element);

                //foreach (var element in xmlDoc.Descendants("channel").First().Descendants("description"))
                //    VerifyCdataHtmlEncoding(buffer, element);

                //buffer.Replace(" xmlns:a10=\"http://www.w3.org/2005/Atom\"", " xmlns:atom=\"http://www.w3.org/2005/Atom\"");
                //buffer.Replace("a10:", "atom:");

                //context.HttpContext.Response.Output.Write(buffer.ToString());

                using (var xmlWriter = new XmlTextWriter(response.Output))
                {
                    //xmlWriter.Settings.Encoding = new UTF8Encoding(false);
                    xmlWriter.Formatting = Formatting.Indented;
                    feed.WriteTo(xmlWriter);
                }
            }
        }

        private static void VerifyCdataHtmlEncoding(StringBuilder buffer, XElement element)
        {
            if (element.Value.Contains(""))
            {
                string cdataValue = string.Format("", element.Name, element.Value, element.Name);
                buffer.Replace(element.ToString(), cdataValue);
            }
        }
    }
}
