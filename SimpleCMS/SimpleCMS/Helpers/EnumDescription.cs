using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Enum
{
    public class EnumDescription : Attribute
    {
        public string Text { get; private set; }

        public EnumDescription(string text)
        {
            this.Text = text;
        }
    }
}
