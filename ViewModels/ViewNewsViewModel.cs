using System;

namespace Belitsoft.Orchard.News.ViewModels
{
    public class ViewNewsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string NewsType { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public string Body { get; set; }
        
        public string UrlToType { get; set; }
        
        public string UrlToNews { get; set; }
        
        public string Headline { get; set; }
    }
}