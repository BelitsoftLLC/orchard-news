using System.Collections.Generic;

namespace Belitsoft.Orchard.News.ViewModels
{
    public class ListNewsWidgetViewModel
    {
        public List<ViewNewsViewModel> News { get; set; }

        public int ViewCount { get; set; }

        public int PageNumber { get; set; }

        public double CountOfPage { get; set; }

        public bool Paging { get; set; }

    }
}