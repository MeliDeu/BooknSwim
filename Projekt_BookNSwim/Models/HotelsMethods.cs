using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class HotelsMethods
    {
        public HotelsMethods()
        {
        }

        public List<HotelsDetails> GetHotels(out string errormsg)
        {

            List<HotelsDetails> hotelList = new List<HotelsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Hotels ORDER BY Tbl_Hotels.hotel_name ASC";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;


                count = myDS.Tables["hotels"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        HotelsDetails hotel = new HotelsDetails();

                        hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                        hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                        hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                        hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                        hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                        hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                        hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                        hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                        hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                        hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                        hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                        hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                        i++;
                        hotelList.Add(hotel);
                    }
                    errormsg = "";
                    return hotelList;
                }
                else
                {
                    errormsg = "There are no hotels connected right now.";
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

        public HotelsDetails GetHotelById(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_id = " + id;

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            // dbCommand.Parameters.Add("@hotelId", MySqlDbType.Int16).Value = id;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;

                count = myDS.Tables["hotels"].Rows.Count;

                if (count > 0)
                {
                    HotelsDetails hotel = new HotelsDetails();

                    hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                    hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                    hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                    hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                    hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                    hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                    hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                    hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                    hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                    hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                    hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                    hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                    errormsg = "";
                    return hotel;
                }
                else
                {
                    errormsg = "There are no hotels connected right now.";
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

        public HotelsDetails GetHotelWithBookingID(int bookingID, out string errormsg)
        {
            // Console.WriteLine("hej");

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_id = (SELECT hotel_id FROM Tbl_Bookings WHERE Tbl_Bookings.hotel_id = Tbl_Hotels.hotel_id AND Tbl_Bookings.booking_id = "+ bookingID + ")";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            //dbCommand.Parameters.Add("@bookingsId", MySqlDbType.Int16).Value = bookingID;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;

                count = myDS.Tables["hotels"].Rows.Count;

                if (count > 0)
                {
                    HotelsDetails hotel = new HotelsDetails();
                    // Console.WriteLine(i);

                    hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                    hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                    hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                    hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                    hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                    hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                    hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                    hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                    hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                    hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                    hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                    hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                    errormsg = "";
                    return hotel;
                }
                else
                {
                    errormsg = "There is no hotels connected right now.";
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

        public List<HotelsDetails> FilterHotelCountry(HotelsDetails searchHotel, out string errormsg)
        {

            List<HotelsDetails> hotelList = new List<HotelsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            //Console.WriteLine(searchHotel.country);

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_country = " + searchHotel.country + " ORDER BY Tbl_Hotels.hotel_name ASC";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            //dbCommand.Parameters.Add("@countryFilter", MySqlDbType.VarChar, 70).Value = searchHotel.country;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();
            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;

                count = myDS.Tables["hotels"].Rows.Count;

                //Console.WriteLine(count);

                if (count > 0)
                {
                    while (i < count)
                    {
                        Console.WriteLine(i);
                        HotelsDetails hotel = new HotelsDetails();

                        hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                        hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                        hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                        hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                        hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                        hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                        hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                        hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                        hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                        hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                        hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                        hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                        i++;
                        hotelList.Add(hotel);
                    }

                    errormsg = "";
                    return hotelList;
                }
                else
                {
                    errormsg = "There are no hotels in that country.";
                    return null;
                }
            }
            catch (Exception err)
            {
                errormsg = err.Message;

                Console.WriteLine(errormsg);
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<HotelsDetails> SearchHotelCity(HotelsDetails searchHotel, out string errormsg)
        {

            List<HotelsDetails> hotelList = new List<HotelsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            // string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_city LIKE %" + searchHotel.city + "% ORDER BY Tbl_Hotels.hotel_name ASC";

            string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_city LIKE '%" + searchHotel.city + "%' ORDER BY Tbl_Hotels.hotel_name ASC";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            // dbCommand.Parameters.Add("@citySearch", MySqlDbType.VarChar, 50).Value = "%" + searchHotel.city + "%";

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;

                count = myDS.Tables["hotels"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        HotelsDetails hotel = new HotelsDetails();

                        hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                        hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                        hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                        hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                        hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                        hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                        hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                        hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                        hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                        hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                        hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                        hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                        i++;
                        hotelList.Add(hotel);
                    }

                    errormsg = "";
                    return hotelList;
                }
                else
                {
                    errormsg = "There are no hotels in that city.";
                    return null;
                }
            }
            catch (Exception err)
            {
                errormsg = err.Message;
                Console.WriteLine(errormsg);
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<HotelsDetails> SearchHotelName(HotelsDetails searchHotel, out string errormsg)
        {

            List<HotelsDetails> hotelList = new List<HotelsDetails>();

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlQuery = "SELECT * FROM Tbl_Hotels WHERE Tbl_Hotels.hotel_name LIKE '%" + searchHotel.name  + "%' ORDER BY Tbl_Hotels.hotel_name ASC";

            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            //dbCommand.Parameters.Add("@nameSearch", MySqlDbType.VarChar, 70).Value = "%" + searchHotel.name + "%";

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "hotels");

                int count = 0;
                int i = 0;

                count = myDS.Tables["hotels"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        HotelsDetails hotel = new HotelsDetails();

                        hotel.id = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_id"]);
                        hotel.name = myDS.Tables["hotels"].Rows[i]["hotel_name"].ToString();
                        hotel.address = myDS.Tables["hotels"].Rows[i]["hotel_address"].ToString();
                        hotel.postal = myDS.Tables["hotels"].Rows[i]["hotel_postal"].ToString();
                        hotel.city = myDS.Tables["hotels"].Rows[i]["hotel_city"].ToString();
                        hotel.country = myDS.Tables["hotels"].Rows[i]["hotel_country"].ToString();
                        hotel.phone = myDS.Tables["hotels"].Rows[i]["hotel_phone"].ToString();
                        hotel.email = myDS.Tables["hotels"].Rows[i]["hotel_email"].ToString();
                        hotel.pricing = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_price"]);
                        hotel.opening_time = myDS.Tables["hotels"].Rows[i]["hotel_opening_time"].ToString();
                        hotel.closing_time = myDS.Tables["hotels"].Rows[i]["hotel_closing_time"].ToString();
                        hotel.max_visitors = Convert.ToInt16(myDS.Tables["hotels"].Rows[i]["hotel_max_visitors"]);

                        i++;
                        hotelList.Add(hotel);
                    }

                    errormsg = "";
                    return hotelList;
                }
                else
                {
                    errormsg = "There is no hotel with that name.";
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
    }
}
