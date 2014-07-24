using System.Collections.Generic;
using System.Linq;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.Services;
using Belitsoft.Orchard.News.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;

namespace Belitsoft.Orchard.News.Drivers
{
    public class ListNewsWidgetDriver : ContentPartDriver<ListNewsWidgetPart>
    {
        private readonly INewsService _newsService;
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "LastNewsWidget"; }
        }

        public ListNewsWidgetDriver(INewsService documentsService
            , IContentManager contentManager)
        {
            _newsService = documentsService;
            _contentManager = contentManager;
        }

        protected override DriverResult Display(ListNewsWidgetPart part, string displayType, dynamic shapeHelper)
        {
            IEnumerable<ViewNewsViewModel> news = null;
            news = _newsService.GetAllNews().Select(BuildNews).Where(p=>p.NewsType != string.Empty);

            ListNewsWidgetViewModel model = new ListNewsWidgetViewModel
                                                {
                                                    News = news.ToList(),
                                                };

            return ContentShape("Parts_ListNewsWidget",
                                () => shapeHelper.Parts_ListNewsWidget(
                                    News: model
                                    ));
        }

        protected override DriverResult Editor(ListNewsWidgetPart part, dynamic shapeHelper)
        {
            var temp = ContentShape("Parts_ListNewsWidget_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/ListNewsWidget",
                                    Model: part,
                                    Prefix: Prefix));
            return temp;
        }

        protected override DriverResult Editor(ListNewsWidgetPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }

        private ViewNewsViewModel BuildNews(NewsPart part)
        {
            ViewNewsViewModel model = new ViewNewsViewModel();
            string docTypeUrl = string.Empty;
            string docTypeTitle = string.Empty;
            var newsType =
                _contentManager.Query<NewsTypePart>(VersionOptions.Published, "NewsType")
                               .Where<NewsTypePartRecord>(t => t.Id == part.NewsTypeId);
            if (newsType.Count() == 1)
            {
                foreach (var type in newsType.List())
                {
                    docTypeTitle = type.Title;
                }
            }
            CommonPart commonPart = (CommonPart)part.ContentItem.Parts.FirstOrDefault(t => t.GetType() == typeof(CommonPart));
            if (commonPart != null)
                model.CreatedDate = commonPart.CreatedUtc;
            BodyPart bodyPart = part.ContentItem.As<BodyPart>();
            model.Body = bodyPart.Text;
            model.NewsType = docTypeTitle;
            model.UrlToType = docTypeUrl;
            model.Title = part.Title;
            model.Headline = part.Headline;
            model.Id = part.Id;
            return model;
        }

    }
}