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
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult BookingConfirm()
        {
            ViewModelHUBSingle vms = new ViewModelHUBSingle();
            HotelsMethods hm = new HotelsMethods();

            string loggedUser = HttpContext.Session.GetString("loggedUser"); // we get the current user
            string currentBooking = HttpContext.Session.GetString("newBook"); // we get the recent booking
            string errormsg = "";
            vms.user = JsonConvert.DeserializeObject<UserDetails>(loggedUser); //we convert the user-json to an object
            vms.booking = JsonConvert.DeserializeObject<BookingsDetails>(currentBooking); // we convert the bookingsdetails to an object
            vms.hotel = hm.GetHotelById(vms.booking.hotel_id, out errormsg);
            ViewBag.error = errormsg;
            return View(vms);
        }

        public ActionResult BookingNotConfirmed()
        {
            string jsonBook = HttpContext.Session.GetString("newBook");
            string jsonPrice = HttpContext.Session.GetString("price");
            ViewBag.book = jsonBook;
            ViewBag.price = jsonPrice;
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewModelHUBSingle hub = new ViewModelHUBSingle();
            BookingsMethods bm = new BookingsMethods();
            HotelsMethods hm = new HotelsMethods();

            string errormsg1 = "";
            string errormsg2 = "";

            hub.booking = bm.GetBookingsByID(id, out errormsg1);
            hub.hotel = hm.GetHotelWithBookingID(id, out errormsg2);

            ViewBag.errmsg1 = errormsg1;
            ViewBag.errmsg2 = errormsg2;

            return View(hub);
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            ViewModelHUBSingle hub = new ViewModelHUBSingle();
            BookingsMethods bm = new BookingsMethods();
            HotelsMethods hm = new HotelsMethods();

            string errormsg1 = "";
            string errormsg2 = "";

            hub.booking = bm.GetBookingsByID(id, out errormsg1);
            hub.hotel = hm.GetHotelWithBookingID(id, out errormsg2);

            ViewBag.errmsg1 = errormsg1;
            ViewBag.errmsg2 = errormsg2;

            return View(hub);
        }

        [HttpPost]
        public ActionResult Cancel(int id, IFormCollection form)
        {
            try
            {
                BookingsMethods bm = new BookingsMethods();
                int i = 0;
                string bookDel = "Your booking has been cancelled.";

                i = bm.CancelBooking(id, out string errormsg);
                // HttpContext.Session.SetString("bookDelete", bookDel);
                ViewBag.errmsg = errormsg;
                return RedirectToAction("Profile", "User");
            }
            catch
            {
                 return View();
            }

        }

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
