using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public abstract class ViewModelBaseWithChangeEvent3 : ViewModelBaseWithChangeEvent, IChangeEvent
    {
        public int? ChangeEventId { get; set; }

        public ChangeEvent ChangeEvent { get; set; }
    }
}