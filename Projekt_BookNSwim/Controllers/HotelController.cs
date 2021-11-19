using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projekt_BookNSwim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Controllers
{
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HotelMainPage()
        {

            List<HotelsDetails> hotelList = new List<HotelsDetails>();
            HotelsMethods methods = new HotelsMethods();

            string errormsg = "";

            hotelList = methods.GetHotels(out errormsg);


            ViewBag.errmsg = errormsg;
            return View(hotelList);
        }

        [HttpPost]
        public ActionResult HotelMainPage(IFormCollection form)
        {
            List<HotelsDetails> hotelList = new List<HotelsDetails>();
            HotelsMethods methods = new HotelsMethods();

            HotelsDetails hotel = new HotelsDetails();

            hotel.name = form["nameSearch"];
            hotel.city = form["citySearch"];
            hotel.country = form["countryFilter"];

            string errormsg = "";

            Console.WriteLine(hotel.city);

            if (hotel.name != "")
            {
                hotelList = methods.SearchHotelName(hotel, out errormsg);
            }
            else if (hotel.city != "")
            {
                hotelList = methods.SearchHotelCity(hotel, out errormsg);
            }
            else if (hotel.country != "")
            {
                hotelList = methods.FilterHotelCountry(hotel, out errormsg);
            }

            //string list = JsonConvert.SerializeObject( hotelList );

            //Console.WriteLine( list );
            //HttpContext.Session.SetString( "hotelList", list );
            ViewBag.errmsg = errormsg;

            return View(hotelList);
        }

        [HttpGet]
        public ActionResult Book(int id)
        {

            if (HttpContext.Session.GetString("loggedUserId") == null)
            {
                return RedirectToAction("Login", "User");
            }

            HotelsDetails hotel = new HotelsDetails();
            HotelsMethods methods = new HotelsMethods();

            string errormsg = "";

            hotel = methods.GetHotelById(id, out errormsg);

            Console.WriteLine(hotel);
            HttpContext.Session.SetInt32("hotelBookingId", id);
            HttpContext.Session.SetInt32("hotelPrice", hotel.pricing);
            errormsg = HttpContext.Session.GetString("errorBook");
            ViewBag.errmsg = errormsg;
            return View(hotel);
        }

        [HttpPost]
        public ActionResult Book(IFormCollection form)
        {
            BookingsDetails newBooking = new BookingsDetails();
            BookingsMethods bm = new BookingsMethods();
            HotelsMethods hm = new HotelsMethods();

            int hotelPrice = (int)HttpContext.Session.GetInt32("hotelPrice"); // gets the hotel price

            newBooking.hotel_id = (int)HttpContext.Session.GetInt32("hotelBookingId"); // the hotel ID
            newBooking.user_id = int.Parse(HttpContext.Session.GetString("loggedUserId")); // the user ID
            newBooking.start_date = DateTime.Parse(form["date"]); // the date
            newBooking.time = form["start-time"] + "-" + form["stop-time"]; // the time --> obs INT in db
            //newBooking.total_price = (float)int.Parse(form["visitors"]) * hotelPrice; // 
            newBooking.total_visitors = int.Parse(form["visitors"]);

            string errormsg = "";

            //things changed:
            // 1. changed type in db for time to string/nvar
            // 2. the total price is calculated by hour and visitors
            // 3. newbooking.totalprice till det nedan
            // 4. lagt till check om enddate är före startdate och lagt in en error i session som hämtas in och sparas i viewbag i getten ovan
            // 5. gjort en kontroll att datum ligger efter dagens datan
           
            // - make a calc-method, where the time is converted and price is calculated, as well as checked if the endtime is bigger than the start
            double totalPrice = bm.CalculateCost(form["start-time"], form["stop-time"], hotelPrice, int.Parse(form["visitors"]), out errormsg);

            // gets todays date to check if the date is in the future
            DateTime now = DateTime.Today;

            // if both are correct/true (date after today and totalprice !=0 then we write to the database, otherwise not, then we show a errormsg
            if (totalPrice != 0 && now.Date < newBooking.start_date.Date)
            {
                newBooking.total_price = totalPrice;

                string jsonbook = JsonConvert.SerializeObject(newBooking);
                HttpContext.Session.SetString("newBook", jsonbook);

                HttpContext.Session.SetString("errorBook", "");

                int i = bm.NewBooking(newBooking, out errormsg);

                if (i > 0)
                {
                    return RedirectToAction("BookingConfirm", "Booking");
                }
                else
                {
                    return RedirectToAction("BookingNotConfirmed", "Booking");
                }
            }

            errormsg = (now.Date > newBooking.start_date.Date) ? "Choose a date in the future" : errormsg; // if a date in the past is chosen the errormsg changes otherwise it is the same 
            ViewBag.errmsg = errormsg;
            HttpContext.Session.SetString("errorBook", errormsg);

            return View(hm.GetHotelById(newBooking.hotel_id, out errormsg)); // if the user leaves wrong input, then the same page will be reloaded with the current hotel 
        }
    }
}
