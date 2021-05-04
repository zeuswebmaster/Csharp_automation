using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCheckoutBot.FootLocker.Models
{
    public class CheckoutBillerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public string StreetAddress { get; set; }
        public string UnitNumber { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string CCNumber { get; set; }
        public string Card_MM { get; set; }
        public string Card_YY { get; set; }
        public string Card_CSC { get; set; }

    }
}
