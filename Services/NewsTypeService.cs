using System;
using System.Collections.Generic;
using System.Linq;
using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;

namespace Belitsoft.Orchard.News.Services
{
    public class NewsTypeService : INewsTypeService
    {
        #region Fields

        private readonly IContentManager _contentManager;
        private readonly IRepository<NewsTypePartRecord> _newsTypeRepository;

        #endregion //Fields

        #region Constructor

        public NewsTypeService(IContentManager contentManager
            , IRepository<NewsTypePartRecord> newsTypeRepository)
        {
            _contentManager = contentManager;
            _newsTypeRepository = newsTypeRepository;
        }

        #endregion //Constructor

        #region Public Methods

        public IEnumerable<NewsTypePart> GetNewsTypes()
        {
            IContentQuery<NewsTypePart, NewsTypePartRecord> query = _contentManager.Query<NewsTypePart, NewsTypePartRecord>(VersionOptions.Published);

            var newsTypes = query
                .Where<CommonPartRecord>(p => p.PublishedUtc != null)
                .OrderByDescending(p => p.PublishedUtc)
                .List<NewsTypePart>();

            return newsTypes;
        }

        public NewsTypePart GetNewsType(int id)
        {
            IContentQuery<NewsTypePart, NewsTypePartRecord> query = _contentManager.Query<NewsTypePart, NewsTypePartRecord>(VersionOptions.Published);

            var newsTypes = query
                .Where<CommonPartRecord>(p => p.PublishedUtc != null && p.Id == id)
                .OrderByDescending(p => p.PublishedUtc)
                .List<NewsTypePart>();

            return newsTypes.FirstOrDefault();
        }

        public IEnumerable<NewsTypePartRecord> Where(Func<NewsTypePartRecord,bool> predicate)
        {
            return _newsTypeRepository.Table.Where(predicate);

        }

        #endregion //FAQ
    }
}