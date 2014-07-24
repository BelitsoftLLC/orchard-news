using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Belitsoft.Orchard.News.Controllers;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.Services;
using Belitsoft.Orchard.News.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.Mvc;

namespace Belitsoft.Orchard.News.Drivers
{
    public class NewsDriver : ContentPartDriver<NewsPart>
    {
        private readonly INewsService _newsService;
        private readonly INewsTypeService _newsTypeService;

        public NewsDriver(INewsTypeService newsTypeService
            , INewsService newsService)
        {
            _newsService = newsService;
            _newsTypeService = newsTypeService;
        }

        public Localizer T { get; set; }

        protected override string Prefix
        {
            get { return "News"; }
        }

        protected override DriverResult Display(NewsPart part, string displayType, dynamic shapeHelper)
        {
            var commonPart =
              (CommonPart)part.ContentItem.Parts.FirstOrDefault(t => t.GetType() == typeof(CommonPart));
            var newsType = _newsTypeService.GetNewsType(part.NewsTypeId);

            var bodyPart =
                (BodyPart) part.ContentItem.Parts.FirstOrDefault(t => t.GetType() == typeof (BodyPart));

            if (displayType == "Detail")
            {
                return ContentShape("Parts_News_Article", () => shapeHelper.Parts_News_Article(
                    Title: part.Title,
                    Body: bodyPart.Text,
                    NewsType: newsType == null ? string.Empty : newsType.Title,
                    CreatedDate: commonPart.CreatedUtc));
            }

            return ContentShape("Parts_News",
                                () => shapeHelper.Parts_News(
                                    Title: part.Title,
                                    NewsType: newsType == null ? string.Empty : newsType.Title,
                                    CreatedDate: commonPart.CreatedUtc));
        }

        protected override DriverResult Editor(NewsPart part, dynamic shapeHelper)
        {
            var temp = ContentShape("Parts_News_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/News",
                                    Model: BuildEditorViewModel(part),
                                    Prefix: Prefix));
            return temp;
        }

        protected override DriverResult Editor(NewsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new EditNewsViewModel();

            updater.TryUpdateModel(model, Prefix, null, null);
            if (part.ContentItem.Id != 0)
            {
                _newsService.UpdateNewsForContentItem(part.ContentItem, model);
            }

            return Editor(part, shapeHelper);
        }

        private EditNewsViewModel BuildEditorViewModel(NewsPart part)
        {

            var newsTypes = _newsTypeService.GetNewsTypes();

            var avm = new EditNewsViewModel
            {
                Title = part.Title,
                Headline = part.Headline,
                NewsTypes = newsTypes == null ? new List<NewsTypePartRecord>() : newsTypes.Select(p => p.Record),
            };

            if (part.NewsTypeId > 0)
            {
                avm.NewsType = part.NewsTypeId;
            }

            return avm;
        }

        protected override void Importing(NewsPart part, global::Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            part.Title = context.Attribute(part.PartDefinition.Name, "Title");
            var newsTypeTitle = context.Attribute(part.PartDefinition.Name, "NewsTypeTitle");

            var newsTypeRecord = _newsTypeService.Where(p => p.Title == newsTypeTitle).FirstOrDefault();

            if (newsTypeRecord != null)
                part.NewsTypeId = newsTypeRecord.Id;
        }

        protected override void Exporting(NewsPart part, global::Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title", part.Title);


            var newsType = _newsTypeService.GetNewsType(part.NewsTypeId);

            if (newsType != null)
                context.Element(part.PartDefinition.Name).SetAttributeValue("NewsTypeTitle", newsType.Title);
        }
    }
}
