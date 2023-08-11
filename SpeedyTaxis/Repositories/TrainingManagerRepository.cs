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
    class TrainingManagerRepository
    {
        static HttpClient client = new HttpClient();
        //static string localHost = "http:// localhost:10293/";

        public static async Task<bool> SaveTraining(Training training)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(training);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "SaveTraining", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        public static async Task<bool> DeleteTraining(int id)
        {
            // var client = new HttpClient();
            //var jsonContent = JsonConvert.SerializeObject(id);
            // var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(WebService.LocalHostUrl + "DeleteTraining/id/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }
        public static async Task<bool> UpdateTraining(Training training)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(training);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "UpdateTraining", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }
        // incorrect method logic
        public static async Task<bool> SaveTrainingFromDriverLicenceID(Training training, Driver driver)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(driver.ID);
            var jsonContent2 = JsonConvert.SerializeObject(training);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpContent2 = new StringContent(jsonContent2, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "SaveDriver", httpContent);
            var response2 = await client.PostAsync(WebService.LocalHostUrl + "SaveTrainingFromDriverLicenceID/id/", httpContent2);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            if (responseString == "false" || responseString2 == "false")
                return false;
            else
                return true;
        }

        #region direct to database methods
        // training database methods that unfortunately do not go through the web service (yet)
        public static List<Driver> FindExpiringTrainingAndDriver()
        {
            // get expiring qualifications first
            List<String> training = new List<String>();
            List<Driver> driver = new List<Driver>();
            OleDbConnection myConnection = DBConnectivity.GetConn();
            DateTime dt = System.DateTime.Today;
            DateTime b = dt.AddDays(-30);
            string str = "#" + b.ToString() + "#";
            string myQuery = "select dID from Training WHERE expiryDate <=" + str;
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    //Driver g = new Driver(myReader["driverName"].ToString());
                    training.Add(myReader["dID"].ToString());
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
            String formatted = String.Format(myQuery0, String.Join(",", training.ToArray()));
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
        public static List<String> FindTrainingForDriver(int dID)
        {
            List<String> qualifications = new List<String>();

            OleDbConnection myConnection = DBConnectivity.GetConn();

            string myQuery = "SELECT title FROM Training WHERE dID = " + dID;
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
        // insert into training new entry with driver id METHOD HERE
        public static string CheckDriverTrainingExistsFromdID(int dID, string title)
        {
            int training = 0;
            string user = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT count(*) FROM Training WHERE title = @title AND dID = @dID";
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
        public static void AddDriverTrainingFromLicenceID(string type, string expiryDate, string lID)
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
                if (type == "advanced driving course")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Advanced Driving Course");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "driving at night")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Driving at Night");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "cyclist awareness")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Cyclist Awareness");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", expiryDate);

                    myCommand0.ExecuteNonQuery();
                }
                else if (type == "reduce fuel consumption")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Reduce Fuel Consumption");
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
        public static void UpdateDriverTrainingFromID(string type, string expiryDate, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            // advanced driving course
            if (type == "advanced driving course")
            {

                string myQuery = "UPDATE Training SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //

                // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                //myCommand.Parameters.AddWithValue("@id", uN);
                myCommand.Parameters.AddWithValue("@title", "Advanced Driving Course");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Advanced Driving Course");

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
            // driving at night
            else if (type == "driving at night")
            {
                string myQuery = "UPDATE Training SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //

                // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                //myCommand.Parameters.AddWithValue("@id", uN);
                myCommand.Parameters.AddWithValue("@title", "Driving at Night");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Driving at Night");

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
            // cyclist awareness
            else if (type == "cyclist awareness")
            {
                string myQuery = "UPDATE Training SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //

                // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                //myCommand.Parameters.AddWithValue("@id", uN);
                myCommand.Parameters.AddWithValue("@title", "Cyclist Awareness");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Cyclist Awareness");

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
            // reduce fuel consumption
            else if (type == "reduce fuel consumption")
            {
                string myQuery = "UPDATE Training SET title = @title, expiryDate = @expiryDate WHERE dID = @dID AND title = @type";
                //

                // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                //myCommand.Parameters.AddWithValue("@id", uN);
                myCommand.Parameters.AddWithValue("@title", "Reduce Fuel Consumption");
                myCommand.Parameters.AddWithValue("@expiryDate", expiryDate);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@type", "Reduce Fuel Consumption");

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
        public static void AssignDriverToTrainingFromID(int id, string title, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            string myQuery = "UPDATE Training SET dID = @dID WHERE ID = @id AND title = @title";
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
        public static void DeleteDriverTrainingFromID(string type, int dID)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            // advanced driving course
            if (type == "advanced driving course")
            {
                /*string myQuery = "SELECT Parent.ID from Parent WHERE userName = @uName";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@uName", uN);*/

                string myQuery = "DELETE * FROM Training WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Advanced Driving Course");
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
            // driving at night
            else if (type == "driving at night")
            {
                string myQuery = "DELETE * FROM Training WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Driving at Night");
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
            // cyclist awareness
            else if (type == "cyclist awareness")
            {
                string myQuery = "DELETE * FROM Training WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Cyclist Awareness");
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
            // reduce fuel consumption
            else if (type == "reduce fuel consumption")
            {
                string myQuery = "DELETE * FROM Training WHERE dID = @dID AND title = @title";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                myCommand.Parameters.AddWithValue("@title", "Reduce Fuel Consumption");
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
                string myQuery = "DELETE * FROM Training WHERE dID = @dID";
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
        // check if driver is assigned onto training course
        public static string CheckDriverTrainingFromID(string dID, string type)
        {
            string driver = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            if (type == "advanced driving course")
            {
                string myQuery = "SELECT dID FROM Training WHERE title = 'Advanced Driving Course' AND dID = " + dID;
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
            else if (type == "driving at night")
            {
                string myQuery = "SELECT dID FROM Training WHERE title = 'Driving at Night' AND dID = " + dID;
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
            else if (type == "cyclist awareness")
            {
                string myQuery = "SELECT dID FROM Training WHERE title = 'Cyclist Awareness' AND dID = " + dID;
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
            else if (type == "reduce fuel consumption")
            {
                string myQuery = "SELECT dID FROM Training WHERE title = 'Reduce Fuel Consumption' AND dID = " + dID;
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
        public static string LoadDriverTrainingFromID(string dID, string type, string s)
        {
            string driver = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            if (type == "advanced driving course" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Training WHERE title = 'Advanced Driving Course' AND dID = " + dID;
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
            else if (type == "driving at night" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Training WHERE title = 'Driving at Night' AND dID = " + dID;
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
            else if (type == "cyclist awareness" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Training WHERE title = 'Cyclist Awareness' AND dID = " + dID;
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
            else if (type == "reduce fuel consumption" && s == "expiry date")
            {
                string myQuery = "SELECT expiryDate FROM Training WHERE title = 'Reduce Fuel Consumption' AND dID = " + dID;
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
        public static string LoadTrainingFromID(int id, string type, string data)
        {
            string d = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //advanced driving course
            if (type == "Advanced Driving Course" && data == "title")
            {
                string myQuery = "SELECT title FROM Training WHERE title = 'Advanced Driving Course' AND ID = " + id;
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
            else if (type == "Advanced Driving Course" && data == "description")
            {
                string myQuery = "SELECT description FROM Training WHERE title = 'Advanced Driving Course' AND ID = " + id;
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
            else if (type == "Advanced Driving Course" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Training WHERE title = 'Advanced Driving Course' AND ID = " + id;
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
            else if (type == "Advanced Driving Course" && data == "time")
            {
                string myQuery = "SELECT time FROM Training WHERE title = 'Advanced Driving Course' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["time"].ToString();
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
            else if (type == "Advanced Driving Course" && data == "completed")
            {
                string myQuery = "SELECT completed FROM Training WHERE title = 'Advanced Driving Course' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["completed"].ToString();
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
            //driving at night
            else if (type == "Driving at Night" && data == "title")
            {
                string myQuery = "SELECT title FROM Training WHERE title = 'Driving at Night' AND ID = " + id;
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
            else if (type == "Driving at Night" && data == "description")
            {
                string myQuery = "SELECT description FROM Training WHERE title = 'Driving at Night' AND ID = " + id;
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
            else if (type == "Driving at Night" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Training WHERE title = 'Driving at Night' AND ID = " + id;
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
            else if (type == "Driving at Night" && data == "time")
            {
                string myQuery = "SELECT time FROM Training WHERE title = 'Driving at Night' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["time"].ToString();
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
            else if (type == "Driving at Night" && data == "completed")
            {
                string myQuery = "SELECT completed FROM Training WHERE title = 'Driving at Night' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["completed"].ToString();
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
            // cyclist awareness
            if (type == "Cyclist Awareness" && data == "title")
            {
                string myQuery = "SELECT title FROM Training WHERE title = 'Cyclist Awareness' AND ID = " + id;
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
            else if (type == "Cyclist Awareness" && data == "description")
            {
                string myQuery = "SELECT description FROM Training WHERE title = 'Cyclist Awareness' AND ID = " + id;
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
            else if (type == "Cyclist Awareness" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Training WHERE title = 'Cyclist Awareness' AND ID = " + id;
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
            else if (type == "Cyclist Awareness" && data == "time")
            {
                string myQuery = "SELECT time FROM Training WHERE title = 'Cyclist Awareness' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["time"].ToString();
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
            else if (type == "Cyclist Awareness" && data == "completed")
            {
                string myQuery = "SELECT completed FROM Training WHERE title = 'Cyclist Awareness' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["completed"].ToString();
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
            // reduce fuel consumption
            else if (type == "Reduce Fuel Consumption" && data == "title")
            {
                string myQuery = "SELECT title FROM Training WHERE title = 'Reduce Fuel Consumption' AND ID = " + id;
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
            else if (type == "Reduce Fuel Consumption" && data == "description")
            {
                string myQuery = "SELECT description FROM Training WHERE title = 'Reduce Fuel Consumption' AND ID = " + id;
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
            else if (type == "Reduce Fuel Consumption" && data == "dueDate")
            {
                string myQuery = "SELECT dueDate FROM Training WHERE title = 'Reduce Fuel Consumption' AND ID = " + id;
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
            else if (type == "Reduce Fuel Consumption" && data == "time")
            {
                string myQuery = "SELECT time FROM Training WHERE title = 'Reduce Fuel Consumption' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["time"].ToString();
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
            else if (type == "Reduce Fuel Consumption" && data == "completed")
            {
                string myQuery = "SELECT completed FROM Training WHERE title = 'Reduce Fuel Consumption' AND ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["completed"].ToString();
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

        /* public static void UpdateTrainingFromID(int id, string t, string d, string date, string time, string c)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();
            string myQuery = "UPDATE Training SET title = @title, description = @description, dueDate = @date, time = @time, completed = @completed WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@title", t);
            myCommand.Parameters.AddWithValue("@description", d);
            myCommand.Parameters.AddWithValue("@dueDate", date);
            myCommand.Parameters.AddWithValue("@time", time);
            myCommand.Parameters.AddWithValue("@completed", c);
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
        }*/
        #endregion
    }
}
