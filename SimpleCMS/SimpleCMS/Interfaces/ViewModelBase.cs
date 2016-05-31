using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public interface IViewModelBase
    {
        int Id { get; set; }
    }

    public abstract class ViewModelBase : IViewModelBase
    {
        public int Id { get; set; }
    }

    public abstract class ViewModelBaseWithChangeEvent : IHasChangeEvent, IViewModelBase
    {
        public int Id { get; set; }
    }

    //public abstract class ViewModelBaseWithPageContentAndChangeEvent : IHasPageContentWithChangeEvent, IViewModelBase
    //{
    //    public int Id { get; set; }
    //}
}