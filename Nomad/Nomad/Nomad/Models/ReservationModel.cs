using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomad.Models
{
    public class ReservationModel
    {
        [Key]
        public int ReservationID { get; set; }

        [Display(Name = "Lot")]
        [Required(ErrorMessage = "Please enter the lot you want to reserve")]
        public int LotID { get; set; }

        //whoever is currently logged in
        public int UserID { get; set; }

        public DateTime ReservationTime { get; set; }

        public DateTime DateCreated { get; set; }

    }
}