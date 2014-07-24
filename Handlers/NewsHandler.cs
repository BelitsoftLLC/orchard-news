using System;
using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Fields.Fields;
using Orchard.Tasks.Indexing;

namespace Belitsoft.Orchard.News.Handlers
{
    public class NewsHandler : ContentHandler
    {
        public NewsHandler(IRepository<NewsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            OnRemoved<NewsPart>((context, part) => repository.Delete(part.Record));

            OnIndexing<NewsPart>((context, contactPart) =>
                                     {
                                         var date = ((DateTimeField)contactPart.Get(typeof(DateTimeField), "DueDate")).DateTime;
                                         if (date > DateTime.Now || date.Equals(DateTime.MinValue))
                                             context.DocumentIndex.Add("news_title", contactPart.Title).Analyze().Store();
                                     });
        }
    }
}