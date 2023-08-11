using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebService.Models;

namespace SpeedyTaxis.Repositories
{
    class QualificationManagerRepository
    {
        static HttpClient client = new HttpClient();
        //static string localHost = "http:// localhost:10293/";
        public static async Task<bool> SaveQualification(Qualification qualification)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(qualification);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "SaveQualification", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        public static async Task<bool> DeleteQualification(int id)
        {
            // var client = new HttpClient();
            //var jsonContent = JsonConvert.SerializeObject(id);
            // var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(WebService.LocalHostUrl + "DeleteQualification/id/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }
        public static async Task<bool> UpdateQualification(Qualification qualification)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(qualification);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "UpdateQualification", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        #region direct to database methods
        // qualification database methods that unfortunately do not go through the web service (yet)
        public static List<String> FindQualifcationsForDriver(int dID)
        {
            List<String> qualifications = new List<String>();

            OleDbConnection myConnection = DBConnectivity.GetConn();

            string myQuery = "SELECT title FROM Qualification WHERE dID = " + dID;
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    qualifications.Add(myReader["title"].ToString());
                }
                return qualifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                return null;
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static List<Driver> FindExpiringQualifcationsAndDriver()
        {
            // get expiring qualifications first
            List<String> qualifications = new List<String>();
            List<Driver> driver = new List<Driver>();
            OleDbConnection myConnection = DBConnectivity.GetConn();
            DateTime dt = System.DateTime.Today;
            DateTime b = dt.AddDays(-30);
            string str = "#" + b.ToString() + "#";
            string myQuery = "select dID from Qualification WHERE expiryDate <=" + str;
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    //Driver g = new Driver(myReader["driverName"].ToString());
                    qualifications.Add(myReader["dID"].ToString());
                }
                //return driver;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                return null;
            }
            finally
            {
                myConnection.Close();
            }

            // get driver names
            string myQuery0 = "SELECT firstName + ' ' + surname as driverName FROM Driver WHERE ID in ({0});";
            String formatted = String.Format(myQuery0, String.Join(",", qualifications.ToArray()));
            OleDbCommand myCommand0 = new OleDbCommand(formatted, myConnection);
            try
            {
                myConnection.Open();
                OleDbDataReader myReader0 = myCommand0.ExecuteReader();
                while (myReader0.Read())
                {
                    Driver g = new Driver(myReader0["driverName"].ToString());
                    driver.Add(g);
                    //driver.Add(myReader0["firstName"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                //return null;
            }
            finally
            {
                myConnection.Close();
            }

            return driver;
        }
        public static string CheckDriverQualificationExistsFromdID(int dID, string title)
        {
            int training = 0;
            string user = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT count(*) FROM Qualification WHERE title = @title AND dID = @dID";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", title);
            myCommand.Parameters.AddWithValue("@dID", dID);

            try
            {
                myConnection.Open();
                training = Convert.ToInt32(myCommand.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                return null;
            }
            finally
            {
                myConnection.Close();
            }
            if (training == 1)
            {
                user = "existing";
                training = 0;
            }
            else
            {
                user = "new";
                training = 0;
            }
            return user;
        }
        public static void AddDriverQualificationsFromLicenceID(string type, string expiryDate, string lID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            // get driver id from licenceID
            int id = 0;
            string myQuery = "SELECT ID FROM Driver WHERE licenceID = @lID";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@lID", lID);

            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    id = int.Parse(myReader["ID"].ToString());
                }
                // add training
                if (type == "central london")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Central London");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand.ExecuteNonQuery();
                }
                else if (type == "north london")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "North London");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "south london")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "South London");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "east london")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "East London");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "west london")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "West London");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "driving licence")
                {
                    string myQuery0 = "INSERT INTO Qualification ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Driving Licence");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void UpdateQualificationFromID(int id, string t, string d, string date)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            string myQuery = "UPDATE Qualification SET title = @title, description = @description, dueDate = @date WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@title", t);
            myCommand.Parameters.AddWithValue("@description", d);
            myCommand.Parameters.AddWithValue("@dueDate", date);
            myCommand.Parameters.AddWithValue("@id", id);

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void UpdateDriverQualificationsFromID(string type, string expiryDate, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            // gt: central london
            if (type == "central london")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "Central London");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Central London");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: north london
            else if (type == "north london")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "North London");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "North London");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: south london
            else if (type == "south london")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "South London");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "South London");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: east london
            else if (type == "east london")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "East London");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "East London");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: west london
            else if (type == "west london")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "West London");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "West London");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // driving licence
            else if (type == "driving licence")
            {
                string myQuery = "UPDATE Qualification SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@title", "Driving Licence");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Driving Licence");

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
        }
        public static void AssignDriverToQualificationFromID(int id, string title, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            string myQuery = "UPDATE Qualification SET dID = @dID WHERE ID = @id AND title = @title";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@dID", dID);
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.Parameters.AddWithValue("@title", title);

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void DeleteDriverQualificationsFromID(string type, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            // gt: central london
            if (type == "central london")
            {
                /*string myQuery = "SELECT Parent.ID from Parent WHERE userName = @uName";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@uName", uN);*/

                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Central London");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: north london
            else if (type == "north london")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "North London");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: south london
            else if (type == "south london")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "South London");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: east london
            else if (type == "east london")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "East London");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // gt: west london
            else if (type == "west london")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "West London");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // driving licence
            else if (type == "driving licence")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Driving Licence");
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "all")
            {
                string myQuery = "DELETE * FROM Qualification WHERE dID = @dID";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
        }
        // check if driver has these qualifications
        public static string CheckDriverQualificationsFromID(string dID, string type)
        {
            string driver = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            if (type == "gt: central london")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'Central London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: north london")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'North London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: south london")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'South London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: east london")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'East London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: west london")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'West London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "driving licence")
            {
                string myQuery = "SELECT dID FROM Qualification WHERE title = 'Driving Licence' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["dID"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return driver;
        }
        public static string LoadDriverQualificationsFromID(string dID, string type, string s)
        {
            string driver = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            if (type == "gt: central london" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'Central London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: north london" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'North London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: south london" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'South London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: east london" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'East London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "gt: west london" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'West London' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "driving licence" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Qualification WHERE title = 'Driving Licence' AND dID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["expiryDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return driver;
        }
        public static string LoadQualificationFromID(int id, string type, string data)
        {
            string d = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //central london
            if (type == "Central London" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'Central London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "Central London" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'Central London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "Central London" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'Central London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }

            //north london
            if (type == "North London" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'North London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "North London" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'North London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "North London" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'North London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // south london
            if (type == "South London" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'South London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "South London" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'South London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "South London" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'South London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // east london
            if (type == "East London" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'East London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "East London" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'East London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "East London" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'East London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // west london
            if (type == "West London" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'West London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "West London" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'West London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "West London" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'West London' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            // driving licence
            if (type == "Driving Licence" && data == "title")
            {
                string myQuery = "SELECT title FROM Qualification WHERE title = 'Driving Licence' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["title"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "Driving Licence" && data == "description")
            {
                string myQuery = "SELECT description FROM Qualification WHERE title = 'Driving Licence' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["description"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else if (type == "Driving Licence" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Qualification WHERE title = 'Driving Licence' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dueDate"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                    return null;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return d;
        }
        #endregion
    }
}
