using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Belitsoft.Orchard.News.Models;

namespace Belitsoft.Orchard.News.ViewModels
{
    public class EditNewsViewModel
    {
        [Required(ErrorMessage = "Fill the required field 'Title'")]
        public string Title { get; set; }

        public string Headline { get; set; }

        [Required(ErrorMessage = "Fill the required field 'News Type'")]
        public int NewsType { get; set; }

        [Required]
        public DateTime NewsDate { get; set; }

        public IEnumerable<NewsTypePartRecord> NewsTypes { get; set; }
    }
}