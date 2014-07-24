using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.Services;
using Belitsoft.Orchard.News.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Belitsoft.Orchard.News.Drivers
{
    public class NewsDriver : ContentPartDriver<NewsPart>
    {
        private readonly INewsTypeService _newsService;

        public NewsDriver(INewsTypeService service)
        {
            _newsService = service;
        }

        protected override string Prefix
        {
            get { return "News"; }
        }

        protected override DriverResult Display(NewsPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_News",
                                () => shapeHelper.Parts_News(
                                    Title: part.Title,
                                    NewsType: part.NewsTypeId != 0 ? _newsService.GetNewsType(part.NewsTypeId).Title : string.Empty));
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

        //POST
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
            var avm = new EditNewsViewModel
            {
                Title  = part.Title,
                NewsTypes = _newsService.GetNewsTypes()
            };

            if (part.NewsTypeId > 0)
            {
                avm.NewsType = part.NewsTypeId;
            }

            return avm;
        }
    }
}
