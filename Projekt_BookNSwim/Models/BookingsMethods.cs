using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class BookingsMethods
    {
        // constructor
        public BookingsMethods()
        {
        }

        // ------------------------------------------ Ny Bokning -----------------------------------------------//
        public int NewBooking(BookingsDetails booking, out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "INSERT INTO [Tbl_Bookings] ([hotel_id], [user_id], [booking_start_date], [booking_time], [booking_total_price], [booking_amount_visitors]) VALUES (@hotel, @user, @date, @time, @price, @visitors)";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);


            dbCommand.Parameters.Add("hotel", SqlDbType.Int).Value = booking.hotel_id;
            dbCommand.Parameters.Add("user", SqlDbType.Int).Value = booking.user_id;
            dbCommand.Parameters.Add("date", SqlDbType.Date).Value = booking.start_date;
            dbCommand.Parameters.Add("time", SqlDbType.VarChar, 50).Value = booking.time;
            dbCommand.Parameters.Add("price", SqlDbType.Float).Value = booking.total_price;
            dbCommand.Parameters.Add("visitors", SqlDbType.Int).Value = booking.total_visitors;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; } else { errormsg = "No booking has been added to the database"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                Console.WriteLine(errormsg);
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // ------------------------------------------ Hämta bokningar -----------------------------------------------//

        // hämtar alla bokningar
        public List<BookingsDetails> GetBookings(out string errormsg)
        {
            List<BookingsDetails> bookingList = new List<BookingsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Bookings";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            SqlDataAdapter myAdpater = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdpater.Fill(myDS, "bookings");

                int count = 0;
                int i = 0;
                count = myDS.Tables["bookings"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        BookingsDetails booking = new BookingsDetails();

                        booking.id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.hotel_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.user_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.start_date = (DateTime)myDS.Tables["bookings"].Rows[i]["booking_start_date"];
                        booking.time = myDS.Tables["bookings"].Rows[i]["booking_time"].ToString();
                        booking.total_price = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.total_visitors = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);

                        i++;
                        bookingList.Add(booking);
                    }

                    errormsg = "";
                    return bookingList;
                }
                else
                {
                    errormsg = "There are no bookings yet";
                    return null;
                }

            }
            catch (Exception err)
            {
                errormsg = err.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // hämtar alla bokningar för en specifik användare, annars returneras null (för profil-sidan)
        public List<BookingsDetails> GetBookingsUser(int userID, out string errormsg)
        {
            List<BookingsDetails> bookingList = new List<BookingsDetails>();
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Bookings INNER JOIN Tbl_Hotels ON Tbl_Bookings.hotel_id = Tbl_Hotels.hotel_id WHERE Tbl_Bookings.user_id = " + userID;

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            //dbCommand.Parameters.Add("usersId", SqlDataTyp.Int).Value = userID;

            SqlDataAdapter myAdpater = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdpater.Fill(myDS, "bookings");

                int countBooking = 0;
                int i = 0;
                countBooking = myDS.Tables["bookings"].Rows.Count;

                if (countBooking > 0)
                {
                    while (i < countBooking)
                    {
                        BookingsDetails booking = new BookingsDetails();

                        booking.id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.hotel_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["hotel_id"]);
                        booking.user_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["user_id"]);
                        booking.start_date = (DateTime)myDS.Tables["bookings"].Rows[i]["booking_start_date"];
                        booking.time = (string)myDS.Tables["bookings"].Rows[i]["booking_time"];
                        booking.total_price = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_total_price"]);
                        booking.total_visitors = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_amount_visitors"]);
                        // booking.hotel_name = myDS.Tables["bookings"].Rows[i]["hotel_name"].ToString();
                        // booking.hotel_address = myDS.Tables["bookings"].Rows[i]["hotel_address"].ToString();

                        i++;
                        bookingList.Add(booking);
                    }

                    errormsg = "";
                    return bookingList;
                }
                else
                {
                    errormsg = "There are no bookings yet";
                    return null;
                }

            }
            catch (Exception err)
            {
                errormsg = err.Message;
                Console.WriteLine(err.Message);
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // hämtar alla bokningar som finns för ett specifikt hotell (för att visa om det går att boka)
        public List<BookingsDetails> GetBookingsHotel(int hotelID, out string errormsg)
        {
            List<BookingsDetails> bookingList = new List<BookingsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Bookings WHERE Tbl_Bookings.hotel_id = " + hotelID;

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            //dbCommand.Parameters.Add("@hotelsId", MySqlDbType.Int16).Value = hotelID;

            SqlDataAdapter myAdpater = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdpater.Fill(myDS, "bookings");

                int count = 0;
                int i = 0;
                count = myDS.Tables["bookings"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        BookingsDetails booking = new BookingsDetails();

                        booking.id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                        booking.hotel_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["hotel_id"]);
                        booking.user_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["user_id"]);
                        booking.start_date = (DateTime)myDS.Tables["bookings"].Rows[i]["booking_start_date"];
                        booking.time = (string)myDS.Tables["bookings"].Rows[i]["booking_time"];
                        booking.total_price = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_total_price"]);
                        booking.total_visitors = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_amount_visitors"]);

                        i++;
                        bookingList.Add(booking);
                    }

                    errormsg = "";
                    return bookingList;
                }
                else
                {
                    errormsg = "There are no bookings yet";
                    return null;
                }

            }
            catch (Exception err)
            {
                errormsg = err.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // hämtar bokningen med hjälp av bokningsID (för details ex)
        public BookingsDetails GetBookingsByID(int bookingID, out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Bookings WHERE Tbl_Bookings.booking_id = " + bookingID;

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

           // dbCommand.Parameters.Add("@bookingsId", MySqlDbType.Int16).Value = bookingID;

            SqlDataAdapter myAdpater = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdpater.Fill(myDS, "bookings");

                int count = 0;
                int i = 0;
                count = myDS.Tables["bookings"].Rows.Count;

                if (count > 0)
                {
                    BookingsDetails booking = new BookingsDetails();

                    booking.id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_id"]);
                    booking.hotel_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["hotel_id"]);
                    booking.user_id = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["user_id"]);
                    booking.start_date = (DateTime)myDS.Tables["bookings"].Rows[i]["booking_start_date"];
                    booking.time = (string)myDS.Tables["bookings"].Rows[i]["booking_time"];
                    booking.total_price = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_total_price"]);
                    booking.total_visitors = Convert.ToInt16(myDS.Tables["bookings"].Rows[i]["booking_amount_visitors"]);


                    errormsg = "";
                    return booking;
                }
                else
                {
                    errormsg = "There are no bookings yet";
                    return null;
                }

            }
            catch (Exception err)
            {
                errormsg = err.Message;
                Console.WriteLine(err.Message);
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // ----------------------------------------- DELETE bokningar -----------------------------------------------//
        public int CancelBooking(int bookingID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string mysqlQuery = "DELETE FROM Tbl_Bookings WHERE booking_id = " + bookingID;

            SqlCommand dbCommand = new SqlCommand(mysqlQuery, dbConnection);

            //dbCommand.Parameters.Add("@bookingID", MySqlDbType.Int16).Value = bookingID;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; } else { errormsg = "The booking was not cancelled."; }
                return i;
            }
            catch (Exception err)
            {
                errormsg = err.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // --------------------------------------- Beräkna pris -----------------------------------------------//
        public double CalculateCost(string start, string end, int price, int visitors, out string errormsg)
        {
            // make a calc - method, where the time is converted and price is calculated, as well as checked if the endtime is bigger than the start

            // the time comes as a string in this format 12:07 or 08:13
            // get the hours and minutes of the time and convert it to int
            string[] startArr = start.Split(':'); // we split the starttime by the : and then assign hours and minutes to own variables
            double startHour = Convert.ToInt32(startArr[0]);
            double startMin = Convert.ToInt32(startArr[1]);

            string[] endArr = end.Split(':'); // we split the endtime by the : and then assign hours and minutes to own variables
            double endHour = Convert.ToInt32(endArr[0]);
            double endMin = Convert.ToInt32(endArr[1]);

            // convert the minutes to hundredths 
            // minutes / 60 = minutes in hundredths

            double startMin100 = startMin / 60;
            double startTime100 = startHour + startMin100; // the minutes are then added to the hours, eg. 5:30 --> 5hours and 30/60 = 0.5 --> 5 + 0.5 = 5.5 --> 5:30

            double endMin100 = endMin / 60;
            double endTime100 = endHour + endMin100;


            // early exit if the starttime is bigger than endtime
            if (startTime100 > endTime100)
            {
                errormsg = "You have chosen an end time that is before the start time. Overnight stays are not allowed";
                //eg, 15.50 starttime and endtime 08.50-- > would mean overnight, not valid, early exit
                return (0);
            }

            // we substract the start-time from the endtime
            // the result is the amount of hours 
            double amountTime = endTime100 - startTime100;

            Console.WriteLine(amountTime);
            // multiplied with price and visitors
            double totalPrice = Math.Round((amountTime * price * visitors), 2);
            Console.WriteLine(totalPrice);

            // return the value with 2 digits after floating point
            errormsg = "";

            //string totalPriceInString = "starttime in 100s: " + startTime100 + ", enddtime in 100s: " + endTime100 + ", hrs: " + amountTime + ", totalprice: " + totalPrice + "(" + visitors + price + ")";

            return totalPrice;
        }
    }
}
