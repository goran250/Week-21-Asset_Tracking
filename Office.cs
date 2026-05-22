using System;
using System.ComponentModel.DataAnnotations;

namespace Asset_Tracking
{
    public class Office
    {
        public int id { get; set; }
        [MaxLength(50)]
        public string country { get; set; }
        [MaxLength(50)]
        public string city { get; set; }
        [MaxLength(50)]
        public string currencyCode { get; set; }

        public Office() {}

        public Office(string country, string city, string currencyCode)
        {
            this.country = country;
            this.city = city;
            this.currencyCode = currencyCode;
        }
    }
}

