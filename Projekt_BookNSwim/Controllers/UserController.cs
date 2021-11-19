using BookNSwin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_BookNSwim.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Projekt_BookNSwim.Controllers
{
    public class UserController : Controller
    {
        //        De controllers vi kommer använda är
        //        Sköter inlogg och resterande av det administrativa för användarna
        //        Genererar startsida, inlogg- och registreringssidor och Min sida.

        // GET: User
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("loggedUserId") != null)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }

        // ----------------------------------- LOGIN --------------------------------------//

        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("loggedUserId") != null)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection collection)
        {
            UserDetails ud = new UserDetails();
            UserMethods um = new UserMethods();

            // get the username and check database --> if there, return the person
            ud.userUserName = collection["userUserName"];
            ud.userPassword = collection["userPassword"];

            string errormsg = "";

            var i = um.checkForUser(ud.userUserName, out errormsg);
            // if the user is in the db, the user is returned, otherwise null
            if (i != null)
            {
                //  check the persons password if it is the same.. if yes, redirect to mypages and save the user to the session / cookie
                if (i.userPassword == ud.userPassword)
                {
                    // send to mypages and set the user as a sessionvariable
                    errormsg = "yeah the password is correct as well";

                    //makes object to json
                    string userJson = JsonConvert.SerializeObject(i);
                    //saves object to session
                    HttpContext.Session.SetString("loggedUser", userJson); // saves the user
                    HttpContext.Session.SetString("loggedUserId", i.userId.ToString()); // saves only the ID

                    ViewBag.user = userJson;
                    ViewBag.userId = i.userId.ToString();
                    return RedirectToAction("Profile");
                }
                else
                {
                    errormsg = "The password is not correct";
                }
                // if no, errormessage
            }
            else
            {
                // if there, show errormsg
                errormsg = "The username does not exist, please register a new user";
            }
            ViewBag.error = errormsg;
            ViewBag.loggedUser = HttpContext.Session.GetString("loggedUserId");
            return View();
        }

        // ----------------------------------- Logout --------------------------------------//

        public IActionResult Logout()
        {
            // om sessionsvariabeln har satts dvs en person har loggat in, visas profilen, annars ej då redirect
            if (HttpContext.Session.GetString("loggedUserId") == null)
            {
                return RedirectToAction("Login");
            }

            HttpContext.Session.Remove("loggedUserId");
            HttpContext.Session.Remove("loggedUser");
            HttpContext.Session.Remove("newBook");
            HttpContext.Session.Remove("errorBook");
            HttpContext.Session.Remove("hotelPrice");
            HttpContext.Session.Remove("hotelBookingId");

            ViewBag.logout = "You are now being logged out and sent back to the login page!";
            return View();
        }


        // ----------------------------------- REGISTER --------------------------------------//

        [HttpGet]
        public ActionResult Register()
        {
            if (HttpContext.Session.GetString("loggedUserId") != null)
            {
                return RedirectToAction("Profile");
            }
            //UserDetails ud = new UserDetails();
            //UserMethods um = new UserMethods();
            return View();
        }

        [HttpPost]
        public ActionResult Register(IFormCollection collection)
        {
            UserDetails ud = new UserDetails();
            UserMethods um = new UserMethods();

            // get the indata from all the fields 
            ud.userUserName = collection["userUserName"];
            ud.userPassword = collection["userPassword"];
            ud.userEmail = collection["userEmail"];
            ud.userFirstName = collection["userFirstName"];
            ud.userLastName = collection["userLastName"];

            string errormsg = "";

            // check for the username in db
            var j = um.checkForUser(ud.userUserName, out errormsg);

            if (j != null)
            {
                // if there, show errormsg
                errormsg = "The username already exists";

            }
            else
            {

                // if not there yet, insert to db 
                int i = um.RegisterUser(ud, out errormsg);
                return RedirectToAction("Login");
            }
            ViewBag.error = errormsg;

            // if not ok, stay on page with errormsg
            return View();
        }

        // ----------------------------------- Profile --------------------------------------//


        [HttpGet]
        public IActionResult Profile()
        {
            // om sessionsvariabeln har satts dvs en person har loggat in, visas profilen, annars ej då redirect
            if (HttpContext.Session.GetString("loggedUserId") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewModelHUB hub = new ViewModelHUB(); // ny instans av vymodell
                BookingsMethods bm = new BookingsMethods(); // ny instans av bookingsmethods

                string loggedUser = HttpContext.Session.GetString("loggedUser"); // hämtar in hela användaren från session
                int loggedId = int.Parse(HttpContext.Session.GetString("loggedUserId")); // hämtar in ID från session
                string errormsg = "";
                hub.User = JsonConvert.DeserializeObject<UserDetails>(loggedUser); // gör om loggedUser från json-format till UserDetails
                hub.BookingsList = bm.GetBookingsUser(loggedId, out errormsg); // anrop till methoden getbookingsuser med ID för användaren
                //ViewBag.delete = HttpContext.Session.GetString("bookDelete");
                ViewBag.errmsg = errormsg;
                return View(hub);

            } 
        }


        // _______________________________ CRUD ____________________________________ //

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("loggedUserId") == null)
            {
                return RedirectToAction("Login", "User");
            }
            UserDetails user = new UserDetails();
            //Gets user from the session
            string jsonUser = HttpContext.Session.GetString("loggedUser");
            user = JsonConvert.DeserializeObject<UserDetails>(jsonUser);

            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (HttpContext.Session.GetString("loggedUserId") == null)
            {
                return RedirectToAction("Login", "User");
            }
            UserDetails loggedUser = new UserDetails();
            UserMethods um = new UserMethods();

            string errormsg = "";
            int i = 0;
            try
            {
                string loggedUserJson = HttpContext.Session.GetString("loggedUser");
                loggedUser = JsonConvert.DeserializeObject<UserDetails>(loggedUserJson);

                loggedUser.userFirstName = collection["userFirstName"];
                loggedUser.userLastName = collection["userLastName"];
                loggedUser.userEmail = collection["userEmail"];

                i = um.UpdateUser(loggedUser, out errormsg);

                loggedUserJson = JsonConvert.SerializeObject(loggedUser);
                HttpContext.Session.SetString("loggedUser", loggedUserJson);

                ViewBag.errmsg = errormsg;
                ViewBag.rowsChanged = i;

                return RedirectToAction(nameof(Profile));
            }
            catch
            {
                return View();
            }
        }


        
    }
}
