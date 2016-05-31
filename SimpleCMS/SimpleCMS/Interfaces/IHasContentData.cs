using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCMS
{
    public interface IHasContentData<T> where T : class
    {
        ICollection<T> ContentData { get; set; }
    }

    public interface IPublishableContent
    {
        bool IsPublished { get; set; }
        DateTime? ActiveFrom { get; set; }
        DateTime? ActiveTo { get; set; }
    }
}
