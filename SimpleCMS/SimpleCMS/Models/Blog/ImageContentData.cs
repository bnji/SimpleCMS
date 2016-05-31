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
    public class ImageContentData : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "Caption", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Caption { get; set; }

        [DataMember]
        [ForeignKey("Translation")]
        public int? TranslationId { get; set; }
        [DataMember]
        [Display(Name = "Translation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ContentTranslation Translation { get; set; }

        public ImageContentData()
        {

        }
    }
}
