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
    public class BlogItemContentData : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [DataMember]
        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        [DataMember]
        [Display(Name = "Excerpt", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Excerpt { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [ForeignKey("Translation")]
        public int? TranslationId { get; set; }
        [DataMember]
        [Display(Name = "Translation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ContentTranslation Translation { get; set; }

        public BlogItemContentData()
        {

        }
    }
}
