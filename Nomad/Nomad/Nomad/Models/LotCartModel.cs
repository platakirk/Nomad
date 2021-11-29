using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomad.Models
{
    public class LotCartModel
    {
        public int LotID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Availability { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

    }
    public class LotCartViewModel
    {
        public List<LotCartModel> LotCartItems { get; set; }
    }
}