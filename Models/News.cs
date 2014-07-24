using System;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Belitsoft.Orchard.News.Models
{
    [Serializable]
    public class NewsPartRecord : ContentPartRecord
    {
        public virtual string Title { get; set; }
        public virtual int NewsTypeId { get; set; }
        public virtual string Headline { get; set; }

    }

    [Serializable]
    public class NewsPart : ContentPart<NewsPartRecord>
    {
        [Required]
        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public int NewsTypeId
        {
            get { return Record.NewsTypeId; }
            set { Record.NewsTypeId = value; }
        }

        public string Headline
        {
            get { return Record.Headline; }
            set { Record.Headline = value; }
        }

    }
}