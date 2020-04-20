using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC2.Models
{
    public class Post
    {
        public int ID { get; set; }

        [Display(Name = "Post Title")]
        //[Required]
        //[StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Cover Photo")]
        public string CoverPhoto { get; set; }

        [Display(Name = "Post Content")]
        public string Content { get; set; }

        [Display(Name = "Published Date")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Last Edit")]
        [DataType(DataType.Date)]
        public DateTime EditedDate { get; set; }

        //[Required]
        //[StringLength(15, MinimumLength = 3)]
        public string Tag { get; set; }
    }
}
