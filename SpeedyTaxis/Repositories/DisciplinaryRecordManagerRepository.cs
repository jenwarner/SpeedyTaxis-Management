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
    class DisciplinaryRecordManagerRepository
    {
        static HttpClient client = new HttpClient();
        //static string localHost = "http:// localhost:10293/";
        public static async Task<bool> SaveDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(disciplinaryRecord);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "SaveDisciplinaryRecord", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }
        public static async Task<bool> DeleteDisciplinaryRecord(int id)
        {
            // var client = new HttpClient();
            //var jsonContent = JsonConvert.SerializeObject(id);
            // var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(WebService.LocalHostUrl + "DeleteDisciplinaryRecord/id/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        public static async Task<bool> UpdateDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            //client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(disciplinaryRecord);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(WebService.LocalHostUrl + "UpdateDisciplinaryRecord", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "false")
                return false;
            else
                return true;
        }

        #region direct to database methods
        public static string LoadDisciplinaryRecordFromID(int id, string data)
        {
            string d = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //central london
            if (data == "title")
            {
                string myQuery = "SELECT title FROM DisciplinaryRecord WHERE ID = " + id;
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
            else if (data == "description")
            {
                string myQuery = "SELECT description FROM DisciplinaryRecord WHERE ID = " + id;
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
            else if (data == "date")
            {
                string myQuery = "SELECT dateAdded FROM DisciplinaryRecord WHERE ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["dateAdded"].ToString();
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
            else if (data == "category")
            {
                string myQuery = "SELECT category FROM DisciplinaryRecord WHERE ID = " + id;
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

                try
                {
                    myConnection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        d = myReader["category"].ToString();
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
        public static void DeleteDisciplinaryRecordFromID(int id)
        {
            OleDbConnection myConnection = DBConnectivity.GetConn();

            string myQuery = "DELETE * FROM DisciplinaryRecord WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@id", id);
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
        public static string CheckDriverDisciplinaryRecordExistsFromdAllColumns(int dID, string title, string description, string category, string dateAdded)
        {
            int training = 0;
            string user = "";
            OleDbConnection myConnection = DBConnectivity.GetConn();
            //OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            string myQuery = "SELECT count(*) FROM DisciplinaryRecord WHERE title = @title AND description = @description AND category = @category AND dateAdded = @dateAdded AND dID = @dID";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", title);
            myCommand.Parameters.AddWithValue("@description", description);
            myCommand.Parameters.AddWithValue("@category", category);
            myCommand.Parameters.AddWithValue("@dateAdded", dateAdded);
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
        #endregion
    }
}
