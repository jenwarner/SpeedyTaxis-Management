using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    class DriverManagerRepository
    {
        static HttpClient client = new HttpClient();
       // static string localHost = "http:// localhost:10293/";

        public static async Task<dynamic> CalcLogData(LogData logData)
        {
            var jsonContent = JsonConvert.SerializeObject(logData);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            string urlString = "LogData/" + logData.StartOfJourney + "/" + logData.EndOfJourney + "/" + logData.StartOfDay + "/" + logData.EndOfDay;
            //urlString = urlString.Replace("x", "xxx").Replace("y", "xxy").Replace(":", "xyy");
            var response = await client.GetAsync(WebService.LocalHostUrl + urlString);
            // urlString = urlString.Replace("xyy", ":").Replace("xxy", "y").Replace("xxx", "x");
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "")
            {
                return "error";
            }
            else
            {
                dynamic data = JObject.Parse(responseString);
                return data;
            }
        }
        public static async Task<bool> SaveDriver(Driver driver)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(driver);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "SaveDriver", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        public static async Task<bool> DeleteDriver(int id)
        {
           // var client = new HttpClient();
            //var jsonContent = JsonConvert.SerializeObject(id);
           // var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(WebService.LocalHostUrl + "DeleteDriver/id/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        public static async Task<bool> UpdateDriver(Driver driver)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(driver);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "UpdateDriver", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }
        //idk
        public static async Task<int> GetDriverIDWithName(Driver driver)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(driver);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.GetAsync(WebService.LocalHostUrl + "GetDriverIDWithName/"+ driver.FirstName + "/" + driver.Surname);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "")
                return 0;
            else
                return int.Parse(responseString);
        }

        #region direct to database methods
        // driver database methods that unfortunately do not go through the web service (yet)

        // check if driver exists
        public static string CheckDriverExistsFromLicenceID(string lID)
        {
            int driver = 0;
            string user = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT count(*) FROM Driver WHERE licenceID = @lID";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@lID", lID);

            try
            {
                myConnection.Open();
                driver = Convert.ToInt32(myCommand.ExecuteScalar().ToString());

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
            if (driver == 1)
            {
                user = "existing";
                driver = 0;
            }
            else
            {
                user = "new";
                driver = 0;
            }
            return user;

        }
        public static int CheckDriverExistsFromName(string fN, string sN)
        {
            int driver = 0;

            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT count(*) FROM Driver WHERE firstName = @firstName AND surname = @surname";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@firstName", fN);
            myCommand.Parameters.AddWithValue("@surname", sN);

            try
            {
                myConnection.Open();
                driver = Convert.ToInt32(myCommand.ExecuteScalar().ToString());

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
        public static int GetDriverIDFromName(string fN, string sN)
        {
            int id = 0;
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT ID FROM Driver WHERE firstName = @firstName AND surname = @surname";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@firstName", fN);
            myCommand.Parameters.AddWithValue("@surname", sN);

            try
            {
                myConnection.Open();
                id = Convert.ToInt32(myCommand.ExecuteScalar().ToString());

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

            return id;

        }
        public static string LoadDriverInfoFromID(string dID, string type)
        {
            string driver = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            if (type == "first name")
            {
                string myQuery = "SELECT firstName FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["firstName"].ToString();
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
            else if (type == "surname")
            {
                string myQuery = "SELECT surname FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["surname"].ToString();
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
            }
            else if (type == "sex")
            {
                string myQuery = "SELECT sex FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["sex"].ToString();
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
            }
            else if (type == "phone number")
            {
                string myQuery = "SELECT phoneNumber FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["phoneNumber"].ToString();
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
            }
            else if (type == "licence id")
            {
                string myQuery = "SELECT licenceID FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["licenceID"].ToString();
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
            }
            else if (type == "address")
            {
                string myQuery = "SELECT address FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["address"].ToString();
                    }
                    return driver;
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
            else if (type == "employment date")
            {
                string myQuery = "SELECT employmentDate FROM Driver WHERE ID = " + dID;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        driver = myReader["employmentDate"].ToString();
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
            }
            return driver;
        }
        // Method that returns a list of Driver objects from the database
        public static List<Driver> LoadDriver(int id)
        {
            List<Driver> driver = new List<Driver>();
            //List<String> d = new List<String>();

            OleDbConnection myConnection = DBConnectivity.GetConn();

            string myQuery = "SELECT * FROM Driver WHERE ID = " + id;
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Driver g = new Driver(myReader["firstName"].ToString(), myReader["surname"].ToString(), myReader["sex"].ToString(), long.Parse(myReader["phoneNumber"].ToString()), myReader["employmentDate"].ToString(), myReader["licenceID"].ToString(), myReader["address"].ToString());
                    driver.Add(g);
                    //d.Add(myReader["firstName"].ToString());
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
            //return d;
        }
        public static List<Driver> GetDuplicateDriverNames(string fN, string sN)
        {
            List<Driver> driver = new List<Driver>();
            //List<String> d = new List<String>();

            OleDbConnection myConnection = DBConnectivity.GetConn();

            string myQuery = "SELECT ID, firstName + ' ' + surname as driverName FROM Driver WHERE firstName = @fName AND surname = @sName";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@fName", fN);
            myCommand.Parameters.AddWithValue("@sName", sN);
            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Driver g = new Driver(int.Parse(myReader["ID"].ToString()), myReader["driverName"].ToString());
                    driver.Add(g);
                    //d.Add(myReader["firstName"].ToString());
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
            //return d;
        }
        

        /*public static void UpdateDriver(string fN, string sN, string s, string pN, string eD, string lID, string a, int dID)
        {
            var myConnection = DBConnectivity.GetConn();
            string myQuery = "UPDATE Driver SET firstName = @firstName, surname = @surname, sex = @sex, phoneNumber = @phoneNumber, employmentDate = @employmentDate, licenceID = @licenceID, address = @address WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@firstName", fN);
            myCommand.Parameters.AddWithValue("@surname", sN);
            myCommand.Parameters.AddWithValue("@sex", s);
            myCommand.Parameters.AddWithValue("@phoneNumber", pN);
            myCommand.Parameters.AddWithValue("@employmentDate", eD);
            myCommand.Parameters.AddWithValue("@licenceDate", lID);
            myCommand.Parameters.AddWithValue("@address", a);
            myCommand.Parameters.AddWithValue("@id", dID);

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
