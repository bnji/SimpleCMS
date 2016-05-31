using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Helpers
{
    public class CultureInfo
    {
        public int Id { get; set; }
        public string CultureIdentifier { get; set; }
        public string CultureName { get; set; }
        public string Locale { get; set; }
        public string Language { get; set; }
        public string LanguageLocal { get; set; }
        public int CallingCode { get; set; }
        public string CountryName { get; set; }
        public string CountryLocalName { get; set; }
        public int ANSIcodepage { get; set; }
        public int OEMcodepage { get; set; }
        public string Country { get; set; }
        public string LanguageAbbreviation { get; set; }
    }
}
