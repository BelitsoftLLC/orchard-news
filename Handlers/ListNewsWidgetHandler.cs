using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Belitsoft.Orchard.News.Handlers
{
    public class ListNewsWidgetHandler : ContentHandler
    {
        public ListNewsWidgetHandler(IRepository<ListNewsWidgetPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }

        protected override void Loading(LoadContentContext context)
        {
            base.Loading(context);

            var widget = context.ContentItem.As<ListNewsWidgetPart>();

            if (widget == null)
                return;
        }
    }
}