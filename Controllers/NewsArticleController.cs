using System.Linq;
using System.Web.Mvc;
using Belitsoft.Orchard.News.Services;
using Orchard;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Themes;

namespace Belitsoft.Orchard.News.Controllers
{
    [Themed]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly INewsTypeService _newsTypeService;
        private readonly IOrchardServices _services;

        public NewsController(IShapeFactory shapeFactory
            , INewsService newsService
            , INewsTypeService newsTypeService
            , IOrchardServices services)
        {
            _newsService = newsService;
            _newsTypeService = newsTypeService;
            _services = services;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }


        public ActionResult Article(int id)
        {
            var newsPart =
              _newsService.GetAllNews().FirstOrDefault(p => p.Id == id);

            if(newsPart == null)
                return new HttpNotFoundResult();

            if (!_services.Authorizer.Authorize(Permissions.ViewContent, newsPart, T("Not allowed to view news")))
                return new HttpUnauthorizedResult();

            dynamic news = _services.ContentManager.BuildDisplay(newsPart, "Detail");

            var newsType = _newsTypeService.GetNewsType(newsPart.NewsTypeId);
            if(newsType == null)
                return new HttpNotFoundResult();

            return new ShapeResult(this, news);
        }
    }
}