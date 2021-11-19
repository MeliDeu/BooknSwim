using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class ViewModelHUBSingle
    {
        public ViewModelHUBSingle()
        {
        }

        public UserDetails user { get; set; }
        public BookingsDetails booking { get; set; }
        public HotelsDetails hotel { get; set; }
    }
}
