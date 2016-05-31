using SimpleCMS.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{

    public interface IPageContentWithChangeEvent : IHasId
    {
        int? PageContentId { get; set; }
        PageContent PageContent { get; set; }

        int? ChangeEventId { get; set; }
        IChangeEvent ChangeEvent { get; set; }
    }
}