using System.Data;
using Belitsoft.Orchard.News.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Localization;

namespace Belitsoft.Orchard.News.DataMigrations
{
    public class Migrations : DataMigrationImpl
    {

        public Localizer T { get; set; }

        public Migrations()
        {
            T = NullLocalizer.Instance;
        }


        public int Create()
        {
            //news
            ContentDefinitionManager.AlterPartDefinition("NewsPart", builder => builder
              .WithField("DueDate", fieldBuilder => fieldBuilder.OfType("DateTimeField").WithDisplayName("Expired Date").WithSetting("DateTimeField.Required", "True")));

            ContentDefinitionManager.AlterTypeDefinition("News",
                                                         cfg => cfg
                                                                    .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                                                                    .WithPart("NewsPart")
                                                                    .WithPart("BodyPart")
                                                                    .Creatable().Draftable());

            //news type
            SchemaBuilder.CreateTable("NewsTypePartRecord", table => table.ContentPartRecord()
                                                                         .Column<string>("Title", cfg => cfg.WithLength(1024)));

            ContentDefinitionManager.AlterTypeDefinition("NewsType",
                                                       cfg => cfg
                                                                  .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                                                                  .WithPart("NewsTypePart")
                                                                  .Creatable());

            //last news widget
            SchemaBuilder.CreateTable("ListNewsWidgetPartRecord", table => table.ContentPartRecord()
                                                                       );

            ContentDefinitionManager.AlterPartDefinition(typeof(ListNewsWidgetPartRecord).Name, cfg => cfg.Attachable());


            ContentDefinitionManager.AlterTypeDefinition("ListNewsWidget", cfg => cfg
                                                                                .WithPart("WidgetPart")
                                                                                .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                                                                                .WithPart("ListNewsWidgetPart")
                                                                                .WithSetting("Stereotype", "Widget"));

            SchemaBuilder.CreateTable("NewsPartRecord", table => table
                                                                    .ContentPartRecord()
                                                                    .Column<string>("Title", cfg=>cfg.WithLength(1024))
                                                                    .Column<int>("NewsTypeId"));

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("NewsPartRecord", table => table
                                                                    .AddColumn<string>("Headline", cfg => cfg.WithLength(1024)));

            return 2;
        }


        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("NewsType", cfg => cfg.DisplayedAs("News Category"));
            return 3;
        }
    }
}