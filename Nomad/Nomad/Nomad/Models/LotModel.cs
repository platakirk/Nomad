using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomad.Models
{
    public class LotModel
    {
        [Key]
        public int LotID { get; set; }

        [Display(Name = "Type of User")]
        [Required(ErrorMessage = "User")]
        public int UserID { get; set; }

        [Display(Name = "Lot Name")]
        [Required(ErrorMessage = "Please enter the Lot's Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the location of the lot")]
        [MaxLength(100)]
        public string Location { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Lot Availability")]
        public int Availability { get; set; }

        public int Upvotes { get; set; }

        
        public int Downvotes { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }
    }
}