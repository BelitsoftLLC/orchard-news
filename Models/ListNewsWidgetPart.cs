using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Belitsoft.Orchard.News.Models
{

    public class ListNewsWidgetPartRecord : ContentPartRecord
    {
    }

    public class ListNewsWidgetPart : ContentPart<ListNewsWidgetPartRecord>
    {
        public string NewsType { get; set; }
        
        public int NewsTypeId { get; set; }
    }
}