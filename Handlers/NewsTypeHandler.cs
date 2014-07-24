using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Belitsoft.Orchard.News.Handlers
{
    public class NewsTypeHandler : ContentHandler
    {
        public NewsTypeHandler(IRepository<NewsTypePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            OnRemoved<NewsTypePart>((context, part) => repository.Delete(part.Record));
            OnIndexing<NewsTypePart>((context, contactPart) =>context.DocumentIndex.Add("newstype_title", contactPart.Title).Analyze().Store());
        }
    }
}