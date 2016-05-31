using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Helpers
{
    public class FileUploadHandlerParameters
    {
        //public string Name { get; set; }

        public bool Multiple { get; set; }

        public string Accept { get; set; }

        public string ControllerName { get; set; }
        
        public int MaxItems { get; set; }

        public object SaveFileRouteValues { get; set; }

        public object DeleteFileRouteValues { get; set; }

        public bool AutoUpload { get; set; }

        public FileUploadHandlerParameters()
        {
            MaxItems = 10;
            SaveFileRouteValues = null;
            DeleteFileRouteValues = null;
            AutoUpload = true;
        }
    }
}
