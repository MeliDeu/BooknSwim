using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class BookingsDetails
    {
        public BookingsDetails()
        {
        }
        public int id { get; set; }
        public int hotel_id { get; set; }
        public int user_id { get; set; }
        public DateTime start_date { get; set; }
        //public DateTime end_date { get; set; }
        public string time { get; set; }
        public double total_price { get; set; }
        public int total_visitors { get; set; }
        public string hotel_name { get; set; }
        public string hotel_address { get; set; }
    }
}
