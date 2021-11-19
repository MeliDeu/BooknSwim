using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class HotelsDetails
    {
        public HotelsDetails()
        {
        }
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string postal { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int pricing { get; set; }
        public string opening_time { get; set; }
        public string closing_time { get; set; }
        public int max_visitors { get; set; }
        public int current_amout_visitors { get; set; }
        public bool availability { get; set; }
    }
}

