using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC2.Models
{
    public class Project
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string CoverPhoto { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditedDate { get; set; }
    }
}

