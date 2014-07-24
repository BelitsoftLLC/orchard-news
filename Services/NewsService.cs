using System;
using System.Collections.Generic;
using System.Linq;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.ViewModels;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Fields.Fields;

namespace Belitsoft.Orchard.News.Services
{
    public class NewsService : INewsService
    {
        #region Fields

        private readonly IRepository<NewsPartRecord> _newsRepository;
        private readonly IContentManager _contentManager;
        private int _typeIdFilter = -1;

        #endregion //Fields

        #region Constructor

        public NewsService(IRepository<NewsPartRecord> newsRepository, IContentManager contentManager)
        {
            _newsRepository = newsRepository;
            _contentManager = contentManager;
        }

        #endregion //Consructor

        #region Properties

        public int FilterByTypeId
        {
            get { return _typeIdFilter; }
            set { _typeIdFilter = value; }
        }

        #endregion //Properties

        #region Public Methods

        public IEnumerable<NewsPart> GetLastNewsVisibleFiltered(int count, int? page = null)
        {
            IContentQuery<NewsPart, NewsPartRecord> query = _contentManager.Query<NewsPart, NewsPartRecord>(VersionOptions.Published);

            if (_typeIdFilter != -1)
            {
                query = query.Where<NewsPartRecord>(t => t.NewsTypeId == _typeIdFilter);
            }

            var publishedNews = query
                .Where<CommonPartRecord>(p => p.PublishedUtc != null)
                .OrderByDescending(p => p.PublishedUtc)
                .List<NewsPart>()
                .Where(p => ((DateTimeField) p.Get(typeof (DateTimeField), "DueDate")) == null ||
                            ((DateTimeField) p.Get(typeof (DateTimeField), "DueDate")).DateTime.ToUniversalTime() >=
                            DateTime.Now.ToUniversalTime() ||
                            ((DateTimeField) p.Get(typeof (DateTimeField), "DueDate")).DateTime <
                            DateTime.Now.AddYears(-100));

            if (page != null)
            {
                var temp = publishedNews.Skip((page.Value - 1) * count)
                    .Take(count);
                return temp.ToList();
            }

            return publishedNews.Take(count);
        }

        public IEnumerable<NewsPart> GetAllNews()
        {
            IContentQuery<NewsPart, NewsPartRecord> query = _contentManager.Query<NewsPart, NewsPartRecord>(VersionOptions.Published);

            var news = query
               .Where<CommonPartRecord>(p => p.PublishedUtc != null)
               .OrderByDescending(p => p.PublishedUtc)
               .List<NewsPart>()
               .Where(p => ((DateTimeField)p.Get(typeof(DateTimeField), "DueDate")) == null ||
                           ((DateTimeField)p.Get(typeof(DateTimeField), "DueDate")).DateTime.ToUniversalTime() >=
                           DateTime.Now.ToUniversalTime() ||
                           ((DateTimeField)p.Get(typeof(DateTimeField), "DueDate")).DateTime <
                           DateTime.Now.AddYears(-100));

            return news;

        }

        public double GetCountOfPage(int count)
        {
            IContentQuery<NewsPart, NewsPartRecord> query = _contentManager.Query<NewsPart, NewsPartRecord>(VersionOptions.Published);

            if (_typeIdFilter != -1)
            {
                query = query.Where<NewsPartRecord>(t => t.NewsTypeId == _typeIdFilter);
            }

            var publishedNews = query
                .Where<CommonPartRecord>(p => p.PublishedUtc != null)
                .OrderBy(p => p.PublishedUtc)
                .List<NewsPart>()
                .Where(p => ((DateTimeField)p
                    .Get(typeof(DateTimeField), "DueDate")).DateTime.ToUniversalTime() >= DateTime.Now.ToUniversalTime() ||
                    ((DateTimeField)p.Get(typeof(DateTimeField), "DueDate")) == null||
                    ((DateTimeField)p.Get(typeof(DateTimeField), "DueDate")).DateTime < DateTime.Now.AddYears(-100));

         
            return Math.Ceiling(publishedNews.Count() / (float)count / 1.0);
        }

        public void UpdateNewsForContentItem(ContentItem item, EditNewsViewModel model)
        {
            var newsPart = item.As<NewsPart>();
            newsPart.Title = model.Title;
            newsPart.NewsTypeId = model.NewsType;
            newsPart.Headline = model.Headline;
            _newsRepository.Flush();
        }

        #endregion //Public Methods

    }
}