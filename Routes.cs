using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Belitsoft.Orchard.News
{
    public class Routes :IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Name = "News",
                    Priority = 5,
                    Route = new Route("News/{id}",
                        new RouteValueDictionary {{"area", "Belitsoft.Orchard.News"},{"controller", "News"},{"action", "Article"}},
                        new RouteValueDictionary(),
                        new RouteValueDictionary {{"area", "Belitsoft.Orchard.News"}},new MvcRouteHandler())
                }
            };
          
        }
        
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
              foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }
}