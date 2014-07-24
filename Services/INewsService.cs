using System.Collections.Generic;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.ViewModels;
using Orchard;
using Orchard.ContentManagement;

namespace Belitsoft.Orchard.News.Services
{
    public interface INewsService : IDependency
    {
        void UpdateNewsForContentItem(ContentItem item, EditNewsViewModel model);
        IEnumerable<NewsPart> GetLastNewsVisibleFiltered(int count, int? page = null);
        int FilterByTypeId { get; set; }
        double GetCountOfPage(int count);
        IEnumerable<NewsPart> GetAllNews();
    }
}
