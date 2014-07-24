using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Belitsoft.Orchard.News.Models
{
    public class NewsTypePartRecord : ContentPartRecord
    {
        public virtual string Title { get; set; }
    }

    public class NewsTypePart : ContentPart<NewsTypePartRecord>
    {
        [Required(ErrorMessage = "Fill the required field 'Title'")]
        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }
    }
}