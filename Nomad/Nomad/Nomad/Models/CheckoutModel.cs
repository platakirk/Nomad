using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomad.Models
{
    public class CheckoutModel
    {
        [Key]
        [Display(Name ="Reservation ID")]
        public int ReservationID  { get; set; }

        public DateTime ReservationTimeIn { get; set; }

        public DateTime ReservationTimeOut { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public List<LotCartModel> LotCartItems { get; set; }
    }
}