using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Belitsoft.Orchard.News.ViewModels;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.Services;
using Belitsoft.Orchard.News.ViewModels;
using Belitsoft.Orchard.News.Models;
using Belitsoft.Orchard.News.Services;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Themes;

namespace Belitsoft.Orchard.News.Controllers
{
    public class NewsController : Controller
    {
        private INewsService _NewsService;
        private IOrchardServices service;
        private INewsTypeService _NewsTypeService;


        public NewsController(IOrchardServices orchardServices,
                                    INewsService NewsService,
                                    INewsTypeService NewsTypeService)
        {
            service = orchardServices;
            _NewsService = NewsService;
            _NewsTypeService = NewsTypeService;
        }

        [Themed]
        public ActionResult Index(int id)
        {


            var news = service.ContentManager.Query<NewsPart>(VersionOptions.Published, "News").Where<NewsPartRecord>(d => d.Id == id);
            int countOfParts = news.Count();
            ViewNewsViewModel model = new ViewNewsViewModel();
            if (countOfParts == 1)
            {
                foreach (var part in news.List())
                {
                    if (part.ContentItem.Parts.Any(p => p.PartDefinition.Name == "AutoroutePart"))
                    {

                        AutoroutePart autoroute = part.ContentItem.As<AutoroutePart>();
                        NewsPart newsPart = part.ContentItem.As<NewsPart>();
                        BodyPart bodyPart = part.ContentItem.As<BodyPart>();
                        CommonPart commonPart = part.ContentItem.As<CommonPart>();
                        if (autoroute != null && newsPart != null)
                        {
                            string urlOfitem = autoroute.Path != null ? "/" + autoroute.Path : string.Empty;

                            string urlToSection = string.Empty;
                            var newsType = service.ContentManager.Query<NewsTypePart>(VersionOptions.Published, "NewsType").Where<NewsTypePartRecord>(s => s.Id == part.NewsTypeId);
                            if (newsType.Count() == 1)
                            {
                                foreach (var sectionPart in newsType.List())
                                {
                                    urlToSection = sectionPart.ContentItem.As<AutoroutePart>().Path;
                                }
                            }
                            string typeTitle = _NewsTypeService.GetNewsType(newsPart.NewsTypeId).Title;
                            string body = bodyPart.Text;

                            model.NewsType = typeTitle;
                            model.Title = newsPart.Title;
                            model.CreatedDate = commonPart.CreatedUtc;
                            model.Body = body;
                            model.UrlToType = urlToSection;
                        }
                    }
                }
            }



            return View(model);
        }

    }
}