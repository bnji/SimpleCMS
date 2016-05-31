using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class CustomContentData : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public string Data { get; set; }

        [DataMember]
        [ForeignKey("Translation")]
        public int? TranslationId { get; set; }
        [DataMember]
        [Display(Name = "Translation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ContentTranslation Translation { get; set; }

        public CustomContentData()
        {
            //            string xml = @"
            //                <Student ID=""100"">
            //                    <Name>Arul</Name>
            //                    <Mark>90</Mark>
            //                </Student>";

            //            dynamic students = DynamicXml.Parse(xml);

            //            var id = students.Student[0].ID;
            //            var name1 = students.Student[1].Name;
        }
    }
}
