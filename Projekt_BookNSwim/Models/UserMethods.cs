using Projekt_BookNSwim.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BookNSwin.Models
{
    // we need a list with all users
    // we need one that takes new registrations, checks if in DB, if not --> INSERT, else ABORT and errormsg
    // we need one that checks the login, if user is in DB, if yes, return 
    public class UserMethods
    {

        // returns list with all users if not sending userdetail with it
        // otherwise a list with only one --> for example login
        public List<UserDetails> getUsers(out string errormsg)
        {
            //create Connection
            SqlConnection dbConnection = new SqlConnection();

            //connect to SQL-server
            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            //SQL-string for getting a list with all registered users
            string sqlString = "SELECT * FROM Tbl_Users";

            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            //create adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<UserDetails> UserList = new List<UserDetails>();

            try
            {
                dbConnection.Open();

                // fills the adapter with table with new dataset
                myAdapter.Fill(myDS, "users");

                int count = 0;
                int i = 0;
                count = myDS.Tables["users"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        UserDetails ud = new UserDetails();
                        // inserts the user id
                        ud.userId = Convert.ToInt16(myDS.Tables["users"].Rows[i]["user_id"]);
                        // inserts the username
                        ud.userUserName = myDS.Tables["users"].Rows[i]["user_username"].ToString();
                        // inserts the password
                        ud.userPassword = myDS.Tables["users"].Rows[i]["user_password"].ToString();
                        // inserts the email
                        ud.userEmail = myDS.Tables["users"].Rows[i]["user_email"].ToString();
                        // inserts the first name
                        ud.userFirstName = myDS.Tables["users"].Rows[i]["user_firstname"].ToString();
                        // inserts the last name
                        ud.userLastName = myDS.Tables["users"].Rows[i]["user_lastname"].ToString();

                        i++;
                        UserList.Add(ud);
                    }
                    errormsg = "";
                    return UserList;
                }
                else
                {
                    errormsg = "Det finns inga personer i databasen";
                    return (null);
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }


        // check if username is in the database already
        public UserDetails checkForUser(string username, out string errormsg)
        {
            //create Connection
            SqlConnection dbConnection = new SqlConnection();

            //connect to SQL-server
            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            //SQL-string for getting a list with all registered users
            string sqlString = "SELECT * FROM Tbl_Users WHERE Tbl_Users.user_username = '" + username + "'";

            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            //create adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                // fills the adapter with table with new dataset
                myAdapter.Fill(myDS, "users");

                int count = 0;
                int i = 0;
                count = myDS.Tables["users"].Rows.Count;

                if (count > 0)
                {
                    UserDetails ud = new UserDetails();
                    // inserts the user id
                    ud.userId = Convert.ToInt16(myDS.Tables["users"].Rows[i]["user_id"]);
                    // inserts the username
                    ud.userUserName = myDS.Tables["users"].Rows[i]["user_username"].ToString();
                    // inserts the password
                    ud.userPassword = myDS.Tables["users"].Rows[i]["user_password"].ToString();
                    // inserts the email
                    ud.userEmail = myDS.Tables["users"].Rows[i]["user_email"].ToString();
                    // inserts the first name
                    ud.userFirstName = myDS.Tables["users"].Rows[i]["user_firstname"].ToString();
                    // inserts the last name
                    ud.userLastName = myDS.Tables["users"].Rows[i]["user_lastname"].ToString();

                    //errormsg = "";
                    //return ud;
                    errormsg = "";
                    return ud;
                }
                else
                {
                    errormsg = "Personen finns inte i Databasen";
                    return (null);
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return (null);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // ------------------------------------------------- REGISTER NEW USER -----------------------------------------//

        public int RegisterUser(UserDetails newUser, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            //koppling mot SQL Server
            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string sqlString = "INSERT INTO Tbl_Users (user_username, user_password, user_email, user_firstname, user_lastname) VALUES(@username, @password, @email, @firstname, @lastname)";

            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("username", SqlDbType.NVarChar).Value = newUser.userUserName;
            dbCommand.Parameters.Add("password", SqlDbType.NVarChar).Value = newUser.userPassword;
            dbCommand.Parameters.Add("email", SqlDbType.NVarChar).Value = newUser.userEmail;
            dbCommand.Parameters.Add("firstname", SqlDbType.NVarChar).Value = newUser.userFirstName;
            dbCommand.Parameters.Add("lastname", SqlDbType.NVarChar).Value = newUser.userLastName;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "No user has been added to the database"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // ------------------------------------------------- EDIT USER ------------------------------------------------//

        //Uppdatera användarens uppgifter

        public int UpdateUser(UserDetails user, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookNSwim;Integrated Security=True";

            string mysqlQuery = "UPDATE Tbl_Users SET user_firstname = " + user.userFirstName + ", user_lastname = " + user.userLastName + ", user_email = " + user.userEmail + " WHERE user_id = " + user.userId;

            SqlCommand dbCommand = new SqlCommand(mysqlQuery, dbConnection);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; } else { errormsg = "There occured a problem while trying to change your information. Please try later."; }
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
    }
}