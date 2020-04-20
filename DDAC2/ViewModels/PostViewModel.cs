using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC2.ViewModels
{
    public class PostViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public IFormFile CoverPhoto { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public string Tag { get; set; }
    }
}
