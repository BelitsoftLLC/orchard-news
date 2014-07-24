using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Belitsoft.Orchard.News.Drivers
{
    public class NewsTypeDriver : ContentPartDriver<NewsTypePart>
    {
        protected override DriverResult Display(NewsTypePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_NewsType",
                                () => shapeHelper.Parts_NewsType(
                                    Title: part.Title,
                                    Id: part.Id));
        }

        protected override string Prefix
        {
            get { return "NewsType"; }
        }

        protected override DriverResult Editor(NewsTypePart part, dynamic shapeHelper)
        {
             var temp = ContentShape("Parts_NewsType_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/NewsType",
                                    Model: part,
                                    Prefix: Prefix));

            return temp;
        }

        protected override DriverResult Editor(NewsTypePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Importing(NewsTypePart part, global::Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            part.Title = context.Attribute(part.PartDefinition.Name, "Title");
        }

        protected override void Exporting(NewsTypePart part, global::Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title", part.Title);
        }
    }
}