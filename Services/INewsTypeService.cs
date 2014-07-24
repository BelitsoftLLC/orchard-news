using System;
using System.Collections.Generic;
using Belitsoft.Orchard.News.Models;
using Orchard;

namespace Belitsoft.Orchard.News.Services
{
    public interface INewsTypeService : IDependency
    {
        IEnumerable<NewsTypePart> GetNewsTypes();
        NewsTypePart GetNewsType(int id);
        IEnumerable<NewsTypePartRecord> Where(Func<NewsTypePartRecord, bool> predicate);
    }
}