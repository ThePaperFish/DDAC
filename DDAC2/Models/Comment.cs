using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC2.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string AuthorId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditedDate { get; set; }
    }
}

