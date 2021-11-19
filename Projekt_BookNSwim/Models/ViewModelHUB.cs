using System;
using System.Collections.Generic;
using Projekt_BookNSwim.Models;

namespace Projekt_BookNSwim.Models
{
    public class ViewModelHUB
    {
        public ViewModelHUB()
        {
        }

        public UserDetails User { get; set; }
        public IEnumerable<BookingsDetails> BookingsList { get; set; }
        public IEnumerable<HotelsDetails> HotelsList { get; set; }
    }
}
